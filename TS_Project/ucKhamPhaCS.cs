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
        private bool _tt;
        private string currentPath = Directory.GetCurrentDirectory();
        QuaMienDiSanEntities _entities = new QuaMienDiSanEntities();
        List<ds_goicaudiscovery> lsCauHoiPhuCP = new List<ds_goicaudiscovery>();
        public ucKhamPhaCS()
        {
            InitializeComponent();
        }

        public ucKhamPhaCS(Socket sock, int doiid, int cauchudeid, int cauhoiphuid, bool trangthai, bool start)
        {
            InitializeComponent();
            _socket = sock;
            _doiid = doiid;
            _isStart = start;
            _tt = trangthai;
            _cauchude = cauchudeid;
            _cauhoiphuid = cauhoiphuid;
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
                            lblThele.Text = "EXAM TOPIC CONTENT";
                            lblCauHoiChinh.Text = cauHoiChinhCP.chude;
                        }
                    }
                }

                if (_tt)
                {
                    onoffCauhoi(true);
                    pBCauHoiChinhCP.Visible= false;
                    if (_cauhoiphuid > 0) {
                        lsCauHoiPhuCP = _entities.ds_goicaudiscovery.Where(x => x.cauhoichaid == cauHoiChinhCP.cauhoiid).ToList();
                        ds_goicaudiscovery cauHoiPhu = lsCauHoiPhuCP.FirstOrDefault(x => x.cauhoiid == _cauhoiphuid);
                        LoadAnhPhuDaLat(_cauchude, thisinh.doiid);


                    }

                }
                else
                {
                    onoffCauhoi(false);
                    pBCauHoiChinhCP.Visible = true;

                    if (_isStart)
                    {
                        pBCauHoiChinhCP.Image = Image.FromFile(currentPath + "\\Resources\\pic\\" + cauHoiChinhCP.noidungthisinh);
                        pBCauHoiChinhCP.BackgroundImageLayout = ImageLayout.Stretch;
                        pBCauHoiChinhCP.SizeMode = PictureBoxSizeMode.StretchImage;

                    }
                    else
                    {
                        pBCauHoiChinhCP.Image = Image.FromFile(currentPath + "\\Resources\\pic\\" + cauHoiChinhCP.noidungchude);
                        pBCauHoiChinhCP.BackgroundImageLayout = ImageLayout.Stretch;
                        pBCauHoiChinhCP.SizeMode = PictureBoxSizeMode.StretchImage;

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

            //lblGioiThieu.Visible = true;
            lblThele.Visible = true;
            //lblGioiThieu.Text = "Sẽ có một bức tranh bí ẩn về mảnh đất, con người và các sự kiện lớn của tỉnh Quảng Bình.\nBức tranh gồm 8 mảnh ghép, mỗi mảnh ghép tương ứng với một câu hỏi phụ.\nĐể trả lời đúng nội dung bức tranh, các thí sinh lần lượt lựa chọn mảnh ghép và trả lời sau 10 giây suy nghĩ.\nNếu trả lời đúng, thí sinh sẽ dành 20 điểm/câu; trả lời sai hoặc không có câu trả lời, 01 trong 03 thí sinh còn lại có cơ hội dành quyền trả lời. Nếu thí sinh dành câu hỏi trả lời đúng thì được cộng 20 điểm (thí sinh chọn mảnh ghép sẽ bị trừ 20 điểm), nếu trả lời sai thì bị trừ 10 điểm (thí sinh chọn mảnh ghép không bị trừ  điểm).\nKhi có mảnh ghép đầu tiên mở ra, các thí sinh có quyền bấm chuông để trả lời câu hỏi của bức tranh.\nTrả lời đúng được điểm, trả lời sai thí sinh đó mất quyền thi tiếp phần thi này.Nếu trả lời câu hỏi chính đúng sau khi ô thứ nhất được mở thí sinh đó đạt 80 điểm, nếu trả lời khi ô thứ hai được mở thí sinh đó đạt 70 điểm, khi ô thứ ba mở là 60 điểm, khi ô thứ tư mở là 50 điểm, khi ô thứ năm mở là 40 điểm, khi ô thứ sáu mở là 30 điểm, khi ô thứ bảy mở là 20 điểm, khi ô cuối cùng được mở ra, thí sinh trả lời đúng câu hỏi chính sẽ được 10 điểm.\nĐiểm tối đa cho phần thi này của mỗi thí sinh là 100 điểm.";
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
            lblThele.Visible = true;
            //lblGioiThieu.Visible = false;

        }
    }
}
