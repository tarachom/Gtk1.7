using Gtk;

namespace GtkTest;

class FirstWindow : Window
{
    Notebook topNotebook = new Notebook()
    {
        Scrollable = true, //Прокрутка сторінок блокнота
        EnablePopup = true,
        BorderWidth = 0,
        ShowBorder = false,
        TabPos = PositionType.Top
    };

    public FirstWindow() : base("GtkTest")
    {
        SetDefaultSize(1200, 900);
        SetPosition(WindowPosition.Center);
        BorderWidth = 5;

        DeleteEvent += delegate { Program.Quit(); };

        //Основний контейнер
        VBox vBox = new VBox();
        Add(vBox);

        //Кнопки
        {
            HBox hBox = new HBox();
            vBox.PackStart(hBox, false, false, 5);

            Button bAddTree = new Button("Додати TreeView");
            bAddTree.Clicked += (object? sender, EventArgs args) =>
            {
                AddPageTreeView();
            };

            hBox.PackStart(bAddTree, false, false, 2);

            Button bAddLink = new Button("Додати Link");
            bAddLink.Clicked += (object? sender, EventArgs args) =>
            {
                AddPageLinks();
            };

            hBox.PackStart(bAddLink, false, false, 2);

            Button bAddToolbar = new Button("Додати Toolbar");
            bAddToolbar.Clicked += (object? sender, EventArgs args) =>
            {
                AddPageToolBar();
            };

            hBox.PackStart(bAddToolbar, false, false, 2);

            Button bAddListBox = new Button("Додати ListBox");
            bAddListBox.Clicked += (object? sender, EventArgs args) =>
            {
                AddPageListBox();
            };

            hBox.PackStart(bAddListBox, false, false, 2);
        }

        //Блокнот
        {
            HBox hBox = new HBox();
            vBox.PackStart(hBox, true, true, 5);

            hBox.PackStart(topNotebook, true, true, 0);
        }

        AddPage();

        ShowAll();
    }

    void AddPage()
    {
        VBox vBox = new VBox();

        for (int i = 0; i < 30; i++)
        {
            HBox hBox = new HBox();
            vBox.PackStart(hBox, false, false, 5);

            //Поле Назва
            Entry name = new Entry();
            hBox.PackStart(new Label("Назва:"), false, false, 2);
            hBox.PackStart(name, false, false, 2);
        }

        CreateNotebookPage("Сторінка", vBox);
    }

