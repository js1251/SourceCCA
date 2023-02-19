using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using core;

namespace ui;

public partial class MainWindow : Window, INotifyPropertyChanged {
    public string CopyRightMessage {
        get => _copyRightMessage;
        set {
            _copyRightMessage = value;
            OnPropertyChanged();
        }
    }


    public bool RemoveComments {
        get => _removeComments;
        set {
            _removeComments = value;
            OnPropertyChanged();
        }
    }

    public bool HideTextures {
        get => _hideTextures;
        set {
            _hideTextures = value;
            OnPropertyChanged();
        }
    }

    private string _copyRightMessage;
    private bool _removeComments = true;
    private bool _hideTextures = true;

    public MainWindow() {
        InitializeComponent();

        // TODO: read from settings
        CopyRightMessage = "© 2023 Jakob Sailer";
        HideTextures = true;
        RemoveComments = true;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void DropTarget_OnMouseMove(object sender, MouseEventArgs e) {
        var rectange = (Rectangle)sender;

        if (e.LeftButton != MouseButtonState.Pressed) {
            return;
        }

        DragDrop.DoDragDrop(rectange, rectange.Fill, DragDropEffects.Copy);
    }

    private void DropTarget_OnDrop(object sender, DragEventArgs e) {
        // check if a folder has been dropped
        if (!e.Data.GetDataPresent(DataFormats.FileDrop)) {
            return;
        }

        // get the folder path
        var rootDirInfo = new DirectoryInfo(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);

        // check if the folder exists
        if (!rootDirInfo.Exists) {
            return;
        }

        // protect contents
        ProtectFolderContents(rootDirInfo);

        var message = $"Done adding Copyright message to \n\n{rootDirInfo.FullName}\n\n"
                      + $"You can find it under\n\n{rootDirInfo.FullName}_copyright";
        MessageBox.Show(message, "Done", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ProtectFolderContents(DirectoryInfo rootDirInfo) {
        var protector = new ContentProtector(rootDirInfo) {
            RemoveExistingComments = RemoveComments,
            AddNoToolTexture = HideTextures,
            Comment = CopyRightMessage.Split('\n')
        };

        protector.Protect();
    }
}