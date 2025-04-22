using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TS_Project.Model;

delegate void AddMessage(string sNewMessage);

namespace TS_Project
{
    public partial class ucKhoiDong : UserControl
    {
        private Socket _socket;
        private int _doiid = 0;
        private int _cauhoiid = 0;
        private int _goicauhoiid = 0;
        private bool _start;
        private int[] _ttgoi;
        private string currentPath = Directory.GetCurrentDirectory();
        QuaMienDiSanEntities _entities = new QuaMienDiSanEntities();
        public ucKhoiDong()
        {
            InitializeComponent();
        }

        public ucKhoiDong(Socket sock, int doiid, int cauhoiid, int goicauhoiid, int[] ttgoi, bool start)
        {
            InitializeComponent();
            _socket = sock;
            _doiid = doiid;
            _cauhoiid = cauhoiid;
            _goicauhoiid = goicauhoiid;
            _ttgoi = ttgoi;
            _start = start;
        }

        private void SendEvent(string str)
        {
            // Check we are connected
            if (_socket == null || !_socket.Connected)
            {
                MessageBox.Show(this, "Must be connected to Send a message");
                return;
            }
            // Read the message from the text box and send it
            try
            {
                // Convert to byte array and send.
                Byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                _socket.Send(byteDateLine, byteDateLine.Length, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Send lenh dieu khien loi!");
            }

        }


        private void ucKhoiDong_Load(object sender, EventArgs e)
        {
            ds_doi tends = _entities.ds_doi.Find(_doiid);
            if (_goicauhoiid == 0)
            {

                invisibleGui();
                lblThele.Text = "Please invite candidate " + tends.tennguoichoi.ToUpper() + " choose question package!";
                
            }
            else
            {
                visibleGui();
                lblGioiThieu.Visible = false;
                lblThele.Visible = true;
                lblThele.Text = "Candidate " + tends.tennguoichoi.ToUpper() + " selected package " + _goicauhoiid;
                if (_cauhoiid > 0)
                {
                    labelNoiDungCauHoi.Text = "Question number " + _entities.ds_goicauhoikhoidong.Find(_cauhoiid).vitri + ":";
                    lblNoiDungCauHoi.Text = _entities.ds_goicauhoikhoidong.Find(_cauhoiid).noidungcauhoi;
                }
                else
                {

                    visibleGui1();
                    if (_start == true)
                    {
                        ds_doi teamnext = _entities.ds_doi.Where(x => x.vitridoi == tends.vitridoi + 1).FirstOrDefault();
                        if(teamnext != null)
                        {
                            lblGioiThieu.Text = "Congratulations to candidate " + tends.tennguoichoi.ToString().ToUpper() + " completed the Warm-up section\nCandidate " + teamnext.tennguoichoi.ToString().ToUpper() + " preparing for the section";
                        }
                        else
                        {
                            lblGioiThieu.Text = "Congratulations to candidate " + tends.tennguoichoi.ToString().ToUpper() + " has completed the Warm-up section";
                        }
                        lblGioiThieu.Visible = true;
                    }
                    else
                    {
                        lblGioiThieu.Visible = false;
                    }
                }
                disableButton(_ttgoi);
                selectedButton(_ttgoi);
            }
        }
        private void invisibleGui()
        {
            lblThele.Visible = true;
            lblNoiDungCauHoi.Visible = false;
            labelNoiDungCauHoi.Visible = false;
           

        }
        private void visibleGui1()
        {

            lblNoiDungCauHoi.Visible = false;
            labelNoiDungCauHoi.Visible = false;
           
        }
        private void visibleGui()
        {
            lblNoiDungCauHoi.Visible = true;
            labelNoiDungCauHoi.Visible = true;
          
        }
        private void selectedButton(int[] _ttgoi)
        {
            if (_ttgoi[0] == 2)
            {
                pbGoi1.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\Group_221.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;

            }
            if (_ttgoi[1] == 2)
            {
                pbGoi2.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\Group_222.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;

            }
            if (_ttgoi[2] == 2)
            {
                pbGoi3.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\Group_223.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;

            }
            if (_ttgoi[3] == 2)
            {
                pbGoi4.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\Group_224.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;

            }
            if (_ttgoi[4] == 2)
            {
                pbGoi5.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\Group_225.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;

            }
            if (_ttgoi[5] == 2)
            {
                pbGoi6.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\Group_226.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;

            }
        }

        private void disableButton(int[] _ttgoi)
        {
            if (_ttgoi[0] == 1)
            {
                pbGoi1.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\kd1_in.png");
                pbGoi1.BackgroundImageLayout = ImageLayout.Stretch;

            }
            if (_ttgoi[1] == 1)
            {
                pbGoi2.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\kd2_in.png");
                pbGoi2.BackgroundImageLayout = ImageLayout.Stretch;

            }
            if (_ttgoi[2] == 1)
            {
                pbGoi3.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\kd3_in.png");
                pbGoi3.BackgroundImageLayout = ImageLayout.Stretch;

            }
            if (_ttgoi[3] == 1)
            {
                pbGoi4.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\kd4_in.png");
                pbGoi4.BackgroundImageLayout = ImageLayout.Stretch;

            }
            if (_ttgoi[4] == 1)
            {
                pbGoi5.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\kd5_in.png");
                pbGoi5.BackgroundImageLayout = ImageLayout.Stretch;

            }
            if (_ttgoi[5] == 1)
            {
                pbGoi6.BackgroundImage = Image.FromFile(currentPath + "\\Resources\\group4\\kd6_in.png");
                pbGoi6.BackgroundImageLayout = ImageLayout.Stretch;

            }
        }

    }
}
