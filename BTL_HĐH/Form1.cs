using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // Cung cấp các lớp cho phép tương tác với các process, event log và performance counters
namespace BTL_HĐH
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
         
        }
        private void Word_Click(object sender, EventArgs e) // sự kiện khi bấm vào nút word
        {
            Process.Start("winword"); // khởi động chương trình có tên file là winword
        }
        private void Excel_Click(object sender, EventArgs e) // sự kiện khi bấm vào nút excel
        {
            Process.Start("excel"); // khởi động chương trình có tên file là excel
         
        }

        private void Powp_Click(object sender, EventArgs e)// sự kiện khi bấm vào nút Powp
        {
            Process.Start("POWERPNT"); // khởi động chương trình có tên file là powerpnt
        }


        private void CloseApp_Click(object sender, EventArgs e)// sự kiện khi bấm vào nút CloseApp
        {
            foreach(var process in Process.GetProcessesByName(comboBox1.Text)) // vòng lặp foreach chạy mảng các danh sách có tên là comboBox1.Text
            {
                process.Kill(); // dừng tiến trình
            }
        }
        public double ByteToMB(long num)// hàm chuyển đổi từ byte sang MB
        {
            return (num / 1024 / 1024);// để chuyển từ byte sang MB ta phải chia cho 2 lần 1024 ,lần đầu chia để lấy ra số Kb lần 2 chia ra để lấy MB
        }
        Process[] process; // tạo mảng process có kiểu là Process
        void Load1() // tạo hàm Load1
        {
            int count = Convert.ToInt32(lbCount.Text); // chuyển lbCount từ dạng string sang dạng int và gắn cho biến count
            process = Process.GetProcesses(); // process được gắn bằng tất cả các process đang chạy trong máy
            if (count != process.Length) // so sánh số lượng count và số lượng chiều dài của mảng process
            {
                listView1.Items.Clear(); // xoá hết các item có trong listView1
                foreach (var x in process) // chạy vòng lặp foreach với mảng là process
                {
                    string status = x.Responding == true ? "Running" : "Not Responding"; // gắn cho biến status 1 trong 2 trạng thái running hoặc not responding
                    ListViewItem newItem = new ListViewItem() // tạo 1 listviewitem mới có biến là new item
                    {
                        Text = x.ProcessName // gắn Text bằng tên của process
                    };
                    newItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = x.Id.ToString() }); // thêm  item con gắn text bằng id của process
                    newItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = status }); // thêm item con gắn text bằng trạng thái của process 
                    newItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = ByteToMB(x.WorkingSet64).ToString() + " MB" }); // thêm item con gắn text bằng số MB của process
                    listView1.Items.Add(newItem); // thêm newIteam vào listView

                }
                lbCount.Text = process.Length.ToString(); // gắn lbCount  bằng chiều dài của mảng process

            }
        }
        void load2() // tạo hàm load 2
        {
            long sum = 0; // khỏi tạo biến sum bằng 0
            Process[] word = Process.GetProcessesByName("winword"); // tạo mảng word gắn mảng với những Process có tên là winword
            foreach(var x in word) // chạy vòng lặp trong mảng word
            {
                sum += x.WorkingSet64; // tổng lượng ram vật lý của mảng word

            }
            chart1.Series["word"].Points.AddY(ByteToMB(sum)); // gắn điểm của đường word bằng tổng lượng ram vật lý của process có tên là winword đang sử dụng, có sử dụng hàm biến đổi từ byte sang MB
            Process[] excel = Process.GetProcessesByName("excel"); //tạo mảng excel gắn mảng với những Process có tên là excel
            sum = 0; // gắn biến sum bằng 0 
            foreach (var x in excel)// chạy vòng lặp trong mảng word
            {
                sum += x.WorkingSet64;// tổng lượng ram vật lý của mảng excel

            }
            chart1.Series["excel"].Points.AddY(ByteToMB(sum));// gắn điểm của đường excel bằng tổng lượng ram vật lý của process có tên là excel đang sử dụng, có sử dụng hàm biến đổi từ byte sang MB
            Process[] pp = Process.GetProcessesByName("POWERPNT");//tạo mảng pp gắn mảng với những Process có tên là POWERPNT
            sum = 0;// gắn biến sum bằng 0 
            foreach (var x in pp)// chạy vòng lặp trong mảng pp
            {
                sum += x.WorkingSet64;// tổng lượng ram vật lý của mảng pp

            }
            chart1.Series["pp"].Points.AddY(ByteToMB(sum)); // gắn điểm của đường pp bằng tổng lượng ram vật lý của process có tên là POWERPNT đang sử dụng, có sử dụng hàm biến đổi từ byte sang MB


        }

        private void Form1_Load(object sender, EventArgs e) // gọi hàm Form1 khi Load 
        {
            Load1();// gọi hàm load1
            load2();// gọi hàm load 2
        }
        
        private void timer1_Tick(object sender, EventArgs e)// Cứ 1s gọi hàm timer1
        {
            load2(); // gọi hàm load 2
            
        }

        private void timer2_Tick(object sender, EventArgs e)// Cứ 0,1s gọi hàm timer2
        {
            Load1(); // gọi hàm load 1
            
        }
        private void button1_Click(object sender, EventArgs e) // sự kiện khi bấm nút button1 tức là nút End Task
        {
            process[listView1.SelectedIndices[0]].Kill(); // dừng tiến trình với item đang chọn trên listView1
        }


        private void listView1_Click(object sender, EventArgs e) // sự kiện khi bấm trên listView1
        {
            comboBox1.Text = listView1.SelectedItems[0].Text; // gắn Text của comboBox bằng item đang chọn trên listView
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
