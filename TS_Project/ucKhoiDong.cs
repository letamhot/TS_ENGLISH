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
            Load += ucKhoiDong_Load;
        }

        private void ucKhoiDong_Load(object sender, EventArgs e)
        {
            ds_doi tends = _entities.ds_doi.Find(_doiid);

            if (_goicauhoiid == 0)
            {
                invisibleGui();
                lblThele.Text = "Question packages!";
                // Reset all packages to available state when starting new selection
                ResetAllPackageStates();
            }
            else
            {
                visibleGui();
                lblGioiThieu.Visible = false;
                lblThele.Visible = true;
                lblThele.Text = "Candidate " + tends.tennguoichoi.ToUpper() + " selected package " + _goicauhoiid;

                // Update all package states first
                UpdateAllPackageStates(_ttgoi);

                if (_cauhoiid > 0)
                {
                    labelNoiDungCauHoi.Text = "Question " + _entities.ds_goicauhoikhoidong.Find(_cauhoiid).vitri + ":";
                    lblNoiDungCauHoi.Text = _entities.ds_goicauhoikhoidong.Find(_cauhoiid).noidungcauhoi;
                    // Add to displayed questions cache
                    if (!dsCauHoiDaHienThi.Contains(_cauhoiid))
                    {
                        dsCauHoiDaHienThi.Add(_cauhoiid);
                    }
                }
                else
                {
                    visibleGui1();
                    if (_start)
                    {
                        ds_doi teamnext = _entities.ds_doi.Where(x => x.vitridoi == tends.vitridoi + 1).FirstOrDefault();
                        lblGioiThieu.Text = teamnext != null
                            ? $"Congratulations to candidate {tends.tennguoichoi.ToString().ToUpper()} completed the Warm-up section\nCandidate {teamnext.tennguoichoi.ToString().ToUpper()} preparing for the section"
                            : $"Congratulations to candidate {tends.tennguoichoi.ToString().ToUpper()} has completed the Warm-up section";
                        lblGioiThieu.Visible = true;
                    }
                    else
                    {
                        lblGioiThieu.Visible = false;
                    }
                }

            }
        }
        // Danh sách các ID câu hỏi đã hiển thị trước đó
        private HashSet<int> dsCauHoiDaHienThi = new HashSet<int>();
        private void ResetAllPackageStates()
        {
            // Reset all packages to available state (0)
            for (int i = 0; i < 6; i++)
            {
                SetPackageImage(i + 1, "ac");
            }
            dsCauHoiDaHienThi.Clear();

        }

        private void UpdateAllPackageStates(int[] states)
        {
            for (int i = 0; i < states.Length; i++)
            {
                int packageNumber = i + 1;
                switch (states[i])
                {
                    case 0: // Available
                        SetPackageImage(packageNumber, "ac"); // Available (normal)
                        break;
                    case 1: // Selected/In-progress
                        SetPackageImage(packageNumber, "in"); // Highlighted/selected
                        break;
                    case 2: // Completed
                        SetPackageImage(packageNumber, "dis"); // Disabled/grayed out
                        break;
                }
            }
        }
        private void SetPackageImage(int packageNumber, string state)
        {
            string imagePath = $"{currentPath}\\Resources\\group4\\";

            switch (state)
            {
                case "ac": // Available
                    imagePath += $"{packageNumber}-ac.png";
                    break;
                case "in": // Disabled
                    imagePath += $"{packageNumber}-in.png";
                    break;
                case "dis": // Selected
                    imagePath += $"{packageNumber}-dis.png"; // Or use a different selected image if available
                    break;
            }

            var pictureBox = GetPictureBoxByPackageNumber(packageNumber);
            if (pictureBox != null && File.Exists(imagePath))
            {
                pictureBox.BackgroundImage = Image.FromFile(imagePath);
                pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private PictureBox GetPictureBoxByPackageNumber(int packageNumber)
        {
            switch (packageNumber)
            {
                case 1: return pbGoi1;
                case 2: return pbGoi2;
                case 3: return pbGoi3;
                case 4: return pbGoi4;
                case 5: return pbGoi5;
                case 6: return pbGoi6;
                default: return null;
            }
        }

        private void SendEvent(string str)
        {
            if (_socket == null || !_socket.Connected)
            {
                MessageBox.Show(this, "Must be connected to Send a message");
                return;
            }

            try
            {
                Byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                _socket.Send(byteDateLine, byteDateLine.Length, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Send lenh dieu khien loi!");
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
    }
}
