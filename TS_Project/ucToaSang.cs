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
using static System.Net.Mime.MediaTypeNames;

namespace TS_Project
{
    public partial class ucToaSang : UserControl
    {
        private Socket _socket;
        private int _doiid = 0;
        private int _cauhoiid = 0;
        private int _goicauhoiid = 0;
        private bool _tt = false;
        private bool _isVideoStart = false;
        private bool _isReady;
        private bool _x2 = false;
        private bool _da = false;
        private bool _trangThaiCau = false;
        private string currentPath = Directory.GetCurrentDirectory();
        QuaMienDiSanEntities _entities = new QuaMienDiSanEntities();
        public ucToaSang()
        {
            InitializeComponent();
        }

        public ucToaSang(Socket sock, int doiid, int cauhoiid, bool isStart, bool x2, bool da, bool trangthai, bool isReady)
        {
            InitializeComponent();
            _socket = sock;
            _doiid = doiid;
            _cauhoiid = cauhoiid;
            _tt = trangthai;
            _isVideoStart = isStart;
            _isReady = isReady;
            _x2 = x2;
            _da = da;
            loadUC();
        }

        private void loadUC()
        {
            if (_cauhoiid == 0)
            {
                VisibleGui();
                lblThele.Text = "Four Candidates will answer five questions";
                // Reset all question states when starting new round
                ResetAllQuestionStates();
            }
            else
            {
                EnabledGui();
                ds_doi doiDangChoi = _entities.ds_doi.Find(_doiid);
                lblThele.Visible = true;

                if (_cauhoiid > 0)
                {


                    ds_goicauhoishining vd = _entities.ds_goicauhoishining.Find(_cauhoiid);
                    _entities.Entry(vd).Reload(); // ⚠️ Nạp lại từ DB

                    disPlayVeDich(_cauhoiid, (int)vd.vitri, _x2);
                    loadNutDangChon(_cauhoiid, _x2);
                    loadNutDaChon(_cauhoiid);
                    // First update all question states based on database
                    UpdateAllQuestionStates(_cauhoiid);
                    lblThele.Text = "Question " + vd.vitri + ": (" + vd.sodiem + " points)";

                    // Rest of your question display logic remains the same...
                    if ((bool)!vd.isvideo)
                    {
                        lblNoiDungCauHoiVD.Visible = true;
                        if (vd.noidungcauhoi.Length > 200)
                        {
                            lblNoiDungCauHoiVD.Font = new Font("Arial", 18,FontStyle.Bold);
                        }
                        else if (vd.noidungcauhoi.Length >= 1 && vd.noidungcauhoi.Length < 30)
                        {
                            lblNoiDungCauHoiVD.Font = new Font("Arial", 24, FontStyle.Bold);

                        }
                        else
                        {
                            lblNoiDungCauHoiVD.Font = new Font("Arial", 20, FontStyle.Bold);

                        }
                        lblNoiDungCauHoiVD.Text = vd.noidungcauhoi;
                        axWinCauHoiHinhAnh.Visible = false;
                        pbImage.Visible = false;

                        if (_da)
                        {
                            pbDapanCH.Visible = true;
                            //pbDACH.Visible = false;
                            lblDA1.Visible = true;
                            //lblDA.Visible = false;
                            lblDA1.Text = vd.dapan;
                            if (vd.dapan.Length > 130)
                            {
                                lblDA1.Font = new Font("Arial", 16, FontStyle.Bold);

                            }
                            else if (vd.dapan.Length > 2 && vd.dapan.Length < 10)
                            {
                                lblDA1.Text = vd.dapan;
                                lblDA1.Font = new Font("Arial", 26, FontStyle.Bold);
                            }
                            else
                            {
                                lblDA1.Text = vd.dapan;
                                lblDA1.Font = new Font("Arial", 22, FontStyle.Bold);
                            }
                        }
                        else
                        {
                            //pbDACH.Visible = false;
                            pbDapanCH.Visible = false;
                            //lblDA.Visible = false;
                            lblDA1.Visible = false;
                        }
                    }
                    else
                    {
                        if (_tt)
                        {
                            if (vd.urlhinhanh != null && vd.urlhinhanh != "")
                            {
                                var url = vd.urlhinhanh.Split('.');
                                if (url.Length > 0)
                                {
                                    if (url[1] == "png" || url[1] == "jpg")
                                    {
                                        pbImage.Visible = true;
                                        axWinCauHoiHinhAnh.Visible = false;

                                        pbImage.BackgroundImage = System.Drawing.Image.FromFile(currentPath + "\\Resources\\pic\\" + vd.urlhinhanh);
                                        pbImage.BackgroundImageLayout = ImageLayout.Stretch;
                                        if (_da)
                                        {
                                            //pbDACH.Visible = false;
                                            pbDapanCH.Visible = true;
                                            //lblDA.Visible = false;
                                            lblDA1.Visible = true;
                                            var dapan = vd.dapan.Trim();
                                            lblDA1.Text = vd.dapan;
                                            if (vd.dapan.Length > 130)
                                            {
                                                lblDA1.Font = new Font("Arial", 16, FontStyle.Bold);

                                            }
                                            else if (vd.dapan.Length > 2 && vd.dapan.Length < 10)
                                            {
                                                lblDA1.Text = vd.dapan;
                                                lblDA1.Font = new Font("Arial", 26, FontStyle.Bold);
                                            }
                                            else
                                            {
                                                lblDA1.Text = vd.dapan;
                                                lblDA1.Font = new Font("Arial", 22, FontStyle.Bold);
                                            }
                                        }
                                        else
                                        {
                                            //pbDACH.Visible = false;
                                            pbDapanCH.Visible = false;
                                            lblDA1.Visible = false;
                                            //lblDA.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        if (_isVideoStart)
                                        {
                                            axWinCauHoiHinhAnh.Visible = true;
                                            axWinCauHoiHinhAnh.URL = currentPath + "\\Resources\\Video\\" + vd.urlhinhanh;
                                            axWinCauHoiHinhAnh.Ctlcontrols.play();
                                        }
                                        else
                                        {
                                            axWinCauHoiHinhAnh.Visible = false;
                                            axWinCauHoiHinhAnh.Ctlcontrols.stop();
                                        }

                                        if (_da)
                                        {
                                            axWinCauHoiHinhAnh.Visible = false;
                                            axWinCauHoiHinhAnh.Ctlcontrols.stop();                                            //pbDACH.Visible = false;
                                            pbDapanCH.Visible = true;
                                            //lblDA.Visible = false;
                                            lblDA1.Visible = true;
                                            lblDA1.Text = vd.dapan;
                                            if (vd.dapan.Length > 130)
                                            {
                                                lblDA1.Font = new Font("Arial", 16, FontStyle.Bold);

                                            }
                                            else if (vd.dapan.Length > 2 && vd.dapan.Length < 10)
                                            {
                                                lblDA1.Text = vd.dapan;
                                                lblDA1.Font = new Font("Arial", 26, FontStyle.Bold);
                                            }
                                            else
                                            {
                                                lblDA1.Text = vd.dapan;
                                                lblDA1.Font = new Font("Arial", 22, FontStyle.Bold);
                                            }
                                        }
                                        else
                                        {
                                            axWinCauHoiHinhAnh.Visible = true;
                                            //pbDACH.Visible = false;
                                            pbDapanCH.Visible = false;
                                            lblDA1.Visible = false;
                                            //lblDA.Visible = false;
                                        }
                                    }

                                }
                            }
                        }
                        lblNoiDungCauHoiVD.Visible = true;
                        if (vd.noidungcauhoi.Length > 200)
                        {
                            lblNoiDungCauHoiVD.Font = new Font("Arial", 18, FontStyle.Bold);
                        }
                        else if (vd.noidungcauhoi.Length >= 1 && vd.noidungcauhoi.Length < 30)
                        {
                            lblNoiDungCauHoiVD.Font = new Font("Arial", 24, FontStyle.Bold);

                        }
                        else
                        {
                            lblNoiDungCauHoiVD.Font = new Font("Arial", 20, FontStyle.Bold);

                        }
                        lblNoiDungCauHoiVD.Text = vd.noidungcauhoi;

                        if (vd.urlhinhanh != null && vd.urlhinhanh != "")
                        {
                            var url = vd.urlhinhanh.Split('.');
                            if (url.Length > 0)
                            {
                                if (url[1] == "png" || url[1] == "jpg")
                                {
                                    pbImage.Visible = true;
                                    axWinCauHoiHinhAnh.Visible = false;

                                    pbImage.BackgroundImage = System.Drawing.Image.FromFile(currentPath + "\\Resources\\pic\\" + vd.urlhinhanh);
                                    pbImage.BackgroundImageLayout = ImageLayout.Stretch;

                                    if (_da)
                                    {
                                        //pbDACH.Visible = false;
                                        pbDapanCH.Visible = true;
                                        //lblDA.Visible = false;
                                        lblDA1.Visible = true;
                                        lblDA1.Text = vd.dapan;
                                        if (vd.dapan.Length > 130)
                                        {
                                            lblDA1.Font = new Font("Arial", 16, FontStyle.Bold);

                                        }
                                        else if (vd.dapan.Length > 2 && vd.dapan.Length < 10)
                                        {
                                            lblDA1.Text = vd.dapan;
                                            lblDA1.Font = new Font("Arial", 26, FontStyle.Bold);
                                        }
                                        else
                                        {
                                            lblDA1.Text = vd.dapan;
                                            lblDA1.Font = new Font("Arial", 22, FontStyle.Bold);
                                        }
                                    }
                                    else
                                    {
                                        //pbDACH.Visible = false;
                                        pbDapanCH.Visible = false;
                                        lblDA1.Visible = false;
                                        //lblDA.Visible = false;
                                    }

                                }
                                else
                                {

                                    pbImage.Visible = false;

                                    if (_isVideoStart)
                                    {
                                        axWinCauHoiHinhAnh.Visible = true;
                                        axWinCauHoiHinhAnh.URL = currentPath + "\\Resources\\Video\\" + vd.urlhinhanh;
                                        axWinCauHoiHinhAnh.Ctlcontrols.play();
                                        axWinCauHoiHinhAnh.Ctlenabled = false;


                                        if (_da)
                                        {
                                            axWinCauHoiHinhAnh.Visible = false;
                                            //pbDACH.Visible = false;
                                            pbDapanCH.Visible = true;
                                            //lblDA.Visible = false;
                                            lblDA1.Visible = true;
                                            lblDA1.Text = vd.dapan;
                                            if (vd.dapan.Length > 130)
                                            {
                                                lblDA1.Font = new Font("Arial", 16, FontStyle.Bold);

                                            }
                                            else if (vd.dapan.Length > 2 && vd.dapan.Length < 10)
                                            {
                                                lblDA1.Text = vd.dapan;
                                                lblDA1.Font = new Font("Arial", 26, FontStyle.Bold);
                                            }
                                            else
                                            {
                                                lblDA1.Text = vd.dapan;
                                                lblDA1.Font = new Font("Arial", 22, FontStyle.Bold);
                                            }
                                        }
                                        else
                                        {
                                            axWinCauHoiHinhAnh.Visible = true;
                                            //pbDACH.Visible = false;
                                            pbDapanCH.Visible = false;
                                            lblDA1.Visible = false;
                                            //lblDA.Visible = false;
                                        }

                                    }
                                    else
                                    {
                                        axWinCauHoiHinhAnh.Visible = false;
                                        axWinCauHoiHinhAnh.URL = currentPath + "\\Resources\\Video\\" + vd.urlhinhanh;
                                        axWinCauHoiHinhAnh.Ctlcontrols.stop();
                                        if (_da)
                                        {
                                            axWinCauHoiHinhAnh.Visible = false;
                                            //pbDACH.Visible = false;
                                            pbDapanCH.Visible = true;
                                            //lblDA.Visible = false;
                                            lblDA1.Visible = true;
                                            lblDA1.Text = vd.dapan;
                                            if (vd.dapan.Length > 130)
                                            {
                                                lblDA1.Font = new Font("Arial", 16, FontStyle.Bold);

                                            }
                                            else if (vd.dapan.Length > 2 && vd.dapan.Length < 10)
                                            {
                                                lblDA1.Text = vd.dapan;
                                                lblDA1.Font = new Font("Arial", 26, FontStyle.Bold);
                                            }
                                            else
                                            {
                                                lblDA1.Text = vd.dapan;
                                                lblDA1.Font = new Font("Arial", 22, FontStyle.Bold);
                                            }
                                        }
                                        else
                                        {
                                            axWinCauHoiHinhAnh.Visible = false;
                                            pbImage.Visible = false;
                                            //pbDACH.Visible = false;
                                            pbDapanCH.Visible = false;
                                            lblDA1.Visible = false;
                                            //lblDA.Visible = false;
                                        }


                                    }


                                }
                            }

                        }






                    }

                }
                else
                {
                    EnabledGui1();
                    pbDapanCH.Visible = false;
                    lblDA1.Visible = false;
                }
            }
        }
        // Danh sách các ID câu hỏi đã hiển thị trước đó
        private HashSet<int> dsCauHoiDaHienThi = new HashSet<int>();
        private void ResetAllQuestionStates()
        {
            // Reset all picture boxes to default state
            pbGoi1.BackgroundImage = System.Drawing.Image.FromFile(currentPath + "\\Resources\\group4\\1-ac.png");
            pbGoi2.BackgroundImage = System.Drawing.Image.FromFile(currentPath + "\\Resources\\group4\\2-ac.png");
            pbGoi3.BackgroundImage = System.Drawing.Image.FromFile(currentPath + "\\Resources\\group4\\3-ac.png");
            pbGoi4.BackgroundImage = System.Drawing.Image.FromFile(currentPath + "\\Resources\\group4\\4-ac.png");
            pbGoi5.BackgroundImage = System.Drawing.Image.FromFile(currentPath + "\\Resources\\group4\\5-ac.png");
            // Clear the displayed questions cache
            dsCauHoiDaHienThi.Clear();
        }

        private void UpdateAllQuestionStates(int cauhoiid)
        {
            // Get all questions from database
            var allQuestions = _entities.ds_goicauhoishining.Where(x =>x.cauhoiid == cauhoiid).ToList();

            foreach (var question in allQuestions)
            {
                _entities.Entry(question).Reload(); // Nạp lại từng dòng mới nhất từ DB

                switch (question.trangThai)
                {
                    case 0: // Not selected - available
                        SetQuestionImage((int)question.vitri, "ac");
                        break;
                    case 1: // Currently selected
                        SetQuestionImage((int)question.vitri, _x2 ? "star" : "in");
                        break;
                    case 2: // Already answered - disabled
                        SetQuestionImage((int)question.vitri, _x2 ? "star" : "dis");
                        break;
                }
            }
        }

        private void SetQuestionImage(int vitri, string state)
        {
            string imagePath = $"{currentPath}\\Resources\\group4\\{vitri}-{state}.png";
            switch (vitri)
            {
                case 1:
                    pbGoi1.BackgroundImageLayout = ImageLayout.Stretch;
                    pbGoi1.BackgroundImage = System.Drawing.Image.FromFile(imagePath);
                    break;
                case 2:
                    pbGoi2.BackgroundImageLayout = ImageLayout.Stretch;
                    pbGoi2.BackgroundImage = System.Drawing.Image.FromFile(imagePath);
                    break;
                case 3:
                    pbGoi3.BackgroundImageLayout = ImageLayout.Stretch;
                    pbGoi3.BackgroundImage = System.Drawing.Image.FromFile(imagePath);
                    break;
                case 4:
                    pbGoi4.BackgroundImageLayout = ImageLayout.Stretch;
                    pbGoi4.BackgroundImage = System.Drawing.Image.FromFile(imagePath);
                    break;
                case 5:
                    pbGoi5.BackgroundImageLayout = ImageLayout.Stretch;
                    pbGoi5.BackgroundImage = System.Drawing.Image.FromFile(imagePath);
                    break;
            }
        }

        private PictureBox GetPictureBoxByVitri(int vitri)
        {
            switch (vitri)
            {
                case 1: return pbGoi1;
                case 2: return pbGoi2;
                case 3: return pbGoi3;
                case 4: return pbGoi4;
                case 5: return pbGoi5;
                default: return null;
            }
        }

        void disPlayVeDich(int cauhoiid, int vitri, bool isX2)
        {
            var cauhoiTS = _entities.ds_goicauhoishining.Find(cauhoiid);
            _entities.Entry(cauhoiTS).Reload(); // Nạp lại từng dòng mới nhất từ DB

            if (cauhoiTS != null && cauhoiTS.vitri == vitri)
            {
                // Always update the display based on current state
                switch (cauhoiTS.trangThai)
                {
                    case 1: // Currently selected
                        SetQuestionImage(vitri, isX2 ? "star" : "in");
                        break;
                    case 2: // Already answered
                        SetQuestionImage(vitri, "dis");
                        break;
                    default: // Not selected
                        SetQuestionImage(vitri, "ac");
                        break;
                }

                // Add to displayed questions cache
                if (!dsCauHoiDaHienThi.Contains(cauhoiid))
                {
                    dsCauHoiDaHienThi.Add(cauhoiid);
                }
            }
        }

        private void loadNutDangChon(int cauhoiid, bool isX2)
        {
            var dsCauChon = _entities.ds_goicauhoishining
                .Where(x => x.cauhoiid == cauhoiid && x.trangThai == 1)
                .ToList();

            foreach (var cauHoi in dsCauChon)
            {
                _entities.Entry(cauHoi).Reload(); // Nạp lại từng dòng mới nhất từ DB

                SetQuestionImage((int)cauHoi.vitri, isX2 ? "star" : "in");
            }
        }

        private void loadNutDaChon(int cauhoiid)
        {
            var dsCauDaChon = _entities.ds_goicauhoishining
                .Where(x => x.cauhoiid == cauhoiid && x.trangThai == 2)
                .ToList();

            foreach (var cauHoi in dsCauDaChon)
            {
                _entities.Entry(cauHoi).Reload(); // Nạp lại từng dòng mới nhất từ DB

                SetQuestionImage((int)cauHoi.vitri, _x2 ? "star" : "dis");
            }
        }

        public void VisibleGui()
        {
            //lblGioiThieu.Visible = true;
            //lblThele.Visible = true;
            lblNoiDungCauHoiVD.Visible = false;
            //pbDACH.Visible = false;
            //lblDA.Visible = false;
            pbDapanCH.Visible = false;
            lblDA1.Visible = false;
            /*pbCauHoi20_2.Visible = false;*/
            pbGoi1.Visible = true;
            pbGoi2.Visible = true;
            pbGoi3.Visible = true;
            pbGoi4.Visible = true;
            pbGoi5.Visible = true;
            //pbGoi6.Visible = true;

        }
        public void EnabledGui()
        {

            //lblGioiThieu.Visible = false;
            lblThele.Visible = true;
            lblNoiDungCauHoiVD.Visible = true;
            /*pbCauHoi20_2.Visible = true;*/
            pbGoi1.Visible = true;
            pbGoi2.Visible = true;
            pbGoi3.Visible = true;
            pbGoi4.Visible = true;
            pbGoi5.Visible = true;
            //pbGoi6.Visible = true;

        }
        public void EnabledGui1()
        {
            //lblGioiThieu.Visible = false;
            lblThele.Visible = true;
            lblNoiDungCauHoiVD.Visible = false;
            pbGoi1.Visible = true;
            pbGoi2.Visible = true;
            pbGoi3.Visible = true;
            pbGoi4.Visible = true;
            pbGoi5.Visible = true;
            //pbGoi6.Visible = true;

        }
    }
}
