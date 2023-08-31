using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using Test.Common;

namespace Test.View
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        Controller.Controller controller = new Controller.Controller();

        private void button1_Click(object sender, EventArgs e)
        {
            //IDとPwdを受け取る
            string userid = textBox1.Text;
            string pwd = textBox2.Text;

            //IDとPwd入力チェック
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show(MessageString.NullUserPass);
                return;
            }
            int id = -1;
            if (!int.TryParse(userid, out id))
            {
                MessageBox.Show(MessageString.NotPossible);
                return;
            }
            
            //ボタン押下時の時刻
            DateTime buttonClickTime = DateTime.Now;

            try
            {
                //IDの存在チェック
                if (!controller.IsUserId(id))
                {
                    MessageBox.Show(MessageString.NotUser);
                    return;
                }
                
                //IDとPwdのひもづきデータのチェック
                if (!controller.IsMatchUserPass(id, pwd))
                {
                    MessageBox.Show(MessageString.DifferentPass);
                    controller.UseInsertLogHistory(ConstNums.Ng, id, buttonClickTime);
                    return;
                }
                
                List<DateTime> loginTimesList = controller.LogTimesList(id);
                //ログリストのチェック
                if (controller.CheckLoginTime(loginTimesList))
                {
                    //最後のミスから3分以内かのチェック
                    if (controller.CheckThreeMinutes(loginTimesList, buttonClickTime))
                    {
                        string lockTime = controller.GetLockTime(loginTimesList, buttonClickTime).ToString(@"mm\:ss");
                        MessageBox.Show(MessageString.Unlock + lockTime);
                        return;
                    }
                }

                //ログイン成功
                MessageBox.Show(MessageString.Success);
                controller.UseInsertLogHistory(ConstNums.Ok, id, buttonClickTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageString.Error);
                ErrorLog errorLog = new ErrorLog();
                errorLog.OutPutError(ex, buttonClickTime);
            }
        }
    }
}


