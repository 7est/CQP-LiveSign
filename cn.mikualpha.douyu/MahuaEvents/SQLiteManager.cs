using System;
using SQLitePCL;

class SQLiteManager
{
    private const string appid = "cn.mikualpha.douyu";
    private string path = "./app/" + appid + "/config.db";
    private static SQLiteManager ins = null;
    private SQLiteConnection _connection = null;

    private SQLiteManager() { 
        _connection = new SQLiteConnection(path);
        createTable("user", "(user TEXT PRIMARY KEY NOT NULL, sub_rooms TEXT, is_group INT NOT NULL)");
        createTable("room", "(id INTEGER PRIMARY KEY, room INT UNIQUE)");
        createTable("status", "(room INT PRIMARY KEY, living INT NOT NULL)");
    }

    public static SQLiteManager getInstance() {
        if (ins == null) ins = new SQLiteManager();
        return ins;
    }

    //������
    private void createTable(string table_name, string elements) { 
        string sql = "CREATE TABLE IF NOT EXISTS " + table_name + " " + elements + ";";
        using (var statement = _connection.Prepare(sql))
        {
            statement.Step();
        }
    }

    //��Ӷ���
    public void addSubscribe(int user, int room, int group = 0) { addSubscribe(user.ToString(), room.ToString(), group); }

    public void addSubscribe(string user, string room, int group = 0) {
        addRoom(room);

        string[] sub_rooms = getSubscribeByUser(user);
        string output = "";

        if (sub_rooms != null) {
            foreach (string i in sub_rooms) {
                if (i == room) return;
                if (output != "") output += ",";
                output += i;
            }
        }

        if (output != "") output += ",";
        output += room;

        insertSubscribe(user, output, group);
    }

    //ɾ������
    public void deleteSubscribe(string user, string room, int group = 0) {
        string[] sub_rooms = getSubscribeByUser(user);
        string output = "";

        if (sub_rooms != null)
        {
            foreach (string i in sub_rooms)
            {
                if (i == room) continue;
                if (output != "") output += ",";
                output += i;
            }
        }

        if (output == null || output == "") deleteUser(user); //�����û�������Ϊ�գ���ɾ�����û�
        else insertSubscribe(user, output, group); //��������û�������Ϣ

        if (getUserByRoom(room).Length == 0 && getGroupByRoom(room).Length == 0) deleteRoom(room); //���÷��������û�/Ⱥ�鶩�ģ���ɾ���˷���
    }

    //���붩�ļ�¼(�����м�¼����)
    private void insertSubscribe(string user, string rooms, int group = 0) {
        string sql = "INSERT OR REPLACE INTO user VALUES(" + user + ",'" + rooms + "', " + group + ");";
        using (var statement = _connection.Prepare(sql))
        {
            statement.Step();
        }
    }

    //ɾ���û�
    private void deleteUser(string user) {
        string sql = "DELETE FROM user WHERE user = " + user + ";";
        using (var statement = _connection.Prepare(sql))
        {
            statement.Step();
        }
    }

    //��ӷ���(����������)
    private void addRoom(int room) { addRoom(room.ToString()); }

    private void addRoom(string room) {
        string sql = "INSERT OR IGNORE INTO room VALUES(NULL, " + room + ");";
        using (var statement = _connection.Prepare(sql)) {
            statement.Step();
        }

        sql = "INSERT OR REPLACE INTO status VALUES(" + room + ",2);";
        using (var statement = _connection.Prepare(sql)) {
            statement.Step();
        }
    }

    //ɾ������(��������������)
    private void deleteRoom(int room) { deleteRoom(room.ToString()); }

    private void deleteRoom(string room) {
        string sql = "DELETE FROM room WHERE room = " + room + ";";
        using (var statement = _connection.Prepare(sql))
        {
            statement.Step();
        }

        sql = "DELETE FROM status WHERE room = " + room + ";";
        using (var statement = _connection.Prepare(sql))
        {
            statement.Step();
        }
    }

    //���ĳ�û��Ƿ��Ѷ���ĳ����
    public bool isSubscribing(string user, string room) {
        string[] temp = getSubscribeByUser(user);
        foreach (string i in temp) {
            if (i == room) return true;
        }
        return false;
    }

    //��ȡ�û����ĵ����з���
    public string[] getSubscribeByUser(string user) {
        string sql = "SELECT sub_rooms FROM user WHERE user = " + user + ";";
        using (var statement = _connection.Prepare(sql))
        {
            SQLiteResult result = statement.Step();
            if (SQLiteResult.ROW == result)
            {
                return statement.GetText(0).Split(new char[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        return null;
    }

    //���ݷ����ȡ�����û���
    public string[] getUserByRoom(string room) {
        string sql = "SELECT user FROM user WHERE is_group = 0 AND (" +
                        "sub_rooms LIKE '%," + room + ",%' " + 
                        "OR sub_rooms LIKE '" + room + ",%' " +
                        "OR sub_rooms LIKE '%," + room + "' " +
                        "OR sub_rooms = '" + room + "');";
        using (var statement = _connection.Prepare(sql))
        {
            string output = "";
            SQLiteResult result = statement.Step();
            while (SQLiteResult.ROW == result)
            {
                if (output != "") output += ",";
                output += statement.GetText(0);
                result = statement.Step();
            }
            return output.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    //���ݷ����ȡ����Ⱥ���
    public string[] getGroupByRoom(string room)
    {
        string sql = "SELECT user FROM user WHERE is_group = 1 AND (" +
                        "sub_rooms LIKE '%," + room + ",%' " +
                        "OR sub_rooms LIKE '" + room + ",%' " +
                        "OR sub_rooms LIKE '%," + room + "' " +
                        "OR sub_rooms = '" + room + "');";
        using (var statement = _connection.Prepare(sql))
        {
            string output = "";
            SQLiteResult result = statement.Step();
            while (SQLiteResult.ROW == result)
            {
                if (output != "") output += ",";
                output += statement.GetText(0);
                result = statement.Step();
            }
            return output.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    //��ȡ���еķ����б�
    public string[] getRooms() {
        string sql = "SELECT room FROM room;";
        using (var statement = _connection.Prepare(sql))
        {
            string output = "";
            SQLiteResult result = statement.Step();
            while (SQLiteResult.ROW == result)
            {
                if (output != "") output += ",";
                output += statement.GetInteger(0).ToString();
                result = statement.Step();
            }
            return output.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    //����Live״̬
    public void setLiveStatus(string room, int status) {
        string sql = "UPDATE status SET living = " + status + " WHERE room = " + room + ";";
        using (var statement = _connection.Prepare(sql)) {
            statement.Step();
        }
    }

    //��ȡLive״̬
    public int getLiveStatus(string room) {
        string sql = "SELECT living FROM status WHERE room = " + room + ";";
        using (var statement = _connection.Prepare(sql))
        {
            SQLiteResult result = statement.Step();
            if (SQLiteResult.ROW == result) {
                return (int)statement.GetInteger(0);
            }
        }
        return 2; //Offline
    }
}

