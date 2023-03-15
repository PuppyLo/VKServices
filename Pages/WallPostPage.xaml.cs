using System.Net;
using VkNet.Model.RequestParams;
using VkNet;
using System.Text;

namespace MauiApp1;

public partial class WallPostPage : ContentPage
{
    VkApi vkapi = new VkApi();

    public static string selectFolder;
    public WallPostPage()
	{
		InitializeComponent();

        SelectFolder_lbl.Text = selectFolder;
    }

private void WallPostBtn_Clicked(object sender, EventArgs e)
    {
        var GroupKyda = Convert.ToInt64(WallPostOwnerID_txt.Text);
        //var CountImage = new DirectoryInfo(Environment.CurrentDirectory).GetFiles().Length;
        var Day = Convert.ToSByte(WallPostDay_txt.Text);
        var Hour = Convert.ToSByte(WallPostHours_txt.Text);

        float addtime;
        DateTime date;

        string[] second = Directory.GetFiles(SelectFolder_lbl.Text); // путь к папке

        var countImg = 125;

        for (int i = 0; i <= second.Length; i++)
        {
            try
            {
                addtime = i + 1;
                addtime += i * 60;
                date = new DateTime();
                date = DateTime.Now.AddDays(Day).AddHours(Hour).AddMinutes(addtime);

                var wc = new WebClient();
                var upServer = vkapi.Photo.GetWallUploadServer(GroupKyda);
                var response = Encoding.ASCII.GetString(wc.UploadFile(upServer.UploadUrl, second[i]));
                var photos = vkapi.Photo.SaveWallPhoto(response, null, (ulong)GroupKyda);

                vkapi.Wall.Post(new WallPostParams
                {
                    OwnerId = -GroupKyda,
                    Attachments = photos,
                    FromGroup = true,
                    PublishDate = date,
                    Copyright = "xentaika.ru",
                    Message = "#xentaika.ru",
                });

                File.Delete(second[i]);

                countImg--;

                Thread.Sleep(500);
            }

            catch 
            {
                if(countImg > 0) {
                    continue;
                }
                else
                {
                    break;
                }
            }
        }
    }
}