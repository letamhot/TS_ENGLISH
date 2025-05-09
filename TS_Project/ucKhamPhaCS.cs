using AxWMPLib;
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

namespace TS_Project
{
    public partial class ucKhamPhaCS : UserControl
    {
        private Socket _socket;
        private int _doiid = 0;
        private int _cauchude = 0, _cauhoiphuid = 0;
        private bool _isStart;
        private bool _isReadyOrOther;
        private bool _tt;
        private string currentPath = Directory.GetCurrentDirectory();
        QuaMienDiSanEntities _entities = new QuaMienDiSanEntities();
        List<ds_goicaudiscovery> lsCauHoiPhuCP = new List<ds_goicaudiscovery>();
        public ucKhamPhaCS()
        {
            InitializeComponent();
        }

        public ucKhamPhaCS(Socket sock, int doiid, int cauchudeid, int cauhoiphuid, bool trangthai, bool start ,bool isReadyOrOther = false)
        {
            InitializeComponent();
            _socket = sock;
            _doiid = doiid;
            _isStart = start;
            _tt = trangthai;
            _cauchude = cauchudeid;
            _cauhoiphuid = cauhoiphuid;
            _isReadyOrOther = isReadyOrOther;
            loadUC();
        }

        private void loadUC()
        {
            if (_cauchude == 0)
            {
                invisibleGui();
            }
            else
            {
                visibleGui();
                ds_goicaudiscovery goi2 = null;
                ds_doi thisinh = null;

                ds_goicaudiscovery cauHoiChinhCP = _entities.ds_goicaudiscovery.Find(_cauchude);
                if (cauHoiChinhCP != null)
                {
                    if (cauHoiChinhCP.cauhoichaid != null)
                    {
                        goi2 = _entities.ds_goicaudiscovery.FirstOrDefault(x => x.cauhoichaid == _cauchude);
                    }
                    else
                    {
                        goi2 = _entities.ds_goicaudiscovery.FirstOrDefault(x => x.cauhoiid == _cauchude);
                    }

                    if (goi2 != null)
                    {
                        thisinh = _entities.ds_doi.Find(goi2.doithiid);

                        if (thisinh != null)
                        {
                            lblCauHoiChinh.Text = "TOPIC: " +cauHoiChinhCP.chude;
                        }
                    }
                }

                // Kiểm tra trạng thái _tt
                if (_tt)
                {
                    onoffCauhoi(true);
                    pBCauHoiChinhCP.Visible = false;
                    if (_cauhoiphuid > 0)
                    {
                        lsCauHoiPhuCP = _entities.ds_goicaudiscovery.Where(x => x.cauhoichaid == cauHoiChinhCP.cauhoiid).ToList();
                        ds_goicaudiscovery cauHoiPhu = lsCauHoiPhuCP.FirstOrDefault(x => x.cauhoiid == _cauhoiphuid);
                        LoadAnhPhuDaLat(_cauchude, thisinh.doiid);
                    }
                }
                else
                {
                    onoffCauhoi(false);
                    pBCauHoiChinhCP.Visible = true;

                    string fileName = cauHoiChinhCP.noidungchude;
                    string imagePath = Path.Combine(currentPath, "Resources", "pic", fileName);
                    string videoPath = Path.Combine(currentPath, "Resources", "Video", fileName);
                    string extension = Path.GetExtension(fileName).ToLower();

                    // Kiểm tra giá trị của isReadyOrOther
                    if (_isReadyOrOther)
                    {
                        // isReadyOrOther = true: Kiểm tra video và phát video nếu có
                        if (extension == ".mp4" || extension == ".avi" || extension == ".mov" || extension == ".wmv" || extension == ".mkv")
                        {
                            if (File.Exists(videoPath))
                            {
                                axWindowsMediaPlayer1.uiMode = "none";
                                axWindowsMediaPlayer1.URL = videoPath;
                                axWindowsMediaPlayer1.Visible = true;
                                axWindowsMediaPlayer1.Ctlcontrols.play();

                            }
                            else
                            {
                                axWindowsMediaPlayer1.Visible = false;
                                axWindowsMediaPlayer1.Ctlcontrols.stop();
                                MessageBox.Show("Không tìm thấy file video: " + videoPath);
                            }
                        }
                        else
                        {
                            // Không phải video, hiển thị hình ảnh
                            if (File.Exists(imagePath))
                            {
                                pBCauHoiChinhCP.BackgroundImage = Image.FromFile(imagePath);
                                pBCauHoiChinhCP.BackgroundImageLayout = ImageLayout.Stretch;
                                pBCauHoiChinhCP.Visible = true;
                                axWindowsMediaPlayer1.Visible = false;
                                axWindowsMediaPlayer1.Ctlcontrols.stop();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy file ảnh: " + imagePath);
                            }
                        }
                    }
                    else
                    {
                        // isReadyOrOther = false: Kiểm tra video và hiển thị video nếu có, không phát
                        if (extension == ".mp4" || extension == ".avi" || extension == ".mov" || extension == ".wmv" || extension == ".mkv")
                        {
                            if (File.Exists(videoPath))
                            {
                                axWindowsMediaPlayer1.URL = videoPath;
                                axWindowsMediaPlayer1.Visible = true;
                                axWindowsMediaPlayer1.Ctlcontrols.stop();  // Không phát video khi isReadyOrOther = false
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy file video: " + videoPath);
                            }
                        }
                        else
                        {
                            // Không phải video, hiển thị hình ảnh
                            if (File.Exists(imagePath))
                            {
                                pBCauHoiChinhCP.BackgroundImage = Image.FromFile(imagePath);
                                pBCauHoiChinhCP.BackgroundImageLayout = ImageLayout.Stretch;
                                pBCauHoiChinhCP.Visible = true;
                                axWindowsMediaPlayer1.Visible = false;
                                axWindowsMediaPlayer1.Ctlcontrols.stop();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy file ảnh: " + imagePath);
                            }
                        }
                    }
                }
            }
        }

