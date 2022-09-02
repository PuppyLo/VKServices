using MauiApp1;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VKServices;

public partial class MainPage : ContentPage
{
    VkApi vkapi = new VkApi();

    string groupID = "";
    string token = "";

    private readonly IFolderPicker _folderPicker;

    public MainPage(IFolderPicker folderPicker)
    {
        InitializeComponent();

        groupID = Preferences.Get("GroupID_txt", "Вставьте ИД группы");
        GroupID_txt.Text = groupID;
        token = Preferences.Get("token_txt", "Вставьте ваш токен");
        token_txt.Text = token;

        _folderPicker = folderPicker;
    }

    private void AuthBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            GroupName_txt.TextColor = Colors.Aquamarine;

            vkapi.Authorize(new ApiAuthParams()
            {
                AccessToken = token_txt.Text,
                ApplicationId = 7847742,
                Settings = Settings.All,
            });

            var groups = vkapi.Groups.GetById(null, GroupID_txt.Text, null);

            foreach (var item in groups)
            {
                GroupAVA_img.Source = item.Photo200.AbsoluteUri;
                GroupName_txt.Text = item.Name;
            }

            Preferences.Default.Set("token_txt", token_txt.Text);
            Preferences.Default.Set("GroupID_txt", GroupID_txt.Text);
        }
        catch
        {
            GroupName_txt.TextColor = Colors.Red;
            GroupName_txt.Text = "Вход не выполнене, проверьте введеные данные!";
            GroupAVA_img.Source = @"dotnet_bot.png";
        }


    }

    private void WallPostPageBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new WallPostPage());
    }


    private async void OnPickFolder_Clicked(object sender, EventArgs e)
    {
        var pickedFolder = await _folderPicker.PickFolder();

        WallPostPage.selectFolder = pickedFolder;
    }
}

