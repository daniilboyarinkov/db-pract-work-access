using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace db_pract_work_access
{
    public partial class Form1 : Form
    {
        // private static readonly string _connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=DB.mdb;";
        private static readonly string _connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=DB.mdb;";
        private readonly OleDbConnection _connection;
        public Form1()
        {
            InitializeComponent();

            _connection = new OleDbConnection(_connectString);

            _connection.Open();
            updateStudents();
            UpdateCourse(true);
        }

        private void updateStudents(string q = "SELECT * FROM Students")
        {
            string query = q;
            OleDbCommand command = new OleDbCommand(query, _connection);
            OleDbDataReader reader = command.ExecuteReader();

            StudentGrid.Rows.Clear();

            while (reader.Read())
            {
                var id = reader[0];
                var fio = reader[1];
                var institute = reader[2];
                var group = reader[3];
                var age = reader[4];
                var course = reader[5] ?? "";
                StudentGrid.Rows.Add(id, fio, institute, group, age, course);
            }

            reader.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string q = "SELECT * FROM Students WHERE FIO LIKE 'И*'";
            updateStudents(q);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            updateStudents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT Max(Age) from students";

            OleDbCommand command = new OleDbCommand(query, _connection);

            var age = command.ExecuteScalar()?.ToString() ?? "?";

            label1.Text = $"Самому старшему: {age} лет";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string q = "SELECT * FROM Students WHERE age between 18 and 19";
            updateStudents(q);
        }

        private void UpdateCourse(bool setNull = false)
        {
            string query;
            if (setNull)
            {
                query = "update students set course = null";
            } else
            {
                query = "update students set course = iif(age <= 19, 1, iif(age <= 21, 2, iif(age <= 23, 3, 4)))";
            }

            OleDbCommand command = new OleDbCommand(query, _connection);

            command.ExecuteNonQuery();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateCourse();
            updateStudents();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO students (FIO, Institute, [Group], Age, Course) SELECT FIO, Institute, Group, Age, Course FROM students WHERE Institute='ИКИТ';";

            OleDbCommand command = new OleDbCommand(query, _connection);

            command.ExecuteNonQuery();
            updateStudents();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string query = "delete from students where id > 6";

            OleDbCommand command = new OleDbCommand(query, _connection);

            command.ExecuteNonQuery();
            updateStudents();
        }
    }
}