    void AddPageTreeView()
    {
        VBox vBox = new VBox();

        //Заголовок
        HBox hBox1 = new HBox();
        vBox.PackStart(hBox1, false, false, 5);
        hBox1.PackStart(new Label("<b>Табличний список</b>") { UseMarkup = true }, false, false, 5);

        //Список
        HBox hBox2 = new HBox();
        vBox.PackStart(hBox2, true, true, 0);

        Menu PopUpContextMenu()
        {
            Menu menu = new Menu();

            MenuItem open = new MenuItem("Додати");
            menu.Append(open);

            MenuItem close = new MenuItem("Видалити");
            menu.Append(close);

            menu.ShowAll();

            return menu;
        }

        void AddColumns(TreeView TreeViewGrid)
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* Image */
            TreeViewGrid.AppendColumn(new TreeViewColumn("Код", new CellRendererText() { Xpad = 4 }, "text", 1)); /* Код */
            TreeViewGrid.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Xpad = 4 }, "text", 2)); /* Назва */
            TreeViewGrid.AppendColumn(new TreeViewColumn());
        }

        int FillStore(ListStore listStore)
        {
            List<Array> list = new List<Array>();

            var pb = new Gdk.Pixbuf("images/ok.png");

            int random = new Random().Next(10, 100);

            for (int i = 0; i < random; i++)
                list.Add(new object[] { pb, $"{i}", $"Назва {i}" });

            foreach (var item in list)
                listStore.AppendValues(item);

            return random;
        }

        ListStore listStore = new ListStore
        (
            typeof(Gdk.Pixbuf) /* Image */,
            typeof(string)     /* Код */,
            typeof(string)     /* Назва */
        );

        ScrolledWindow scrollTree = new ScrolledWindow() { ShadowType = ShadowType.In, BorderWidth = 0 };
        scrollTree.SetPolicy(PolicyType.Never, PolicyType.Automatic);

        TreeView TreeViewGrid = new TreeView(listStore);
        AddColumns(TreeViewGrid);

        TreeViewGrid.Selection.Mode = SelectionMode.Multiple;
        TreeViewGrid.ActivateOnSingleClick = true;
        TreeViewGrid.ButtonReleaseEvent += (object? sender, ButtonReleaseEventArgs args) =>
        {
            if (args.Event.Button == 3 && TreeViewGrid.Selection.CountSelectedRows() != 0)
                PopUpContextMenu().Popup();
        };

        scrollTree.Add(TreeViewGrid);
        hBox2.PackStart(scrollTree, true, true, 0);

        int numAdd = FillStore(listStore);

        //Підсумок
        HBox hBox3 = new HBox();
        vBox.PackStart(hBox3, false, false, 5);
        hBox3.PackStart(new Label($"<b>Підсумок:</b> додано {numAdd} рядочків") { UseMarkup = true }, false, false, 5);

        CreateNotebookPage("Сторінка TreeView", vBox);
    }

    void AddPageLinks()
    {
        VBox vBox = new VBox();

        for (int i = 0; i < 10; i++)
        {
            HBox hBox = new HBox();
            vBox.PackStart(hBox, false, false, 5);

            //Link
            LinkButton lb = new LinkButton("", $" Link {i}") { Halign = Align.Start, Image = new Image("images/ok.png"), AlwaysShowImage = true };
            hBox.PackStart(lb, false, false, 2);
        }

        CreateNotebookPage("Сторінка Links", vBox);
    }

    void AddPageToolBar()
    {
        VBox vBox = new VBox();

        HBox hBox = new HBox();
        vBox.PackStart(hBox, false, false, 5);

        Toolbar toolbar = new Toolbar();

        ToolButton addButton = new ToolButton(Stock.Add) { Label = "Додати", IsImportant = true, TooltipText = "Додати" };
        toolbar.Add(addButton);

        ToolButton upButton = new ToolButton(Stock.Edit) { Label = "Редагувати", IsImportant = true, TooltipText = "Редагувати" };
        toolbar.Add(upButton);

        ToolButton copyButton = new ToolButton(Stock.Copy) { Label = "Копіювати", IsImportant = true, TooltipText = "Копіювати" };
        toolbar.Add(copyButton);

        ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true, TooltipText = "Видалити" };
        toolbar.Add(deleteButton);

        ToolButton refreshButton = new ToolButton(Stock.Refresh) { Label = "Обновити", IsImportant = true, TooltipText = "Обновити" };
        toolbar.Add(refreshButton);

        hBox.PackStart(toolbar, false, false, 2);

        //Text
        HBox hBoxText = new HBox();
        vBox.PackStart(hBoxText, true, true, 5);

        TextView text = new TextView();

        for (int i = 0; i < 10; i++)
            text.Buffer.Text += @$"
{i}
GtkTextView has a main css node with name textview and style class .view, 
and subnodes for each of the border windows, and the main text area, 
with names border and text, respectively. 
The border nodes each get one of the style classes .left, .right, .top or .bottom.
";

        ScrolledWindow scrol = new ScrolledWindow();
        scrol.KineticScrolling = true;
        scrol.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
        scrol.Add(text);

        hBoxText.PackStart(scrol, true, true, 5);

        CreateNotebookPage("Сторінка Toolbar", vBox);
    }

    void AddPageListBox()
    {
        VBox vBox = new VBox();

        //Заголовок
        HBox hBox1 = new HBox();
        vBox.PackStart(hBox1, false, false, 5);
        hBox1.PackStart(new Label("<b>Cписки</b>") { UseMarkup = true }, false, false, 5);

        HBox hBox = new HBox();
        vBox.PackStart(hBox, false, false, 5);

        int random = new Random().Next(1, 5);

        for (int n = 0; n <= random; n++)
        {
            ListBox listBox = new ListBox();

            ScrolledWindow scroll = new ScrolledWindow() { WidthRequest = 200, HeightRequest = 600, ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);
            scroll.Add(listBox);

            for (int i = 0; i < 100; i++)
            {
                ListBoxRow row = new ListBoxRow();
                row.Add(new Label($"Рядок №<b>{i}</b>") { Halign = Align.Start, UseMarkup = true });

                listBox.Add(row);
            }

            hBox.PackStart(scroll, false, false, 2);
        }

        CreateNotebookPage("Сторінка ListBox", vBox);
    }

    /// <summary>
    /// Створити сторінку в блокноті
    /// </summary>
    /// <param name="tabName">Назва сторінки</param>
    /// <param name="pageWidget">Віджет для сторінки</param>
    /// <param name="insertPage">Вставити сторінку перед поточною</param>
    public void CreateNotebookPage(string tabName, Widget? pageWidget, bool insertPage = false)
    {
        int numPage;
        string codePage = Guid.NewGuid().ToString();

        ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In, Name = codePage };
        scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

        HBox hBoxLabel = CreateLabelPageWidget(tabName, codePage, topNotebook);

        if (insertPage)
            numPage = topNotebook.InsertPage(scroll, hBoxLabel, topNotebook.CurrentPage);
        else
            numPage = topNotebook.AppendPage(scroll, hBoxLabel);

        if (pageWidget != null)
            scroll.Add(pageWidget);

        topNotebook.ShowAll();
        topNotebook.CurrentPage = numPage;
        topNotebook.GrabFocus();
    }

    /// <summary>
    /// Заголовок сторінки блокноту
    /// </summary>
    /// <param name="caption">Заголовок</param>
    /// <param name="codePage">Код сторінки</param>
    /// <param name="notebook">Блокнот</param>
    /// <returns></returns>
    public HBox CreateLabelPageWidget(string caption, string codePage, Notebook notebook)
    {
        HBox hBoxLabel = new HBox();

        Label label = new Label { Text = caption, Expand = false, Halign = Align.Start };
        hBoxLabel.PackStart(label, false, false, 4);

        //Лінк закриття сторінки
        LinkButton lbClose = new LinkButton("Закрити", " ")
        {
            Halign = Align.Start,
            Image = new Image("images/clean.png"),
            AlwaysShowImage = true,
            Name = codePage
        };

        lbClose.Clicked += (object? sender, EventArgs args) =>
        {
            //Пошук сторінки по коду і видалення
            notebook.Foreach(
                (Widget wg) =>
                {
                    if (wg.Name == codePage)
                        notebook.DetachTab(wg);
                });
        };

        hBoxLabel.PackEnd(lbClose, false, false, 0);
        hBoxLabel.ShowAll();

        return hBoxLabel;
    }
}