        private void LoadAnhPhuDaLat(int cauchude, int doiid)
        {
            var dsAnhDaLat = _entities.ds_goicaudiscovery
                .Where(x => x.cauhoichaid == cauchude && x.doithiid == doiid && x.trangthailatAnhPhu == 1)
                .ToList();

            foreach (var cauHoiPhu in dsAnhDaLat)
            {
                _entities.Entry(cauHoiPhu).Reload(); // ⚠️ Nạp lại từ DB

                string imagePath = Path.Combine(currentPath, "Resources", "pic", cauHoiPhu.noidungchude);
                if (!File.Exists(imagePath)) continue;

                Image img = Image.FromFile(imagePath);
                PictureBox targetPictureBox = null;

                switch (cauHoiPhu.vitri)
                {
                    case 1: targetPictureBox = pbCau1; break;
                    case 2: targetPictureBox = pbCau2; break;
                    case 3: targetPictureBox = pbCau3; break;
                    case 4: targetPictureBox = pbCau4; break;
                    case 5: targetPictureBox = pbCau5; break;
                    case 6: targetPictureBox = pbCau6; break;
                }

                if (targetPictureBox != null)
                {
                    targetPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    targetPictureBox.Image = img;
                    targetPictureBox.Tag = img;
                }
            }
        }

        public void onoffCauhoi(bool onoff)
        {
            pbCau1.Visible = onoff;
            pbCau2.Visible = onoff;
            pbCau3.Visible = onoff;
            pbCau4.Visible = onoff;
            pbCau5.Visible = onoff;
            pbCau6.Visible = onoff;

        }



        private void invisibleGui()
        {
            lblCauHoiChinh.Visible = false;
            pbCau1.Visible = false;
            pbCau2.Visible = false;
            pbCau3.Visible = false;
            pbCau4.Visible = false;
            pbCau5.Visible = false;
            pbCau6.Visible = false;

            onoffCauhoi(true);

           
        }

        private void visibleGui()
        {
            onoffCauhoi(true);

            lblCauHoiChinh.Visible = true;
            pbCau1.Visible = true;
            pbCau2.Visible = true;
            pbCau3.Visible = true;
            pbCau4.Visible = true;
            pbCau5.Visible = true;
            pbCau6.Visible = true;
            //lblGioiThieu.Visible = false;

        }
    }
}
