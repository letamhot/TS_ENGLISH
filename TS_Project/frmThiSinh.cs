using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using TS_Project.Model;

namespace TS_Project
{
    public partial class frmThiSinh : Form
    {
        QuaMienDiSanEntities _entities = new QuaMienDiSanEntities();
        private Socket sock;
        static string message;
        private int thoiGianConLai = 0;
        private byte[] byBuff = new byte[256];
        private string currentPath = Directory.GetCurrentDirectory();
        private event AddMessage addMessage;
        int[] ttGoiKD = { 0, 0, 0, 0, 0, 0 };
        int[] ttGoiVD = { 0, 0, 0, 0, 0, 0 };
        int id = 0;
        ds_doi ds_Doi = new ds_doi();
        SqlDataAccess sqlObject = new SqlDataAccess();


        public frmThiSinh()
        {
            InitializeComponent();
        }

        public frmThiSinh(int doiId)
        {
            this.ClientSize = new Size(1366, 768);
            this.Size = new Size(1366, 768);

            ds_cuocthi cuocthi = _entities.ds_cuocthi.FirstOrDefault(x => x.trangthai == true);
            try
            {
                if (cuocthi != null)
                {
                    id = doiId;
                    ds_Doi = _entities.ds_doi.FirstOrDefault(y => y.doiid == id && y.cuocthiid == cuocthi.cuocthiid);
                    InitializeComponent();
                    connecServer();
                    addMessage = new AddMessage(OnAddMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Tài khoản này không dành cho cuộc thi hiện tại");
            }


        }

        private void connecServer()
        {
            Cursor cursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (sock != null && sock.Connected)
                {
                    sock.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(10);
                    sock.Close();
                }
                string server_ip = ConfigurationManager.AppSettings["IPServer"];
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint epServer = new IPEndPoint(IPAddress.Parse(server_ip), 399); //192.168.2.117
                sock.Blocking = false;
                AsyncCallback onconnect = new AsyncCallback(OnConnect);
                sock.BeginConnect(epServer, onconnect, sock);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Server Connect failed!");
            }
            Cursor.Current = cursor;
        }
        public void OnConnect(IAsyncResult ar)
        {

            Socket sock = (Socket)ar.AsyncState;
            try
            {
                if (sock.Connected)
                {
                    SetupRecieveCallback(sock);
                    SendEvent(id.ToString() + ",cli,connected,on");
                } 
                else
                    MessageBox.Show(this, "khong cho phep connect den may o xa", "loi ket noi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "loi khi thuc hien connect!");
            }
        }
        public void SetupRecieveCallback(Socket sock)
        {
            try
            {
                AsyncCallback recieveData = new AsyncCallback(OnRecievedData);
                sock.BeginReceive(byBuff, 0, byBuff.Length, SocketFlags.None, recieveData, sock);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Setup Recieve Callback failed!");
            }
        }
        /*public void OnRecievedData(IAsyncResult ar)
        {
            Socket socks = (Socket)ar.AsyncState;
            try
            {
                int nBytesRec = socks.EndReceive(ar);
                if (nBytesRec > 0)
                {
                    string sRecieved = Encoding.ASCII.GetString(byBuff, 0, nBytesRec);
                    Invoke(addMessage, new string[] { sRecieved });
                    SetupRecieveCallback(socks);
                }
                else
                {
                    Console.WriteLine("Client {0}, disconnected", socks.RemoteEndPoint);
                    socks.Shutdown(SocketShutdown.Both);
                    socks.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Loi xay ra khi nhan ket qua tra ve!");
            }
        }*/
        public void OnRecievedData(IAsyncResult ar)
        {
            Socket socks = (Socket)ar.AsyncState;
            try
            {
                int nBytesRec = socks.EndReceive(ar);
                if (nBytesRec > 0)
                {
                    string sRecieved = Encoding.ASCII.GetString(byBuff, 0, nBytesRec);

                    // Kiểm tra xem form đã được khởi tạo và có handle chưa
                    if (this.IsHandleCreated)
                    {
                        this.BeginInvoke(addMessage, new string[] { sRecieved });
                    }
                    else
                    {
                        Console.WriteLine("Handle chưa được tạo.");
                    }

                    SetupRecieveCallback(socks);
                }
                else
                {
                    Console.WriteLine("Client {0}, disconnected", socks.RemoteEndPoint);
                    socks.Shutdown(SocketShutdown.Both);
                    socks.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Lỗi xảy ra khi nhận kết quả trả về!");
            }
        }
        public void OnAddMessage(string sMessage)
        {
            //Cấu trúc tin nhắn từ server x,y,z
            //Trong đó x: id đội (x = 0: tin nhắn all broadcast, x = 5: MC, x = 6: Trình chiếu)
            //y: cli: tin nhắn từ thí sinh, mc, trình chiếu. ser: tin nhắn từ điều khiển chương trình
            message = sMessage;
            string[] spl = message.Split(',');
            //id = Convert.ToInt32(spl[0]);
            string src = spl[1];
            if (src.Equals("ser"))
            {
                if (spl[0] == "0")
                {
                    if (spl[2] == "playkhoidong")
                    {
                        lblThoiGian.ForeColor = Color.White;
                        lblTenThiSinh.ForeColor = Color.White;
                        lblTenTruong.ForeColor = Color.White;
                        lblTenThiSinh.Location = new Point(430, 147);
                        lblTenTruong.Location = new Point(884, 147);

                        TongDiemKD();
                        pnlNoiDung.Controls.Clear();
                        //visibleManHinhChinh();
                        this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group\\kd_tl.png");
                        this.BackgroundImageLayout = ImageLayout.Stretch;
                        lblTenThiSinh.Visible = false;
                        lblTenTruong.Visible = false;
                        lblThoiGian.Visible = false;
                        lblTongDiem.Visible = false;
                        ngoisaoSchool.Visible = false;
                        ngoisaoTS.Visible = false;
                        /*displayTeamInfo();
                        thoiGianConLai = 60;
                        lblThoiGian.Text = thoiGianConLai.ToString();
                        pnlNoiDung.Controls.Clear();
                        pnlNoiDung.Controls.Add(new ucKhoiDong(sock, id, 0, 0, ttGoiKD, false));*/
                    }
                    if (spl[2] == "playthuthach")
                    {
                        lblThoiGian.ForeColor = Color.White;
                        lblTenThiSinh.ForeColor = Color.White;
                        lblTenTruong.ForeColor = Color.White;
                        lblTenThiSinh.Location = new Point(430, 147);
                        lblTenTruong.Location = new Point(884, 147);
                        TongDiemKD();
                        lblTongDiem.Visible = true;

                        if (spl[3] == "0")
                        {
                            ngoisaoSchool.Visible = false;
                            ngoisaoTS.Visible = false;
                            pnlNoiDung.Controls.Clear();
                            this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group\\kp_tl.png");
                            this.BackgroundImageLayout = ImageLayout.Stretch;
                            lblTenThiSinh.Visible = false;
                            lblTenTruong.Visible = false;
                            lblThoiGian.Visible = false;
                            lblTongDiem.Visible = false;
                        }
                        else
                        {
                            ngoisaoSchool.Visible = true;
                            ngoisaoTS.Visible = true;
                            TongDiemKD();
                            displayTeamInfo();
                            thoiGianConLai = 30;
                            lblThoiGian.Text = thoiGianConLai.ToString();
                            lblTongDiem.Visible = true;

                            if (spl[4] == "ready")
                            {
                                tmClient.Enabled = false;
                                this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\ts_kp.png");
                                this.BackgroundImageLayout = ImageLayout.Stretch;
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucThuThach(sock, id, int.Parse(spl[3]), false, 0));
                            }
                            if (spl[4] == "start")
                            {
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucThuThach(sock, id, int.Parse(spl[3]), true, int.Parse(spl[5])));
                                this.Focus();
                                //Thread.Sleep(1000);
                                tmClient.Enabled = true;
                               
                            }
                            if (spl[4] == "stop")
                            {
                                lblTongDiem.Visible = true;

                            }
                            if (spl[4] == "hienthidapanCT")
                            {
                                lblTongDiem.Visible = true;

                            }
                            if (spl[4] == "hienthidiemKP")
                            {
                                tmClient.Enabled = false;
                                lblTongDiem.Visible = true;
                                TongDiemKD();
                            }
                            if (spl[4] == "capNhatDienManHinhTT")
                            {
                                TongDiemKD();
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucThuThach(sock, id, int.Parse(spl[3]), true, 0));
                            }

                        }
                    }
                    if (spl[2] == "playkhamphachiase")
                    {

                        TongDiemKD();

                        lblThoiGian.ForeColor = Color.White;
                        lblTenThiSinh.ForeColor = Color.White;
                        lblTenTruong.ForeColor = Color.White;
                        lblTenThiSinh.Location = new Point(430, 147);
                        lblTenTruong.Location = new Point(884, 147);
                        lblTongDiem.Visible = true;
                        if (spl[3] == "0")
                        {
                            ngoisaoSchool.Visible = false;
                            ngoisaoTS.Visible = false;
                            pnlNoiDung.Controls.Clear();
                            this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group\\cp_tl.png");
                            this.BackgroundImageLayout = ImageLayout.Stretch;
                            lblTenThiSinh.Visible = false;
                            lblTenTruong.Visible = false;
                            lblThoiGian.Visible = false;
                            lblTongDiem.Visible = false;
                        }
                        else
                        {
                            ngoisaoSchool.Visible = true;
                            ngoisaoTS.Visible = true;
                            TongDiemKD();
                            lblTongDiem.Visible = true;

                            displayTeamInfo();
                            this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\ts_cp.png");
                            this.BackgroundImageLayout = ImageLayout.Stretch;
                            

                            // Kiểm tra xem mảng spl có đủ phần tử không
                            if (spl.Length > 5)
                            {
                                if (spl[5] == "start")
                                {
                                    int cauhoiId = Convert.ToInt32(spl[3]);
                                    var khamPha = _entities.ds_goicaudiscovery.FirstOrDefault(x => x.cauhoiid == cauhoiId && x.trangthai == true);

                                    if (khamPha != null)
                                    {
                                        pnlNoiDung.Controls.Clear();

                                        // Kiểm tra xem có video hay không, nếu có thì chạy video, nếu không thì xử lý hình ảnh
                                        if (!string.IsNullOrWhiteSpace(khamPha.noidungthisinh))
                                        {
                                            pnlNoiDung.Controls.Add(new ucKhamPhaCS(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), true, true, true)); // Thêm tham số true để phát video
                                            tmClient.Enabled = true;
                                        }
                                        else
                                        {
                                            pnlNoiDung.Controls.Add(new ucKhamPhaCS(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), false, false, true));
                                            tmClient.Enabled = true;
                                        }
                                    }
                                }

                                if (spl[5] == "ready")
                                {
                                    thoiGianConLai = 180;
                                    lblThoiGian.Text = thoiGianConLai.ToString();
                                    // Khi trạng thái "ready", đảm bảo rằng video không phát
                                    pnlNoiDung.Controls.Clear();
                                    pnlNoiDung.Controls.Add(new ucKhamPhaCS(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), false, false, false));  // Hình ảnh
                                    tmClient.Enabled = false;
                                }

                                if (spl[5] == "stopTime")
                                {
                                    // Khi dừng thời gian, xử lý video nếu có
                                    int cauhoiId = Convert.ToInt32(spl[3]);
                                    var khamPha = _entities.ds_goicaudiscovery.FirstOrDefault(x => x.cauhoiid == cauhoiId && x.trangthai == true);
                                    if (!string.IsNullOrWhiteSpace(khamPha.noidungthisinh))
                                    {
                                        pnlNoiDung.Controls.Clear();
                                        pnlNoiDung.Controls.Add(new ucKhamPhaCS(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), false, true, false)); // Dừng video
                                        tmClient.Enabled = false;
                                    }
                                    else
                                    {
                                        pnlNoiDung.Controls.Clear();
                                        pnlNoiDung.Controls.Add(new ucKhamPhaCS(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), false, false, false)); // Hình ảnh
                                        tmClient.Enabled = false;
                                    }
                                }
                                if (spl[5] == "hienthianhthisinh")
                                {
                                    this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\ts_cp.png");
                                    this.BackgroundImageLayout = ImageLayout.Stretch;
                                    tmClient.Enabled = false;
                                    pnlNoiDung.Controls.Clear();
                                    pnlNoiDung.Controls.Add(new ucKhamPhaCS(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), false, true, false));
                                }
                                if (spl[5] == "hienthimanh")
                                {
                                    pnlNoiDung.Controls.Clear();
                                    pnlNoiDung.Controls.Add(new ucKhamPhaCS(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), true, false, false));
                                    //tmClient.Enabled = true;
                                }
                                if (spl[5] == "load6nut")
                                {
                                    pnlNoiDung.Controls.Clear();
                                    pnlNoiDung.Controls.Add(new ucKhamPhaCS(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), true, false, false));
                                    //tmClient.Enabled = true;
                                }
                                if (spl[5] == "capnhatTongDiem")
                                {
                                    int cauhoiId = Convert.ToInt32(spl[3]);
                                    var khamPha = _entities.ds_goicaudiscovery.FirstOrDefault(x => x.cauhoiid == cauhoiId && x.trangthai == true);
                                    if (!string.IsNullOrWhiteSpace(khamPha.noidungthisinh))
                                    {
                                        pnlNoiDung.Controls.Clear();
                                        pnlNoiDung.Controls.Add(new ucKhamPhaCS(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), false, true, false));
                                        lblTongDiem.Visible = true;
                                        TongDiemKD();
                                    }
                                    else
                                    {
                                        pnlNoiDung.Controls.Clear();
                                        pnlNoiDung.Controls.Add(new ucKhamPhaCS(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), false, false, false));
                                        lblTongDiem.Visible = true;
                                        TongDiemKD();
                                    }

                                }
                                
                            }
                            else
                            {
                                // Xử lý khi mảng spl không có đủ phần tử
                                MessageBox.Show("Lỗi dữ liệu nhận được không đầy đủ.");
                            }
                        }

                    }
                    if (spl[2] == "playtoasang")
                    {
                        
                        lblThoiGian.ForeColor = Color.White;
                        lblTenThiSinh.ForeColor = Color.White;
                        lblTenTruong.ForeColor = Color.White;
                        lblTenThiSinh.Location = new Point(430, 147);
                        lblTenTruong.Location = new Point(884, 147);
                        TongDiemKD();
                        bool trangThaiHienThiCau = false;
                        bool x2 = false;
                        bool da = false;
                        bool tt = false;
                        if (spl[5] == "0")
                        {
                            ngoisaoSchool.Visible = false;
                            ngoisaoTS.Visible = false;
                            pnlNoiDung.Controls.Clear();
                            this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group\\vd_tl.png");
                            this.BackgroundImageLayout = ImageLayout.Stretch;
                            lblTenThiSinh.Visible = false;
                            lblTenTruong.Visible = false;
                            lblThoiGian.Visible = false;
                            lblTongDiem.Visible = false;
                            
                        }
                        else
                        {
                            ngoisaoSchool.Visible = true;
                            ngoisaoTS.Visible = true;
                            lblTongDiem.Visible = true;
                            TongDiemKD();
                            thoiGianConLai = 20;
                            lblThoiGian.Text = thoiGianConLai.ToString();
                            displayTeamInfo();
                            this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\ts_vd.png");
                            this.BackgroundImageLayout = ImageLayout.Stretch;
                            tmClient.Enabled = false;
                            if (spl[5] == "hienthi5NutCauHoi")
                            {
                                this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\ts_vd.png");
                                this.BackgroundImageLayout = ImageLayout.Stretch;
                                tmClient.Enabled = false;
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, false, false, false, false));
                            }
                            else if (spl[5] == "loadDanhSachCauHoi")
                            {
                                this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\ts_vd.png");
                                this.BackgroundImageLayout = ImageLayout.Stretch;
                                tmClient.Enabled = false;
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, false, false, false, false, true));
                            }

                            else if (spl[5] == "ready")
                            {

                                this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\ts_vd.png");
                                this.BackgroundImageLayout = ImageLayout.Stretch;
                                tmClient.Enabled = false;
                                tt = true;
                                if (int.Parse(spl[4]) != 0)
                                {
                                    var cauhoiVD = _entities.ds_goicauhoishining.Find(int.Parse(spl[4]));

                                    thoiGianConLai = 20;
                                    lblThoiGian.Text = thoiGianConLai.ToString();
                                    pnlNoiDung.Controls.Clear();
                                    pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, false, false, true, false));
                                }
                                else
                                {
                                    pnlNoiDung.Controls.Clear();
                                    pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, false, false, true, false));
                                }



                            }
                            else if (spl[5] == "start")
                            {

                                tmClient.Enabled = true;
                                tt = false;
                                if (int.Parse(spl[4]) != 0)
                                {
                                    var cauhoiVD = _entities.ds_goicauhoishining.Find(int.Parse(spl[4]));

                                    thoiGianConLai = 20;
                                    lblThoiGian.Text = thoiGianConLai.ToString();
                                    pnlNoiDung.Controls.Clear();
                                    pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, false, false, false, false));
                                }
                                else
                                {
                                    pnlNoiDung.Controls.Clear();
                                    pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, false, false, false, false));
                                }

                            }
                            else if (spl[5] == "start_x2")
                            {
                                tmClient.Enabled = true;
                                tt = false;
                                if (int.Parse(spl[4]) != 0)
                                {
                                    var cauhoiVD = _entities.ds_goicauhoishining.Find(int.Parse(spl[4]));

                                    thoiGianConLai = 20;
                                    lblThoiGian.Text = thoiGianConLai.ToString();
                                    pnlNoiDung.Controls.Clear();
                                    pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, true, false, false, false));
                                }
                                else
                                {
                                    pnlNoiDung.Controls.Clear();
                                    pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, true, false, false, false));
                                }
                            }
                            else if (spl[5] == "forceanswer")
                            {
                                tmClient.Enabled = false;

                                // Hiển thị đáp án khi thí sinh 1 trả lời đúng hoặc thí sinh 2 dành quyền
                                TongDiemKD();
                                da = true;
                                pnlNoiDung.Controls.Clear();

                                pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, x2, da, false, false));
                            }
                        }


                    }
                    if (spl[2] == "playgioithieu")
                    {
                        ngoisaoSchool.Visible = false;
                        ngoisaoTS.Visible = false;
                        pnlNoiDung.Controls.Clear();
                        this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\gt_qmds.png");
                        this.BackgroundImageLayout = ImageLayout.Stretch;
                        lblTenThiSinh.Visible = false;
                        lblTenTruong.Visible = false;
                        lblThoiGian.Visible = false;
                        lblTongDiem.Visible = false;
                    }

                }
                else 
                {
                    bool x2 = false;
                    bool da = false;
                    bool tt = false;
                    if (int.Parse(spl[0]) == id)
                    {
                        if (spl[2] == "playkhoidong")
                        {
                            ngoisaoSchool.Visible = true;
                            ngoisaoTS.Visible = true;
                            TongDiemKD();
                            displayTeamInfo();
                            lblTongDiem.Visible = true;

                            if (spl[5] == "start")
                            {
                                thoiGianConLai = 60;
                                tmClient.Enabled = true;
                                lblTongDiem.Visible = true;
                                lblThoiGian.Text = thoiGianConLai.ToString();
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucKhoiDong(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), ttGoiKD, false));
                            }
                            else if (spl[5] == "stop")
                            {
                                // When stopping, mark the current package as completed (status 2)
                                if (int.Parse(spl[4]) > 0)
                                {
                                    ttGoiKD[int.Parse(spl[4]) - 1] = 2; // Changed from 1 to 2 to indicate completed
                                }
                                tmClient.Enabled = false;
                                lblTongDiem.Visible = true;
                                TongDiemKD();
                                thoiGianConLai = 60;
                                lblThoiGian.Text = thoiGianConLai.ToString();
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucKhoiDong(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), ttGoiKD, true));
                            }
                            else if (spl[5] == "ready")
                            {
                                // When ready, mark the package as selected (status 1)
                                if (int.Parse(spl[4]) > 0)
                                {
                                    ttGoiKD[int.Parse(spl[4]) - 1] = 1; // Keep as 1 to indicate selected/in-progress
                                }
                                else
                                {
                                    this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\ts_kd.png");
                                    this.BackgroundImageLayout = ImageLayout.Stretch;
                                }
                                tmClient.Enabled = false;
                                thoiGianConLai = 60;
                                lblThoiGian.Text = thoiGianConLai.ToString();
                                lblTongDiem.Visible = true;
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucKhoiDong(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), ttGoiKD, false));
                            }
                            else if (spl[5] == "next")
                            {
                                TongDiemKD();
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucKhoiDong(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), ttGoiKD, false));
                            }
                            else if (spl[5] == "capNhatDiemManHinh")
                            {
                                TongDiemKD();
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucKhoiDong(sock, id, int.Parse(spl[3]), int.Parse(spl[4]), ttGoiKD, true));
                            }
                        }
                        if (spl[2] == "playtoasang")
                        {
                            TongDiemKD();

                            if (spl[5] == "showanswer")
                            {
                                tmClient.Enabled = false;
                                thoiGianConLai = 20;
                                lblThoiGian.Text = thoiGianConLai.ToString();

                                // Hiển thị đáp án khi thí sinh 1 trả lời đúng hoặc thí sinh 2 dành quyền
                                TongDiemKD();
                                da = true;
                                pnlNoiDung.Controls.Clear();

                                pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, x2, da, false, false));
                            }
                            else if (spl[5] == "noanswer")
                            {
                                tmClient.Enabled = false;
                                thoiGianConLai = 20;
                                lblThoiGian.Text = thoiGianConLai.ToString();

                                // Không hiển thị đáp án khi thí sinh 1 trả lời sai
                                TongDiemKD();
                                da = false;
                                pnlNoiDung.Controls.Clear();

                                pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, x2, da, false, false));
                            }
                            else if (spl[5] == "capNhatDiemManHinhTS")
                            {
                                TongDiemKD();
                                tmClient.Enabled = false;
                                tt = false;
                                x2 = true;
                                da = true;
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), tt, x2, da, false, false));
                            }
                            else if (spl[5] == "start_ngoisaohivong")
                            {
                                x2 = true;
                                pnlNoiDung.Controls.Clear();

                                pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), false, x2, da, false, false));
                            }
                            else if (spl[5] == "start_Nongoisaohivong")
                            {
                                x2 = false;
                                pnlNoiDung.Controls.Clear();

                                pnlNoiDung.Controls.Add(new ucToaSang(sock, id, int.Parse(spl[4]), false, x2, da, false, false));
                            }

                        }
                    }
                    else
                    {
                        int[] ttGoiKD = { 0, 0, 0, 0, 0, 0 };
                        if (spl[2] == "playkhoidong")
                        {
                            ngoisaoSchool.Visible = true;
                            ngoisaoTS.Visible = true;
                            TongDiemKD();
                            displayTeamInfo();
                            lblTongDiem.Visible = true;

                            if (spl[5] == "stop")
                            {
                                // When stopping, mark the current package as completed (status 2)
                                if (int.Parse(spl[4]) > 0)
                                {
                                    ttGoiKD[int.Parse(spl[4]) - 1] = 2; // Changed from 1 to 2 to indicate completed
                                }
                                tmClient.Enabled = false;
                                lblTongDiem.Visible = true;
                                TongDiemKD();
                                thoiGianConLai = 60;
                                lblThoiGian.Text = thoiGianConLai.ToString();
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucKhoiDong(sock, int.Parse(spl[0]), int.Parse(spl[3]), int.Parse(spl[4]), ttGoiKD, true));
                            }
                            else if (spl[5] == "ready")
                            {
                                // When ready, mark the package as selected (status 1)
                                if (int.Parse(spl[4]) > 0)
                                {
                                    ttGoiKD[int.Parse(spl[4]) - 1] = 1; // Keep as 1 to indicate selected/in-progress
                                }
                                else
                                {
                                    this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\ts_kd.png");
                                    this.BackgroundImageLayout = ImageLayout.Stretch;
                                }
                                tmClient.Enabled = false;
                                thoiGianConLai = 60;
                                lblThoiGian.Text = thoiGianConLai.ToString();
                                lblTongDiem.Visible = true;
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucKhoiDong(sock, int.Parse(spl[0]), int.Parse(spl[3]), int.Parse(spl[4]), ttGoiKD, false));
                            }  
                            else if (spl[5] == "capNhatDiemManHinh")
                            {
                                TongDiemKD();
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucKhoiDong(sock, int.Parse(spl[0]), int.Parse(spl[3]), int.Parse(spl[4]), ttGoiKD, true));
                            }
                        }
                        if (spl[2] == "playtoasang")
                        {
                            TongDiemKD();


                            if (spl[5] == "showanswer")
                            {
                                // Hiển thị đáp án khi thí sinh 1 trả lời đúng hoặc thí sinh 2 dành quyền
                                TongDiemKD();
                                da = true;
                                pnlNoiDung.Controls.Clear();

                                pnlNoiDung.Controls.Add(new ucToaSang(sock, int.Parse(spl[0]), int.Parse(spl[4]), tt, x2, da, false, false));
                            }
                            else if (spl[5] == "noanswer")
                            {
                                // Không hiển thị đáp án khi thí sinh 1 trả lời sai
                                TongDiemKD();
                                da = false;
                                pnlNoiDung.Controls.Clear();

                                pnlNoiDung.Controls.Add(new ucToaSang(sock, int.Parse(spl[0]), int.Parse(spl[4]), tt, x2, da, false, false));
                            }
                            else if (spl[5] == "capNhatDiemManHinhTS")
                            {
                                TongDiemKD();
                                tmClient.Enabled = false;
                                tt = false;
                                x2 = false;
                                da = false;
                                pnlNoiDung.Controls.Clear();
                                pnlNoiDung.Controls.Add(new ucToaSang(sock, int.Parse(spl[0]), int.Parse(spl[4]), tt, x2, da, false, false));
                            }
                            else if (spl[5] == "start_ngoisaohivong")
                            {
                                x2 = true;
                                pnlNoiDung.Controls.Clear();

                                pnlNoiDung.Controls.Add(new ucToaSang(sock, int.Parse(spl[0]), int.Parse(spl[4]), false, x2, da, false, false));
                            }
                            else if (spl[5] == "start_Nongoisaohivong")
                            {
                                x2 = false;
                                pnlNoiDung.Controls.Clear();

                                pnlNoiDung.Controls.Add(new ucToaSang(sock, int.Parse(spl[0]), int.Parse(spl[4]), false, x2, da, false, false));
                            }
                        }
                    }

                }
            }
        }

        public void hideManHinh()
        {
            /*grbBangThoiGian.Visible = false;
            grbDiemThi.Visible = false;
            grbManHinhChinh.Visible = false;
            grbThongTinDoiChoi.Visible = false;
            grbTieuDe.Visible = false;*/
        }

        private void TongDiemKD()
        {
            lblTongDiem.Text = "0";

            string sql = "SELECT doiid, sum(sodiem) as tongdiem from ds_diem WHERE doiid = " + id + " GROUP BY cuocthiid, doiid";
            DataTable dt = sqlObject.getDataFromSql(sql, "").Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    ds_doi doiChoi = _entities.ds_doi.Find(int.Parse(dr["doiid"].ToString()));
                    if (doiChoi != null)
                    {
                        if (doiChoi.vitridoi == 1)
                        {
                            lblTongDiem.Text = dr["tongdiem"].ToString();
                        }
                        if (doiChoi.vitridoi == 2)
                        {
                            lblTongDiem.Text = dr["tongdiem"].ToString();
                        }
                        if (doiChoi.vitridoi == 3)
                        {
                            lblTongDiem.Text = dr["tongdiem"].ToString();
                        }
                        if (doiChoi.vitridoi == 4)
                        {
                            lblTongDiem.Text = dr["tongdiem"].ToString();
                        }
                    }
                }
            }
            _entities.SaveChanges();


        }
        public void displayManHinh()
        {
            /*grbBangThoiGian.Visible = true;
            grbDiemThi.Visible = true;
            grbManHinhChinh.Visible = true;
            grbThongTinDoiChoi.Visible = true;
            grbTieuDe.Visible = true;*/
        }
        private void SendEvent(string str)
        {
            // Check we are connected
            if (sock == null || !sock.Connected)
            {
                MessageBox.Show(this, "Must be connected to Send a message");
                return;
            }
            // Read the message from the text box and send it
            try
            {
                // Convert to byte array and send.
                Byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                sock.Send(byteDateLine, byteDateLine.Length, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Send lenh dieu khien loi!");
            }

        }

        private void frmThiSinh_FormClosing(object sender, FormClosingEventArgs e)
        {
            SendEvent(id.ToString() + ",cli,connected,off");
            Application.Exit();
        }

        private void displayTeamInfo()
        {
            /*lblTeam.Visible = true;
            labelTenTruong.Visible = true;
            labelThiSinh.Visible = true;*/
            lblTenThiSinh.Visible = true;
            lblTenTruong.Visible = true;
            lblThoiGian.Visible = true;
            lblTongDiem.Visible = true;
            //lblTeam.Text = "ĐỘI " + ds_Doi.vitridoi;
            lblTenTruong.Text = ds_Doi.tendoi;
            lblTenThiSinh.Text = ds_Doi.tennguoichoi;
        }

        private void frmThiSinh_Load(object sender, EventArgs e)
        {
            invisibleManHinhChinh();
            btnCloseTS.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\close11.png");
            btnMiniTS.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\mini1.png");
            btnCloseTS.Visible = true;
            btnMiniTS.Visible = true;
        }

        private void tmClient_Tick(object sender, EventArgs e)
        {
            if (thoiGianConLai > 1)
            {
                thoiGianConLai--;
                lblThoiGian.Text = thoiGianConLai.ToString();;
            }
            else
            {
                

                if (!string.IsNullOrEmpty(message))
                {
                    string[] spl = message.Split(',');

                    // Đảm bảo mảng có ít nhất 3 phần tử trước khi truy cập spl[2]
                    if (spl.Length > 2 && spl[2] == "playkhamphachiase")
                    {
                        thoiGianConLai--; // Giảm thêm 1 giây nếu điều kiện đúng
                        lblThoiGian.Text = thoiGianConLai.ToString();;
                    }
                    else
                    {
                        tmClient.Enabled = false;
                        lblThoiGian.Text = "END";
                    }
                }
            }
        }


        private void invisibleManHinhChinh()
        {
            /*grbTieuDe.Visible = false;
            grbThongTinDoiChoi.Visible = false;
            grbBangThoiGian.Visible = false;
            grbDiemThi.Visible = false;*/
            //grbManHinhChinh.Visible = false;
            lblTenThiSinh.Visible = false;
            lblTenTruong.Visible = false;
            this.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group6\\gt_qmds.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            
        }

        private void visibleManHinhChinh()
        {
            /*grbTieuDe.Visible = true;
            grbThongTinDoiChoi.Visible = true;
            grbBangThoiGian.Visible = true;
            grbDiemThi.Visible = true;*/
            //grbManHinhChinh.Visible = true;
            this.BackgroundImage = null;
        }

        private void pnlNoiDung_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCloseTS_Click(object sender, EventArgs e)
        {
            try
            {
                // Gửi thông điệp đóng nếu cần
                SendEvent(id.ToString() + ",cli,connected,off");

                // Đảm bảo socket đã khởi tạo và chưa bị dispose
                if (sock != null && sock.Connected)
                {
                    try
                    {
                        sock.Shutdown(SocketShutdown.Both);
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("Socket shutdown error: " + ex.Message);
                    }

                    try
                    {
                        sock.Close();
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("Socket close error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on pbClose: " + ex.Message);
            }
            finally
            {
                // Thoát ứng dụng sau khi đã xử lý mọi thứ
                Application.Exit();
            }
        }

        private void btnMiniTS_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
    }
}
