Imports System.Windows.Interop
Imports System.Runtime.InteropServices
Imports IWshRuntimeLibrary '引用 WSH COM 类库
Imports Microsoft.Win32
Imports System.IO

Public Class MainWindow

    REM ------------------拖动窗口------------------
    Private Const WM_SYSCOMMAND As Integer = &H112 '移动和调整都用到
    Private _HwndSource As HwndSource

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(hWnd As IntPtr, Msg As UInteger, wParam As IntPtr, lParam As IntPtr) As IntPtr
    End Function

    Public Sub mydelegate(sender As Object, e As EventArgs)
        Me._HwndSource = CType(PresentationSource.FromVisual(CType(sender, Visual)), HwndSource)
    End Sub

    '移动
    Public Const HTCAPTION = &H2
    Public Const SC_MOVE = &HF010

    <DllImport("user32.dll")> Private Shared Function ReleaseCapture() As Integer
    End Function

    <DllImport("user32")> Private Shared Function AnimateWindow(ByVal hwnd As Integer, ByVal dwTime As Integer, ByVal dwFlags As Integer) As Boolean
    End Function

    Sub move()
        ReleaseCapture()
        SendMessage(_HwndSource.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0)
    End Sub
    REM ------------------拖动窗口(END)------------------

    REM ------------------程序加载------------------
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        REM 版本号
        UI_Version.Text = "v" & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision
        REM 初始化自然数组
        NB_AbundantArray(0) = "A_redbird"
        NB_AbundantArray(1) = "N_flower_1"
        NB_OccasionalArray(0) = "G_flint"
        NB_OccasionalArray(1) = "F_carrot"
        NB_OccasionalArray(2) = "N_berry_bush_1"
        NB_OccasionalArray(3) = "N_evergreen_1"
        NB_OccasionalArray(4) = "N_grass"
        NB_OccasionalArray(5) = "N_sapling"
        NB_OccasionalArray(6) = "A_bee"
        NB_OccasionalArray(7) = "N_beehive"
        NB_OccasionalArray(8) = "A_butterfly"
        NB_OccasionalArray(9) = "A_fireflies"
        NB_OccasionalArray(10) = "N_pond"
        NB_OccasionalArray(11) = "A_rabbit"
        NB_OccasionalArray(12) = "N_rabbit_hole"
        NB_RareArray(0) = "A_spider"
        NB_RareArray(1) = "N_spider_den"
        NB_RareArray(2) = "N_red_mushroom"
        NB_RareArray(3) = "A_killer_bee"
        NB_RareArray(4) = "N_killer_bee_hive"
        REM 初始化物品栏数组
        GM_ScienceArray(0) = "S_healing_salve"
        GM_AnimalArray(0) = "A_red_hound"
        REM 读取注册表
        Dim RegReadBG As String = Reg_Read_string("Background")
        Dim RegReadAlpha As Integer = Reg_Read("PanelAlphaValue")
        Dim RegReadC As Integer = Reg_Read("Character")
        Dim RegReadF As Integer = Reg_Read("Food")
        Dim RegReadS As Integer = Reg_Read("Science")
        Dim RegReadCS As Integer = Reg_Read("Cooking_Simulator")
        Dim RegReadCS_CrockPot As Integer = Reg_Read("CS_PortableCrockPot")
        Dim RegReadCS_AutoCalculation As Integer = Reg_Read("CS_AutoCalculation")
        Dim RegReadCS_AutoClean As Integer = Reg_Read("CS_AutoClean")
        Dim RegReadA As Integer = Reg_Read("Animal")
        Dim RegReadG As Integer = Reg_Read("Goods")

        If RegReadAlpha = 0 Then
            RegReadAlpha = 60
        Else
            RegReadAlpha -= 1
        End If
        Setting_slider_Alpha.Value = RegReadAlpha

        Dim LeftCanvas() As Canvas = {Canvas_CharacterLeft, Canvas_CharacterLeft_Wolfgang, Canvas_FoodLeft, Canvas_ScienceLeft, Canvas_CookingSimulatorLeft, Canvas_AnimalLeft, Canvas_AnimalLeft_Krampus, Canvas_AnimalLeft_Apackim_baggims, Canvas_AnimalLeft_PigKing, Canvas_AnimalLeft_Yaarctopus, Canvas_NaturalLeft_B, Canvas_NaturalLeft_P, Canvas_GoodsLeft_M, Canvas_GoodsLeft_E, Canvas_GoodsLeft_S, Canvas_GoodsLeft_A, Canvas_GoodsLeft_T, Canvas_GoodsLeft_P, Canvas_GoodsLeft_PL, Canvas_GoodsLeft_U, Canvas_GoodsLeft_C, Canvas_GoodsLeft_B, Canvas_GoodsLeft_BFT, Canvas_GoodsLeft_MIAB, Canvas_Setting}
        Dim RightWrapPanel() As WrapPanel = {WrapPanel_Character, WrapPanel_Food, WrapPanel_Science, WrapPanel_CookingSimulator, WrapPanel_Animal, WrapPanel_Natural, WrapPanel_Goods}
        If RegReadBG <> "" Then
            Dim brush As New ImageBrush
            brush.ImageSource = New BitmapImage(New Uri(RegReadBG))
            BackGroundBorder.Background = brush
            For i = 0 To LeftCanvas.Length - 1
                LeftCanvas(i).Background.Opacity = PanelAlpha
            Next
            For i = 0 To RightWrapPanel.Length - 1
                RightWrapPanel(i).Background.Opacity = PanelAlpha
            Next
        Else
            For i = 0 To LeftCanvas.Length - 1
                LeftCanvas(i).Background.Opacity = 1
            Next
            For i = 0 To RightWrapPanel.Length - 1
                RightWrapPanel(i).Background.Opacity = 1
            Next
            Se_TextBlock_Alpha.Foreground = Brushes.Silver
            Setting_slider_Alpha.IsEnabled = False
        End If

        If RegReadC = 0 Then
            RegReadC = 7
            Reg_Write("Character", 7)
        End If
        If RegReadF = 0 Then
            RegReadF = 7
            Reg_Write("Food", 7)
        End If
        If RegReadS = 0 Then
            RegReadS = 7
            Reg_Write("Science", 7)
        End If
        If RegReadCS = 0 Then
            RegReadCS = 7
            Reg_Write("Cooking_Simulator", 7)
        End If
        If RegReadA = 0 Then
            RegReadA = 7
            Reg_Write("Animal", 7)
        End If
        If RegReadG = 0 Then
            RegReadG = 7
            Reg_Write("Goods", 7)
        End If

        F_DLC_Check_initialization()
        S_DLC_Check_initialization()
        CS_DLC_Check_initialization()
        A_DLC_Check_initialization()
        G_DLC_Check_initialization()

        Select Case RegReadC
            Case 5
                checkBox_C_DLC_SW.IsChecked = False
                button_C_Walani.Visibility = Visibility.Collapsed
                button_C_warly.Visibility = Visibility.Collapsed
                button_C_wilbur.Visibility = Visibility.Collapsed
                button_C_woodlegs.Visibility = Visibility.Collapsed
                WrapPanel_Character.Height = 870
            Case 7
                button_C_Walani.Visibility = Visibility.Visible
                button_C_warly.Visibility = Visibility.Visible
                button_C_wilbur.Visibility = Visibility.Visible
                button_C_woodlegs.Visibility = Visibility.Visible
                WrapPanel_Character.Height = 1075
        End Select

        Select Case RegReadF
            Case 1
                F_DLC_ROG_SHOW()
                WrapPanel_F_recipe.Height = 250
                WrapPanel_F_meats.Height = 170
                WrapPanel_F_others.Height = 90
                WrapPanel_F_no_fc.Height = 330
                WrapPanel_Food.Height = 1500
            Case 2
                F_DLC_SW_SHOW()
                WrapPanel_F_recipe.Height = 410
                WrapPanel_F_meats.Height = 250
                WrapPanel_F_others.Height = 90
                WrapPanel_F_no_fc.Height = 330
                WrapPanel_Food.Height = 1820
            Case 30
                F_DLC_ROG_SHOW()
                F_DLC_SW_SHOW()
                WrapPanel_F_recipe.Height = 410
                WrapPanel_F_meats.Height = 330
                WrapPanel_F_others.Height = 170
                WrapPanel_F_no_fc.Height = 410
                WrapPanel_Food.Height = 1980
            Case 4
                F_DLC_DST_SHOW()
                WrapPanel_F_recipe.Height = 250
                WrapPanel_F_meats.Height = 170
                WrapPanel_F_others.Height = 90
                WrapPanel_F_no_fc.Height = 330
                WrapPanel_Food.Height = 1500
            Case 5
                F_DLC_ROG_SHOW()
                F_DLC_DST_SHOW()
                WrapPanel_F_recipe.Height = 250
                WrapPanel_F_meats.Height = 170
                WrapPanel_F_others.Height = 90
                WrapPanel_F_no_fc.Height = 330
                WrapPanel_Food.Height = 1500
            Case 6
                F_DLC_SW_SHOW()
                F_DLC_DST_SHOW()
                WrapPanel_F_recipe.Height = 410
                WrapPanel_F_meats.Height = 330
                WrapPanel_F_others.Height = 170
                WrapPanel_F_no_fc.Height = 410
                WrapPanel_Food.Height = 1980
            Case 7
                F_DLC_ROG_SHOW()
                F_DLC_SW_SHOW()
                F_DLC_DST_SHOW()
                WrapPanel_F_recipe.Height = 410
                WrapPanel_F_meats.Height = 330
                WrapPanel_F_others.Height = 170
                WrapPanel_F_no_fc.Height = 410
                WrapPanel_Food.Height = 1980
        End Select

        Select Case RegReadS
            Case 1
                checkBox_S_DLC_SW.IsChecked = False
                checkBox_S_DLC_DST.IsChecked = False
                S_DLC_ROG_SHOW()
                WrapPanel_S_Tools.Height = 90
                WrapPanel_S_light.Height = 90
                WrapPanel_S_survival.Height = 170
                WrapPanel_S_fight.Height = 170
                WrapPanel_S_structures.Height = 170
                WrapPanel_S_refine.Height = 90
                WrapPanel_S_dress.Height = 250
                WrapPanel_S_ancient.Height = 170
                WrapPanel_Science.Height = 2223.8
            Case 2
                checkBox_S_DLC_ROG.IsChecked = False
                checkBox_S_DLC_DST.IsChecked = False
                S_DLC_SW_SHOW()
                WrapPanel_S_Tools.Height = 90
                WrapPanel_S_light.Height = 90
                WrapPanel_S_nautical.Height = 250
                WrapPanel_S_survival.Height = 250
                WrapPanel_S_fight.Height = 170
                WrapPanel_S_structures.Height = 170
                WrapPanel_S_refine.Height = 90
                WrapPanel_S_dress.Height = 170
                WrapPanel_S_volcano.Height = 90
                WrapPanel_Science.Height = 2409.2
            Case 3
                checkBox_S_DLC_DST.IsChecked = False
                S_DLC_ROG_SHOW()
                S_DLC_SW_SHOW()
                WrapPanel_S_Tools.Height = 90
                WrapPanel_S_light.Height = 170
                WrapPanel_S_nautical.Height = 250
                WrapPanel_S_survival.Height = 250
                WrapPanel_S_fight.Height = 250
                WrapPanel_S_structures.Height = 170
                WrapPanel_S_refine.Height = 90
                WrapPanel_S_dress.Height = 250
                WrapPanel_S_ancient.Height = 170
                WrapPanel_S_volcano.Height = 90
                WrapPanel_Science.Height = 2889.6
            Case 4
                checkBox_S_DLC_ROG.IsChecked = False
                checkBox_S_DLC_SW.IsChecked = False
                S_DLC_DST_SHOW()
                WrapPanel_S_Tools.Height = 170
                WrapPanel_S_light.Height = 90
                WrapPanel_S_survival.Height = 250
                WrapPanel_S_fight.Height = 170
                WrapPanel_S_structures.Height = 170
                WrapPanel_S_refine.Height = 90
                WrapPanel_S_dress.Height = 250
                WrapPanel_S_ancient.Height = 170
                WrapPanel_S_shadow.Height = 90
                WrapPanel_Science.Height = 2489.2
            Case 5
                checkBox_S_DLC_SW.IsChecked = False
                S_DLC_ROG_SHOW()
                S_DLC_DST_SHOW()
                WrapPanel_S_Tools.Height = 170
                WrapPanel_S_light.Height = 90
                WrapPanel_S_survival.Height = 250
                WrapPanel_S_fight.Height = 170
                WrapPanel_S_structures.Height = 170
                WrapPanel_S_refine.Height = 90
                WrapPanel_S_dress.Height = 250
                WrapPanel_S_ancient.Height = 170
                WrapPanel_S_shadow.Height = 90
                WrapPanel_Science.Height = 2489.2
            Case 6
                checkBox_S_DLC_ROG.IsChecked = False
                S_DLC_SW_SHOW()
                S_DLC_DST_SHOW()
                WrapPanel_S_Tools.Height = 170
                WrapPanel_S_light.Height = 170
                WrapPanel_S_nautical.Height = 250
                WrapPanel_S_survival.Height = 250
                WrapPanel_S_fight.Height = 250
                WrapPanel_S_structures.Height = 250
                WrapPanel_S_refine.Height = 170
                WrapPanel_S_dress.Height = 250
                WrapPanel_S_ancient.Height = 170
                WrapPanel_S_shadow.Height = 90
                WrapPanel_S_volcano.Height = 90
                WrapPanel_Science.Height = 3260
            Case 7
                S_DLC_ROG_SHOW()
                S_DLC_SW_SHOW()
                S_DLC_DST_SHOW()
                WrapPanel_S_Tools.Height = 170
                WrapPanel_S_light.Height = 170
                WrapPanel_S_nautical.Height = 250
                WrapPanel_S_survival.Height = 250
                WrapPanel_S_fight.Height = 250
                WrapPanel_S_structures.Height = 250
                WrapPanel_S_refine.Height = 170
                WrapPanel_S_dress.Height = 250
                WrapPanel_S_ancient.Height = 170
                WrapPanel_S_shadow.Height = 90
                WrapPanel_S_volcano.Height = 90
                WrapPanel_Science.Height = 3260
        End Select

        Select Case RegReadCS
            Case 1
                CS_DLC_ROG_SHOW()
                WrapPanel_CS_meats.Height = 170
                WrapPanel_CS_others.Height = 90
                WrapPanel_CookingSimulator.Height = 940
            Case 2
                CS_DLC_SW_SHOW()
                WrapPanel_CS_meats.Height = 250
                WrapPanel_CS_others.Height = 90
                WrapPanel_CookingSimulator.Height = 1020
            Case 3
                CS_DLC_ROG_SHOW()
                CS_DLC_SW_SHOW()
                WrapPanel_CS_meats.Height = 330
                WrapPanel_CS_others.Height = 170
                WrapPanel_CookingSimulator.Height = 1180
            Case 4
                CS_DLC_DST_SHOW()
                WrapPanel_CS_meats.Height = 170
                WrapPanel_CS_others.Height = 90
                WrapPanel_CookingSimulator.Height = 940
            Case 5
                CS_DLC_ROG_SHOW()
                CS_DLC_DST_SHOW()
                WrapPanel_CS_meats.Height = 170
                WrapPanel_CS_others.Height = 90
                WrapPanel_CookingSimulator.Height = 940
            Case 6
                CS_DLC_SW_SHOW()
                CS_DLC_DST_SHOW()
                WrapPanel_CS_meats.Height = 330
                WrapPanel_CS_others.Height = 170
                WrapPanel_CookingSimulator.Height = 1180
            Case 7
                CS_DLC_ROG_SHOW()
                CS_DLC_SW_SHOW()
                CS_DLC_DST_SHOW()
                WrapPanel_CS_meats.Height = 330
                WrapPanel_CS_others.Height = 170
                WrapPanel_CookingSimulator.Height = 1180
        End Select

        If RegReadCS_CrockPot = 1 Then
            checkBox_CS_PortableCrockPot.IsChecked = False
            checkBox_CS_CrockPot.IsChecked = True
            CS_image_CrockPot.Source = Picture_Short_Name(Res_Short_Name("CP_CrockPot"))
            CS_PortableCrockPot = False
        End If

        If RegReadCS_AutoCalculation = 1 Then
            checkBox_CS_AutoCalculation.IsChecked = True
        End If

        If RegReadCS_AutoClean = 1 Then
            checkBox_CS_AutoClean.IsChecked = True
        End If

        Select Case RegReadA
            Case 1
                checkBox_A_DLC_SW.IsChecked = False
                checkBox_A_DLC_DST.IsChecked = False
                A_DLC_ROG_SHOW()
                WrapPanel_A_terrestrial.Height = 550
                WrapPanel_A_fly.Height = 220
                WrapPanel_A_cave.Height = 330
                WrapPanel_A_evil.Height = 330
                WrapPanel_A_other.Height = 110
                WrapPanel_A_megafauna.Height = 330
                WrapPanel_Animal.Height = 2870 - 7 * 110 - 25.4
            Case 2
                checkBox_A_DLC_ROG.IsChecked = False
                checkBox_A_DLC_DST.IsChecked = False
                A_DLC_SW_SHOW()
                WrapPanel_A_terrestrial.Height = 330
                WrapPanel_A_sea.Height = 220
                WrapPanel_A_fly.Height = 220
                WrapPanel_A_evil.Height = 220
                WrapPanel_A_other.Height = 110
                WrapPanel_A_megafauna.Height = 110
                WrapPanel_Animal.Height = 2870 - 12 * 110 - 25.4
            Case 3
                checkBox_A_DLC_DST.IsChecked = False
                A_DLC_ROG_SHOW()
                A_DLC_SW_SHOW()
                WrapPanel_A_terrestrial.Height = 770
                WrapPanel_A_sea.Height = 220
                WrapPanel_A_fly.Height = 330
                WrapPanel_A_cave.Height = 330
                WrapPanel_A_evil.Height = 330
                WrapPanel_A_other.Height = 220
                WrapPanel_A_megafauna.Height = 330
                WrapPanel_Animal.Height = 2870
            Case 4
                checkBox_A_DLC_ROG.IsChecked = False
                checkBox_A_DLC_SW.IsChecked = False
                A_DLC_DST_SHOW()
                WrapPanel_A_terrestrial.Height = 550
                WrapPanel_A_fly.Height = 220
                WrapPanel_A_cave.Height = 330
                WrapPanel_A_evil.Height = 330
                WrapPanel_A_other.Height = 220
                WrapPanel_A_megafauna.Height = 330
                WrapPanel_Animal.Height = 2870 - 6 * 110 - 25.4
            Case 5
                checkBox_A_DLC_SW.IsChecked = False
                A_DLC_ROG_SHOW()
                A_DLC_DST_SHOW()
                WrapPanel_A_terrestrial.Height = 550
                WrapPanel_A_fly.Height = 220
                WrapPanel_A_cave.Height = 330
                WrapPanel_A_evil.Height = 330
                WrapPanel_A_other.Height = 220
                WrapPanel_A_megafauna.Height = 330
                WrapPanel_Animal.Height = 2870 - 6 * 110 - 25.4
            Case 6
                checkBox_A_DLC_ROG.IsChecked = False
                A_DLC_SW_SHOW()
                A_DLC_DST_SHOW()
                WrapPanel_A_terrestrial.Height = 770
                WrapPanel_A_sea.Height = 220
                WrapPanel_A_fly.Height = 330
                WrapPanel_A_cave.Height = 330
                WrapPanel_A_evil.Height = 330
                WrapPanel_A_other.Height = 220
                WrapPanel_A_megafauna.Height = 330
                WrapPanel_Animal.Height = 2870
            Case 7
                A_DLC_ROG_SHOW()
                A_DLC_SW_SHOW()
                A_DLC_DST_SHOW()
                WrapPanel_A_terrestrial.Height = 770
                WrapPanel_A_sea.Height = 220
                WrapPanel_A_fly.Height = 330
                WrapPanel_A_cave.Height = 330
                WrapPanel_A_evil.Height = 330
                WrapPanel_A_other.Height = 220
                WrapPanel_A_megafauna.Height = 330
                WrapPanel_Animal.Height = 2870
        End Select

        Select Case RegReadG
            Case 1
                checkBox_G_DLC_SW.IsChecked = False
                checkBox_G_DLC_DST.IsChecked = False
                G_DLC_ROG_SHOW()
                WrapPanel_G_material.Height = 410
                WrapPanel_G_equipment.Height = 570
                WrapPanel_G_sapling.Height = 90
                WrapPanel_G_animal.Height = 170
                WrapPanel_G_toys.Height = 170
                WrapPanel_G_turf.Height = 170
                TextBlock_G_unlock.Visibility = Visibility.Visible
                WrapPanel_G_unlock.Visibility = Visibility.Visible
                TextBlock_G_component.Visibility = Visibility.Visible
                WrapPanel_G_component.Visibility = Visibility.Visible
                WrapPanel_Goods.Height = 3270 - 11 * 80
            Case 2
                checkBox_G_DLC_ROG.IsChecked = False
                checkBox_G_DLC_DST.IsChecked = False
                G_DLC_SW_SHOW()
                WrapPanel_G_material.Height = 490
                WrapPanel_G_equipment.Height = 570
                WrapPanel_G_sapling.Height = 90
                WrapPanel_G_animal.Height = 170
                WrapPanel_G_toys.Height = 170
                WrapPanel_G_turf.Height = 90
                TextBlock_G_unlock.Visibility = Visibility.Visible
                WrapPanel_G_unlock.Visibility = Visibility.Visible
                TextBlock_G_component.Visibility = Visibility.Visible
                WrapPanel_G_component.Visibility = Visibility.Visible
                WrapPanel_Goods.Height = 3270 - 11 * 80
            Case 3
                checkBox_G_DLC_DST.IsChecked = False
                G_DLC_ROG_SHOW()
                G_DLC_SW_SHOW()
                WrapPanel_G_material.Height = 570
                WrapPanel_G_equipment.Height = 730
                WrapPanel_G_sapling.Height = 170
                WrapPanel_G_animal.Height = 250
                WrapPanel_G_toys.Height = 250
                WrapPanel_G_turf.Height = 250
                TextBlock_G_unlock.Visibility = Visibility.Visible
                WrapPanel_G_unlock.Visibility = Visibility.Visible
                TextBlock_G_component.Visibility = Visibility.Visible
                WrapPanel_G_component.Visibility = Visibility.Visible
                WrapPanel_Goods.Height = 3270 - 3 * 80
            Case 4
                checkBox_G_DLC_ROG.IsChecked = False
                checkBox_G_DLC_SW.IsChecked = False
                G_DLC_DST_SHOW()
                WrapPanel_G_material.Height = 490
                WrapPanel_G_equipment.Height = 570
                WrapPanel_G_sapling.Height = 90
                WrapPanel_G_animal.Height = 170
                WrapPanel_G_toys.Height = 250
                WrapPanel_G_turf.Height = 170
                TextBlock_G_unlock.Visibility = Visibility.Collapsed
                WrapPanel_G_unlock.Visibility = Visibility.Collapsed
                TextBlock_G_component.Visibility = Visibility.Collapsed
                WrapPanel_G_component.Visibility = Visibility.Collapsed
                WrapPanel_Goods.Height = 3270 - 11 * 80 - 35.4 * 2
            Case 5
                checkBox_G_DLC_SW.IsChecked = False
                G_DLC_ROG_SHOW()
                G_DLC_DST_SHOW()
                WrapPanel_G_material.Height = 490
                WrapPanel_G_equipment.Height = 570
                WrapPanel_G_sapling.Height = 90
                WrapPanel_G_animal.Height = 170
                WrapPanel_G_toys.Height = 250
                WrapPanel_G_turf.Height = 170
                TextBlock_G_unlock.Visibility = Visibility.Visible
                WrapPanel_G_unlock.Visibility = Visibility.Visible
                TextBlock_G_component.Visibility = Visibility.Visible
                WrapPanel_G_component.Visibility = Visibility.Visible
                WrapPanel_Goods.Height = 3270 - 9 * 80
            Case 6
                checkBox_G_DLC_ROG.IsChecked = False
                G_DLC_SW_SHOW()
                G_DLC_DST_SHOW()
                WrapPanel_G_material.Height = 650
                WrapPanel_G_equipment.Height = 810
                WrapPanel_G_sapling.Height = 170
                WrapPanel_G_animal.Height = 250
                WrapPanel_G_toys.Height = 330
                WrapPanel_G_turf.Height = 250
                TextBlock_G_unlock.Visibility = Visibility.Visible
                WrapPanel_G_unlock.Visibility = Visibility.Visible
                TextBlock_G_component.Visibility = Visibility.Visible
                WrapPanel_G_component.Visibility = Visibility.Visible
                WrapPanel_Goods.Height = 3270
            Case 7
                G_DLC_ROG_SHOW()
                G_DLC_SW_SHOW()
                G_DLC_DST_SHOW()
                WrapPanel_G_material.Height = 650
                WrapPanel_G_equipment.Height = 810
                WrapPanel_G_sapling.Height = 170
                WrapPanel_G_animal.Height = 250
                WrapPanel_G_toys.Height = 330
                WrapPanel_G_turf.Height = 250
                TextBlock_G_unlock.Visibility = Visibility.Visible
                WrapPanel_G_unlock.Visibility = Visibility.Visible
                TextBlock_G_component.Visibility = Visibility.Visible
                WrapPanel_G_component.Visibility = Visibility.Visible
                WrapPanel_Goods.Height = 3270
        End Select
    End Sub

    REM ------------------关闭/最小化按钮------------------
    Private Sub btn_close_click(sender As Object, e As RoutedEventArgs)
        Reg_Write("PanelAlphaValue", Setting_slider_Alpha.Value + 1)
        End
    End Sub

    Private Sub btn_minimized_click(sender As Object, e As RoutedEventArgs)
        WindowState = WindowState.Minimized
    End Sub

    REM ------------------首页链接按钮------------------
    Private Sub W_button_official_website_click(sender As Object, e As RoutedEventArgs) Handles W_button_official_website.Click
        Process.Start("http://www.jihuangbaike.com")
    End Sub

    Private Sub W_button_DSNews_click(sender As Object, e As RoutedEventArgs) Handles W_button_DSNews.Click
        Process.Start("http://store.steampowered.com/news/?appids=219740")
    End Sub

    Private Sub W_button_DSTNews_click(sender As Object, e As RoutedEventArgs) Handles W_button_DSTNewS.Click
        Process.Start("http://store.steampowered.com/news/?appids=322330")
    End Sub

    Private Sub W_button_Mods_click(sender As Object, e As RoutedEventArgs) Handles W_button_Mods.Click
        Process.Start("http://steamcommunity.com/sharedfiles/filedetails/?id=635215011")
    End Sub

    Private Sub W_button_QRCode_Qun_click(sender As Object, e As RoutedEventArgs) Handles W_button_QRCode_Qun.Click
        Process.Start("tencent://groupwpa/?subcmd=all&param=7B2267726F757055696E223A3538303333323236382C2274696D655374616D70223A313437303132323238337D0A")
    End Sub

    Private Sub W_button_QRCode_click(sender As Object, e As RoutedEventArgs) Handles W_button_QRCode.Click
        Process.Start("http://wpa.qq.com/msgrd?v=3&uin=351765204&site=qq&menu=yes")
    End Sub

    REM ------------------跳转选择------------------
    Private Sub ButtonJump(VariableName As String, Optional FirstChange As String = "")
        Dim FirstLetter As String = ""
        If VariableName <> "" Then
            If FirstChange <> "" Then
                VariableName = FirstChange & Mid(VariableName, 2)
            End If
            FirstLetter = GetChar(VariableName, 1)
        End If
        Select Case FirstLetter
            Case "C"
                LeftTabItem_Character.IsSelected = True
                Select Case VariableName
                    Case "C_wilson"
                        button_C_Wilson_click(Nothing, Nothing)
                    Case "C_willow"
                        button_C_Willow_click(Nothing, Nothing)
                    Case "C_wolfgang"
                        button_C_Wolfgang_click(Nothing, Nothing)
                    Case "C_wendy"
                        button_C_Wendy_click(Nothing, Nothing)
                    Case "C_wx78"
                        button_C_Wx78_click(Nothing, Nothing)
                    Case "C_wickerbottom"
                        button_C_Wickerbottom_click(Nothing, Nothing)
                    Case "C_woodie"
                        button_C_Woodie_click(Nothing, Nothing)
                    Case "C_wes"
                        button_C_Wes_click(Nothing, Nothing)
                    Case "C_wathgrithr"
                        button_C_Wathgrithr_click(Nothing, Nothing)
                    Case "C_webber"
                        button_C_Webber_click(Nothing, Nothing)
                    Case "C_waxwell"
                        button_C_waxwell_click(Nothing, Nothing)
                    Case "C_walani"
                        button_C_Walani_click(Nothing, Nothing)
                    Case "C_warly"
                        button_C_warly_click(Nothing, Nothing)
                    Case "C_wilbur"
                        button_C_wilbur_click(Nothing, Nothing)
                    Case "C_woodlegs"
                        button_C_woodlegs_click(Nothing, Nothing)
                    Case "C_wendy_1"
                        button_C_Abigail_click(Nothing, Nothing)
                    Case "C_woodie_1"
                        button_C_woodie_1_click(Nothing, Nothing)
                End Select
            Case "F"
                LeftTabItem_Food.IsSelected = True
                Select Case VariableName
                    Case "FC_Meats"
                        Canvas_FoodLeft_Meats.Visibility = Visibility.Visible
                    Case "FC_Monster_Meats"
                        Canvas_FoodLeft_Monster_Meats.Visibility = Visibility.Visible
                    Case "FC_Fishes"
                        Canvas_FoodLeft_Fishes.Visibility = Visibility.Visible
                    Case "FC_Vegetables"
                        Canvas_FoodLeft_Vegetables.Visibility = Visibility.Visible
                    Case "FC_Fruit"
                        Canvas_FoodLeft_Fruit.Visibility = Visibility.Visible
                    Case "F_california_roll"
                        button_F_california_roll_click(Nothing, Nothing)
                    Case "F_seafood_gumbo"
                        button_F_seafood_gumbo_click(Nothing, Nothing)
                    Case "F_bisque"
                        button_F_bisque_click(Nothing, Nothing)
                    Case "F_jelly-O_pop"
                        button_F_jelly_O_pop_click(Nothing, Nothing)
                    Case "F_ceviche"
                        button_F_ceviche_click(Nothing, Nothing)
                    Case "F_surf_'n'_turf"
                        button_F_surf_n_turf_click(Nothing, Nothing)
                    Case "F_lobster_bisque"
                        button_F_lobster_bisque_click(Nothing, Nothing)
                    Case "F_lobster_dinner"
                        button_F_lobster_dinner_click(Nothing, Nothing)
                    Case "F_meat"
                        button_F_meat_click(Nothing, Nothing)
                    Case "F_cooked_meat"
                        button_F_cooked_meat_click(Nothing, Nothing)
                    Case "F_jerky"
                        button_F_jerky_click(Nothing, Nothing)
                    Case "F_monster_meat"
                        button_F_monster_meat_click(Nothing, Nothing)
                    Case "F_cooked_monster_meat"
                        button_F_cooked_monster_meat_click(Nothing, Nothing)
                    Case "F_monster_jerky"
                        button_F_monster_jerky_click(Nothing, Nothing)
                    Case "F_morsel"
                        button_F_morsel_click(Nothing, Nothing)
                    Case "F_cooked_morsel"
                        button_F_cooked_morsel_click(Nothing, Nothing)
                    Case "F_small_jerky"
                        button_F_small_jerky_click(Nothing, Nothing)
                    Case "F_drumstick"
                        button_F_drumstick_click(Nothing, Nothing)
                    Case "F_fried_drumstick"
                        button_F_fried_drumstick_click(Nothing, Nothing)
                    Case "F_frog_legs"
                        button_F_frog_legs_click(Nothing, Nothing)
                    Case "F_cooked_frog_legs"
                        button_F_cooked_frog_legs_click(Nothing, Nothing)
                    Case "F_fish"
                        button_F_fish_click(Nothing, Nothing)
                    Case "F_cooked_fish"
                        button_F_cooked_fish_click(Nothing, Nothing)
                    Case "F_eel"
                        button_F_eel_click(Nothing, Nothing)
                    Case "F_cooked_eel"
                        button_F_cooked_eel_click(Nothing, Nothing)
                    Case "F_moleworm"
                        button_F_moleworm_click(Nothing, Nothing)
                    Case "F_limpets"
                        button_F_limpets_click(Nothing, Nothing)
                    Case "F_cooked_limpets"
                        button_F_cooked_limpets_click(Nothing, Nothing)
                    Case "F_tropical_fish"
                        button_F_tropical_fish_click(Nothing, Nothing)
                    Case "F_fish_morsel"
                        button_F_fish_morsel_click(Nothing, Nothing)
                    Case "F_cooked_fish_morsel"
                        button_F_cooked_fish_morsel_click(Nothing, Nothing)
                    Case "F_jellyfish"
                        button_F_jellyfish_click(Nothing, Nothing)
                    Case "F_dead_jellyfish"
                        button_F_dead_jellyfish_click(Nothing, Nothing)
                    Case "F_cooked_jellyfish"
                        button_F_cooked_jellyfish_click(Nothing, Nothing)
                    Case "F_dried_jellyfish"
                        button_F_dried_jellyfish_click(Nothing, Nothing)
                    Case "F_mussel"
                        button_F_mussel_click(Nothing, Nothing)
                    Case "F_cooked_mussel"
                        button_F_cooked_mussel_click(Nothing, Nothing)
                    Case "F_dead_dogfish"
                        button_F_dead_dogfish_click(Nothing, Nothing)
                    Case "F_wobster"
                        button_F_wobster_click(Nothing, Nothing)
                    Case "F_raw_fish"
                        button_F_raw_fish_click(Nothing, Nothing)
                    Case "F_fish_steak"
                        button_F_fish_steak_click(Nothing, Nothing)
                    Case "F_shark_fin"
                        button_F_shark_fin_click(Nothing, Nothing)
                    Case "F_blue_cap"
                        button_F_blue_cap_click(Nothing, Nothing)
                    Case "F_cooked_blue_cap"
                        button_F_cooked_blue_cap_click(Nothing, Nothing)
                    Case "F_green_cap"
                        button_F_green_cap_click(Nothing, Nothing)
                    Case "F_cooked_green_cap"
                        button_F_cooked_green_cap_click(Nothing, Nothing)
                    Case "F_red_cap"
                        button_F_red_cap_click(Nothing, Nothing)
                    Case "F_cooked_red_cap"
                        button_F_cooked_red_cap_click(Nothing, Nothing)
                    Case "F_eggplant"
                        button_F_eggplant_click(Nothing, Nothing)
                    Case "F_braised_eggplant"
                        button_F_braised_eggplant_click(Nothing, Nothing)
                    Case "F_carrot"
                        button_F_carrot_click(Nothing, Nothing)
                    Case "F_roasted_carrot"
                        button_F_roasted_carrot_click(Nothing, Nothing)
                    Case "F_corn"
                        button_F_corn_click(Nothing, Nothing)
                    Case "F_popcorn"
                        button_F_popcorn_click(Nothing, Nothing)
                    Case "F_pumpkin"
                        button_F_pumpkin_click(Nothing, Nothing)
                    Case "F_hot_pumpkin"
                        button_F_hot_pumpkin_click(Nothing, Nothing)
                    Case "F_cactus_flesh"
                        button_F_cactus_flesh_click(Nothing, Nothing)
                    Case "F_cooked_cactus_flesh"
                        button_F_cooked_cactus_flesh_click(Nothing, Nothing)
                    Case "F_cactus_flower"
                        button_F_cactus_flower_click(Nothing, Nothing)
                    Case "F_sweet_potato"
                        button_F_sweet_potato_click(Nothing, Nothing)
                    Case "F_cooked_sweet_potato"
                        button_F_cooked_sweet_potato_click(Nothing, Nothing)
                    Case "F_seaweed"
                        button_F_seaweed_click(Nothing, Nothing)
                    Case "F_roasted_seaweed"
                        button_F_roasted_seaweed_click(Nothing, Nothing)
                    Case "F_dried_seaweed"
                        button_F_dried_seaweed_click(Nothing, Nothing)
                    Case "F_juicy_berries"
                        button_F_juicy_berries_click(Nothing, Nothing)
                    Case "F_roasted_juicy_berries"
                        button_F_roasted_juicy_berries_click(Nothing, Nothing)
                    Case "F_berries"
                        button_F_berries_click(Nothing, Nothing)
                    Case "F_roasted_berrie"
                        button_F_roasted_berrie_click(Nothing, Nothing)
                    Case "F_banana"
                        button_F_banana_click(Nothing, Nothing)
                    Case "F_cooked_banana"
                        button_F_cooked_banana_click(Nothing, Nothing)
                    Case "F_dragon_fruit"
                        button_F_dragon_fruit_click(Nothing, Nothing)
                    Case "F_prepared_dragon_fruit"
                        button_F_prepared_dragon_fruit_click(Nothing, Nothing)
                    Case "F_durian"
                        button_F_durian_click(Nothing, Nothing)
                    Case "F_extra_smelly_durian"
                        button_F_extra_smelly_durian_click(Nothing, Nothing)
                    Case "F_pomegranate"
                        button_F_pomegranate_click(Nothing, Nothing)
                    Case "F_sliced_pomegranate"
                        button_F_sliced_pomegranate_click(Nothing, Nothing)
                    Case "F_watermelon"
                        button_F_watermelon_click(Nothing, Nothing)
                    Case "F_grilled_watermelon"
                        button_F_grilled_watermelon_click(Nothing, Nothing)
                    Case "F_halved_coconut"
                        button_F_halved_coconut_click(Nothing, Nothing)
                    Case "F_roasted_coconut"
                        button_F_roasted_coconut_click(Nothing, Nothing)
                    Case "F_egg"
                        button_F_egg_click(Nothing, Nothing)
                    Case "F_cooked_egg"
                        button_F_cooked_egg_click(Nothing, Nothing)
                    Case "F_tallbird_egg"
                        button_F_tallbird_egg_click(Nothing, Nothing)
                    Case "F_fried_tallbird_egg"
                        button_F_fried_tallbird_egg_click(Nothing, Nothing)
                    Case "F_doydoy_egg"
                        button_F_doydoy_egg_click(Nothing, Nothing)
                    Case "F_fried_doydoy_egg"
                        button_F_fried_doydoy_egg_click(Nothing, Nothing)
                    Case "F_butterfly_wing"
                        button_F_butterfly_wing_click(Nothing, Nothing)
                    Case "F_butter"
                        button_F_butter_click(Nothing, Nothing)
                    Case "F_twigs"
                        button_F_twigs_click(Nothing, Nothing)
                    Case "F_honey"
                        button_F_honey_click(Nothing, Nothing)
                    Case "F_honeycomb"
                        button_F_honeycomb_click(Nothing, Nothing)
                    Case "F_lichen"
                        button_F_lichen_click(Nothing, Nothing)
                    Case "F_mandrake"
                        button_F_mandrake_click(Nothing, Nothing)
                    Case "F_electric_milk"
                        button_F_electric_milk_click(Nothing, Nothing)
                    Case "F_ice"
                        button_F_ice_click(Nothing, Nothing)
                    Case "F_roasted_birchnut"
                        button_F_roasted_birchnut_click(Nothing, Nothing)
                    Case "F_butterfly_wing_sw"
                        button_F_butterfly_wing_sw_click(Nothing, Nothing)
                    Case "F_coffee_beans"
                        button_F_coffee_beans_click(Nothing, Nothing)
                    Case "F_roasted_coffee_beans"
                        button_F_roasted_coffee_beans_click(Nothing, Nothing)
                    Case "F_petals"
                        button_F_petals_click(Nothing, Nothing)
                    Case "F_dark_petals"
                        button_F_dark_petals_click(Nothing, Nothing)
                    Case "F_rotten_egg"
                        button_F_rotten_egg_click(Nothing, Nothing)
                    Case "F_batilisk_wing"
                        button_F_batilisk_wing_click(Nothing, Nothing)
                    Case "F_cooked_batilisk_wing"
                        button_F_cooked_batilisk_wing_click(Nothing, Nothing)
                    Case "F_koalefant_trunk"
                        button_F_koalefant_trunk_click(Nothing, Nothing)
                    Case "F_winter_koalefant_trunk"
                        button_F_winter_koalefant_trunk_click(Nothing, Nothing)
                    Case "F_koalefant_trunk_steak"
                        button_F_koalefant_trunk_steak_click(Nothing, Nothing)
                    Case "F_leafy_meat"
                        button_F_leafy_meat_click(Nothing, Nothing)
                    Case "F_cooked_leafy_meat"
                        button_F_cooked_leafy_meat_click(Nothing, Nothing)
                    Case "F_cooked_mandrake"
                        button_F_cooked_mandrake_click(Nothing, Nothing)
                    Case "F_deerclops_eyeball"
                        button_F_deerclops_eyeball_click(Nothing, Nothing)
                    Case "F_foliage"
                        button_F_foliage_click(Nothing, Nothing)
                    Case "F_gears"
                        button_F_gears_click(Nothing, Nothing)
                    Case "F_glommer's_goop"
                        button_F_glommers_goop_click(Nothing, Nothing)
                    Case "F_phlegm"
                        button_F_phlegm_click(Nothing, Nothing)
                    Case "F_glow_berry"
                        button_F_glow_berry_click(Nothing, Nothing)
                    Case "F_lesser_glow_berry"
                        button_F_lesser_glow_berry_click(Nothing, Nothing)
                    Case "F_guardian's_horn"
                        button_F_guardians_horn_click(Nothing, Nothing)
                    Case "F_hatching_tallbird_egg"
                        button_F_hatching_tallbird_egg_click(Nothing, Nothing)
                    Case "F_light_bulb"
                        button_F_light_bulb_click(Nothing, Nothing)
                    Case "F_seeds"
                        button_F_seeds_click(Nothing, Nothing)
                    Case "F_toasted_seeds"
                        button_F_toasted_seeds_click(Nothing, Nothing)
                    Case "F_carrot_seeds"
                        button_F_carrot_seeds_click(Nothing, Nothing)
                    Case "F_corn_seeds"
                        button_F_corn_seeds_click(Nothing, Nothing)
                    Case "F_dragon_fruit_seeds"
                        button_F_dragon_fruit_seeds_click(Nothing, Nothing)
                    Case "F_durian_seeds"
                        button_F_durian_seeds_click(Nothing, Nothing)
                    Case "F_eggplant_seeds"
                        button_F_eggplant_seeds_click(Nothing, Nothing)
                    Case "F_pomegranate_seeds"
                        button_F_pomegranate_seeds_click(Nothing, Nothing)
                    Case "F_pumpkin_seeds"
                        button_F_pumpkin_seeds_click(Nothing, Nothing)
                    Case "F_watermelon_seeds"
                        button_F_watermelon_seeds_click(Nothing, Nothing)
                    Case "F_sweet_potato_seeds"
                        button_F_sweet_potato_seeds_click(Nothing, Nothing)
                    Case "F_dead_swordfish"
                        button_F_dead_swordfish_click(Nothing, Nothing)
                    Case "F_blubber"
                        button_F_blubber_click(Nothing, Nothing)
                    Case "F_brainy_matter"
                        button_F_brainy_matter_click(Nothing, Nothing)
                    Case "F_dead_wobster"
                        button_F_dead_wobster_click(Nothing, Nothing)
                    Case "F_delicious_wobster"
                        button_F_delicious_wobster_click(Nothing, Nothing)
                    Case "F_dragoon_heart"
                        button_F_dragoon_heart_click(Nothing, Nothing)
                    Case "F_hail"
                        button_F_hail_click(Nothing, Nothing)
                    Case "F_coconut"
                        button_F_coconut_click(Nothing, Nothing)
                    Case "F_spoiled_fish"
                        button_F_spoiled_fish_click(Nothing, Nothing)
                    Case "F_rot"
                        button_F_rot_click(Nothing, Nothing)
                    Case "F_eye_of_the_tiger_shark"
                        button_F_eye_of_the_tiger_shark_click(Nothing, Nothing)
                    Case Else
                        Canvas_FoodLeft_Others.Visibility = Visibility.Visible
                End Select
            Case "S"
                LeftTabItem_Science.IsSelected = True
                Select Case VariableName
                    Case "S_axe"
                        button_S_axe_click(Nothing, Nothing)
                    Case "S_goldenaxe"
                        button_S_goldenaxe_click(Nothing, Nothing)
                    Case "S_machete"
                        button_S_machete_click(Nothing, Nothing)
                    Case "S_luxury_machete"
                        button_S_luxury_machete_click(Nothing, Nothing)
                    Case "S_pickaxe"
                        button_S_pickaxe_click(Nothing, Nothing)
                    Case "S_goldenpickaxe"
                        button_S_goldenpickaxe_click(Nothing, Nothing)
                    Case "S_shovel"
                        button_S_shovel_click(Nothing, Nothing)
                    Case "S_goldenshovel"
                        button_S_goldenshovel_click(Nothing, Nothing)
                    Case "S_hammer"
                        button_S_hammer_click(Nothing, Nothing)
                    Case "S_pitchfork"
                        button_S_pitchfork_click(Nothing, Nothing)
                    Case "S_razor"
                        button_S_razor_click(Nothing, Nothing)
                    Case "S_saddlehorn"
                        button_S_saddlehorn_click(Nothing, Nothing)
                    Case "S_saddle"
                        button_S_saddle_click(Nothing, Nothing)
                    Case "S_war_saddle"
                        button_S_war_saddle_click(Nothing, Nothing)
                    Case "S_glossamer_saddle"
                        button_S_glossamer_saddle_click(Nothing, Nothing)
                    Case "S_brush"
                        button_S_brush_click(Nothing, Nothing)
                    Case "S_salt_lick"
                        button_S_salt_lick_click(Nothing, Nothing)
                    Case "S_campfire"
                        button_S_campfire_click(Nothing, Nothing)
                    Case "S_fire_pit"
                        button_S_fire_pit_click(Nothing, Nothing)
                    Case "S_willow's_lighter"
                        button_S_willows_lighter_click(Nothing, Nothing)
                    Case "S_chiminea"
                        button_S_chiminea_click(Nothing, Nothing)
                    Case "S_torch"
                        button_S_torch_click(Nothing, Nothing)
                    Case "S_endothermic_fire"
                        button_S_endothermic_fire_click(Nothing, Nothing)
                    Case "S_endothermic_fire_pit"
                        button_S_endothermic_fire_pit_click(Nothing, Nothing)
                    Case "S_obsidian_fire_pit"
                        button_S_obsidian_fire_pit_click(Nothing, Nothing)
                    Case "S_miner_hat"
                        button_S_miner_hat_click(Nothing, Nothing)
                    Case "S_moggles"
                        button_S_moggles_click(Nothing, Nothing)
                    Case "S_pumpkin_lantern"
                        button_S_pumpkin_lantern_click(Nothing, Nothing)
                    Case "S_lantern_1"
                        button_S_lantern_click(Nothing, Nothing)
                    Case "S_bottle_lantern"
                        button_S_bottle_lantern_click(Nothing, Nothing)
                    Case "S_boat_torch"
                        button_S_boat_torch_click(Nothing, Nothing)
                    Case "S_boat_lantern"
                        button_S_boat_lantern_click(Nothing, Nothing)
                    Case "S_surfboard"
                        button_S_surfboard_click(Nothing, Nothing)
                    Case "S_lucky_hat"
                        button_S_lucky_hat_click(Nothing, Nothing)
                    Case "S_the_'sea_legs'"
                        button_S_the_sea_legs_click(Nothing, Nothing)
                    Case "S_log_raft"
                        button_S_log_raft_click(Nothing, Nothing)
                    Case "S_raft"
                        button_S_raft_click(Nothing, Nothing)
                    Case "S_row_boat"
                        button_S_row_boat_click(Nothing, Nothing)
                    Case "S_cargo_boat"
                        button_S_cargo_boat_click(Nothing, Nothing)
                    Case "S_armoured_boat"
                        button_S_armoured_boat_click(Nothing, Nothing)
                    Case "S_boat_repair_kit"
                        button_S_boat_repair_kit_click(Nothing, Nothing)
                    Case "S_thatch_sail"
                        button_S_thatch_sail_click(Nothing, Nothing)
                    Case "S_cloth_sail"
                        button_S_cloth_sail_click(Nothing, Nothing)
                    Case "S_snakeskin_sail"
                        button_S_snakeskin_sail_click(Nothing, Nothing)
                    Case "S_feather_lite_sail"
                        button_S_feather_lite_sail_click(Nothing, Nothing)
                    Case "S_iron_wind"
                        button_S_iron_wind_click(Nothing, Nothing)
                    Case "S_boat_cannon"
                        button_S_boat_cannon_click(Nothing, Nothing)
                    Case "S_sea_trap"
                        button_S_sea_trap_click(Nothing, Nothing)
                    Case "S_trawl_net"
                        button_S_trawl_net_click(Nothing, Nothing)
                    Case "S_spyglass"
                        button_S_spyglass_click(Nothing, Nothing)
                    Case "S_super_spyglass"
                        button_S_super_spyglass_click(Nothing, Nothing)
                    Case "S_captain_hat"
                        button_S_captain_hat_click(Nothing, Nothing)
                    Case "S_pirate_hat"
                        button_S_pirate_hat_click(Nothing, Nothing)
                    Case "S_life_jacket"
                        button_S_life_jacket_click(Nothing, Nothing)
                    Case "S_buoy"
                        button_S_buoy_click(Nothing, Nothing)
                    Case "S_chef_pouch"
                        button_S_chef_pouch_click(Nothing, Nothing)
                    Case "S_telltale_heart"
                        button_S_telltale_heart_click(Nothing, Nothing)
                    Case "S_healing_salve"
                        button_S_healing_salve_click(Nothing, Nothing)
                    Case "S_honey_poultice"
                        button_S_honey_poultice_click(Nothing, Nothing)
                    Case "S_booster_shot"
                        button_S_booster_shot_click(Nothing, Nothing)
                    Case "S_bernie_1"
                        button_S_bernie_click(Nothing, Nothing)
                    Case "S_trap"
                        button_S_trap_click(Nothing, Nothing)
                    Case "S_bird_trap"
                        button_S_bird_trap_click(Nothing, Nothing)
                    Case "S_bug_net"
                        button_S_bug_net_click(Nothing, Nothing)
                    Case "S_fishing_rod"
                        button_S_fishing_rod_click(Nothing, Nothing)
                    Case "S_silly_monkey_ball"
                        button_S_silly_monkey_ball_click(Nothing, Nothing)
                    Case "S_tropical_parasol"
                        button_S_tropical_parasol_click(Nothing, Nothing)
                    Case "S_pretty_parasol"
                        button_S_pretty_parasol_click(Nothing, Nothing)
                    Case "S_umbrella"
                        button_S_umbrella_click(Nothing, Nothing)
                    Case "S_anti_venom"
                        button_S_anti_venom_click(Nothing, Nothing)
                    Case "S_waterballoon"
                        button_S_waterballoon_click(Nothing, Nothing)
                    Case "S_pile_o'_balloons"
                        button_S_pile_o_balloons_click(Nothing, Nothing)
                    Case "S_compass"
                        button_S_compass_click(Nothing, Nothing)
                    Case "S_thermal_stone"
                        button_S_thermal_stone_click(Nothing, Nothing)
                    Case "S_thatch_pack"
                        button_S_thatch_pack_click(Nothing, Nothing)
                    Case "S_backpack"
                        button_S_backpack_click(Nothing, Nothing)
                    Case "S_piggyback"
                        button_S_piggyback_click(Nothing, Nothing)
                    Case "S_straw_roll"
                        button_S_straw_roll_click(Nothing, Nothing)
                    Case "S_fur_roll"
                        button_S_fur_roll_click(Nothing, Nothing)
                    Case "S_tent"
                        button_S_tent_click(Nothing, Nothing)
                    Case "S_siesta_lean-to"
                        button_S_siesta_lean_to_click(Nothing, Nothing)
                    Case "S_palm_leaf_hut"
                        button_S_palm_leaf_hut_click(Nothing, Nothing)
                    Case "S_whirly_fan"
                        button_S_whirly_fan_click(Nothing, Nothing)
                    Case "S_luxury_fan"
                        button_S_luxury_fan_click(Nothing, Nothing)
                    Case "S_tropical_fan"
                        button_S_tropical_fan_click(Nothing, Nothing)
                    Case "S_insulated_pack"
                        button_S_insulated_pack_click(Nothing, Nothing)
                    Case "S_sea_sack"
                        button_S_sea_sack_click(Nothing, Nothing)
                    Case "S_doydoy_nest"
                        button_S_doydoy_nest_click(Nothing, Nothing)
                    Case "S_mussel_stick"
                        button_S_mussel_stick_click(Nothing, Nothing)
                    Case "S_basic_farm"
                        button_S_basic_farm_click(Nothing, Nothing)
                    Case "S_improved_farm"
                        button_S_improved_farm_click(Nothing, Nothing)
                    Case "S_bucket-o-poop"
                        button_S_bucket_o_poop_click(Nothing, Nothing)
                    Case "S_bee_box"
                        button_S_bee_box_click(Nothing, Nothing)
                    Case "S_drying_rack"
                        button_S_drying_rack_click(Nothing, Nothing)
                    Case "S_crock_pot"
                        button_S_crock_pot_click(Nothing, Nothing)
                    Case "S_ice_box"
                        button_S_ice_box_click(Nothing, Nothing)
                    Case "S_science_machine"
                        button_S_science_machine_click(Nothing, Nothing)
                    Case "S_alchemy_engine"
                        button_S_alchemy_engine_click(Nothing, Nothing)
                    Case "S_electrical_doodad"
                        button_S_electrical_doodad_click(Nothing, Nothing)
                    Case "S_divining_rod"
                        button_S_divining_rod_click(Nothing, Nothing)
                    Case "S_thermal_measurer"
                        button_S_thermal_measurer_click(Nothing, Nothing)
                    Case "S_rainometer"
                        button_S_rainometer_click(Nothing, Nothing)
                    Case "S_gunpowder"
                        button_S_gunpowder_click(Nothing, Nothing)
                    Case "S_lightning_rod"
                        button_S_lightning_rod_click(Nothing, Nothing)
                    Case "S_ice_flingomatic"
                        button_S_ice_flingomatic_click(Nothing, Nothing)
                    Case "S_ice_maker_3000"
                        button_S_ice_maker_3000_click(Nothing, Nothing)
                    Case "S_accomploshrine"
                        button_S_accomploshrine_click(Nothing, Nothing)
                    Case "S_battle_spear"
                        button_S_battle_spear_click(Nothing, Nothing)
                    Case "S_battle_helm"
                        button_S_battle_helm_click(Nothing, Nothing)
                    Case "S_spear"
                        button_S_spear_click(Nothing, Nothing)
                    Case "S_poison_spear"
                        button_S_poison_spear_click(Nothing, Nothing)
                    Case "S_ham_bat"
                        button_S_ham_bat_click(Nothing, Nothing)
                    Case "S_morning_star"
                        button_S_morning_star_click(Nothing, Nothing)
                    Case "S_tail_o'_three_cats"
                        button_S_tail_o_three_cats_click(Nothing, Nothing)
                    Case "S_grass_suit"
                        button_S_grass_suit_click(Nothing, Nothing)
                    Case "S_log_suit"
                        button_S_log_suit_click(Nothing, Nothing)
                    Case "S_marble_suit"
                        button_S_marble_suit_click(Nothing, Nothing)
                    Case "S_seashell_suit"
                        button_S_seashell_suit_click(Nothing, Nothing)
                    Case "S_limestone_suit"
                        button_S_limestone_suit_click(Nothing, Nothing)
                    Case "S_cactus_armour"
                        button_S_cactus_armour_click(Nothing, Nothing)
                    Case "S_football_helmet"
                        button_S_football_helmet_click(Nothing, Nothing)
                    Case "S_horned_helmet"
                        button_S_horned_helmet_click(Nothing, Nothing)
                    Case "S_blow_dart"
                        button_S_blow_dart_click(Nothing, Nothing)
                    Case "S_sleep_dart"
                        button_S_sleep_dart_click(Nothing, Nothing)
                    Case "S_fire_dart"
                        button_S_fire_dart_click(Nothing, Nothing)
                    Case "S_poison_dart"
                        button_S_poison_dart_click(Nothing, Nothing)
                    Case "S_boomerang"
                        button_S_boomerang_click(Nothing, Nothing)
                    Case "S_bee_mine"
                        button_S_bee_mine_click(Nothing, Nothing)
                    Case "S_tooth_trap"
                        button_S_tooth_trap_click(Nothing, Nothing)
                    Case "S_scalemail"
                        button_S_scalemail_click(Nothing, Nothing)
                    Case "S_weather_pain"
                        button_S_weather_pain_click(Nothing, Nothing)
                    Case "S_coconade"
                        button_S_coconade_click(Nothing, Nothing)
                    Case "S_spear_gun"
                        button_S_spear_gun_click(Nothing, Nothing)
                    Case "S_cutlass_supreme"
                        button_S_cutlass_supreme_click(Nothing, Nothing)
                    Case "S_spider_eggs"
                        button_S_spider_eggs_click(Nothing, Nothing)
                    Case "S_chest"
                        button_S_chest_click(Nothing, Nothing)
                    Case "S_sign"
                        button_S_sign_click(Nothing, Nothing)
                    Case "S_directional_sign"
                        button_S_directional_sign_click(Nothing, Nothing)
                    Case "S_hay_wall"
                        button_S_hay_wall_click(Nothing, Nothing)
                    Case "S_wood_wall"
                        button_S_wood_wall_click(Nothing, Nothing)
                    Case "S_stone_wall"
                        button_S_stone_wall_click(Nothing, Nothing)
                    Case "S_limestone_wall"
                        button_S_limestone_wall_click(Nothing, Nothing)
                    Case "S_moon_rock_wall"
                        button_S_moon_rock_wall_click(Nothing, Nothing)
                    Case "S_wardrobe"
                        button_S_wardrobe_click(Nothing, Nothing)
                    Case "S_pig_house"
                        button_S_pig_house_click(Nothing, Nothing)
                    Case "S_rabbit_hutch"
                        button_S_rabbit_hutch_click(Nothing, Nothing)
                    Case "S_wildbore_house"
                        button_S_wildbore_house_click(Nothing, Nothing)
                    Case "S_prime_ape_hut"
                        button_S_prime_ape_hut_click(Nothing, Nothing)
                    Case "S_dragoon_den"
                        button_S_dragoon_den_click(Nothing, Nothing)
                    Case "S_birdcage"
                        button_S_birdcage_click(Nothing, Nothing)
                    Case "S_cobblestones"
                        button_S_cobblestones_click(Nothing, Nothing)
                    Case "S_wooden_flooring"
                        button_S_wooden_flooring_click(Nothing, Nothing)
                    Case "S_checkered_flooring"
                        button_S_checkered_flooring_click(Nothing, Nothing)
                    Case "S_carpeted_flooring"
                        button_S_carpeted_flooring_click(Nothing, Nothing)
                    Case "S_scaled_flooring"
                        button_S_scaled_flooring_click(Nothing, Nothing)
                    Case "S_snakeskin_rug"
                        button_S_snakeskin_rug_click(Nothing, Nothing)
                    Case "S_potted_fern"
                        button_S_potted_fern_click(Nothing, Nothing)
                    Case "S_scaled_chest"
                        button_S_scaled_chest_click(Nothing, Nothing)
                    Case "S_sandbag"
                        button_S_sandbag_click(Nothing, Nothing)
                    Case "S_sand_castle"
                        button_S_sand_castle_click(Nothing, Nothing)
                    Case "S_rope"
                        button_S_rope_click(Nothing, Nothing)
                    Case "S_boards"
                        button_S_boards_click(Nothing, Nothing)
                    Case "S_cut_stone"
                        button_S_cut_stone_click(Nothing, Nothing)
                    Case "S_papyrus"
                        button_S_papyrus_click(Nothing, Nothing)
                    Case "S_thick_fur"
                        button_S_thick_fur_click(Nothing, Nothing)
                    Case "S_cloth"
                        button_S_cloth_click(Nothing, Nothing)
                    Case "S_limestone"
                        button_S_limestone_click(Nothing, Nothing)
                    Case "S_gold_nugget"
                        button_S_gold_nugget_click(Nothing, Nothing)
                    Case "S_ice"
                        button_S_ice_click(Nothing, Nothing)
                    Case "S_empty_bottle"
                        button_S_empty_bottle_click(Nothing, Nothing)
                    Case "S_nightmare_fuel"
                        button_S_nightmare_fuel_click(Nothing, Nothing)
                    Case "S_purple_gem"
                        button_S_purple_gem_click(Nothing, Nothing)
                    Case "S_abigail's_flower"
                        button_S_abigails_flower_click(Nothing, Nothing)
                    Case "S_prestihatitator"
                        button_S_prestihatitator_click(Nothing, Nothing)
                    Case "S_prestihatitator_SW"
                        button_S_prestihatitator_SW_click(Nothing, Nothing)
                    Case "S_shadow_manipulator"
                        button_S_shadow_manipulator_click(Nothing, Nothing)
                    Case "S_meat_effigy"
                        button_S_meat_effigy_click(Nothing, Nothing)
                    Case "S_pan_flute"
                        button_S_pan_flute_click(Nothing, Nothing)
                    Case "S_dripple_pipes"
                        button_S_dripple_pipes_click(Nothing, Nothing)
                    Case "S_old_bell"
                        button_S_old_bell_click(Nothing, Nothing)
                    Case "S_one-man_band"
                        button_S_one_man_band_click(Nothing, Nothing)
                    Case "S_night_light"
                        button_S_night_light_click(Nothing, Nothing)
                    Case "S_night_armour"
                        button_S_night_armour_click(Nothing, Nothing)
                    Case "S_dark_sword"
                        button_S_dark_sword_click(Nothing, Nothing)
                    Case "S_bat_bat"
                        button_S_bat_bat_click(Nothing, Nothing)
                    Case "S_belt_of_hunger"
                        button_S_belt_of_hunger_click(Nothing, Nothing)
                    Case "S_life_giving_amulet"
                        button_S_life_giving_amulet_click(Nothing, Nothing)
                    Case "S_chilled_amulet"
                        button_S_chilled_amulet_click(Nothing, Nothing)
                    Case "S_nightmare_amulet"
                        button_S_nightmare_amulet_click(Nothing, Nothing)
                    Case "S_fire_staff"
                        button_S_fire_staff_click(Nothing, Nothing)
                    Case "S_ice_staff"
                        button_S_ice_staff_click(Nothing, Nothing)
                    Case "S_telelocator_staff"
                        button_S_telelocator_staff_click(Nothing, Nothing)
                    Case "S_telelocator_focus"
                        button_S_telelocator_focus_click(Nothing, Nothing)
                    Case "S_Seaworthy"
                        button_S_seaworthy_click(Nothing, Nothing)
                    Case "S_sewing_kit"
                        button_S_sewing_kit_click(Nothing, Nothing)
                    Case "S_garland"
                        button_S_garland_click(Nothing, Nothing)
                    Case "S_straw_hat"
                        button_S_straw_hat_click(Nothing, Nothing)
                    Case "S_top_hat"
                        button_S_top_hat_click(Nothing, Nothing)
                    Case "S_rain_hat"
                        button_S_rain_hat_click(Nothing, Nothing)
                    Case "S_rabbit_earmuffs"
                        button_S_rabbit_earmuffs_click(Nothing, Nothing)
                    Case "S_beefalo_hat"
                        button_S_beefalo_hat_click(Nothing, Nothing)
                    Case "S_winter_hat"
                        button_S_winter_hat_click(Nothing, Nothing)
                    Case "S_cat_cap"
                        button_S_cat_cap_click(Nothing, Nothing)
                    Case "S_fashion_melon"
                        button_S_fashion_melon_click(Nothing, Nothing)
                    Case "S_ice_cube"
                        button_S_ice_cube_click(Nothing, Nothing)
                    Case "S_brain_of_thought"
                        button_S_brain_of_thought_click(Nothing, Nothing)
                    Case "S_shark_tooth_crown"
                        button_S_shark_tooth_crown_click(Nothing, Nothing)
                    Case "S_beekeeper_hat"
                        button_S_beekeeper_hat_click(Nothing, Nothing)
                    Case "S_feather_hat"
                        button_S_feather_hat_click(Nothing, Nothing)
                    Case "S_bush_hat"
                        button_S_bush_hat_click(Nothing, Nothing)
                    Case "S_snakeskin_hat"
                        button_S_snakeskin_hat_click(Nothing, Nothing)
                    Case "S_snakeskin_jacket"
                        button_S_snakeskin_jacket_click(Nothing, Nothing)
                    Case "S_rain_coat"
                        button_S_rain_coat_click(Nothing, Nothing)
                    Case "S_blubber_suit"
                        button_S_blubber_suit_click(Nothing, Nothing)
                    Case "S_dapper_vest"
                        button_S_dapper_vest_click(Nothing, Nothing)
                    Case "S_breezy_vest"
                        button_S_breezy_vest_click(Nothing, Nothing)
                    Case "S_puffy_vest"
                        button_S_puffy_vest_click(Nothing, Nothing)
                    Case "S_summer_frest"
                        button_S_summer_frest_click(Nothing, Nothing)
                    Case "S_floral_shirt"
                        button_S_floral_shirt_click(Nothing, Nothing)
                    Case "S_walking_cane"
                        button_S_walking_cane_click(Nothing, Nothing)
                    Case "S_hibearnation_vest"
                        button_S_hibearnation_vest_click(Nothing, Nothing)
                    Case "S_eyebrella"
                        button_S_eyebrella_click(Nothing, Nothing)
                    Case "S_dumbrella"
                        button_S_dumbrella_click(Nothing, Nothing)
                    Case "S_windbreaker"
                        button_S_windbreaker_click(Nothing, Nothing)
                    Case "S_particulate_purifier"
                        button_S_particulate_purifier_click(Nothing, Nothing)
                    Case "S_sleek_hat"
                        button_S_sleek_hat_click(Nothing, Nothing)
                    Case "S_thulecite"
                        button_S_thulecite_click(Nothing, Nothing)
                    Case "S_thulecite_wall"
                        button_S_thulecite_wall_click(Nothing, Nothing)
                    Case "S_thulecite_medallion"
                        button_S_thulecite_medallion_click(Nothing, Nothing)
                    Case "S_the_lazy_forager"
                        button_S_the_lazy_forager_click(Nothing, Nothing)
                    Case "S_magiluminescence"
                        button_S_magiluminescence_click(Nothing, Nothing)
                    Case "S_construction_amulet"
                        button_S_construction_amulet_click(Nothing, Nothing)
                    Case "S_the_lazy_explorer"
                        button_S_the_lazy_explorer_click(Nothing, Nothing)
                    Case "S_star_caller's_staff"
                        button_S_star_callers_staff_click(Nothing, Nothing)
                    Case "S_deconstruction_staff"
                        button_S_deconstruction_staff_click(Nothing, Nothing)
                    Case "S_pickaxe_1"
                        button_S_pickaxe_1_click(Nothing, Nothing)
                    Case "S_thulecite_crown"
                        button_S_thulecite_crown_click(Nothing, Nothing)
                    Case "S_thulecite_suit"
                        button_S_thulecite_suit_click(Nothing, Nothing)
                    Case "S_thulecite_club"
                        button_S_thulecite_club_click(Nothing, Nothing)
                    Case "S_houndius_shootius"
                        button_S_houndius_shootius_click(Nothing, Nothing)
                    Case "S_birds_of_the_world"
                        button_S_birds_of_the_world_click(Nothing, Nothing)
                    Case "S_applied_horticulture"
                        button_S_applied_horticulture_click(Nothing, Nothing)
                    Case "S_sleepytime_stories"
                        button_S_sleepytime_stories_click(Nothing, Nothing)
                    Case "S_the_end_is_nigh!"
                        button_S_the_end_is_nigh_click(Nothing, Nothing)
                    Case "S_on_tentacles"
                        button_S_on_tentacles_click(Nothing, Nothing)
                    Case "S_joy_of_volcanology"
                        button_S_joy_of_volcanology_click(Nothing, Nothing)
                    Case "S_codex_umbra"
                        button_S_codex_umbra_click(Nothing, Nothing)
                    Case "S_shadow_logger"
                        button_S_shadow_logger_click(Nothing, Nothing)
                    Case "S_shadow_miner"
                        button_S_shadow_miner_click(Nothing, Nothing)
                    Case "S_shadow_digger"
                        button_S_shadow_digger_click(Nothing, Nothing)
                    Case "S_shadow_duelist"
                        button_S_shadow_duelist_click(Nothing, Nothing)
                    Case "S_obsidian_machete"
                        button_S_obsidian_machete_click(Nothing, Nothing)
                    Case "S_obsidian_axe"
                        button_S_obsidian_axe_click(Nothing, Nothing)
                    Case "S_obsidian_spear"
                        button_S_obsidian_spear_click(Nothing, Nothing)
                    Case "S_volcano_staff"
                        button_S_volcano_staff_click(Nothing, Nothing)
                    Case "S_obsidian_armour"
                        button_S_obsidian_armour_click(Nothing, Nothing)
                    Case "S_obsidian_coconade"
                        button_S_obsidian_coconade_click(Nothing, Nothing)
                    Case "S_howling_conch"
                        button_S_howling_conch_click(Nothing, Nothing)
                    Case "S_sail_stick"
                        button_S_sail_stick_click(Nothing, Nothing)
                End Select
            Case "A"
                LeftTabItem_Animal.IsSelected = True
                Select Case VariableName
                    Case "A_rabbit"
                        button_A_rabbit_click(Nothing, Nothing)
                    Case "A_rabbit_winter"
                        button_A_rabbit_winter_click(Nothing, Nothing)
                    Case "A_beardling"
                        button_A_beardling_click(Nothing, Nothing)
                    Case "A_gobbler"
                        button_A_gobbler_click(Nothing, Nothing)
                    Case "A_frog"
                        button_A_frog_click(Nothing, Nothing)
                    Case "A_catcoon"
                        button_A_catcoon_click(Nothing, Nothing)
                    Case "A_moleworm"
                        button_A_moleworm_click(Nothing, Nothing)
                    Case "A_spider"
                        button_A_spider_click(Nothing, Nothing)
                    Case "A_spider_warrior"
                        button_A_spider_warrior_click(Nothing, Nothing)
                    Case "A_pig_man"
                        button_A_pig_man_click(Nothing, Nothing)
                    Case "A_guard_pig"
                        button_A_guard_pig_click(Nothing, Nothing)
                    Case "A_werepig"
                        button_A_werepig_click(Nothing, Nothing)
                    Case "A_baby_beefalo"
                        button_A_baby_beefalo_click(Nothing, Nothing)
                    Case "A_baby_beefalo_2"
                        button_A_toddler_beefalo_click(Nothing, Nothing)
                    Case "A_baby_beefalo_3"
                        button_A_teen_beefalo_click(Nothing, Nothing)
                    Case "A_beefalo"
                        button_A_beefalo_click(Nothing, Nothing)
                    Case "A_smallbird"
                        button_A_smallbird_click(Nothing, Nothing)
                    Case "A_teentallbird"
                        button_A_teentallbird_click(Nothing, Nothing)
                    Case "A_tallbird"
                        button_A_tallbird_click(Nothing, Nothing)
                    Case "A_pengull"
                        button_A_pengull_click(Nothing, Nothing)
                    Case "A_lureplants"
                        button_A_lureplants_click(Nothing, Nothing)
                    Case "A_eye_plant"
                        button_A_eye_plant_click(Nothing, Nothing)
                    Case "A_mandrake"
                        button_A_mandrake_click(Nothing, Nothing)
                    Case "A_merm"
                        button_A_merm_click(Nothing, Nothing)
                    Case "A_wee_mactusk"
                        button_A_wee_mactusk_click(Nothing, Nothing)
                    Case "A_mactusk"
                        button_A_mactusk_click(Nothing, Nothing)
                    Case "A_volt_goat"
                        button_A_volt_goat_click(Nothing, Nothing)
                    Case "A_volt_goat_withelectric"
                        button_A_volt_goat_withelectric_click(Nothing, Nothing)
                    Case "A_koalefant"
                        button_A_koalefant_click(Nothing, Nothing)
                    Case "A_winter_koalefant"
                        button_A_winter_koalefant_click(Nothing, Nothing)
                    Case "A_clockwork_knight"
                        button_A_clockwork_knight_click(Nothing, Nothing)
                    Case "A_clockwork_bishop"
                        button_A_clockwork_bishop_click(Nothing, Nothing)
                    Case "A_clckwork_rook"
                        button_A_clckwork_rook_click(Nothing, Nothing)
                    Case "A_grass_gekko"
                        button_A_grass_gekko_click(Nothing, Nothing)
                    Case "A_grass_gekko_disease"
                        button_A_grass_gekko_disease_click(Nothing, Nothing)
                    Case "A_crabbit"
                        button_A_crabbit_click(Nothing, Nothing)
                    Case "A_beardling_sw"
                        button_A_beardling_sw_click(Nothing, Nothing)
                    Case "A_prime_ape"
                        button_A_prime_ape_click(Nothing, Nothing)
                    Case "A_spider_warrior_sw"
                        button_A_spider_warrior_sw_click(Nothing, Nothing)
                    Case "A_fishermerm"
                        button_A_fishermerm_click(Nothing, Nothing)
                    Case "A_flup"
                        button_A_flup_click(Nothing, Nothing)
                    Case "A_wildbore"
                        button_A_wildbore_click(Nothing, Nothing)
                    Case "A_snake"
                        button_A_snake_click(Nothing, Nothing)
                    Case "A_poison_snake"
                        button_A_poison_snake_click(Nothing, Nothing)
                    Case "A_baby_doydoy"
                        button_A_baby_doydoy_click(Nothing, Nothing)
                    Case "A_doydoy_child"
                        button_A_doydoy_child_click(Nothing, Nothing)
                    Case "A_doydoy"
                        button_A_doydoy_click(Nothing, Nothing)
                    Case "A_dragoon"
                        button_A_dragoon_click(Nothing, Nothing)
                    Case "A_elephant_cactus"
                        button_A_elephant_cactus_click(Nothing, Nothing)
                    Case "A_dogfish"
                        button_A_dogfish_click(Nothing, Nothing)
                    Case "A_swordfish"
                        button_A_swordfish_click(Nothing, Nothing)
                    Case "A_wobster"
                        button_A_wobster_click(Nothing, Nothing)
                    Case "A_bioluminescence"
                        button_A_bioluminescence_click(Nothing, Nothing)
                    Case "A_jellyfish"
                        button_A_jellyfish_click(Nothing, Nothing)
                    Case "A_bottenosed_ballphin"
                        button_A_bottenosed_ballphin_click(Nothing, Nothing)
                    Case "A_blue_whale"
                        button_A_blue_whale_click(Nothing, Nothing)
                    Case "A_white_whale"
                        button_A_white_whale_click(Nothing, Nothing)
                    Case "A_floaty_boaty_knight"
                        button_A_floaty_boaty_knight_click(Nothing, Nothing)
                    Case "A_stink_ray"
                        button_A_stink_ray_click(Nothing, Nothing)
                    Case "A_water_beefalo"
                        button_A_water_beefalo_click(Nothing, Nothing)
                    Case "A_bee"
                        button_A_bee_click(Nothing, Nothing)
                    Case "A_killer_bee"
                        button_A_killer_bee_click(Nothing, Nothing)
                    Case "A_butterfly"
                        button_A_butterfly_click(Nothing, Nothing)
                    Case "A_butterfly_sw"
                        button_A_butterfly_sw_click(Nothing, Nothing)
                    Case "A_mosquito"
                        button_A_mosquito_click(Nothing, Nothing)
                    Case "A_mosquito_sw"
                        button_A_mosquito_sw_click(Nothing, Nothing)
                    Case "A_redbird"
                        button_A_redbird_click(Nothing, Nothing)
                    Case "A_snowbird"
                        button_A_snowbird_click(Nothing, Nothing)
                    Case "A_crow"
                        button_A_crow_click(Nothing, Nothing)
                    Case "A_buzzards"
                        button_A_buzzards_click(Nothing, Nothing)
                    Case "A_parrot"
                        button_A_parrot_click(Nothing, Nothing)
                    Case "A_parrot_pirate"
                        button_A_parrot_pirate_click(Nothing, Nothing)
                    Case "A_toucan"
                        button_A_toucan_click(Nothing, Nothing)
                    Case "A_seagull"
                        button_A_seagull_click(Nothing, Nothing)
                    Case "A_fireflies"
                        button_A_fireflies_click(Nothing, Nothing)
                    Case "A_bunnyman"
                        button_A_bunnyman_click(Nothing, Nothing)
                    Case "A_beardlord"
                        button_A_beardlord_click(Nothing, Nothing)
                    Case "A_blue_spore"
                        button_A_blue_spore_click(Nothing, Nothing)
                    Case "A_green_spore"
                        button_A_green_spore_click(Nothing, Nothing)
                    Case "A_red_spore"
                        button_A_red_spore_click(Nothing, Nothing)
                    Case "A_splumonkey"
                        button_A_splumonkey_click(Nothing, Nothing)
                    Case "A_shadow_splumonkey"
                        button_A_shadow_splumonkey_click(Nothing, Nothing)
                    Case "A_cave_spider"
                        button_A_cave_spider_click(Nothing, Nothing)
                    Case "A_spitter"
                        button_A_spitter_click(Nothing, Nothing)
                    Case "A_dangling_depth_dweller"
                        button_A_dangling_depth_dweller_click(Nothing, Nothing)
                    Case "A_batilisk"
                        button_A_batilisk_click(Nothing, Nothing)
                    Case "A_slurtles"
                        button_A_slurtles_click(Nothing, Nothing)
                    Case "A_snurtles"
                        button_A_snurtles_click(Nothing, Nothing)
                    Case "A_rock_lobster"
                        button_A_rock_lobster_click(Nothing, Nothing)
                    Case "A_slurper"
                        button_A_slurper_click(Nothing, Nothing)
                    Case "A_big_tentacle"
                        button_A_big_tentacle_click(Nothing, Nothing)
                    Case "A_baby_tentacle"
                        button_A_baby_tentacle_click(Nothing, Nothing)
                    Case "A_damaged_knight"
                        button_A_damaged_knight_click(Nothing, Nothing)
                    Case "A_damaged_bishop"
                        button_A_damaged_bishop_click(Nothing, Nothing)
                    Case "A_damage_rook"
                        button_A_damage_rook_click(Nothing, Nothing)
                    Case "A_tentacle"
                        button_A_tentacle_click(Nothing, Nothing)
                    Case "A_shadow_tentacle"
                        button_A_shadow_tentacle_click(Nothing, Nothing)
                    Case "A_hound"
                        button_A_hound_click(Nothing, Nothing)
                    Case "A_red_hound"
                        button_A_red_hound_click(Nothing, Nothing)
                    Case "A_blue_hound"
                        button_A_blue_hound_click(Nothing, Nothing)
                    Case "A_depths_worm"
                        button_A_depths_worm_click(Nothing, Nothing)
                    Case "A_sea_hound"
                        button_A_sea_hound_click(Nothing, Nothing)
                    Case "A_krampus"
                        button_A_krampus_click(Nothing, Nothing)
                    Case "A_krampus_sw"
                        button_A_krampus_sw_click(Nothing, Nothing)
                    Case "A_ghost"
                        button_A_ghost_click(Nothing, Nothing)
                    Case "A_pirate_ghost"
                        button_A_pirate_ghost_click(Nothing, Nothing)
                    Case "A_night_hand"
                        button_A_night_hand_click(Nothing, Nothing)
                    Case "A_shadow_watcher"
                        button_A_shadow_watcher_click(Nothing, Nothing)
                    Case "A_mr_skits"
                        button_A_mr_skits_click(Nothing, Nothing)
                    Case "A_mr_skittish"
                        button_A_mr_skittish_click(Nothing, Nothing)
                    Case "A_crawling_nightmare"
                        button_A_crawling_nightmare_click(Nothing, Nothing)
                    Case "A_crawling_horror"
                        button_A_crawling_horror_click(Nothing, Nothing)
                    Case "A_swimming_horror"
                        button_A_swimming_horror_click(Nothing, Nothing)
                    Case "A_charlie"
                        button_A_charlie_click(Nothing, Nothing)
                    Case "A_varg"
                        button_A_varg_click(Nothing, Nothing)
                    Case "A_ewecus"
                        button_A_ewecus_click(Nothing, Nothing)
                    Case "A_glommer"
                        button_A_glommer_click(Nothing, Nothing)
                    Case "A_chester"
                        button_A_chester_click(Nothing, Nothing)
                    Case "A_snow_chester"
                        button_A_snow_chester_click(Nothing, Nothing)
                    Case "A_shadow_chester"
                        button_A_shadow_chester_click(Nothing, Nothing)
                    Case "A_hutch"
                        button_A_hutch_click(Nothing, Nothing)
                    Case "A_fugu_hutch"
                        button_A_fugu_hutch_click(Nothing, Nothing)
                    Case "A_music_box_hutch"
                        button_A_music_box_hutch_click(Nothing, Nothing)
                    Case "A_packim_baggims"
                        button_A_packim_baggims_click(Nothing, Nothing)
                    Case "A_fat_packim_baggims"
                        button_A_fat_packim_baggims_click(Nothing, Nothing)
                    Case "A_fire_packim_baggims"
                        button_A_fire_packim_baggims_click(Nothing, Nothing)
                    Case "A_pig_king"
                        button_A_pig_king_click(Nothing, Nothing)
                    Case "A_yaarctopus"
                        button_A_yaarctopus_click(Nothing, Nothing)
                    Case "A_treeguard"
                        button_A_treeguard_1_click(Nothing, Nothing)
                    Case "A_treeguard"
                        button_A_treeguard_2_click(Nothing, Nothing)
                    Case "A_treeguard"
                        button_A_treeguard_3_click(Nothing, Nothing)
                    Case "A_poison_birchnut_trees"
                        button_A_poison_birchnut_trees_click(Nothing, Nothing)
                    Case "A_birchnutter"
                        button_A_birchnutter_click(Nothing, Nothing)
                    Case "A_palm_treeguard"
                        button_A_palm_treeguard_click(Nothing, Nothing)
                    Case "A_spider_queen"
                        button_A_spider_queen_click(Nothing, Nothing)
                    Case "A_ancient_guardian"
                        button_A_ancient_guardian_click(Nothing, Nothing)
                    Case "A_moose"
                        button_A_moose_click(Nothing, Nothing)
                    Case "A_mosling"
                        button_A_mosling_click(Nothing, Nothing)
                    Case "A_dragonfly"
                        button_A_dragonfly_click(Nothing, Nothing)
                    Case "A_lavae"
                        button_A_lavae_click(Nothing, Nothing)
                    Case "A_lavae"
                        button_A_Extra_Adorable_Lavae_click(Nothing, Nothing)
                    Case "A_bearger"
                        button_A_bearger_click(Nothing, Nothing)
                    Case "A_deerclops"
                        button_A_deerclops_click(Nothing, Nothing)
                    Case "A_quacken"
                        button_A_quacken_click(Nothing, Nothing)
                    Case "A_quacken_tentacle"
                        button_A_quacken_tentacle_click(Nothing, Nothing)
                    Case "A_sealnado"
                        button_A_sealnado_click(Nothing, Nothing)
                    Case "A_seal"
                        button_A_seal_click(Nothing, Nothing)
                    Case "A_tiger_shark"
                        button_A_tiger_shark_click(Nothing, Nothing)
                    Case "A_sharkitten"
                        button_A_sharkitten_click(Nothing, Nothing)
                End Select
            Case "N"
                LeftTabItem_Natural.IsSelected = True
                Select Case VariableName
                    Case "N_grasslands"
                        button_N_grasslands_click(Nothing, Nothing)
                    Case "N_savanna"
                        button_N_savanna_click(Nothing, Nothing)
                    Case "N_deciduous_forest"
                        button_N_deciduous_forest_click(Nothing, Nothing)
                    Case "N_forest"
                        button_N_forest_click(Nothing, Nothing)
                    Case "N_desert"
                        button_N_desert_click(Nothing, Nothing)
                    Case "N_graveyard"
                        button_N_graveyard_click(Nothing, Nothing)
                    Case "N_marsh"
                        button_N_marsh_click(Nothing, Nothing)
                    Case "N_chess"
                        button_N_chess_click(Nothing, Nothing)
                    Case "N_mosaic"
                        button_N_mosaic_click(Nothing, Nothing)
                    Case "N_rockyland"
                        button_N_rockyland_click(Nothing, Nothing)
                    Case "N_ocean"
                        button_N_ocean_click(Nothing, Nothing)
                    Case "N_spider_infested"
                        button_N_spider_infested_click(Nothing, Nothing)
                    Case "N_light_flower_swamp"
                        button_N_light_flower_swamp_click(Nothing, Nothing)
                    Case "N_mushtree_forest"
                        button_N_mushtree_forest_click(Nothing, Nothing)
                    Case "N_blue_mushtree_forest"
                        button_N_blue_mushtree_forest_click(Nothing, Nothing)
                    Case "N_green_mushtree_forest"
                        button_N_green_mushtree_forest_click(Nothing, Nothing)
                    Case "N_red_mushtree_forest"
                        button_N_red_mushtree_forest_click(Nothing, Nothing)
                    Case "N_rocky_plains"
                        button_N_rocky_plains_click(Nothing, Nothing)
                    Case "N_stalagmite_biomes"
                        button_N_stalagmite_biomes_click(Nothing, Nothing)
                    Case "N_tall_stalagmite_biomes"
                        button_N_tall_stalagmite_biomes_click(Nothing, Nothing)
                    Case "N_sunken_forest"
                        button_N_sunken_forest_click(Nothing, Nothing)
                    Case "N_labyrinth"
                        button_N_labyrinth_click(Nothing, Nothing)
                    Case "N_military"
                        button_N_military_click(Nothing, Nothing)
                    Case "N_sacred"
                        button_N_sacred_click(Nothing, Nothing)
                    Case "N_village"
                        button_N_village_click(Nothing, Nothing)
                    Case "N_wilds"
                        button_N_wilds_click(Nothing, Nothing)
                    Case "N_beach"
                        button_N_beach_click(Nothing, Nothing)
                    Case "N_jungle"
                        button_N_jungle_click(Nothing, Nothing)
                    Case "N_mangrove_bio"
                        button_N_mangrove_bio_click(Nothing, Nothing)
                    Case "N_magma_field"
                        button_N_magma_field_click(Nothing, Nothing)
                    Case "N_meadow"
                        button_N_meadow_click(Nothing, Nothing)
                    Case "N_tidal_marsh"
                        button_N_tidal_marsh_click(Nothing, Nothing)
                    Case "N_coral_reef_bio"
                        button_N_coral_reef_bio_click(Nothing, Nothing)
                    Case "N_ocean_shallow"
                        button_N_ocean_shallow_click(Nothing, Nothing)
                    Case "N_ocean_medium"
                        button_N_ocean_medium_click(Nothing, Nothing)
                    Case "N_ocean_deep"
                        button_N_ocean_deep_click(Nothing, Nothing)
                    Case "N_ship_graveyard"
                        button_N_ship_graveyard_click(Nothing, Nothing)
                    Case "N_volcano_bio"
                        button_N_volcano_bio_click(Nothing, Nothing)
                    Case "N_flower"
                        button_N_flower_click(Nothing, Nothing)
                    Case "N_evil_flower"
                        button_N_evil_flower_click(Nothing, Nothing)
                    Case "N_sapling"
                        button_N_sapling_click(Nothing, Nothing)
                    Case "N_grass"
                        button_N_grass_click(Nothing, Nothing)
                    Case "N_berry_bush"
                        button_N_berry_bush_click(Nothing, Nothing)
                    Case "N_berry_bush_2"
                        button_N_berry_bush_2_click(Nothing, Nothing)
                    Case "N_juicy_berry_bush"
                        button_N_juicy_berry_bush_click(Nothing, Nothing)
                    Case "N_reeds"
                        button_N_reeds_click(Nothing, Nothing)
                    Case "N_spiky_bush"
                        button_N_spiky_bush_click(Nothing, Nothing)
                    Case "N_cactus"
                        button_N_cactus_click(Nothing, Nothing)
                    Case "N_plant"
                        button_N_plant_click(Nothing, Nothing)
                    Case "N_algae"
                        button_N_algae_click(Nothing, Nothing)
                    Case "N_blue_mushroom"
                        button_N_blue_mushroom_click(Nothing, Nothing)
                    Case "N_green_mushroom"
                        button_N_green_mushroom_click(Nothing, Nothing)
                    Case "N_red_mushroom"
                        button_N_red_mushroom_click(Nothing, Nothing)
                    Case "N_light_flower_1"
                        button_N_light_flower_1_click(Nothing, Nothing)
                    Case "N_fern_1"
                        button_N_fern_1_click(Nothing, Nothing)
                    Case "N_cave_lichen"
                        button_N_cave_lichen_click(Nothing, Nothing)
                    Case "N_grass_sw"
                        button_N_grass_sw_click(Nothing, Nothing)
                    Case "N_bamboo_patch"
                        button_N_bamboo_patch_click(Nothing, Nothing)
                    Case "N_viney_bush"
                        button_N_viney_bush_click(Nothing, Nothing)
                    Case "N_seaweed"
                        button_N_seaweed_click(Nothing, Nothing)
                    Case "N_coffee_plant"
                        button_N_coffee_plant_click(Nothing, Nothing)
                    Case "N_elephant_cactus"
                        button_N_elephant_cactus_click(Nothing, Nothing)
                    Case "N_sapling_diseased"
                        button_N_sapling_diseased_click(Nothing, Nothing)
                    Case "N_grass_diseased"
                        button_N_grass_diseased_click(Nothing, Nothing)
                    Case "N_berry_bush_diseased"
                        button_N_berry_bush_diseased_click(Nothing, Nothing)
                    Case "N_juicy_berry_bush_diseased"
                        button_N_juicy_berry_bush_diseased_click(Nothing, Nothing)
                    Case "N_evergreen"
                        button_N_evergreen_click(Nothing, Nothing)
                    Case "N_lumpy_evergreen"
                        button_N_lumpy_evergreen_click(Nothing, Nothing)
                    Case "N_birchnut_tree"
                        button_N_birchnut_tree_click(Nothing, Nothing)
                    Case "N_totally_normal_tree"
                        button_N_totally_normal_tree_click(Nothing, Nothing)
                    Case "N_spiky_tree"
                        button_N_spiky_tree_click(Nothing, Nothing)
                    Case "N_blue_mushtree"
                        button_N_blue_mushtree_click(Nothing, Nothing)
                    Case "N_green_mushtree"
                        button_N_green_mushtree_click(Nothing, Nothing)
                    Case "N_red_mushtree"
                        button_N_red_mushtree_click(Nothing, Nothing)
                    Case "N_webbed_blue_mushtree"
                        button_N_webbed_blue_mushtree_click(Nothing, Nothing)
                    Case "N_cave_banana_tree"
                        button_N_cave_banana_tree_click(Nothing, Nothing)
                    Case "N_palm_tree"
                        button_N_palm_tree_click(Nothing, Nothing)
                    Case "N_jungle_tree"
                        button_N_jungle_tree_click(Nothing, Nothing)
                    Case "N_mangrove"
                        button_N_mangrove_click(Nothing, Nothing)
                    Case "N_regular_jungle_tree"
                        button_N_regular_jungle_tree_click(Nothing, Nothing)
                    Case "N_burnt_ash_tree"
                        button_N_burnt_ash_tree_click(Nothing, Nothing)
                    Case "N_brainy_sprout"
                        button_N_brainy_sprout_click(Nothing, Nothing)
                    Case "N_twiggy_tree"
                        button_N_twiggy_tree_click(Nothing, Nothing)
                    Case "N_twiggy_tree_diseased"
                        button_N_twiggy_tree_diseased_click(Nothing, Nothing)
                    Case "N_petrified_tree"
                        button_N_petrified_tree_click(Nothing, Nothing)

                End Select
            Case "G"
                LeftTabItem_Goods.IsSelected = True
                Select Case VariableName
                    Case "G_ash"
                        button_G_ash_click(Nothing, Nothing)
                    Case "G_flint"
                        button_G_flint_click(Nothing, Nothing)
                    Case "G_nitre"
                        button_G_nitre_click(Nothing, Nothing)
                    Case "G_rocks"
                        button_G_rocks_click(Nothing, Nothing)
                    Case "G_gold_nugget"
                        button_G_gold_nugget_click(Nothing, Nothing)
                    Case "G_marble"
                        button_G_marble_click(Nothing, Nothing)
                    Case "G_moon_rock"
                        button_G_moon_rock_click(Nothing, Nothing)
                    Case "G_twigs"
                        button_G_twigs_click(Nothing, Nothing)
                    Case "G_cut_grass"
                        button_G_cut_grass_click(Nothing, Nothing)
                    Case "G_log"
                        button_G_log_click(Nothing, Nothing)
                    Case "G_charcoal"
                        button_G_charcoal_click(Nothing, Nothing)
                    Case "G_cut_reeds"
                        button_G_cut_reeds_click(Nothing, Nothing)
                    Case "G_petals"
                        button_G_petals_click(Nothing, Nothing)
                    Case "G_dark_petals"
                        button_G_dark_petals_click(Nothing, Nothing)
                    Case "G_boneshard"
                        button_G_boneshard_click(Nothing, Nothing)
                    Case "G_stinger"
                        button_G_stinger_click(Nothing, Nothing)
                    Case "G_hound's_tooth"
                        button_G_hounds_tooth_click(Nothing, Nothing)
                    Case "G_azure_feather"
                        button_G_azure_feather_click(Nothing, Nothing)
                    Case "G_crimson_feather"
                        button_G_crimson_feather_click(Nothing, Nothing)
                    Case "G_jet_feather"
                        button_G_jet_feather_click(Nothing, Nothing)
                    Case "G_pig_skin"
                        button_G_pig_skin_click(Nothing, Nothing)
                    Case "G_beefalo_wool"
                        button_G_beefalo_wool_click(Nothing, Nothing)
                    Case "G_beefalo_horn"
                        button_G_beefalo_horn_click(Nothing, Nothing)
                    Case "G_manure"
                        button_G_manure_click(Nothing, Nothing)
                    Case "G_guano"
                        button_G_guano_click(Nothing, Nothing)
                    Case "G_coontail"
                        button_G_coontail_click(Nothing, Nothing)
                    Case "G_fleshy_bulb"
                        button_G_fleshy_bulb_click(Nothing, Nothing)
                    Case "G_silk"
                        button_G_silk_click(Nothing, Nothing)
                    Case "G_spider_gland"
                        button_G_spider_gland_click(Nothing, Nothing)
                    Case "G_gears"
                        button_G_gears_click(Nothing, Nothing)
                    Case "G_glommer's_flower"
                        button_G_glommers_flower_click(Nothing, Nothing)
                    Case "G_glommer's_wings"
                        button_G_glommers_wings_click(Nothing, Nothing)
                    Case "G_tentacle_spots"
                        button_G_tentacle_spots_click(Nothing, Nothing)
                    Case "G_nightmare_fuel"
                        button_G_nightmare_fuel_click(Nothing, Nothing)
                    Case "G_living_log"
                        button_G_living_log_click(Nothing, Nothing)
                    Case "G_mosquito_sack"
                        button_G_mosquito_sack_click(Nothing, Nothing)
                    Case "G_volt_goat_horn"
                        button_G_volt_goat_horn_click(Nothing, Nothing)
                    Case "G_walrus_tusk"
                        button_G_walrus_tusk_click(Nothing, Nothing)
                    Case "G_steel_wool"
                        button_G_steel_wool_click(Nothing, Nothing)
                    Case "G_phlegm"
                        button_G_phlegm_click(Nothing, Nothing)
                    Case "G_red_gem"
                        button_G_red_gem_click(Nothing, Nothing)
                    Case "G_blue_gem"
                        button_G_blue_gem_click(Nothing, Nothing)
                    Case "G_purple_gem"
                        button_G_purple_gem_click(Nothing, Nothing)
                    Case "G_orange_gem"
                        button_G_orange_gem_click(Nothing, Nothing)
                    Case "G_green_gem"
                        button_G_green_gem_click(Nothing, Nothing)
                    Case "G_yellow_gem"
                        button_G_yellow_gem_click(Nothing, Nothing)
                    Case "G_light_bulb"
                        button_G_light_bulb_click(Nothing, Nothing)
                    Case "G_bunny_puff"
                        button_G_bunny_puff_click(Nothing, Nothing)
                    Case "G_foliage"
                        button_G_foliage_click(Nothing, Nothing)
                    Case "G_broken_shell"
                        button_G_broken_shell_click(Nothing, Nothing)
                    Case "G_slurtle_slime"
                        button_G_slurtle_slime_click(Nothing, Nothing)
                    Case "G_beard_hair"
                        button_G_beard_hair_click(Nothing, Nothing)
                    Case "G_slurper_pelt"
                        button_G_slurper_pelt_click(Nothing, Nothing)
                    Case "G_thulecite_fragments"
                        button_G_thulecite_fragments_click(Nothing, Nothing)
                    Case "G_down_feather"
                        button_G_down_feather_click(Nothing, Nothing)
                    Case "G_scales"
                        button_G_scales_click(Nothing, Nothing)
                    Case "G_fur_tuft"
                        button_G_fur_tuft_click(Nothing, Nothing)
                    Case "G_thick_fur"
                        button_G_thick_fur_click(Nothing, Nothing)
                    Case "G_bamboo_patch"
                        button_G_bamboo_patch_click(Nothing, Nothing)
                    Case "G_cut_grass_SW"
                        button_G_cut_grass_SW_click(Nothing, Nothing)
                    Case "G_vine"
                        button_G_vine_click(Nothing, Nothing)
                    Case "G_sand"
                        button_G_sand_click(Nothing, Nothing)
                    Case "G_snakeskin"
                        button_G_snakeskin_click(Nothing, Nothing)
                    Case "G_snake_oil"
                        button_G_snake_oil_click(Nothing, Nothing)
                    Case "G_palm_leaf"
                        button_G_palm_leaf_click(Nothing, Nothing)
                    Case "G_venom_gland"
                        button_G_venom_gland_click(Nothing, Nothing)
                    Case "G_yellow_mosquito_sack"
                        button_G_yellow_mosquito_sack_click(Nothing, Nothing)
                    Case "G_seashell"
                        button_G_seashell_click(Nothing, Nothing)
                    Case "G_doydoy_feather"
                        button_G_doydoy_feather_click(Nothing, Nothing)
                    Case "G_dubloons"
                        button_G_dubloons_click(Nothing, Nothing)
                    Case "G_hail"
                        button_G_hail_click(Nothing, Nothing)
                    Case "G_horn"
                        button_G_horn_click(Nothing, Nothing)
                    Case "G_coral"
                        button_G_coral_click(Nothing, Nothing)
                    Case "G_obsidian"
                        button_G_obsidian_click(Nothing, Nothing)
                    Case "G_cactus_spike"
                        button_G_cactus_spike_click(Nothing, Nothing)
                    Case "G_dragoon_heart"
                        button_G_dragoon_heart_click(Nothing, Nothing)
                    Case "G_turbine_blades"
                        button_G_turbine_blades_click(Nothing, Nothing)
                    Case "G_magic_seal"
                        button_G_magic_seal_click(Nothing, Nothing)
                    Case "G_shark_gills"
                        button_G_shark_gills_click(Nothing, Nothing)
                    Case "G_axe"
                        button_G_axe_click(Nothing, Nothing)
                    Case "G_goldenaxe"
                        button_G_goldenaxe_click(Nothing, Nothing)
                    Case "G_machete"
                        button_G_machete_click(Nothing, Nothing)
                    Case "G_luxury_machete"
                        button_G_luxury_machete_click(Nothing, Nothing)
                    Case "G_pickaxe"
                        button_G_pickaxe_click(Nothing, Nothing)
                    Case "G_goldenpickaxe"
                        button_G_goldenpickaxe_click(Nothing, Nothing)
                    Case "G_shovel"
                        button_G_shovel_click(Nothing, Nothing)
                    Case "G_goldenshovel"
                        button_G_goldenshovel_click(Nothing, Nothing)
                    Case "G_hammer"
                        button_G_hammer_click(Nothing, Nothing)
                    Case "G_pitchfork"
                        button_G_pitchfork_click(Nothing, Nothing)
                    Case "G_saddlehorn"
                        button_G_saddlehorn_click(Nothing, Nothing)
                    Case "G_brush"
                        button_G_brush_click(Nothing, Nothing)
                    Case "G_pickaxe_1"
                        button_G_pickaxe_1_click(Nothing, Nothing)
                    Case "G_obsidian_machete"
                        button_G_obsidian_machete_click(Nothing, Nothing)
                    Case "G_obsidian_axe"
                        button_G_obsidian_axe_click(Nothing, Nothing)
                    Case "G_lucy_the_axe"
                        button_G_lucy_the_axe_click(Nothing, Nothing)
                    Case "G_bug_net"
                        button_G_bug_net_click(Nothing, Nothing)
                    Case "G_fishing_rod"
                        button_G_fishing_rod_click(Nothing, Nothing)
                    Case "G_walking_cane"
                        button_G_walking_cane_click(Nothing, Nothing)
                    Case "G_rawling"
                        button_G_rawling_click(Nothing, Nothing)
                    Case "G_torch"
                        button_G_torch_click(Nothing, Nothing)
                    Case "G_miner_hat"
                        button_G_miner_hat_click(Nothing, Nothing)
                    Case "G_pirate_hat"
                        button_G_pirate_hat_click(Nothing, Nothing)
                    Case "G_tropical_parasol"
                        button_G_tropical_parasol_click(Nothing, Nothing)
                    Case "G_pretty_parasol"
                        button_G_pretty_parasol_click(Nothing, Nothing)
                    Case "G_umbrella"
                        button_G_umbrella_click(Nothing, Nothing)
                    Case "G_straw_hat"
                        button_G_straw_hat_click(Nothing, Nothing)
                    Case "G_top_hat"
                        button_G_top_hat_click(Nothing, Nothing)
                    Case "G_rain_hat"
                        button_G_rain_hat_click(Nothing, Nothing)
                    Case "G_rain_coat"
                        button_G_rain_coat_click(Nothing, Nothing)
                    Case "G_snakeskin_hat"
                        button_G_snakeskin_hat_click(Nothing, Nothing)
                    Case "G_snakeskin_jacket"
                        button_G_snakeskin_jacket_click(Nothing, Nothing)
                    Case "G_blubber_suit"
                        button_G_blubber_suit_click(Nothing, Nothing)
                    Case "G_eyebrella"
                        button_G_eyebrella_click(Nothing, Nothing)
                    Case "G_dumbrella"
                        button_G_dumbrella_click(Nothing, Nothing)
                    Case "G_windbreaker"
                        button_G_windbreaker_click(Nothing, Nothing)
                    Case "G_thermal_stone"
                        button_G_thermal_stone_click(Nothing, Nothing)
                    Case "G_whirly_fan"
                        button_G_whirly_fan_click(Nothing, Nothing)
                    Case "G_rabbit_earmuffs"
                        button_G_rabbit_earmuffs_click(Nothing, Nothing)
                    Case "G_beefalo_hat"
                        button_G_beefalo_hat_click(Nothing, Nothing)
                    Case "G_winter_hat"
                        button_G_winter_hat_click(Nothing, Nothing)
                    Case "G_cat_cap"
                        button_G_cat_cap_click(Nothing, Nothing)
                    Case "G_fashion_melon"
                        button_G_fashion_melon_click(Nothing, Nothing)
                    Case "G_ice_cube"
                        button_G_ice_cube_click(Nothing, Nothing)
                    Case "G_dapper_vest"
                        button_G_dapper_vest_click(Nothing, Nothing)
                    Case "G_breezy_vest"
                        button_G_breezy_vest_click(Nothing, Nothing)
                    Case "G_puffy_vest"
                        button_G_puffy_vest_click(Nothing, Nothing)
                    Case "G_summer_frest"
                        button_G_summer_frest_click(Nothing, Nothing)
                    Case "G_floral_shirt"
                        button_G_floral_shirt_click(Nothing, Nothing)
                    Case "G_hibearnation_vest"
                        button_G_hibearnation_vest_click(Nothing, Nothing)
                    Case "G_boat_cannon"
                        button_G_boat_cannon_click(Nothing, Nothing)
                    Case "G_gunpowder"
                        button_G_gunpowder_click(Nothing, Nothing)
                    Case "G_coconade"
                        button_G_coconade_click(Nothing, Nothing)
                    Case "G_obsidian_coconade"
                        button_G_obsidian_coconade_click(Nothing, Nothing)
                    Case "G_battle_spear"
                        button_G_battle_spear_click(Nothing, Nothing)
                    Case "G_spear"
                        button_G_spear_click(Nothing, Nothing)
                    Case "G_poison_spear"
                        button_G_poison_spear_click(Nothing, Nothing)
                    Case "G_ham_bat"
                        button_G_ham_bat_click(Nothing, Nothing)
                    Case "G_morning_star"
                        button_G_morning_star_click(Nothing, Nothing)
                    Case "G_tail_o'_three_cats"
                        button_G_tail_o_three_cats_click(Nothing, Nothing)
                    Case "G_blow_dart"
                        button_G_blow_dart_click(Nothing, Nothing)
                    Case "G_sleep_dart"
                        button_G_sleep_dart_click(Nothing, Nothing)
                    Case "G_fire_dart"
                        button_G_fire_dart_click(Nothing, Nothing)
                    Case "G_poison_dart"
                        button_G_poison_dart_click(Nothing, Nothing)
                    Case "G_boomerang"
                        button_G_boomerang_click(Nothing, Nothing)
                    Case "G_weather_pain"
                        button_G_weather_pain_click(Nothing, Nothing)
                    Case "G_spear_gun"
                        button_G_spear_gun_click(Nothing, Nothing)
                    Case "G_cutlass_supreme"
                        button_G_cutlass_supreme_click(Nothing, Nothing)
                    Case "G_dark_sword"
                        button_G_dark_sword_click(Nothing, Nothing)
                    Case "G_bat_bat"
                        button_G_bat_bat_click(Nothing, Nothing)
                    Case "G_belt_of_hunger"
                        button_G_belt_of_hunger_click(Nothing, Nothing)
                    Case "G_fire_staff"
                        button_G_fire_staff_click(Nothing, Nothing)
                    Case "G_ice_staff"
                        button_G_ice_staff_click(Nothing, Nothing)
                    Case "G_thulecite_club"
                        button_G_thulecite_club_click(Nothing, Nothing)
                    Case "G_obsidian_spear"
                        button_G_obsidian_spear_click(Nothing, Nothing)
                    Case "G_tentacle_spike"
                        button_G_tentacle_spike_click(Nothing, Nothing)
                    Case "G_eyeshot"
                        button_G_eyeshot_click(Nothing, Nothing)
                    Case "G_harpoon"
                        button_G_harpoon_click(Nothing, Nothing)
                    Case "G_peg_leg"
                        button_G_peg_leg_click(Nothing, Nothing)
                    Case "G_trident"
                        button_G_trident_click(Nothing, Nothing)
                    Case "G_battle_helm"
                        button_G_battle_helm_click(Nothing, Nothing)
                    Case "G_grass_suit"
                        button_G_grass_suit_click(Nothing, Nothing)
                    Case "G_log_suit"
                        button_G_log_suit_click(Nothing, Nothing)
                    Case "G_marble_suit"
                        button_G_marble_suit_click(Nothing, Nothing)
                    Case "G_seashell_suit"
                        button_G_seashell_suit_click(Nothing, Nothing)
                    Case "G_limestone_suit"
                        button_G_limestone_suit_click(Nothing, Nothing)
                    Case "G_cactus_armour"
                        button_G_cactus_armour_click(Nothing, Nothing)
                    Case "G_football_helmet"
                        button_G_football_helmet_click(Nothing, Nothing)
                    Case "G_horned_helmet"
                        button_G_horned_helmet_click(Nothing, Nothing)
                    Case "G_scalemail"
                        button_G_scalemail_click(Nothing, Nothing)
                    Case "G_night_armour"
                        button_G_night_armour_click(Nothing, Nothing)
                    Case "G_beekeeper_hat"
                        button_G_beekeeper_hat_click(Nothing, Nothing)
                    Case "G_thulecite_crown"
                        button_G_thulecite_crown_click(Nothing, Nothing)
                    Case "G_thulecite_suit"
                        button_G_thulecite_suit_click(Nothing, Nothing)
                    Case "G_obsidian_armour"
                        button_G_obsidian_armour_click(Nothing, Nothing)
                    Case "G_shelmet"
                        button_G_shelmet_click(Nothing, Nothing)
                    Case "G_snurtle_shell_armor"
                        button_G_snurtle_shell_armor_click(Nothing, Nothing)
                    Case "G_tam_o'shanter"
                        button_G_tam_o_shanter_click(Nothing, Nothing)
                    Case "G_spiderhat"
                        button_G_spiderhat_click(Nothing, Nothing)
                    Case "G_slurper"
                        button_G_slurper_click(Nothing, Nothing)
                    Case "G_krampus_sack"
                        button_G_krampus_sack_click(Nothing, Nothing)
                    Case "G_portable_crock_pot"
                        button_G_portable_crock_pot_click(Nothing, Nothing)
                    Case "G_birchnut"
                        button_G_birchnut_click(Nothing, Nothing)
                    Case "G_pine_cone"
                        button_G_pine_cone_click(Nothing, Nothing)
                    Case "G_sapling"
                        button_G_sapling_click(Nothing, Nothing)
                    Case "G_grass_tuft"
                        button_G_grass_tuft_click(Nothing, Nothing)
                    Case "G_berry_bush"
                        button_G_berry_bush_click(Nothing, Nothing)
                    Case "G_berry_bush_2"
                        button_G_berry_bush_2_click(Nothing, Nothing)
                    Case "G_spiky_bushes"
                        button_G_spiky_bushes_click(Nothing, Nothing)
                    Case "G_bamboo_root"
                        button_G_bamboo_root_click(Nothing, Nothing)
                    Case "G_grass_tuft_SW"
                        button_G_grass_tuft_SW_click(Nothing, Nothing)
                    Case "G_coconut"
                        button_G_coconut_click(Nothing, Nothing)
                    Case "G_jungle_tree_seed"
                        button_G_jungle_tree_seed_click(Nothing, Nothing)
                    Case "G_viney_bush_root"
                        button_G_viney_bush_root_click(Nothing, Nothing)
                    Case "G_coffee_plant"
                        button_G_coffee_plant_click(Nothing, Nothing)
                    Case "G_elephant_cactus_stump"
                        button_G_elephant_cactus_stump_click(Nothing, Nothing)
                    Case "G_juicy_berry_bush"
                        button_G_juicy_berry_bush_click(Nothing, Nothing)
                    Case "G_twiggy_tree_cone"
                        button_G_twiggy_tree_cone_click(Nothing, Nothing)
                    Case "G_rabbit"
                        button_G_rabbit_click(Nothing, Nothing)
                    Case "G_rabbit_winter"
                        button_G_winter_rabbit_click(Nothing, Nothing)
                    Case "G_beardling"
                        button_G_beardling_click(Nothing, Nothing)
                    Case "G_moleworm"
                        button_G_moleworm_click(Nothing, Nothing)
                    Case "G_bee"
                        button_G_bee_click(Nothing, Nothing)
                    Case "G_killer_bee"
                        button_G_killer_bee_click(Nothing, Nothing)
                    Case "G_butterfly"
                        button_G_butterfly_click(Nothing, Nothing)
                    Case "G_mosquito"
                        button_G_mosquito_click(Nothing, Nothing)
                    Case "G_redbird"
                        button_G_redbird_click(Nothing, Nothing)
                    Case "G_snowbird"
                        button_G_snowbird_click(Nothing, Nothing)
                    Case "G_crow"
                        button_G_crow_click(Nothing, Nothing)
                    Case "G_fireflies"
                        button_G_fireflies_click(Nothing, Nothing)
                    Case "G_crabbit"
                        button_G_crabbit_click(Nothing, Nothing)
                    Case "G_beardling_sw"
                        button_G_beardling_sw_click(Nothing, Nothing)
                    Case "G_dead_dogfish"
                        button_G_dead_dogfish_click(Nothing, Nothing)
                    Case "G_dead_swordfish"
                        button_G_dead_swordfish_click(Nothing, Nothing)
                    Case "G_dead_wobster"
                        button_G_dead_wobster_click(Nothing, Nothing)
                    Case "G_bioluminescence"
                        button_G_bioluminescence_click(Nothing, Nothing)
                    Case "G_jellyfish"
                        button_G_jellyfish_click(Nothing, Nothing)
                    Case "G_dead_jellyfish"
                        button_G_dead_jellyfish_click(Nothing, Nothing)
                    Case "G_butterfly_sw"
                        button_G_butterfly_sw_click(Nothing, Nothing)
                    Case "G_mosquito_sw"
                        button_G_mosquito_sw_click(Nothing, Nothing)
                    Case "G_parrot"
                        button_G_parrot_click(Nothing, Nothing)
                    Case "G_parrot_pirate"
                        button_G_parrot_pirate_click(Nothing, Nothing)
                    Case "G_toucan"
                        button_G_toucan_click(Nothing, Nothing)
                    Case "G_seagull"
                        button_G_seagull_click(Nothing, Nothing)
                    Case "G_blue_spore"
                        button_G_blue_spore_click(Nothing, Nothing)
                    Case "G_green_spore"
                        button_G_green_spore_click(Nothing, Nothing)
                    Case "G_red_spore"
                        button_G_red_spore_click(Nothing, Nothing)
                    Case "G_cobblestones"
                        button_G_cobblestones_click(Nothing, Nothing)
                    Case "G_wooden_flooring"
                        button_G_wooden_flooring_click(Nothing, Nothing)
                    Case "G_checkered_flooring"
                        button_G_checkered_flooring_click(Nothing, Nothing)
                    Case "G_carpeted_flooring"
                        button_G_carpeted_flooring_click(Nothing, Nothing)
                    Case "G_scaled_flooring"
                        button_G_scaled_flooring_click(Nothing, Nothing)
                    Case "G_snakeskin_rug"
                        button_G_snakeskin_rug_click(Nothing, Nothing)
                    Case "G_deciduous_turf"
                        button_G_deciduous_turf_click(Nothing, Nothing)
                    Case "G_forest_turf"
                        button_G_forest_turf_click(Nothing, Nothing)
                    Case "G_grass_turf"
                        button_G_grass_turf_click(Nothing, Nothing)
                    Case "G_jungle_turf"
                        button_G_jungle_turf_click(Nothing, Nothing)
                    Case "G_magma_turf"
                        button_G_magma_turf_click(Nothing, Nothing)
                    Case "G_marsh_turf"
                        button_G_marsh_turf_click(Nothing, Nothing)
                    Case "G_meadow_turf"
                        button_G_meadow_turf_click(Nothing, Nothing)
                    Case "G_rocky_turf"
                        button_G_rocky_turf_click(Nothing, Nothing)
                    Case "G_sandy_turf"
                        button_G_sandy_turf_click(Nothing, Nothing)
                    Case "G_savanna_turf"
                        button_G_savanna_turf_click(Nothing, Nothing)
                    Case "G_tidal_marsh_turf"
                        button_G_tidal_marsh_turf_click(Nothing, Nothing)
                    Case "G_cave_rock_turf"
                        button_G_cave_rock_turf_click(Nothing, Nothing)
                    Case "G_fungal_turf_blue"
                        button_G_fungal_turf_blue_click(Nothing, Nothing)
                    Case "G_fungal_turf_green"
                        button_G_fungal_turf_green_click(Nothing, Nothing)
                    Case "G_fungal_turf_red"
                        button_G_fungal_turf_red_click(Nothing, Nothing)
                    Case "G_guano_turf"
                        button_G_guano_turf_click(Nothing, Nothing)
                    Case "G_mud_turf"
                        button_G_mud_turf_click(Nothing, Nothing)
                    Case "G_slimey_turf"
                        button_G_slimey_turf_click(Nothing, Nothing)
                    Case "G_ashy_turf"
                        button_G_ashy_turf_click(Nothing, Nothing)
                    Case "G_volcano_turf"
                        button_G_volcano_turf_click(Nothing, Nothing)
                    Case "G_sticky_webbing"
                        button_G_sticky_webbing_click(Nothing, Nothing)
                    Case "G_beach_turf"
                        button_G_beach_turf_click(Nothing, Nothing)
                    Case "G_eye_bone"
                        button_G_eye_bone_Click(Nothing, Nothing)
                    Case "G_fishbone"
                        button_G_fishbone_Click(Nothing, Nothing)
                    Case "G_lavae_egg"
                        button_G_lavae_egg_Click(Nothing, Nothing)
                    Case "G_lavae_tooth"
                        button_G_lavae_tooth_Click(Nothing, Nothing)
                    Case "G_star_sky"
                        button_G_star_sky_Click(Nothing, Nothing)
                    Case "G_webber's_skull"
                        button_G_webbers_skull_Click(Nothing, Nothing)
                    Case "G_iron_key"
                        button_G_iron_key_Click(Nothing, Nothing)
                    Case "G_golden_key"
                        button_G_golden_key_Click(Nothing, Nothing)
                    Case "G_bone_key"
                        button_G_bone_key_Click(Nothing, Nothing)
                    Case "G_tarnished_crown"
                        button_G_tarnished_crown_Click(Nothing, Nothing)
                    Case "G_box_thing_1"
                        button_G_box_thing_Click(Nothing, Nothing)
                    Case "G_crank_thing_1"
                        button_G_crank_thing_Click(Nothing, Nothing)
                    Case "G_metal_potato_thing_1"
                        button_G_metal_potato_thing_Click(Nothing, Nothing)
                    Case "G_ring_thing_1"
                        button_G_ring_thing_Click(Nothing, Nothing)
                    Case "G_grassy_thing"
                        button_G_grassy_thing_Click(Nothing, Nothing)
                    Case "G_ring_thing"
                        button_G_ring_thing_Click(Nothing, Nothing)
                    Case "G_screw_thing"
                        button_G_screw_thing_Click(Nothing, Nothing)
                    Case "G_wooden_potato_thing"
                        button_G_wooden_potato_thing_Click(Nothing, Nothing)
                    Case "G_blueprint"
                        button_G_blueprint_Click(Nothing, Nothing)
                    Case "G_ballphin_free_tuna"
                        button_G_ballphin_free_tuna_Click(Nothing, Nothing)
                    Case "G_message_in_a_bottle"
                        button_G_message_in_a_bottle_Click(Nothing, Nothing)
                End Select
            Case "T"
                LeftTabItem_Goods.IsSelected = True
                Select Case VariableName
                    Case "T_ball_and_cup"
                        button_G_ball_and_cup_click(Nothing, Nothing)
                    Case "T_dessicated_tentacle"
                        button_G_dessicated_tentacle_click(Nothing, Nothing)
                    Case "T_fake_kazoo"
                        button_G_fake_kazoo_click(Nothing, Nothing)
                    Case "T_frazzled_wires"
                        button_G_frazzled_wires_click(Nothing, Nothing)
                    Case "T_gnome_1"
                        button_G_gnome_1_click(Nothing, Nothing)
                    Case "T_gord's_knot"
                        button_G_gords_knot_click(Nothing, Nothing)
                    Case "T_hardened_rubber_bung"
                        button_G_hardened_rubber_bung_click(Nothing, Nothing)
                    Case "T_lying_robot"
                        button_G_lying_robot_click(Nothing, Nothing)
                    Case "T_melty_marbles"
                        button_G_melty_marbles_click(Nothing, Nothing)
                    Case "T_mismatched_buttons"
                        button_G_mismatched_buttons_click(Nothing, Nothing)
                    Case "T_second_hand_dentures"
                        button_G_second_hand_dentures_click(Nothing, Nothing)
                    Case "T_tiny_rocketship"
                        button_G_tiny_rocketship_click(Nothing, Nothing)
                    Case "T_ancient_vase"
                        button_G_ancient_vase_click(Nothing, Nothing)
                    Case "T_brain_cloud_pill"
                        button_G_brain_cloud_pill_click(Nothing, Nothing)
                    Case "T_broken_AAC_device"
                        button_G_broken_AAC_device_click(Nothing, Nothing)
                    Case "T_license_plate"
                        button_G_license_plate_click(Nothing, Nothing)
                    Case "T_old_boot"
                        button_G_old_boot_click(Nothing, Nothing)
                    Case "T_one_true_earring"
                        button_G_one_true_earring_click(Nothing, Nothing)
                    Case "T_orange_soda"
                        button_G_orange_soda_click(Nothing, Nothing)
                    Case "T_sea_worther"
                        button_G_sea_worther_click(Nothing, Nothing)
                    Case "T_sextant"
                        button_G_sextant_click(Nothing, Nothing)
                    Case "T_soaked_candle"
                        button_G_soaked_candle_click(Nothing, Nothing)
                    Case "T_toy_boat"
                        button_G_toy_boat_click(Nothing, Nothing)
                    Case "T_ukulele"
                        button_G_ukulele_click(Nothing, Nothing)
                    Case "T_voodoo_doll"
                        button_G_voodoo_doll_click(Nothing, Nothing)
                    Case "T_wine_bottle_candle"
                        button_G_wine_bottle_candle_click(Nothing, Nothing)
                    Case "T_air_unfreshener"
                        button_G_air_unfreshener_click(Nothing, Nothing)
                    Case "T_back_scratcher"
                        button_G_back_scratcher_click(Nothing, Nothing)
                    Case "T_beaten_beater"
                        button_G_beaten_beater_click(Nothing, Nothing)
                    Case "T_bent_spork"
                        button_G_bent_spork_click(Nothing, Nothing)
                    Case "T_black_bishop"
                        button_G_black_bishop_click(Nothing, Nothing)
                    Case "T_frayed_yarn"
                        button_G_frayed_yarn_click(Nothing, Nothing)
                    Case "T_gnome_2"
                        button_G_gnome_2_click(Nothing, Nothing)
                    Case "T_leaky_teacup"
                        button_G_leaky_teacup_click(Nothing, Nothing)
                    Case "T_lucky_cat_jar"
                        button_G_lucky_cat_jar_click(Nothing, Nothing)
                    Case "T_potato_cup"
                        button_G_potato_cup_click(Nothing, Nothing)
                    Case "T_shoe_horn"
                        button_G_shoe_horn_click(Nothing, Nothing)
                    Case "T_toy_trojan_horse"
                        button_G_toy_trojan_horse_click(Nothing, Nothing)
                    Case "T_unbalanced_top"
                        button_G_unbalanced_top_click(Nothing, Nothing)
                    Case "T_white_bishop"
                        button_G_white_bishop_click(Nothing, Nothing)
                    Case "T_wire_hanger"
                        button_G_wire_hanger_click(Nothing, Nothing)
                End Select
        End Select
    End Sub

    REM ------------------左侧面板(人物)------------------
    Private Sub C_Show(C_Name As String, C_EnName As String, C_picture As String, C_Motto As String, C_Descriptions_1 As String, C_Descriptions_2 As String, C_Descriptions_3 As String, C_HealthValue As Single, C_HungerValue As Single, C_SanityValue As Single, C_DamageValue As Single, C_DLC As String, C_Introduce As String)
        REM ------------------特殊情况------------------
        Canvas_CharacterLeft_Wolfgang.Visibility = Visibility.Collapsed
        Canvas_CharacterLeft.Visibility = Visibility.Visible
        If C_Name = "海獭伍迪" Then
            TextBlock_C_Health.Text = "树木值"
        Else
            TextBlock_C_Health.Text = "生命"
        End If
        REM ------------------人物名字------------------
        CL_textBlock_CharacterName.Text = C_Name
        CL_textBlock_CharacterName.UpdateLayout()
        Dim C_N_MarginLeft As Integer
        C_N_MarginLeft = (Canvas_CharacterLeft.ActualWidth - CL_textBlock_CharacterName.ActualWidth) / 2
        Dim C_N_T As New Thickness()
        C_N_T.Top = 190
        C_N_T.Left = C_N_MarginLeft
        CL_textBlock_CharacterName.Margin = C_N_T

        CL_textBlock_CharacterEnName.Text = C_EnName
        CL_textBlock_CharacterName.UpdateLayout()
        Dim C_EnN_MarginLeft As Integer
        C_EnN_MarginLeft = (Canvas_CharacterLeft.ActualWidth - CL_textBlock_CharacterEnName.ActualWidth) / 2
        Dim C_EnN_T As New Thickness()
        C_EnN_T.Top = 220
        C_EnN_T.Left = C_EnN_MarginLeft
        CL_textBlock_CharacterEnName.Margin = C_EnN_T
        REM ------------------人物图片------------------
        CL_image_CharacterPicture.Source = Picture_Short_Name(Res_Short_Name(C_picture))
        REM ------------------人物座右铭、描述------------------
        If C_Motto = "" Or C_Motto = "......" Then
            TextBlock_C_Motto.Text = C_Motto
        Else
            TextBlock_C_Motto.Text = """" & C_Motto & """"
        End If
        TextBlock_C_Motto.UpdateLayout()
        Dim C_M_MarginLeft As Integer
        C_M_MarginLeft = (Canvas_CharacterLeft.ActualWidth - TextBlock_C_Motto.ActualWidth) / 2
        Dim C_M_T As New Thickness()
        C_M_T.Top = 256
        C_M_T.Left = C_M_MarginLeft
        TextBlock_C_Motto.Margin = C_M_T

        If C_Descriptions_1 = "" Then
            TextBlock_C_Descriptions_1.Text = ""
        Else
            TextBlock_C_Descriptions_1.Text = "*" & C_Descriptions_1
        End If
        If C_Descriptions_2 = "" Then
            TextBlock_C_Descriptions_2.Text = ""
        Else
            TextBlock_C_Descriptions_2.Text = "*" & C_Descriptions_2
        End If
        If C_Descriptions_3 = "" Then
            TextBlock_C_Descriptions_3.Text = ""
        Else
            TextBlock_C_Descriptions_3.Text = "*" & C_Descriptions_3
        End If
        REM ------------------人物属性------------------
        If C_HealthValue = 0 Then
            TextBlock_C_HealthValue.Text = "无"
        Else
            TextBlock_C_HealthValue.Text = C_HealthValue
        End If
        If C_HungerValue = 0 Then
            TextBlock_C_HungerValue.Text = "无"
        Else
            TextBlock_C_HungerValue.Text = C_HungerValue
        End If
        If C_SanityValue = 0 Then
            TextBlock_C_SanityValue.Text = "无"
        Else
            TextBlock_C_SanityValue.Text = C_SanityValue
        End If
        If C_DamageValue <= 2 Then
            TextBlock_C_DamageValue.Text = C_DamageValue & "X"
        Else
            TextBlock_C_DamageValue.Text = C_DamageValue
        End If
        If C_Name = "阿比盖尔" Then
            Image_C_PB_Health.Width = 200
        Else
            Image_C_PB_Health.Width = C_HealthValue / 1.5
        End If
        Image_C_PB_Hunger.Width = C_HungerValue / 1.5
        Image_C_PB_Sanity.Width = C_SanityValue / 1.25
        If C_DamageValue > 2 Then
            Image_C_PB_Damage.Width = 200
        Else
            Image_C_PB_Damage.Width = C_DamageValue * 100
        End If
        REM ------------------人物DLC-------------------
        If C_DLC = "ROG" Then
            CL_image_C_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf C_DLC = "SW" Then
            CL_image_C_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        Else
            CL_image_C_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------食物简介-------------------
        TextBlock_C_Introduce.Text = C_Introduce
    End Sub

    REM ------------------人物-------------------
    Private Sub button_C_Wilson_click(sender As Object, e As RoutedEventArgs) Handles button_C_Wilson.Click
        C_Show("威尔逊", "Wilson", "C_wilson", "我要用我的意念征服它！", "长出一大把胡须", "", "", 150, 150, 200, 1, "NoDLC", "威尔逊 (绅士科学家) 是一位绅士，同时也是科学家，将他传送到荒岛的机器就是他亲手制作的。威尔逊与其他角色不同的地方在于他会长胡子，一位帅哥长时间不刮胡子也能变成野人的模样。胡子能在冬天为主角保暖，同时也是制作肉块雕像必不可少的材料。")
    End Sub

    Private Sub button_C_Willow_click(sender As Object, e As RoutedEventArgs) Handles button_C_Willow.Click
        C_Show("薇洛", "Willow", "C_willow", "东西总在燃烧的时候更绚丽。", "对火焰伤害免疫", "有一个很棒的打火机", "紧张的时候就会点火", 150, 150, 120, 1, "NoDLC", "薇洛 (纵火者) 完全免疫火焰伤害(单机版免疫，联机版受到一半伤害)，当靠近火焰时能略微补充一下精神。开局自带打火机(联机版与单机版不同，打火机有使用时限)，可以照明黑暗，也能用来纵火，非常有用的一个小道具；还附带一个伯尼(仅联机版)，精神低的时候放在地上可以吸引暗影怪的攻击仇恨。薇洛精神较低时可能会乱放火，这将非常容易引来森林大火，或者让自己辛苦种植的作物付之一炬。")
    End Sub

    Private Sub button_C_Wolfgang_click(sender As Object, e As RoutedEventArgs) Handles button_C_Wolfgang.Click
        Canvas_CharacterLeft.Visibility = Visibility.Collapsed
        Canvas_CharacterLeft_Wolfgang.Visibility = Visibility.Visible
    End Sub

    Private Sub button_C_Wendy_click(sender As Object, e As RoutedEventArgs) Handles button_C_Wendy.Click
        C_Show("温蒂", "Wendy", "C_wendy", "阿比盖尔？回来！我还没跟你玩够呢。", "受到她双胞胎妹妹的折磨", "在黑暗里很舒服", "打得不重", 150, 150, 200, 0.75, "NoDLC", "温蒂 (丧失亲人者) 长得非常可爱，当然这对孤岛生存毫无帮助，攻击比其他角色伤害要低，不过夜晚或者在怪物附近受到的精神惩罚只有75%。温蒂开局会携带一个鲜花头饰，使温蒂拥有唤醒她妹妹阿比盖尔灵魂的能力，死后掉落头饰需要经过一段时间充能即可再次用于召唤(将花放在地上然后杀死一只生物)。")
    End Sub

    Private Sub button_C_Wx78_click(sender As Object, e As RoutedEventArgs) Handles button_C_Wx78.Click
        C_Show("WX-78", "WX-78", "C_wx78", "移情组件无响应", "不挑食", "通过闪电来充能，但会受到水的伤害", "可以用装备来升级", 100, 100, 100, 1, "NoDLC", "WX-78 特有的机械胃能让他不受食物鲜度的惩罚，食用齿轮能让他恢复60点生命、75点饥饿与50点精神，并提高属性上限(15个到达顶值400/200/300)。下雨时请装备雨伞不然会持续受到伤害(雨水值>0时)，被闪电劈中减少精神但恢复生命，同时点燃周围的可燃物，用闪电充能后会让身体发光并增加少许移动速度。联机版初始三维150/150/150。")
    End Sub

    Private Sub button_C_Wickerbottom_click(sender As Object, e As RoutedEventArgs) Handles button_C_Wickerbottom.Click
        C_Show("维克波顿", "Wickerbottom", "C_wickerbottom", "嘘！图书管理不准说话！", "知道很多事情", "自己发行了书籍", "不能睡觉，讨厌变质食物", 150, 150, 250, 1, "NoDLC", "丰富的学识能让你提高科技水平，她的精神值很高，但患有失眠的她无法在晚上入眠，她还有挑食症，讨厌吃不新鲜的食物。但是她是可以制作书籍并且用其施展魔法的强力角色。")
    End Sub

    Private Sub button_C_Woodie_click(sender As Object, e As RoutedEventArgs) Handles button_C_Woodie.Click
        C_Show("伍迪", "Woodie", "C_woodie", "这树不错，是吧？", "有一把可爱的斧子", "有一个糟糕的秘密", "对丰富的收获物充满感恩(DST)", 150, 150, 200, 1, "NoDLC", "伍迪 (伐木工) 开局自带露西斧，无限耐久，可以快速伐木。这把斧头是有灵性的，可是会说话的哦。伍迪伐木多了会变身成一只海獭！伍迪身上的装备也会全部掉出来，变身前露西斧会给予提示。另外每当月圆之夜也会变身。")
    End Sub

    Private Sub button_C_Wes_click(sender As Object, e As RoutedEventArgs) Handles button_C_Wes.Click
        C_Show("韦斯", "Wes", "C_wes", "......", "不能说话", "很难活着", "使用气球魔法", 113, 113, 150, 0.75, "NoDLC", "韦斯 (沉默的人) 是一位默剧演员。他的属性和他的长相一样滑稽，真是低得不行。在游戏中生存比较困难，饥饿速度也比一般角色要稍微快一些。出生自带技能吹气球。每次吹气球都会减少5点精神，怪物击破气球后周围的气球也会一起爆炸，带有极低的伤害。")
    End Sub

    Private Sub button_C_Wathgrithr_click(sender As Object, e As RoutedEventArgs) Handles button_C_Wathgrithr.Click
        C_Show("威戈芙瑞德", "Wigfrid", "C_wathgrithr", "一个对我来说绝佳的能力考验！", "擅长战斗", "从打倒的敌人身上获得能量", "只吃肉", 200, 120, 120, 1.25, "NoDLC", "威戈芙德 (表演艺术家) 是一位勇敢的女战士，出生自带战斗头盔、战斗长矛和4块大肉，以保证食物的充足。她的攻击具有神奇的力量，每次攻击能够从敌人身上吸取生命(单机版只有杀死有攻击性的生物才会回复生命)，并且受到的伤害减少25%。然而只能吃肉食的特性让其生存具有一定的挑战。联机版里她能够制作战斗头盔和战斗长矛给其他玩家使用，打触手和BOSS都是不可或缺的强力装备。")
    End Sub

    Private Sub button_C_Webber_click(sender As Object, e As RoutedEventArgs) Handles button_C_Webber.Click
        C_Show("维伯", "Webber", "C_webber", "我们可以征服一切！", "是一只怪物", "可以和蜘蛛做伙伴", "会长出一嘴毛绒绒的小胡子", 175, 175, 100, 1, "NoDLC", "维伯 (难以消化) 与其他角色不同，出生自带一个蜘蛛卵和两个怪兽肉。吃怪兽肉和怪兽千层饼不会有血量和精神惩罚。得小心别靠近猪人和兔人，它们会主动攻击你。相反的可以用怪兽肉收买蜘蛛作为你的帮手。他会长出毛绒绒的小胡子，具有一定的保温功能，还可以刮下少量蜘蛛网。")
    End Sub

    Private Sub button_C_waxwell_click(sender As Object, e As RoutedEventArgs) Handles button_C_waxwell.Click
        C_Show("麦克斯韦", "Maxwell", "C_waxwell", "自由！", "短小精悍却很脆弱", "可以使自己人格分裂", "有自己的剑", 75, 150, 200, 1, "NoDLC", "麦克斯韦 (傀儡师) 开局自带暗影秘典和六个噩梦燃料。他的的生命甚至低至75！但是高达200的精神却可以帮助他制作各种傀儡，可以工作，也可以杀敌，然而会降低精神上限。他的精神值恢复得很快，使得玩家能更加自如地操纵暗影装备。如果傀儡死亡，之前消耗的精神上限也将返还。单机版开局自带暗影秘典、暗夜剑、夜魔盔甲、紫宝石和两个噩梦燃料，阅读暗影秘典召唤的是暗影麦克斯韦而不是分工的傀儡。")
    End Sub

    Private Sub button_C_Walani_click(sender As Object, e As RoutedEventArgs) Handles button_C_Walani.Click
        C_Show("瓦拉尼", "Walani", "C_walani", "有人可以给我一些吃的吗？", "喜欢冲浪", "干得快", "是一个相当冷艳的女孩", 120, 200, 200, 1, "SW", "拉瓦尼 (冲浪女娃) 开局自带冲浪板，这让她能够开心地在水上冲浪，一副高冷的表情也是深的某些玩家的喜爱。常年混迹海浪之上让她获得了独一无二的速干能力。当她遇上浪花时，会增加一点精神并且加速，她的湿度下降速度要快20%，并且精神的变化也慢10%，但是饥饿速度要快10%。带上你的冲浪板弄潮去吧！")
    End Sub

    Private Sub button_C_warly_click(sender As Object, e As RoutedEventArgs) Handles button_C_warly.Click
        C_Show("沃利", "Warly", "C_warly", "祝你好胃口！", "是个美食家", "使用特制的厨具", "有个拉风的厨师袋", 150, 250, 200, 1, "SW", "沃利 (厨神) 自带便携式烹饪锅，这个锅可不止便携的特性，它还可以烹饪四种专属美食！其中新鲜水果薄饼更是大补之物。食用食材原料只能获得70%三围，晒干的80%，篝火上烤熟的90%，而用烹煮锅做出来的食物会额外增加33%的三围(负面效果降低33%)。天资聪慧的他还可以制作带有冰箱大部分功能的厨师袋，这大概是为了弥补他飞快下降的饥饿吧(比正常角色多33%)。")
    End Sub

    Private Sub button_C_wilbur_click(sender As Object, e As RoutedEventArgs) Handles button_C_wilbur.Click
        C_Show("威尔伯", "Wilbur", "C_wilbur", "哦哦哦哈！", "不会说话", "跑得快", "是只猴子", 125, 175, 150, 1, "SW", "威尔伯 (猴王) 灵活的手脚让他能够跑的更快，然而速度快也是需要代价的，那就是饿得慌！跑起来提高33%的速度但是饥饿速度也增加33%，这意味着同样的距离他的饥饿消耗和其他角色一样。据说他有特殊的投便技巧，一个便便扔过去砸得你脸都绿了(10点攻击力)！")
    End Sub

    Private Sub button_C_woodlegs_click(sender As Object, e As RoutedEventArgs) Handles button_C_woodlegs.Click
        C_Show("伍德莱格", "Woodlegs", "C_woodlegs", "耶！", "是个海盗", "是海腿号船长", "能够发现宝藏", 150, 150, 120, 1, "SW", "伍德莱格 (木腿海盗) 自带幸运帽、望远镜和足矣制作他的宠物——海腿号的材料。作为亲近大海的海盗，在陆地上每分钟失去4.8的精神。海腿号的航行速度为6，不需要也不能再装推进装置，并且拥有无限的火炮(每炮造成50点伤害)。拥有幸运帽的他也会更频繁地发现宝藏，不想下船的话一炮轰过去！")
    End Sub

    Private Sub button_C_Abigail_click(sender As Object, e As RoutedEventArgs) Handles button_C_Abigail.Click
        C_Show("阿比盖尔", "Abigail", "C_wendy_1", "", "", "", "", 600, 0, 0, 40, "NoDLC", "当温蒂的鲜花头饰已经展开并且浮空，代表已经充能完毕。丢在地上并且杀死生物，阿比盖尔将会在头饰处重生并与温蒂协同作战。她的移动速度比其他幽灵要快很多，杀蝴蝶很好用。她不会伤害温蒂，直到她死亡为止都不会离开，死亡后头饰会掉落出来。她在白天攻击力只有10点，黄昏20点，晚上达到最强40点。")
    End Sub

    Private Sub button_C_woodie_1_click(sender As Object, e As RoutedEventArgs) Handles button_C_woodie_1.Click
        C_Show("海獭伍迪", "Woodie", "C_woodie_1", "", "", "", "", 100, 0, 0, 51, "NoDLC", "伍迪树木值下降到25就会变身。锋利的牙齿能快速啃咬树木、矿石等。啃咬生物拥有狼牙棒一般的威力。变身后树木值取代原来的状态，随时间减少，受到伤害也会减少。海獭伍迪对严寒免疫且能在黑暗中视物，树木值为0后的第二天或者树木值吃到100会在原地变回人类，生命饥饿精神都降为极低。")
    End Sub

    Private Sub C_slider_Wolfgang_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles C_slider_Wolfgang.ValueChanged
        Dim Val As Double
        Val = C_slider_Wolfgang.Value
        Image_C_PB_Hunger_Wolfgang.Width = Val * 2 / 3
        TextBlock_C_HungerValue_Wolfgang.Text = CInt(Val)
        If Val > 220 And Val <= 300 Then
            Image_C_PB_Health_Wolfgang.Width = (1.25 * Val - 75) / 1.5
            TextBlock_C_HealthValue_Wolfgang.Text = CStr(CInt(1.25 * Val - 75))
            Image_C_PB_Damage_Wolfgang.Width = 0.9375 * Val - 81.25
            TextBlock_C_DamageValue_Wolfgang.Text = CStr(Math.Round((0.009375 * Val - 0.8125), 2)) & "X"
        ElseIf Val > 100 And Val <= 220 Then
            Image_C_PB_Health_Wolfgang.Width = 133
            TextBlock_C_HealthValue_Wolfgang.Text = 200
            Image_C_PB_Damage_Wolfgang.Width = 100
            TextBlock_C_DamageValue_Wolfgang.Text = "1X"
        Else
            Image_C_PB_Health_Wolfgang.Width = (0.5 * Val + 150) / 1.5
            TextBlock_C_HealthValue_Wolfgang.Text = CStr(CInt(0.5 * Val + 150))
            Image_C_PB_Damage_Wolfgang.Width = 0.25 * Val + 50
            TextBlock_C_DamageValue_Wolfgang.Text = CStr(Math.Round((0.0025 * Val + 0.5), 2)) & "X"
        End If
    End Sub

    REM 人物DLC
    Private Sub checkBox_C_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles checkBox_C_DLC_SW.Click
        If checkBox_C_DLC_SW.IsChecked = True Then
            button_C_Walani.Visibility = Visibility.Visible
            button_C_warly.Visibility = Visibility.Visible
            button_C_wilbur.Visibility = Visibility.Visible
            button_C_woodlegs.Visibility = Visibility.Visible
            WrapPanel_Character.Height = 1075
            Reg_Write("Character", 7)
        Else
            button_C_Walani.Visibility = Visibility.Collapsed
            button_C_warly.Visibility = Visibility.Collapsed
            button_C_wilbur.Visibility = Visibility.Collapsed
            button_C_woodlegs.Visibility = Visibility.Collapsed
            WrapPanel_Character.Height = 870
            Reg_Write("Character", 5)
        End If
    End Sub

    Private Sub CL_button_C_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles CL_button_C_DLC_SW.Click
        If checkBox_C_DLC_SW.IsChecked = True Then
            checkBox_C_DLC_SW.IsChecked = False
        Else
            checkBox_C_DLC_SW.IsChecked = True
        End If
        checkBox_C_DLC_SW_click(Nothing, Nothing)
    End Sub

    REM ------------------左侧面板(食谱)------------------
    Private Sub F_Show(F_Name As String, F_EnName As String, F_picture As String, F_HealthValue As Single, F_HungerValue As Single, F_SanityValue As Single, F_PerishValue As Single, F_CooktimeValue As Single, F_PriorityValue As Single, F_DLC As String, F_Image_FN_1 As String, F_TextBlock_FN_1 As String, F_Image_FN_OR As String, F_TextBlock_FN_OR As String, F_Image_FN_2 As String, F_TextBlock_FN_2 As String, F_Image_FN_3 As String, F_TextBlock_FN_3 As String, F_Restrictions_State As Short, F_Restrictions_image_1 As String, F_Restrictions_image_2 As String, F_Restrictions_image_3 As String, F_Restrictions_image_4 As String, F_Restrictions_image_5 As String, F_Restrictions_image_6 As String, F_Restrictions_image_7 As String, F_Restrictions_image_Compare As String, F_Restrictions_TextBlock_Compare As String, F_Introduce As String, ParamArray Recommend() As String)
        REM ------------------初始化------------------
        FL_image_F_DLC.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_No.Text = "无"
        FL_image_Portable_Crock_Pot.Visibility = Visibility.Collapsed
        TextBlock_F_Cooktime.Visibility = Visibility.Visible
        TextBlock_F_priority.Visibility = Visibility.Visible
        Image_F_PBB_Cooktime.Visibility = Visibility.Visible
        Image_F_PBB_Priority.Visibility = Visibility.Visible
        Image_F_PB_Cooktime.Visibility = Visibility.Visible
        Image_F_PB_Priority.Visibility = Visibility.Visible
        TextBlock_F_CooktimeValue.Visibility = Visibility.Visible
        TextBlock_F_PriorityValue.Visibility = Visibility.Visible
        TextBlock_F_CooKRequirements.Visibility = Visibility.Visible
        button_F_FoodNeed_1.Visibility = Visibility.Visible
        button_F_FoodNeed_or.Visibility = Visibility.Visible
        button_F_FoodNeed_2.Visibility = Visibility.Visible
        button_F_FoodNeed_3.Visibility = Visibility.Visible
        FL_TextBlock_FoodNeed_1.Visibility = Visibility.Visible
        FL_TextBlock_FoodNeed_or.Visibility = Visibility.Visible
        FL_TextBlock_FoodNeed_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodNeed_3.Visibility = Visibility.Visible
        TextBlock_F_CookFillerRestrictions.Visibility = Visibility.Visible
        button_F_FoodAttribute_1.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Collapsed
        TextBlock_F_FoodAttribute.Visibility = Visibility.Collapsed
        button_F_FoodAttribute_2.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodAttribute.Visibility = Visibility.Collapsed
        FL_image_FoodAttribute_2.Visibility = Visibility.Collapsed
        TextBlock_F_FoodValue.Visibility = Visibility.Collapsed
        TextBlock_F_HealthValue.Foreground = Brushes.Black
        TextBlock_F_HungerValue.Foreground = Brushes.Black
        TextBlock_F_SanityValue.Foreground = Brushes.Black
        TextBlock_F_Recommend.Visibility = Visibility.Visible
        F_WrapPanel_Recommend_1.Visibility = Visibility.Visible
        F_WrapPanel_Recommend_2.Visibility = Visibility.Visible
        REM ------------------食物名字------------------
        FL_textBlock_FoodName.Text = F_Name
        FL_textBlock_FoodName.UpdateLayout()
        Dim F_N_MarginLeft As Integer
        F_N_MarginLeft = (Canvas_FoodLeft.ActualWidth - FL_textBlock_FoodName.ActualWidth) / 2
        Dim F_N_T As New Thickness()
        F_N_T.Top = 80
        F_N_T.Left = F_N_MarginLeft
        FL_textBlock_FoodName.Margin = F_N_T

        FL_textBlock_FoodEnName.Text = F_EnName
        FL_textBlock_FoodEnName.UpdateLayout()
        Dim F_EnN_MarginLeft As Integer
        F_EnN_MarginLeft = (Canvas_FoodLeft.ActualWidth - FL_textBlock_FoodEnName.ActualWidth) / 2
        Dim F_EnN_T As New Thickness()
        F_EnN_T.Top = 100
        F_EnN_T.Left = F_EnN_MarginLeft
        FL_textBlock_FoodEnName.Margin = F_EnN_T
        REM ------------------食物图片------------------
        FL_image_FoodPicture.Source = Picture_Short_Name(Res_Short_Name(F_picture))
        REM ------------------食物属性------------------
        TextBlock_F_HealthValue.Text = F_HealthValue
        TextBlock_F_HungerValue.Text = F_HungerValue
        TextBlock_F_SanityValue.Text = F_SanityValue
        TextBlock_F_PerishValue.Text = F_PerishValue
        TextBlock_F_CooktimeValue.Text = F_CooktimeValue
        TextBlock_F_PriorityValue.Text = F_PriorityValue
        If F_HealthValue >= 0 Then
            Image_F_PB_Health.Width = F_HealthValue
        Else
            Image_F_PB_Health.Width = 0
            TextBlock_F_HealthValue.Foreground = Brushes.Red
        End If
        If F_HungerValue < 0 Then
            TextBlock_F_HungerValue.Foreground = Brushes.Red
        End If
        Image_F_PB_Hunger.Width = F_HungerValue / 1.5
        If F_SanityValue >= 0 Then
            Image_F_PB_Sanity.Width = F_SanityValue / 0.5
        Else
            Image_F_PB_Sanity.Width = 0
            TextBlock_F_SanityValue.Foreground = Brushes.Red
        End If
        If F_PerishValue <= 20 Then
            Image_F_PB_Perish.Width = F_PerishValue / 0.2
        Else
            Image_F_PB_Perish.Width = 100
        End If
        Image_F_PB_Cooktime.Width = F_CooktimeValue / 0.6
        If F_PriorityValue >= 0 Then
            Image_F_PB_Priority.Width = F_PriorityValue / 0.3
        Else
            Image_F_PB_Priority.Width = 0
        End If
        REM ------------------食物DLC-------------------
        If F_DLC = "ROG" Then
            FL_image_F_DLC.Visibility = Visibility.Visible
            FL_image_F_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf F_DLC = "SW" Then
            FL_image_F_DLC.Visibility = Visibility.Visible
            FL_image_F_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        Else
            FL_image_F_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------烹饪需求-------------------
        Dim F_FN_1 As New BitmapImage
        Dim F_FN_OR As New BitmapImage
        Dim F_FN_2 As New BitmapImage
        Dim F_FN_3 As New BitmapImage
        If F_Image_FN_1 = "" Then
            F_Image_FN_1 = "Resources/P_NULL.png"
        End If
        If F_Image_FN_OR = "" Then
            F_Image_FN_OR = "Resources/P_NULL.png"
        End If
        If F_Image_FN_2 = "" Then
            F_Image_FN_2 = "Resources/P_NULL.png"
        End If
        If F_Image_FN_3 = "" Then
            F_Image_FN_3 = "Resources/P_NULL.png"
        End If
        F_FN_1.BeginInit()
        F_FN_1.UriSource = New Uri(Res_Short_Name(F_Image_FN_1), UriKind.Relative)
        F_FN_1.EndInit()
        F_FN_OR.BeginInit()
        F_FN_OR.UriSource = New Uri(Res_Short_Name(F_Image_FN_OR), UriKind.Relative)
        F_FN_OR.EndInit()
        F_FN_2.BeginInit()
        F_FN_2.UriSource = New Uri(Res_Short_Name(F_Image_FN_2), UriKind.Relative)
        F_FN_2.EndInit()
        F_FN_3.BeginInit()
        F_FN_3.UriSource = New Uri(Res_Short_Name(F_Image_FN_3), UriKind.Relative)
        F_FN_3.EndInit()
        FL_image_FoodNeed_1.Source = F_FN_1
        FL_image_FoodNeed_or.Source = F_FN_OR
        FL_image_FoodNeed_2.Source = F_FN_2
        FL_image_FoodNeed_3.Source = F_FN_3
        FL_TextBlock_FoodNeed_1.Text = F_TextBlock_FN_1
        FL_TextBlock_FoodNeed_or.Text = F_TextBlock_FN_OR
        FL_TextBlock_FoodNeed_2.Text = F_TextBlock_FN_2
        FL_TextBlock_FoodNeed_3.Text = F_TextBlock_FN_3
        F_FoodNeed_1 = F_Image_FN_1
        F_FoodNeed_or = F_Image_FN_OR
        F_FoodNeed_2 = F_Image_FN_2
        F_FoodNeed_3 = F_Image_FN_3
        REM ------------------填充限制-------------------
        FL_TextBlock_Restrictions_No.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_LessThanOne_1.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_LessThanOne_2.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_Compare.Visibility = Visibility.Collapsed
        button_F_Restrictions_1.Visibility = Visibility.Collapsed
        button_F_Restrictions_2.Visibility = Visibility.Collapsed
        button_F_Restrictions_3.Visibility = Visibility.Collapsed
        button_F_Restrictions_4.Visibility = Visibility.Collapsed
        button_F_Restrictions_5.Visibility = Visibility.Collapsed
        button_F_Restrictions_6.Visibility = Visibility.Collapsed
        button_F_Restrictions_7.Visibility = Visibility.Collapsed
        button_F_Restrictions_Compare.Visibility = Visibility.Collapsed
        Dim F_RES_1 As New BitmapImage
        Dim F_RES_2 As New BitmapImage
        Dim F_RES_3 As New BitmapImage
        Dim F_RES_4 As New BitmapImage
        Dim F_RES_5 As New BitmapImage
        Dim F_RES_6 As New BitmapImage
        Dim F_RES_7 As New BitmapImage
        Dim F_RES_Compare As New BitmapImage
        If F_Restrictions_State = 0 Then
            REM ------------------"无"-------------------
            If F_Restrictions_image_1 = "" Then
                F_Restrictions_image_1 = "Resources/P_NULL.png"
            End If
            If F_Restrictions_image_2 = "" Then
                F_Restrictions_image_2 = "Resources/P_NULL.png"
            End If
            If F_Restrictions_image_3 = "" Then
                F_Restrictions_image_3 = "Resources/P_NULL.png"
            End If
            If F_Restrictions_image_4 = "" Then
                F_Restrictions_image_4 = "Resources/P_NULL.png"
            End If
            If F_Restrictions_image_5 = "" Then
                F_Restrictions_image_5 = "Resources/P_NULL.png"
            End If

            F_RES_1.BeginInit()
            F_RES_1.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_1), UriKind.Relative)
            F_RES_1.EndInit()
            F_RES_2.BeginInit()
            F_RES_2.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_2), UriKind.Relative)
            F_RES_2.EndInit()
            F_RES_3.BeginInit()
            F_RES_3.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_3), UriKind.Relative)
            F_RES_3.EndInit()
            F_RES_4.BeginInit()
            F_RES_4.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_4), UriKind.Relative)
            F_RES_4.EndInit()
            F_RES_5.BeginInit()
            F_RES_5.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_5), UriKind.Relative)
            F_RES_5.EndInit()

            FL_image_Restrictions_1.Source = F_RES_1
            FL_image_Restrictions_2.Source = F_RES_2
            FL_image_Restrictions_3.Source = F_RES_3
            FL_image_Restrictions_4.Source = F_RES_4
            FL_image_Restrictions_5.Source = F_RES_5

            FL_TextBlock_Restrictions_No.Visibility = Visibility.Visible
            button_F_Restrictions_1.Visibility = Visibility.Visible
            button_F_Restrictions_2.Visibility = Visibility.Visible
            button_F_Restrictions_3.Visibility = Visibility.Visible
            button_F_Restrictions_4.Visibility = Visibility.Visible
            button_F_Restrictions_5.Visibility = Visibility.Visible
            F_Restrictions_1 = F_Restrictions_image_1
            F_Restrictions_2 = F_Restrictions_image_2
            F_Restrictions_3 = F_Restrictions_image_3
            F_Restrictions_4 = F_Restrictions_image_4
            F_Restrictions_5 = F_Restrictions_image_5
        ElseIf F_Restrictions_State = 1 Then
            REM ------------------"最多一个"-------------------
            If F_Restrictions_image_6 = "" Then
                F_Restrictions_image_6 = "Resources/P_NULL.png"
            Else
                FL_TextBlock_Restrictions_LessThanOne_1.Visibility = Visibility.Visible
                FL_TextBlock_Restrictions_LessThanOne_1.Text = "最多一个"
            End If
            If F_Restrictions_image_6 = "F_seaweed" Then
                FL_TextBlock_Restrictions_LessThanOne_1.Text = "最多两个"
            End If
            If F_Restrictions_image_7 = "" Then
                F_Restrictions_image_7 = "Resources/P_NULL.png"
            Else
                FL_TextBlock_Restrictions_LessThanOne_2.Visibility = Visibility.Visible
            End If

            F_RES_6.BeginInit()
            F_RES_6.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_6), UriKind.Relative)
            F_RES_6.EndInit()
            F_RES_7.BeginInit()
            F_RES_7.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_7), UriKind.Relative)
            F_RES_7.EndInit()

            FL_image_Restrictions_6.Source = F_RES_6
            FL_image_Restrictions_7.Source = F_RES_7

            button_F_Restrictions_6.Visibility = Visibility.Visible
            button_F_Restrictions_7.Visibility = Visibility.Visible
            F_Restrictions_6 = F_Restrictions_image_6
            F_Restrictions_7 = F_Restrictions_image_7
        ElseIf F_Restrictions_State = 2 Then
            REM ------------------"比较"-------------------
            If F_Restrictions_image_1 = "" Then
                F_Restrictions_image_1 = "Resources/P_NULL.png"
            End If
            If F_Restrictions_image_2 = "" Then
                F_Restrictions_image_2 = "Resources/P_NULL.png"
            End If
            If F_Restrictions_image_3 = "" Then
                F_Restrictions_image_3 = "Resources/P_NULL.png"
            End If

            F_RES_1.BeginInit()
            F_RES_1.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_1), UriKind.Relative)
            F_RES_1.EndInit()
            F_RES_2.BeginInit()
            F_RES_2.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_2), UriKind.Relative)
            F_RES_2.EndInit()
            F_RES_3.BeginInit()
            F_RES_3.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_3), UriKind.Relative)
            F_RES_3.EndInit()
            F_RES_Compare.BeginInit()
            F_RES_Compare.UriSource = New Uri(Res_Short_Name(F_Restrictions_image_Compare), UriKind.Relative)
            F_RES_Compare.EndInit()

            FL_image_Restrictions_1.Source = F_RES_1
            FL_image_Restrictions_2.Source = F_RES_2
            FL_image_Restrictions_3.Source = F_RES_3
            FL_image_Restrictions_Compare.Source = F_RES_Compare
            FL_TextBlock_Restrictions_Compare.Text = F_Restrictions_TextBlock_Compare

            FL_TextBlock_Restrictions_No.Visibility = Visibility.Visible
            FL_TextBlock_Restrictions_Compare.Visibility = Visibility.Visible
            button_F_Restrictions_1.Visibility = Visibility.Visible
            button_F_Restrictions_2.Visibility = Visibility.Visible
            button_F_Restrictions_3.Visibility = Visibility.Visible
            button_F_Restrictions_Compare.Visibility = Visibility.Visible
            F_Restrictions_1 = F_Restrictions_image_1
            F_Restrictions_2 = F_Restrictions_image_2
            F_Restrictions_3 = F_Restrictions_image_3
            F_Restrictions_Compare = F_Restrictions_image_Compare
        End If
        REM ------------------推荐食谱-------------------
        Dim RecommendLength = Recommend.Length
        If F_Name <> "湿腻焦糊" Then
            FL_image_Recommend_11.Source = Picture_Short_Name(Res_Short_Name(Recommend(0)))
            FL_image_Recommend_12.Source = Picture_Short_Name(Res_Short_Name(Recommend(1)))
            FL_image_Recommend_13.Source = Picture_Short_Name(Res_Short_Name(Recommend(2)))
            FL_image_Recommend_14.Source = Picture_Short_Name(Res_Short_Name(Recommend(3)))
            F_Recommend_Select_1 = Recommend(0)
            F_Recommend_Select_2 = Recommend(1)
            F_Recommend_Select_3 = Recommend(2)
            F_Recommend_Select_4 = Recommend(3)
            If RecommendLength = 4 Then
                F_WrapPanel_Recommend_2.Visibility = Visibility.Collapsed
            Else
                FL_image_Recommend_21.Source = Picture_Short_Name(Res_Short_Name(Recommend(4)))
                FL_image_Recommend_22.Source = Picture_Short_Name(Res_Short_Name(Recommend(5)))
                FL_image_Recommend_23.Source = Picture_Short_Name(Res_Short_Name(Recommend(6)))
                FL_image_Recommend_24.Source = Picture_Short_Name(Res_Short_Name(Recommend(7)))
                F_Recommend_Select_5 = Recommend(4)
                F_Recommend_Select_6 = Recommend(5)
                F_Recommend_Select_7 = Recommend(6)
                F_Recommend_Select_8 = Recommend(7)
            End If
        End If
        REM ------------------食物简介-------------------
        TextBlock_F_Introduce.Text = F_Introduce
        TextBlock_F_Introduce.Height = SetTextBlockHeight(F_Introduce, 14)
        If RecommendLength = 4 Then
            TextBlock_F_Introduce.SetValue(Canvas.TopProperty, CDbl(541))
        Else
            TextBlock_F_Introduce.SetValue(Canvas.TopProperty, CDbl(575))
        End If
        REM ------------------湿腻焦糊-------------------
        If F_Name = "湿腻焦糊" Then
            TextBlock_F_CookFillerRestrictions.Visibility = Visibility.Collapsed
            TextBlock_F_Recommend.Visibility = Visibility.Collapsed
            F_WrapPanel_Recommend_1.Visibility = Visibility.Collapsed
            F_WrapPanel_Recommend_2.Visibility = Visibility.Collapsed
            TextBlock_F_Introduce.SetValue(Canvas.TopProperty, CDbl(350))
        End If
        REM ------------------高度设置-------------------
        If RecommendLength = 4 Then
            Canvas_FoodLeft.Height = 541 + TextBlock_F_Introduce.Height + 20
        Else
            Canvas_FoodLeft.Height = 575 + TextBlock_F_Introduce.Height + 20
        End If
        If Canvas_FoodLeft.Height < 604 Then
            Canvas_FoodLeft.Height = 604
        End If
        REM ------------------关闭食物种类-------------------
        FL_CLOSE()
    End Sub

    Private Sub button_F_Recommend_11_click(sender As Object, e As RoutedEventArgs) Handles button_F_Recommend_11.Click
        ButtonJump(F_Recommend_Select_1, "F")
    End Sub

    Private Sub button_F_Recommend_12_click(sender As Object, e As RoutedEventArgs) Handles button_F_Recommend_12.Click
        ButtonJump(F_Recommend_Select_2, "F")
    End Sub

    Private Sub button_F_Recommend_13_click(sender As Object, e As RoutedEventArgs) Handles button_F_Recommend_13.Click
        ButtonJump(F_Recommend_Select_3, "F")
    End Sub

    Private Sub button_F_Recommend_14_click(sender As Object, e As RoutedEventArgs) Handles button_F_Recommend_14.Click
        ButtonJump(F_Recommend_Select_4, "F")
    End Sub

    Private Sub button_F_Recommend_21_click(sender As Object, e As RoutedEventArgs) Handles button_F_Recommend_21.Click
        ButtonJump(F_Recommend_Select_5, "F")
    End Sub

    Private Sub button_F_Recommend_22_click(sender As Object, e As RoutedEventArgs) Handles button_F_Recommend_22.Click
        ButtonJump(F_Recommend_Select_6, "F")
    End Sub

    Private Sub button_F_Recommend_23_click(sender As Object, e As RoutedEventArgs) Handles button_F_Recommend_23.Click
        ButtonJump(F_Recommend_Select_7, "F")
    End Sub

    Private Sub button_F_Recommend_24_click(sender As Object, e As RoutedEventArgs) Handles button_F_Recommend_24.Click
        ButtonJump(F_Recommend_Select_8, "F")
    End Sub

    REM ------------------食谱-------------------
    Private Sub button_F_bacon_and_eggs_click(sender As Object, e As RoutedEventArgs) Handles button_F_bacon_and_eggs.Click
        F_Show("培根煎蛋", "Bacon and Eggs", "F_bacon_and_eggs", 20, 75, 5, 20, 40, 10, "NoDLC", "FC_Meats", "×1.5", "", "", "FC_Eggs", "×2", "", "", 0, "FC_Vegetables", "", "", "", "", "", "", "", "", "恢复能力比较中庸，其中蛋比较难找，如果有多的蛋还是做饺子比较划算。", {"F_meat", "F_morsel", "F_egg", "F_egg"})
    End Sub

    Private Sub button_F_meatballs_click(sender As Object, e As RoutedEventArgs) Handles button_F_meatballs.Click
        F_Show("肉丸", "Meatballs", "F_meatballs", 3, 62.5, 5, 10, 15, -1, "NoDLC", "FC_Meats", "×0.5", "", "", "", "", "", "", 2, "G_twigs", "", "", "", "", "", "", "FC_Meats", "<3", "最最常见的料理，解决饥饿的佳品，推荐用小肉和三个浆果制作。", {"F_morsel", "F_ice", "F_ice", "F_ice", "F_morsel", "F_berries", "F_berries", "F_berries"})
    End Sub

    Private Sub button_F_meaty_stew_click(sender As Object, e As RoutedEventArgs) Handles button_F_meaty_stew.Click
        F_Show("肉汤", "Meaty Stew", "F_meaty_stew", 12, 150, 5, 10, 15, 0, "NoDLC", "FC_Meats", "×3", "", "", "", "", "", "", 0, "G_twigs", "", "", "", "", "", "", "", "", "回复大量饥饿。需要不少肉。要注意很多人物饥饿在120以下，能吃肉丸还是吃肉丸吧。(仅单机版)食用后15秒增加40温度。", {"F_monster_meat", "F_meat", "F_meat", "F_berries", "F_monster_meat", "F_meat", "F_morsel", "F_morsel"})
    End Sub

    Private Sub button_F_butter_muffin_click(sender As Object, e As RoutedEventArgs) Handles button_F_butter_muffin.Click
        F_Show("奶油松饼", "Butter Muffin", "F_butter_muffin", 20, 37.5, 5, 15, 40, 1, "NoDLC", "F_butterfly_wing", "×1", "", "", "FC_Vegetables", "×0.5", "", "", 0, "FC_Meats", "", "", "", "", "", "", "", "", "俗称蝴蝶排。食材简单，回血不少。", {"F_butterfly_wing", "F_carrot", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_dragonpie_click(sender As Object, e As RoutedEventArgs) Handles button_F_dragonpie.Click
        F_Show("火龙果派", "Dragonpie", "F_dragonpie", 40, 75, 5, 15, 40, 1, "NoDLC", "F_dragon_fruit", "×1", "", "", "", "", "", "", 0, "FC_Meats", "", "", "", "", "", "", "", "", "补血神器。用火龙果可以和鸟换1-2个种子，不是急需的话可以试试运气多换几个出来。食用后10秒增加40温度。", {"F_dragon_fruit", "G_twigs", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_fish_tacos_click(sender As Object, e As RoutedEventArgs) Handles button_F_fish_tacos.Click
        F_Show("玉米饼包炸鱼", "Fish Tacos", "F_fish_tacos", 20, 37.5, 5, 6, 10, 10, "NoDLC", "FC_Fishes", "×0.5", "", "", "F_corn", "×1", "", "", 3, "", "", "", "", "", "", "", "", "", "可怜的回复量并不值得做。", {"F_fish", "F_corn", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_fishsticks_click(sender As Object, e As RoutedEventArgs) Handles button_F_fishsticks.Click
        F_Show("炸鱼条", "Fishsticks", "F_fishsticks", 40, 37.5, 5, 10, 40, 10, "NoDLC", "FC_Fishes", "×0.5", "", "", "G_twigs", "×1", "", "", 1, "", "", "", "", "", "G_twigs", "", "", "", "俗称鱼排。家离池塘比较近的话可以考虑量产，回血能力和火龙果派相当。", {"F_fish", "G_twigs", "F_berries", "F_berries"})
    End Sub

    Private Sub button_F_fist_full_of_jam_click(sender As Object, e As RoutedEventArgs) Handles button_F_fist_full_of_jam.Click
        F_Show("果酱蜜饯", "Fist Full of Jam", "F_fist_full_of_jam", 3, 37.5, 5, 15, 10, 0, "NoDLC", "FC_Fruit", "×0.5", "", "", "", "", "", "", 0, "FC_Meats", "FC_Vegetables", "G_twigs", "", "", "", "", "", "", "大概是不小心丢了四个浆果在锅里...还是放一点肉做肉丸子比较好。", {"F_berries", "F_berries", "F_berries", "F_berries"})
    End Sub

    Private Sub button_F_froggle_bunwich_click(sender As Object, e As RoutedEventArgs) Handles button_F_froggle_bunwich.Click
        F_Show("青蛙圆面包三明治", "Froggle Bunwich", "F_froggle_bunwich", 20, 37.5, 0, 10, 10, 1, "NoDLC", "F_frog_legs", "×1", "", "", "FC_Vegetables", "×0.5", "", "", 3, "", "", "", "", "", "", "", "", "", "俗称青蛙排。在青蛙雨过后如果不知道青蛙腿怎么用做做这个也不错。", {"F_frog_legs", "F_carrot", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_fruit_medley_click(sender As Object, e As RoutedEventArgs) Handles button_F_fruit_medley.Click
        F_Show("水果拼盘", "Fruit Medley", "F_fruit_medley", 20, 25, 5, 6, 10, 0, "NoDLC", "FC_Fruit", "×3", "", "", "", "", "", "", 0, "FC_Meats", "FC_Vegetables", "", "", "", "", "", "", "", "要求的水果量和回复能力不成正比，还不如直接吃水果。食用后5秒降低40温度。", {"F_banana", "F_banana", "F_banana", "F_banana", "F_watermelon", "F_watermelon", "F_watermelon", "F_watermelon"})
    End Sub

    Private Sub button_F_honey_nuggets_click(sender As Object, e As RoutedEventArgs) Handles button_F_honey_nuggets.Click
        F_Show("甜蜜金砖", "Honey Nuggets", "F_honey_nuggets", 20, 37.5, 5, 15, 40, 2, "NoDLC", "F_honey", "×1", "", "", "FC_Meats", "×0.5", "", "", 2, "G_twigs", "", "", "", "", "", "", "FC_Meats", "<2", "蜜汁火腿的简化版，唯一的好处就是肉需求少了一些。", {"F_honey", "F_morsel", "F_berries", "F_berries"})
    End Sub

    Private Sub button_F_honey_ham_click(sender As Object, e As RoutedEventArgs) Handles button_F_honey_ham.Click
        F_Show("蜜汁火腿", "Honey Ham", "F_honey_ham", 30, 75, 5, 15, 40, 2, "NoDLC", "F_honey", "×1", "", "", "FC_Meats", "×2", "", "", 0, "G_twigs", "", "", "", "", "", "", "", "", "材料好找，回复能力不错，下洞穴可以多带点。食用后10秒增加40温度。", {"F_honey", "F_morsel", "F_monster_meat", "F_berries"})
    End Sub

    Private Sub button_F_kabobs_click(sender As Object, e As RoutedEventArgs) Handles button_F_kabobs.Click
        F_Show("肉串", "Kabobs", "F_kabobs", 3, 37.5, 3, 15, 40, 5, "NoDLC", "G_twigs", "×1", "", "", "FC_Meats", "×0.5", "", "", 1, "", "", "", "", "", "G_twigs", "FC_Monster_Meats", "", "", "在肉系料理中加入树枝很有可能做成的东西。回复量感人。(仅联机版)食用后15秒增加40温度。", {"G_twigs", "F_morsel", "F_berries", "F_berries"})
    End Sub

    Private Sub button_F_mandrake_soup_click(sender As Object, e As RoutedEventArgs) Handles button_F_mandrake_soup.Click
        F_Show("曼德拉汤", "Mandrake Soup", "F_mandrake_soup", 100, 150, 5, 6, 60, 10, "NoDLC", "F_mandrake", "×1", "", "", "", "", "", "", 3, "", "", "", "", "", "", "", "", "", "超级超级大补！然而曼德拉草的罕见度...", {"F_mandrake", "G_twigs", "G_twigs", "G_twigs", "F_mandrake", "F_berries", "F_berries", "F_berries"})
    End Sub

    Private Sub button_F_monster_lasagna_click(sender As Object, e As RoutedEventArgs) Handles button_F_monster_lasagna.Click
        F_Show("怪物千层饼", "Monster Lasagna", "F_monster_lasagna", -20, 37.5, -20, 6, 10, 10, "NoDLC", "FC_Monster_Meats", "或", "F_durian", "或", "FC_Jellyfish", "×2", "", "", 0, "G_twigs", "", "", "", "", "", "", "", "", "如果一不小心放入两块怪兽肉或者榴莲基本上就做出这货了。一般不会有人想吃它吧。不过韦伯吃了能回37.5的饥饿。", {"F_monster_meat", "F_monster_meat", "F_berries", "F_berries", "F_durian", "F_durian", "F_roasted_birchnut", "F_roasted_birchnut"})
    End Sub

    Private Sub button_F_pierogi_click(sender As Object, e As RoutedEventArgs) Handles button_F_pierogi.Click
        F_Show("饺子", "Pierogi", "F_pierogi", 40, 37.5, 5, 20, 20, 5, "NoDLC", "FC_Eggs", "×1", "", "", "FC_Meats", "×0.5", "FC_Vegetables", "×0.5", 0, "G_twigs", "", "", "", "", "", "", "", "", "回血能力优秀，蛋类一般都用来做饺子，蔬菜的话有农场也不会缺。", {"F_egg", "F_morsel", "F_cooked_green_cap", "F_berries"})
    End Sub

    Private Sub button_F_powdercake_click(sender As Object, e As RoutedEventArgs) Handles button_F_powdercake.Click
        F_Show("芝士蛋糕", "Powdercake", "F_powdercake", -3, 0, 0, 18750, 10, 10, "NoDLC", "F_corn", "×1", "", "", "F_honey", "×1", "G_twigs", "×1", 3, "", "", "", "", "", "", "", "", "", "看回复量就知道不是人吃的。一般用来做成火鸡陷阱勾引火鸡吃，这个保质期大概到世界末日都不会坏吧。", {"F_corn", "F_honey", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_pumpkin_cookie_click(sender As Object, e As RoutedEventArgs) Handles button_F_pumpkin_cookie.Click
        F_Show("南瓜饼", "Pumpkin Cookies", "F_pumpkin_cookie", 0, 37.5, 15, 10, 40, 10, "NoDLC", "F_pumpkin", "×1", "", "", "FC_Sweetener", "×2", "", "", 3, "", "", "", "", "", "", "", "", "", "蜂蜜有多的话可以做一些，精神回复能力不错。", {"F_pumpkin", "F_honey", "F_honey", "G_twigs", "F_pumpkin", "F_honeycomb", "F_honeycomb", "F_berries"})
    End Sub

    Private Sub button_F_ratatouille_click(sender As Object, e As RoutedEventArgs) Handles button_F_ratatouille.Click
        F_Show("蔬菜杂烩", "Ratatouille", "F_ratatouille", 3, 25, 5, 15, 20, 0, "NoDLC", "FC_Vegetables", "×0.5", "", "", "", "", "", "", 0, "FC_Meats", "G_twigs", "", "", "", "", "", "", "", "光吃蔬菜可不好，要注意荤素搭配哦！", {"F_carrot", "F_berries", "F_berries", "F_berries", "F_red_cap", "F_red_cap", "F_red_cap", "F_red_cap"})
    End Sub

    Private Sub button_F_stuffed_eggplant_click(sender As Object, e As RoutedEventArgs) Handles button_F_stuffed_eggplant.Click
        F_Show("香酥茄盒", "Stuffed Eggplant", "F_stuffed_eggplant", 3, 37.5, 5, 15, 40, 1, "NoDLC", "FC_Vegetables", "×0.5", "", "", "F_eggplant", "×1", "", "", 3, "", "", "", "", "", "", "", "", "", "看起来蛮好吃的，不过为什么不吃回复能力更佳的红烧茄子呢？食用后5秒增加40温度。", {"F_eggplant", "F_carrot", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_taffy_click(sender As Object, e As RoutedEventArgs) Handles button_F_taffy.Click
        F_Show("太妃糖", "Taffy", "F_taffy", -3, 25, 15, 15, 40, 10, "NoDLC", "FC_Sweetener", "×3", "", "", "", "", "", "", 0, "FC_Meats", "", "", "", "", "", "", "", "", "糖吃多了可是会长蛀牙的！", {"F_honey", "F_honey", "F_honey", "G_twigs", "F_honeycomb", "F_honeycomb", "F_honeycomb", "F_berries"})
    End Sub

    Private Sub button_F_turkey_dinner_click(sender As Object, e As RoutedEventArgs) Handles button_F_turkey_dinner.Click
        F_Show("火鸡正餐", "Turkey Dinner", "F_turkey_dinner", 20, 75, 5, 6, 60, 10, "NoDLC", "FC_Fruit", "或", "FC_Vegetables", "×0.5", "F_drumstick", "×2", "FC_Meats", "×0.5", 3, "", "", "", "", "", "", "", "", "", "这个生命回复能力对比于肉丸来说还是值得做的。食用后10秒增加40温度。", {"F_drumstick", "F_drumstick", "F_morsel", "F_carrot", "F_drumstick", "F_drumstick", "F_frog_legs", "F_berries"})
    End Sub

    Private Sub button_F_unagi_click(sender As Object, e As RoutedEventArgs) Handles button_F_unagi.Click
        F_Show("鳗鱼", "Unagi", "F_unagi", 20, 18.75, 5, 10, 10, 20, "NoDLC", "F_eel", "×1", "", "", "F_lichen", "×1", "", "", 3, "", "", "", "", "", "", "", "", "", "食材需要到洞穴二层(联机版洞穴一层)钓鱼获得，而且食材需求和回复能力与炸鱼条相比实在是小巫见大巫。", {"F_eel", "F_lichen", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_waffles_click(sender As Object, e As RoutedEventArgs) Handles button_F_waffles.Click
        F_Show("华夫饼", "Waffles", "F_waffles", 60, 37.5, 5, 6, 10, 10, "NoDLC", "F_butter", "×1", "", "", "FC_Eggs", "×1", "F_berries", "×1", 3, "", "", "", "", "", "", "", "", "", "打蝴蝶出了蝴蝶黄油就做这个吧，回血能力惊人。", {"F_butter", "F_egg", "F_berries", "F_berries"})
    End Sub

    Private Sub button_F_flower_salad_click(sender As Object, e As RoutedEventArgs) Handles button_F_flower_salad.Click
        F_Show("花瓣沙拉", "Flower Salad", "F_flower_salad", 40, 12.5, 5, 6, 10, 10, "NoDLC", "F_cactus_flower", "×1", "", "", "FC_Vegetables", "×2", "", "", 0, "FC_Fruit", "FC_Meats", "FC_Eggs", "FC_Sweetener", "G_twigs", "", "", "", "", "夏天才有仙人掌花，回血能力不错。", {"F_cactus_flower", "F_eggplant", "F_pumpkin", "F_corn"})
    End Sub

    Private Sub button_F_guacamole_click(sender As Object, e As RoutedEventArgs) Handles button_F_guacamole.Click
        F_Show("鼹鼠鳄梨酱", "Guacamole", "F_guacamole", 20, 37.5, 0, 10, 10, 10, "NoDLC", "F_cactus_flesh", "×1", "", "", "F_moleworm", "×1", "", "", 0, "FC_Fruit", "", "", "", "", "", "", "", "", "食材还算比较容易获得，路过沙漠去采一点仙人掌就行了。有一定的回血能力。", {"F_cactus_flesh", "F_cactus_flesh", "F_cactus_flesh", "F_moleworm"})
    End Sub

    Private Sub button_F_ice_cream_click(sender As Object, e As RoutedEventArgs) Handles button_F_ice_cream.Click
        F_Show("冰淇淋", "Ice Creams", "F_ice_cream", 0, 25, 50, 3, 10, 10, "NoDLC", "FC_Dairy_product", "×1", "", "", "F_ice", "×1", "FC_Sweetener", "×1", 0, "FC_Vegetables", "FC_Meats", "FC_Eggs", "G_twigs", "", "", "", "", "", "精神回复神器！美中不足的是需要一个蝴蝶黄油或者电羊奶。食用后15秒降低40温度。", {"F_electric_milk", "F_ice", "F_honey", "F_honey"})
    End Sub

    Private Sub button_F_melonsicle_click(sender As Object, e As RoutedEventArgs) Handles button_F_melonsicle.Click
        F_Show("西瓜冰", "Melonsicle", "F_melonsicle", 3, 12.5, 20, 3, 10, 10, "NoDLC", "F_watermelon", "×1", "", "", "F_ice", "×1", "G_twigs", "×1", 0, "FC_Vegetables", "FC_Meats", "FC_Eggs", "", "", "", "", "", "", "回复中量精神，材料中冰块可以长期保存，西瓜也可以通过农场较容易获得。食用后10秒降低40温度。", {"F_watermelon", "F_ice", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_spicy_chili_click(sender As Object, e As RoutedEventArgs) Handles button_F_spicy_chili.Click
        F_Show("辣椒酱", "Spicy Chili", "F_spicy_chili", 20, 37.5, 0, 10, 40, 10, "NoDLC", "FC_Meats", "×1.5", "", "", "FC_Vegetables", "×1.5", "", "", 3, "", "", "", "", "", "", "", "", "", "蔬菜和肉都多到用不完的话可以做一些。食用后15秒增加40温度。", {"F_meat", "F_morsel", "F_eggplant", "F_red_cap", "F_meat", "F_meat", "F_eggplant", "F_eggplant"})
    End Sub

    Private Sub button_F_trail_mix_click(sender As Object, e As RoutedEventArgs) Handles button_F_trail_mix.Click
        F_Show("水果杂烩", "Trail Mix", "F_trail_mix", 30, 12.5, 5, 15, 10, 10, "NoDLC", "F_roasted_birchnut", "×1", "", "", "F_berries", "×1", "FC_Fruit", "×0.5", 0, "FC_Vegetables", "FC_Meats", "FC_Eggs", "FC_Dairy_product", "", "", "", "", "", "在猪王旁建家的话烤坚果是不会缺的，回复不少血量的神器。", {"F_roasted_birchnut", "F_berries", "F_berries", "F_berries"})
    End Sub

    Private Sub button_F_banana_pop_click(sender As Object, e As RoutedEventArgs) Handles button_F_banana_pop.Click
        F_Show("香蕉冰淇淋", "Banana Pop", "F_banana_pop", 20, 12.5, 33, 10, 10, 20, "SW", "F_banana", "×1", "", "", "F_ice", "×1", "G_twigs", "×1", 0, "FC_Meats", "FC_Fishes", "", "", "", "", "", "", "", "回复中量的精神，制作简单，关键是还好看！必须得用生香蕉制作哦！食用后10秒降低40温度。", {"F_banana", "F_ice", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_bisque_click(sender As Object, e As RoutedEventArgs) Handles button_F_bisque.Click
        F_Show("汤", "Bisque", "F_bisque", 60, 18.75, 5, 10, 20, 30, "SW", "F_limpets", "×3", "", "", "F_ice", "×1", "", "", 3, "", "", "", "", "", "", "", "", "", "回复大量血量，需求不少帽贝。和亚克章鱼可以交换到拖网。", {"F_limpets", "F_limpets", "F_limpets", "F_ice"})
    End Sub

    Private Sub button_F_california_roll_click(sender As Object, e As RoutedEventArgs) Handles button_F_california_roll.Click
        F_Show("加州卷", "California Roll", "F_california_roll", 20, 37.5, 10, 10, 10, 20, "SW", "F_seaweed", "×2", "", "", "FC_Fishes", "×1", "", "", 1, "", "", "", "", "", "F_seaweed", "", "", "", "比较中庸的回复食物。和亚克章鱼可以交换到草帆。", {"F_seaweed", "F_seaweed", "F_limpets", "F_limpets"})
    End Sub

    Private Sub button_F_ceviche_click(sender As Object, e As RoutedEventArgs) Handles button_F_ceviche.Click
        F_Show("橘汁腌鱼", "Ceviche", "F_ceviche", 20, 25, 5, 10, 10, 20, "SW", "FC_Fishes", "×2", "", "", "F_ice", "×1", "", "", 3, "", "", "", "", "", "", "", "", "", "嘛...根本看不出是鱼哦...和亚克章鱼可以交换到望远镜。食用后10秒降低40温度。", {"F_tropical_fish", "F_tropical_fish", "F_tropical_fish", "F_ice"})
    End Sub

    Private Sub button_F_coffee_click(sender As Object, e As RoutedEventArgs) Handles button_F_coffee.Click
        F_Show("咖啡", "Coffee", "F_coffee", 3, 9.375, -5, 10, 10, 30, "SW", "F_roasted_coffee_beans", "×3", "", "", "", "", "", "", 0, "FC_Dairy_product", "FC_Sweetener", "F_roasted_coffee_beans", "", "", "", "", "", "", "回复量感人，还减精神！不过能够提升5点速度(人物基础速度为6)，持续半天。", {"F_roasted_coffee_beans", "F_roasted_coffee_beans", "F_roasted_coffee_beans", "F_roasted_coffee_beans", "F_roasted_coffee_beans", "F_roasted_coffee_beans", "F_roasted_coffee_beans", "F_honey"})
        FL_TextBlock_Restrictions_No.Text = "仅"
    End Sub

    Private Sub button_F_fresh_fruit_crepes_click(sender As Object, e As RoutedEventArgs) Handles button_F_fresh_fruit_crepes.Click
        F_Show("新鲜水果薄饼", "Fresh Fruit Crepes", "F_fresh_fruit_crepes", 60, 150, 15, 10, 20, 30, "SW", "FC_Fruit", "×2", "", "", "F_butter", "×1", "F_honey", "×1", 3, "", "", "", "", "", "", "", "", "", "绝佳回复道具，不过需要便携式烹饪锅才能制作。", {"F_banana", "F_banana", "F_butter", "F_honey"})
        FL_image_Portable_Crock_Pot.Visibility = Visibility.Visible
    End Sub

    Private Sub button_F_jelly_O_pop_click(sender As Object, e As RoutedEventArgs) Handles button_F_jelly_O_pop.Click
        F_Show("果冻冰淇淋", "Jelly-O Pop", "F_jelly-O_pop", 20, 12.5, 0, 3, 10, 20, "SW", "F_jellyfish", "×1", "", "", "F_ice", "×1", "G_twigs", "×1", 3, "", "", "", "", "", "", "", "", "", "只能算是小甜点吧？必须得是活水母才行！和亚克章鱼可以交换到海洋陷阱。食用后10秒降低40温度。", {"F_jellyfish", "F_ice", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_lobster_bisque_click(sender As Object, e As RoutedEventArgs) Handles button_F_lobster_bisque.Click
        F_Show("龙虾浓汤", "Wobster Bisque", "F_lobster_bisque", 60, 25, 10, 10, 10, 30, "SW", "F_wobster", "×1", "", "", "F_ice", "×1", "", "", 3, "", "", "", "", "", "", "", "", "", "需要活龙虾！和亚克章鱼可以交换到海盗帽。", {"F_wobster", "F_ice", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_lobster_dinner_click(sender As Object, e As RoutedEventArgs) Handles button_F_lobster_dinner.Click
        F_Show("龙虾正餐", "Wobster Dinner", "F_lobster_dinner", 60, 37.5, 50, 15, 20, 25, "SW", "F_wobster", "×1", "", "", "F_butter", "×1", "", "", 0, "FC_Meats", "F_ice", "", "", "", "", "", "", "", "同样需要活龙虾，回复量惊人，可惜蝴蝶黄油比较罕见。和亚克章鱼可以交换到加农炮。", {"F_wobster", "F_butter", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_monster_tartare_click(sender As Object, e As RoutedEventArgs) Handles button_F_monster_tartare.Click
        F_Show("怪物鞑靼", "Monster Tartare", "F_monster_tartare", 3, 37.5, 10, 10, 20, 30, "SW", "FC_Monster_Meats", "或", "F_durian", "×2", "FC_Eggs", "×1", "FC_Vegetables", "×0.5", 3, "", "", "", "", "", "", "", "", "", "大概是怪兽肉没地方用才会做这个吧？需要便携式烹饪锅！", {"F_monster_meat", "F_monster_meat", "F_egg", "F_seaweed"})
        FL_image_Portable_Crock_Pot.Visibility = Visibility.Visible
    End Sub

    Private Sub button_F_mussel_bouillabaise_click(sender As Object, e As RoutedEventArgs) Handles button_F_mussel_bouillabaise.Click
        F_Show("贝类淡菜汤", "Mussel Bouillabaise", "F_mussel_bouillabaise", 20, 37.5, 15, 10, 20, 30, "SW", "F_mussel", "×2", "", "", "FC_Vegetables", "×2", "", "", 3, "", "", "", "", "", "", "", "", "", "同样需要便携式烹饪锅，然而回复量一般。", {"F_mussel", "F_mussel", "F_sweet_potato", "F_sweet_potato"})
        FL_image_Portable_Crock_Pot.Visibility = Visibility.Visible
    End Sub

    Private Sub button_F_seafood_gumbo_click(sender As Object, e As RoutedEventArgs) Handles button_F_seafood_gumbo.Click
        F_Show("海鲜汤", "Seafood Gumbo", "F_seafood_gumbo", 40, 37.5, 20, 10, 20, 10, "SW", "FC_Fishes", "×2.5", "", "", "", "", "", "", 3, "", "", "", "", "", "", "", "", "", "很不错的船难版回复食物！和亚克章鱼可以交换到布帆。", {"F_tropical_fish", "F_tropical_fish", "F_tropical_fish", "F_tropical_fish"})
    End Sub

    Private Sub button_F_shark_fin_soup_click(sender As Object, e As RoutedEventArgs) Handles button_F_shark_fin_soup.Click
        F_Show("鱼翅汤", "Shark Fin Soup", "F_shark_fin_soup", 40, 12.5, -10, 10, 10, 20, "SW", "F_shark_fin", "×1", "", "", "", "", "", "", 3, "", "", "", "", "", "", "", "", "", "这个精神-10，还需求一个鱼翅，食用后增加10点顽皮值。击杀海狗后不错的回复食物。", {"F_shark_fin", "G_twigs", "G_twigs", "G_twigs"})
    End Sub

    Private Sub button_F_surf_n_turf_click(sender As Object, e As RoutedEventArgs) Handles button_F_surf_n_turf.Click
        F_Show("海鲜牛排", "Surf 'n' Turf", "F_surf_'n'_turf", 60, 37.5, 33, 10, 10, 30, "SW", "FC_Meats", "×2.5", "", "", "FC_Fishes", "×1.5", "", "", 0, "F_ice", "", "", "", "", "", "", "", "", "船难版性价比最高回复料理！和亚克章鱼可以交换到船灯。", {"F_meat", "F_morsel", "F_tropical_fish", "F_tropical_fish"})
    End Sub

    Private Sub button_F_sweet_potato_souffle_click(sender As Object, e As RoutedEventArgs) Handles button_F_sweet_potato_souffle.Click
        F_Show("薯蛋奶酥", "Sweet Potato Souffle", "F_sweet_potato_souffle", 20, 37.5, 15, 10, 20, 30, "SW", "F_sweet_potato", "×2", "", "", "FC_Eggs", "×2", "", "", 3, "", "", "", "", "", "", "", "", "", "两个甘薯两个蛋，还要便携式烹饪锅，回复能力一般般。果然是厨师做的好看料理。", {"F_sweet_potato", "F_sweet_potato", "F_egg", "F_egg"})
        FL_image_Portable_Crock_Pot.Visibility = Visibility.Visible
    End Sub

    Private Sub button_F_wet_goop_click(sender As Object, e As RoutedEventArgs) Handles button_F_wet_goop.Click
        F_Show("湿腻焦糊", "Wet Goop", "F_wet_goop", 0, 0, 0, 6, 5, -2, "NoDLC", "", "任何无效菜谱", "", "", "", "", "", "", 3, "", "", "", "", "", "", "", "", "", "你到底放了些什么！", {})
    End Sub

    REM ------------------左侧面板(食材)------------------
    Private Sub F_Show_Ingredients(F_Name As String, F_EnName As String, F_picture As String, F_HealthValue As Single, F_HungerValue As Single, F_SanityValue As Single, F_PerishValue As Single, F_DLC As String, F_Image_A As String, F_TextBlock_A As String, F_Introduce As String)
        REM ------------------初始化------------------
        Canvas_FoodLeft.Height = 604
        FL_image_F_DLC.Visibility = Visibility.Collapsed
        FL_image_Portable_Crock_Pot.Visibility = Visibility.Collapsed
        TextBlock_F_Cooktime.Visibility = Visibility.Collapsed
        TextBlock_F_priority.Visibility = Visibility.Collapsed
        Image_F_PBB_Cooktime.Visibility = Visibility.Collapsed
        Image_F_PBB_Priority.Visibility = Visibility.Collapsed
        Image_F_PB_Cooktime.Visibility = Visibility.Collapsed
        Image_F_PB_Priority.Visibility = Visibility.Collapsed
        TextBlock_F_CooktimeValue.Visibility = Visibility.Collapsed
        TextBlock_F_PriorityValue.Visibility = Visibility.Collapsed
        TextBlock_F_CooKRequirements.Visibility = Visibility.Collapsed
        button_F_FoodNeed_1.Visibility = Visibility.Collapsed
        button_F_FoodNeed_or.Visibility = Visibility.Collapsed
        button_F_FoodNeed_2.Visibility = Visibility.Collapsed
        button_F_FoodNeed_3.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodNeed_1.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodNeed_or.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodNeed_2.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodNeed_3.Visibility = Visibility.Collapsed
        TextBlock_F_CookFillerRestrictions.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_No.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_LessThanOne_1.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_LessThanOne_2.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_Compare.Visibility = Visibility.Collapsed
        button_F_Restrictions_1.Visibility = Visibility.Collapsed
        button_F_Restrictions_2.Visibility = Visibility.Collapsed
        button_F_Restrictions_3.Visibility = Visibility.Collapsed
        button_F_Restrictions_4.Visibility = Visibility.Collapsed
        button_F_Restrictions_5.Visibility = Visibility.Collapsed
        button_F_Restrictions_6.Visibility = Visibility.Collapsed
        button_F_Restrictions_7.Visibility = Visibility.Collapsed
        button_F_Restrictions_Compare.Visibility = Visibility.Collapsed
        button_F_FoodAttribute_2.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Collapsed
        TextBlock_F_FoodAttribute.Visibility = Visibility.Visible
        button_F_FoodAttribute_1.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute.Visibility = Visibility.Visible
        TextBlock_F_FoodValue.Visibility = Visibility.Collapsed
        TextBlock_F_HealthValue.Foreground = Brushes.Black
        TextBlock_F_HungerValue.Foreground = Brushes.Black
        TextBlock_F_SanityValue.Foreground = Brushes.Black
        TextBlock_F_Recommend.Visibility = Visibility.Collapsed
        F_WrapPanel_Recommend_1.Visibility = Visibility.Collapsed
        F_WrapPanel_Recommend_2.Visibility = Visibility.Collapsed
        REM ------------------食物名字------------------
        FL_textBlock_FoodName.Text = F_Name
        FL_textBlock_FoodName.UpdateLayout()
        Dim F_N_MarginLeft As Integer
        F_N_MarginLeft = (Canvas_FoodLeft.ActualWidth - FL_textBlock_FoodName.ActualWidth) / 2
        Dim F_N_T As New Thickness()
        F_N_T.Top = 80
        F_N_T.Left = F_N_MarginLeft
        FL_textBlock_FoodName.Margin = F_N_T

        FL_textBlock_FoodEnName.Text = F_EnName
        FL_textBlock_FoodEnName.UpdateLayout()
        Dim F_EnN_MarginLeft As Integer
        F_EnN_MarginLeft = (Canvas_FoodLeft.ActualWidth - FL_textBlock_FoodEnName.ActualWidth) / 2
        Dim F_EnN_T As New Thickness()
        F_EnN_T.Top = 100
        F_EnN_T.Left = F_EnN_MarginLeft
        FL_textBlock_FoodEnName.Margin = F_EnN_T
        REM ------------------食物图片------------------
        FL_image_FoodPicture.Source = Picture_Short_Name(Res_Short_Name(F_picture))
        REM ------------------食物属性------------------
        TextBlock_F_HealthValue.Text = F_HealthValue
        TextBlock_F_HungerValue.Text = F_HungerValue
        TextBlock_F_SanityValue.Text = F_SanityValue
        If F_PerishValue = 1000 Then
            TextBlock_F_PerishValue.Text = "∞"
        Else
            TextBlock_F_PerishValue.Text = F_PerishValue
        End If

        If F_HealthValue >= 0 Then
            Image_F_PB_Health.Width = F_HealthValue
        Else
            Image_F_PB_Health.Width = 0
            TextBlock_F_HealthValue.Foreground = Brushes.Red
        End If
        If F_HungerValue < 0 Then
            TextBlock_F_HungerValue.Foreground = Brushes.Red
        End If
        Image_F_PB_Hunger.Width = F_HungerValue / 1.5
        If F_SanityValue >= 0 Then
            Image_F_PB_Sanity.Width = F_SanityValue / 0.5
        Else
            Image_F_PB_Sanity.Width = 0
            TextBlock_F_SanityValue.Foreground = Brushes.Red
        End If
        If F_PerishValue <= 20 Then
            Image_F_PB_Perish.Width = F_PerishValue / 0.2
        Else
            Image_F_PB_Perish.Width = 100
        End If
        REM ------------------食物DLC-------------------
        If F_DLC = "ROG" Then
            FL_image_F_DLC.Visibility = Visibility.Visible
            FL_image_F_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf F_DLC = "SW" Then
            FL_image_F_DLC.Visibility = Visibility.Visible
            FL_image_F_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf F_DLC = "DST" Then
            FL_image_F_DLC.Visibility = Visibility.Visible
            FL_image_F_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            FL_image_F_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------食物属性-------------------
        Dim F_A As New BitmapImage

        If F_Image_A = "" Then
            F_Image_A = "Resources/P_NULL.png"
        End If

        F_A.BeginInit()
        F_A.UriSource = New Uri(Res_Short_Name(F_Image_A), UriKind.Relative)
        F_A.EndInit()
        FL_image_FoodAttribute_1.Source = F_A
        FL_TextBlock_FoodAttribute.Text = F_TextBlock_A
        F_Ingredients_1 = F_Image_A
        REM ------------------第二属性鱼初始化------------------
        FL_image_FoodAttribute_Fish.Source = Picture_Short_Name(Res_Short_Name("FC_Fishes"))
        REM ------------------食物简介-------------------
        TextBlock_F_Introduce.Text = F_Introduce
        TextBlock_F_Introduce.Height = SetTextBlockHeight(F_Introduce, 14)
        TextBlock_F_Introduce.SetValue(Canvas.TopProperty, CDbl(335))
        REM ------------------关闭食物种类-------------------
        FL_CLOSE()
    End Sub

    REM ------------------食材(肉类)-------------------
    Private Sub button_F_eel_click(sender As Object, e As RoutedEventArgs) Handles button_F_eel.Click
        F_Show_Ingredients("鳗鱼", "Eel", "F_eel", 3, 10, 0, 6, "NoDLC", "FC_Meats", "×0.5", "绿色池塘垂钓获得。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_cooked_eel_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_eel.Click
        F_Show_Ingredients("煮熟的鳗鱼", "Cooked Eel", "F_cooked_eel", 8, 13, 0, 10, "NoDLC", "FC_Meats", "×0.5", "把鳗鱼放在篝火上烤制。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_fish_click(sender As Object, e As RoutedEventArgs) Handles button_F_fish.Click
        F_Show_Ingredients("鱼", "Fish", "F_fish", 1, 12.5, 0, 3, "NoDLC", "FC_Meats", "×0.5", "蓝色池塘垂钓或者猎杀鱼人掉落。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_cooked_fish_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_fish.Click
        F_Show_Ingredients("熟鱼", "Cooked Fish", "F_cooked_fish", 1, 12.5, 0, 6, "NoDLC", "FC_Meats", "×0.5", "把鱼放在篝火上烤制。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_frog_legs_click(sender As Object, e As RoutedEventArgs) Handles button_F_frog_legs.Click
        F_Show_Ingredients("青蛙腿", "Frog Legs", "F_frog_legs", 0, 12.5, -10, 6, "NoDLC", "FC_Meats", "×0.5", "狩猎青蛙和鱼人获得。")
    End Sub

    Private Sub button_F_cooked_frog_legs_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_frog_legs.Click
        F_Show_Ingredients("熟青蛙腿", "Cooked Frog Legs", "F_cooked_frog_legs", 1, 12.5, 0, 10, "NoDLC", "FC_Meats", "×0.5", "把青蛙放在篝火上烤制。")
    End Sub

    Private Sub button_F_meat_click(sender As Object, e As RoutedEventArgs) Handles button_F_meat.Click
        F_Show_Ingredients("肉", "Meat", "F_meat", 1, 25, -10, 6, "NoDLC", "FC_Meats", "×1", "狩猎猪人、牛、高脚鸟等较大生物获得。")
    End Sub

    Private Sub button_F_cooked_meat_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_meat.Click
        F_Show_Ingredients("熟肉", "Cooked Meat", "F_cooked_meat", 3, 25, 0, 10, "NoDLC", "FC_Meats", "×1", "把肉放在篝火上烤制。")
    End Sub

    Private Sub button_F_jerky_click(sender As Object, e As RoutedEventArgs) Handles button_F_jerky.Click
        F_Show_Ingredients("肉干", "Jerky", "F_jerky", 20, 25, 15, 20, "NoDLC", "FC_Meats", "×1", "把肉挂在晒肉架上风干制成。")
    End Sub

    Private Sub button_F_monster_meat_click(sender As Object, e As RoutedEventArgs) Handles button_F_monster_meat.Click
        F_Show_Ingredients("怪兽肉", "Monster Jerky", "F_monster_meat", -20, 18.75, -15, 6, "NoDLC", "FC_Monster_Meats", "×1", "狩猎蜘蛛、狗、触手、小偷等怪物获得。")
    End Sub

    Private Sub button_F_cooked_monster_meat_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_monster_meat.Click
        F_Show_Ingredients("熟怪兽肉", "Cooked Monster Meat", "F_cooked_monster_meat", -3, 18.75, -10, 15, "NoDLC", "FC_Monster_Meats", "×1", "把怪兽肉放在篝火上烤制。")
    End Sub

    Private Sub button_F_monster_jerky_click(sender As Object, e As RoutedEventArgs) Handles button_F_monster_jerky.Click
        F_Show_Ingredients("怪兽肉干", "Monster Jerky", "F_monster_jerky", -3, 18.75, -5, 20, "NoDLC", "FC_Monster_Meats", "×1", "把怪兽肉挂在晒肉架上风干制成。")
    End Sub

    Private Sub button_F_morsel_click(sender As Object, e As RoutedEventArgs) Handles button_F_morsel.Click
        F_Show_Ingredients("小肉块", "Morsel", "F_morsel", 1, 12.5, -10, 6, "NoDLC", "FC_Meats", "×0.5", "狩猎鸟、小兔、小牛等小型生物获得。")
    End Sub

    Private Sub button_F_cooked_morsel_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_morsel.Click
        F_Show_Ingredients("熟肉块", "Cooked Morsel", "F_cooked_morsel", 3, 12.5, 0, 10, "NoDLC", "FC_Meats", "×0.5", "把小块肉放在篝火上烤制。")
    End Sub

    Private Sub button_F_small_jerky_click(sender As Object, e As RoutedEventArgs) Handles button_F_small_jerky.Click
        F_Show_Ingredients("小块肉干", "Small Jerky", "F_small_jerky", 8, 12.5, 10, 20, "NoDLC", "FC_Meats", "×0.5", "把小肉块、鸡腿、鱼、青蛙腿挂在晒肉架上风干制成。")
    End Sub

    Private Sub button_F_drumstick_click(sender As Object, e As RoutedEventArgs) Handles button_F_drumstick.Click
        F_Show_Ingredients("鸡腿", "Drumstick", "F_drumstick", 0, 12.4, -10, 6, "NoDLC", "FC_Meats", "×0.5", "狩猎火鸡、渡渡鸟和莫斯林获得。")
    End Sub

    Private Sub button_F_fried_drumstick_click(sender As Object, e As RoutedEventArgs) Handles button_F_fried_drumstick.Click
        F_Show_Ingredients("炸鸡腿", "Fried Drumstick", "F_fried_drumstick", 1, 12.5, 0, 10, "NoDLC", "FC_Meats", "×0.5", "把鸡腿放在篝火上烤制。")
    End Sub

    Private Sub button_F_moleworm_click(sender As Object, e As RoutedEventArgs) Handles button_F_moleworm.Click
        F_Show_Ingredients("鼹鼠", "Moleworm", "F_moleworm", 0, 0, 0, 2, "NoDLC", "FC_Meats", "×0.5", "用锤子把鼹鼠敲晕获得，不可食用。")
    End Sub

    Private Sub button_F_tropical_fish_click(sender As Object, e As RoutedEventArgs) Handles button_F_tropical_fish.Click
        F_Show_Ingredients("热带鱼", "Tropical Fish", "F_tropical_fish", 1, 12.5, 0, 3, "SW", "FC_Meats", "×0.5", "浅滩或水坑钓鱼、狩猎鱼人或渔人获得。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_fish_morsel_click(sender As Object, e As RoutedEventArgs) Handles button_F_fish_morsel.Click
        F_Show_Ingredients("小块鱼肉", "Fish Morsel", "F_fish_morsel", 1, 12.5, 0, 3, "SW", "FC_Fishes", "×0.5", "狩猎兔蟹和宽吻海豚获得。")
    End Sub

    Private Sub button_F_cooked_fish_morsel_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_fish_morsel.Click
        F_Show_Ingredients("熟小块鱼肉", "Cooked Fish Morsel", "F_cooked_fish_morsel", 1, 12.5, 0, 6, "SW", "FC_Fishes", "×0.5", "把热带鱼或小块鱼肉放在篝火上烤制。")
    End Sub

    Private Sub button_F_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_F_jellyfish.Click
        F_Show_Ingredients("水母", "Jellyfish", "F_jellyfish", 0, 0, 0, 2, "SW", "FC_Monster_Meats", "×1", "狩猎水母获得，不可食用。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub


    Private Sub button_F_dead_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_F_dead_jellyfish.Click
        F_Show_Ingredients("死水母", "Dead Jellyfish", "F_dead_jellyfish", 10, 10, 0, 6, "SW", "FC_Monster_Meats", "×1", "水母过两天变成死水母。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_cooked_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_jellyfish.Click
        F_Show_Ingredients("熟水母", "Cooked Jellyfish", "F_cooked_jellyfish", 10, 18.75, 0, 10, "SW", "FC_Monster_Meats", "×1", "把水母或死水母放在篝火上烤制。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_dried_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_F_dried_jellyfish.Click
        F_Show_Ingredients("水母干", "Dried Jellyfish", "F_dried_jellyfish", 10, 18.75, 0, 20, "SW", "FC_Monster_Meats", "×1", "把水母挂在晒肉架上风干制成。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_limpets_click(sender As Object, e As RoutedEventArgs) Handles button_F_limpets.Click
        F_Show_Ingredients("帽贝", "Limpets", "F_limpets", 0, 12.5, -10, 6, "SW", "FC_Fishes", "×0.5", "从帽贝岩上或船的残骸上获得。")
    End Sub

    Private Sub button_F_cooked_limpets_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_limpets.Click
        F_Show_Ingredients("熟帽贝", "Cooked Limpets", "F_cooked_limpets", 1, 12.5, 0, 10, "SW", "FC_Fishes", "×0.5", "把帽贝放在篝火上烤制。")
    End Sub

    Private Sub button_F_mussel_click(sender As Object, e As RoutedEventArgs) Handles button_F_mussel.Click
        F_Show_Ingredients("贻贝", "Mussel", "F_mussel", 0, 12.5, -15, 3, "SW", "FC_Fishes", "×0.5", "用采贝器插在青口贝上过几天即可收获。")
    End Sub

    Private Sub button_F_cooked_mussel_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_mussel.Click
        F_Show_Ingredients("烤贻贝", "Cooked Mussel", "F_cooked_mussel", 1, 12.5, 0, 10, "SW", "FC_Fishes", "×0.5", "把贻贝放在篝火上烤制。")
    End Sub

    Private Sub button_F_dead_dogfish_click(sender As Object, e As RoutedEventArgs) Handles button_F_dead_dogfish.Click
        F_Show_Ingredients("死狗鱼", "Dead Dogfish", "F_dead_dogfish", 1, 25, 0, 6, "SW", "FC_Meats", "×0.5", "狩猎狗鱼获得。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_wobster_click(sender As Object, e As RoutedEventArgs) Handles button_F_wobster.Click
        F_Show_Ingredients("龙虾", "Wobster", "F_wobster", 0, 0, 0, 2, "SW", "FC_Fishes", "×2", "狩猎龙虾获得，不可食用。")
    End Sub

    Private Sub button_F_raw_fish_click(sender As Object, e As RoutedEventArgs) Handles button_F_raw_fish.Click
        F_Show_Ingredients("生鱼片", "Raw Fish", "F_raw_fish", 1, 12.5, 0, 3, "SW", "FC_Meats", "×0.5", "狩猎白鲸、蓝鲸、猫纹鲨鱼获得。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_fish_steak_click(sender As Object, e As RoutedEventArgs) Handles button_F_fish_steak.Click
        F_Show_Ingredients("鱼排", "Fish Steak", "F_fish_steak", 20, 25, 0, 6, "SW", "FC_Meats", "×0.5", "把生鱼片、死狗鱼、死旗鱼放在篝火上烤制。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    Private Sub button_F_shark_fin_click(sender As Object, e As RoutedEventArgs) Handles button_F_shark_fin.Click
        F_Show_Ingredients("鱼翅", "Shark Fin", "F_shark_fin", 20, 25, -15, 6, "SW", "FC_Meats", "×0.5", "狩猎海狗获得。")
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
        F_Ingredients_2 = "FC_Fishes"
    End Sub

    REM ------------------食材(蔬菜)-------------------
    Private Sub button_F_blue_cap_click(sender As Object, e As RoutedEventArgs) Handles button_F_blue_cap.Click
        F_Show_Ingredients("蓝蘑菇", "Blue Cap", "F_blue_cap", 20, 12.5, -15, 10, "NoDLC", "FC_Vegetables", "×0.5", "黑夜在野外采摘。")
    End Sub

    Private Sub button_F_cooked_blue_cap_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_blue_cap.Click
        F_Show_Ingredients("烤蓝蘑菇", "Cooked Blue Cap", "F_cooked_blue_cap", -3, 0, 10, 10, "NoDLC", "FC_Vegetables", "×0.5", "把蓝蘑菇放在篝火上烤制。")
    End Sub

    Private Sub button_F_green_cap_click(sender As Object, e As RoutedEventArgs) Handles button_F_green_cap.Click
        F_Show_Ingredients("绿蘑菇", "Green Cap", "F_green_cap", 0, 12.5, -50, 10, "NoDLC", "FC_Vegetables", "×0.5", "黄昏在野外采摘。")
    End Sub

    Private Sub button_F_cooked_green_cap_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_green_cap.Click
        F_Show_Ingredients("烤绿蘑菇", "Cooked Green Cap", "F_cooked_green_cap", -1, 0, 15, 10, "NoDLC", "FC_Vegetables", "×0.5", "把绿蘑菇放在篝火上烤制。")
    End Sub

    Private Sub button_F_red_cap_click(sender As Object, e As RoutedEventArgs) Handles button_F_red_cap.Click
        F_Show_Ingredients("红蘑菇", "Red Cap", "F_red_cap", -20, 12.5, 0, 10, "NoDLC", "FC_Vegetables", "×0.5", "白天在野外采摘。")
    End Sub

    Private Sub button_F_cooked_red_cap_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_red_cap.Click
        F_Show_Ingredients("烤红蘑菇", "Cooked Red Cap", "F_cooked_red_cap", 1, 0, -10, 10, "NoDLC", "FC_Vegetables", "×0.5", "把红蘑菇放在篝火上烤制。")
    End Sub

    Private Sub button_F_eggplant_click(sender As Object, e As RoutedEventArgs) Handles button_F_eggplant.Click
        F_Show_Ingredients("茄子", "Eggplant", "F_eggplant", 8, 25, 0, 10, "NoDLC", "FC_Vegetables", "×1", "农场种植。")
    End Sub

    Private Sub button_F_braised_eggplant_click(sender As Object, e As RoutedEventArgs) Handles button_F_braised_eggplant.Click
        F_Show_Ingredients("红烧茄子", "Braised Eggplant", "F_braised_eggplant", 20, 25, 0, 6, "NoDLC", "FC_Vegetables", "×1", "把茄子放在篝火上烤制。")
    End Sub

    Private Sub button_F_carrot_click(sender As Object, e As RoutedEventArgs) Handles button_F_carrot.Click
        F_Show_Ingredients("胡萝卜", "Carrot", "F_carrot", 1, 12.5, 0, 10, "NoDLC", "FC_Vegetables", "×1", "野外采摘或农场种植。")
    End Sub

    Private Sub button_F_roasted_carrot_click(sender As Object, e As RoutedEventArgs) Handles button_F_roasted_carrot.Click
        F_Show_Ingredients("烤胡萝卜", "Roasted Carrot", "F_roasted_carrot", 3, 12.5, 0, 6, "NoDLC", "FC_Vegetables", "×1", "把胡萝卜放在篝火上烤制。")
    End Sub

    Private Sub button_F_corn_click(sender As Object, e As RoutedEventArgs) Handles button_F_corn.Click
        F_Show_Ingredients("玉米", "Corn", "F_corn", 3, 25, 0, 10, "NoDLC", "FC_Vegetables", "×1", "农场种植。")
    End Sub

    Private Sub button_F_popcorn_click(sender As Object, e As RoutedEventArgs) Handles button_F_popcorn.Click
        F_Show_Ingredients("爆米花", "Popcorn", "F_popcorn", 3, 12.5, 0, 15, "NoDLC", "FC_Vegetables", "×1", "把玉米放在篝火上烤制。")
    End Sub

    Private Sub button_F_pumpkin_click(sender As Object, e As RoutedEventArgs) Handles button_F_pumpkin.Click
        F_Show_Ingredients("南瓜", "Pumpkin", "F_pumpkin", 3, 37.5, 0, 10, "NoDLC", "FC_Vegetables", "×1", "农场种植。")
    End Sub

    Private Sub button_F_hot_pumpkin_click(sender As Object, e As RoutedEventArgs) Handles button_F_hot_pumpkin.Click
        F_Show_Ingredients("热南瓜", "Hot Pumpkin", "F_hot_pumpkin", 8, 37.5, 0, 6, "NoDLC", "FC_Vegetables", "×1", "把南瓜放在篝火上烤制。")
    End Sub

    Private Sub button_F_cactus_flesh_click(sender As Object, e As RoutedEventArgs) Handles button_F_cactus_flesh.Click
        F_Show_Ingredients("仙人掌肉", "Cactus Flesh", "F_cactus_flesh", -3, 12.5, -5, 10, "NoDLC", "FC_Vegetables", "×1", "沙漠采摘。")
    End Sub

    Private Sub button_F_cooked_cactus_flesh_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_cactus_flesh.Click
        F_Show_Ingredients("煮好的仙人掌肉", "Cooked Cactus Flesh", "F_cooked_cactus_flesh", 1, 12.5, 15, 10, "NoDLC", "FC_Vegetables", "×1", "把仙人掌放在篝火上烤制。")
    End Sub

    Private Sub button_F_cactus_flower_click(sender As Object, e As RoutedEventArgs) Handles button_F_cactus_flower.Click
        F_Show_Ingredients("仙人掌花", "Cactus Flower", "F_cactus_flower", 8, 12.5, 5, 10, "NoDLC", "FC_Vegetables", "×1", "夏天在沙漠采摘。")
    End Sub

    Private Sub button_F_sweet_potato_click(sender As Object, e As RoutedEventArgs) Handles button_F_sweet_potato.Click
        F_Show_Ingredients("甘薯", "Sweet Potato", "F_sweet_potato", 1, 12.5, 0, 10, "SW", "FC_Vegetables", "×1", "野外采摘或农场种植。")
    End Sub

    Private Sub button_F_cooked_sweet_potato_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_sweet_potato.Click
        F_Show_Ingredients("烤甘薯", "Cooked Sweet Potato", "F_cooked_sweet_potato", 3, 12.5, 0, 6, "SW", "FC_Vegetables", "×1", "把甘薯放在篝火上烤制。")
    End Sub

    Private Sub button_F_seaweed_click(sender As Object, e As RoutedEventArgs) Handles button_F_seaweed.Click
        F_Show_Ingredients("海藻", "Seaweed", "F_seaweed", 1, 9.375, -10, 6, "SW", "FC_Vegetables", "×0.5", "海上采摘。")
    End Sub

    Private Sub button_F_roasted_seaweed_click(sender As Object, e As RoutedEventArgs) Handles button_F_roasted_seaweed.Click
        F_Show_Ingredients("烤海藻", "Roasted Seaweed", "F_roasted_seaweed", 3, 9.375, 0, 10, "SW", "FC_Vegetables", "×0.5", "把海藻放在篝火上烤制。")
    End Sub

    Private Sub button_F_dried_seaweed_click(sender As Object, e As RoutedEventArgs) Handles button_F_dried_seaweed.Click
        F_Show_Ingredients("晾干的海藻", "Dried Seaweed", "F_dried_seaweed", 3, 12.5, 0, 20, "SW", "FC_Vegetables", "×0.5", "把海藻挂在晒肉架上风干制成。")
    End Sub

    REM ------------------食材(水果)-------------------
    Private Sub button_F_banana_click(sender As Object, e As RoutedEventArgs) Handles button_F_banana.Click
        F_Show_Ingredients("香蕉", "Banana", "F_banana", 1, 12.5, 0, 10, "NoDLC", "FC_Fruit", "×1", "香蕉树、丛林树(船难)采摘或狩猎猴子获得。")
    End Sub

    Private Sub button_F_cooked_banana_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_banana.Click
        F_Show_Ingredients("熟香蕉", "Cooked Banana", "F_cooked_banana", 3, 12.5, 0, 6, "NoDLC", "FC_Fruit", "×1", "把香蕉放在篝火上烤制。")
    End Sub

    Private Sub button_F_juicy_berries_click(sender As Object, e As RoutedEventArgs) Handles button_F_juicy_berries.Click
        F_Show_Ingredients("蜜汁浆果", "Juicy Berries", "F_juicy_berries", 1, 12.5, 0, 2, "DST", "FC_Fruit", "×0.5", "蜜汁浆果丛采摘。")
    End Sub

    Private Sub button_F_roasted_juicy_berries_click(sender As Object, e As RoutedEventArgs) Handles button_F_roasted_juicy_berries.Click
        F_Show_Ingredients("烤蜜汁浆果", "Roasted Juicy Berries", "F_roasted_juicy_berries", 3, 18.75, 0, 1, "DST", "FC_Fruit", "×0.5", "把蜜汁浆果放在篝火上烤制。")
    End Sub

    Private Sub button_F_berries_click(sender As Object, e As RoutedEventArgs) Handles button_F_berries.Click
        F_Show_Ingredients("浆果", "Berries", "F_berries", 0, 9.375, 0, 6, "NoDLC", "FC_Fruit", "×0.5", "浆果灌木丛采摘。")
    End Sub

    Private Sub button_F_roasted_berrie_click(sender As Object, e As RoutedEventArgs) Handles button_F_roasted_berrie.Click
        F_Show_Ingredients("烤浆果", "Roasted Berries", "F_roasted_berrie", 1, 12.5, 0, 3, "NoDLC", "FC_Fruit", "×0.5", "把浆果放在篝火上烤制。")
    End Sub

    Private Sub button_F_dragon_fruit_click(sender As Object, e As RoutedEventArgs) Handles button_F_dragon_fruit.Click
        F_Show_Ingredients("火龙果", "Dragon Fruit", "F_dragon_fruit", 3, 9.375, 0, 6, "NoDLC", "FC_Fruit", "×1", "农场种植。")
    End Sub

    Private Sub button_F_prepared_dragon_fruit_click(sender As Object, e As RoutedEventArgs) Handles button_F_prepared_dragon_fruit.Click
        F_Show_Ingredients("精致火龙果", "Prepared Dragon Fruit", "F_prepared_dragon_fruit", 20, 12.5, 0, 3, "NoDLC", "FC_Fruit", "×1", "把火龙果放在篝火上烤制。")
    End Sub

    Private Sub button_F_durian_click(sender As Object, e As RoutedEventArgs) Handles button_F_durian.Click
        F_Show_Ingredients("榴莲", "", "F_durian", -3, 25, -5, 10, "NoDLC", "FC_Fruit", "×1", "农场种植。")
        F_Ingredients_2 = "FC_Monster_Meats"
        FL_image_FoodAttribute_Fish.Source = Picture_Short_Name(Res_Short_Name("FC_Monster_Meats"))
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
    End Sub

    Private Sub button_F_extra_smelly_durian_click(sender As Object, e As RoutedEventArgs) Handles button_F_extra_smelly_durian.Click
        F_Show_Ingredients("超臭榴莲", "Extra Smelly Durian", "F_extra_smelly_durian", 0, 25, -5, 6, "NoDLC", "FC_Fruit", "×1", "把榴莲放在篝火上烤制。")
        F_Ingredients_2 = "FC_Monster_Meats"
        FL_image_FoodAttribute_Fish.Source = Picture_Short_Name(Res_Short_Name("FC_Monster_Meats"))
        button_F_FoodAttribute_2.Visibility = Visibility.Visible
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Visible
    End Sub

    Private Sub button_F_pomegranate_click(sender As Object, e As RoutedEventArgs) Handles button_F_pomegranate.Click
        F_Show_Ingredients("石榴", "Pomegranate", "F_pomegranate", 3, 9.375, 0, 6, "NoDLC", "FC_Fruit", "×1", "农场种植。")
    End Sub

    Private Sub button_F_sliced_pomegranate_click(sender As Object, e As RoutedEventArgs) Handles button_F_sliced_pomegranate.Click
        F_Show_Ingredients("石榴片", "Sliced Pomegranate", "F_sliced_pomegranate", 20, 12.5, 0, 3, "NoDLC", "FC_Fruit", "×1", "把石榴放在篝火上烤制。")
    End Sub

    Private Sub button_F_watermelon_click(sender As Object, e As RoutedEventArgs) Handles button_F_watermelon.Click
        F_Show_Ingredients("西瓜", "Watermelon", "F_watermelon", 3, 12.5, 5, 8, "NoDLC", "FC_Fruit", "×1", "农场种植。食用后5秒降低40温度。")
    End Sub

    Private Sub button_F_grilled_watermelon_click(sender As Object, e As RoutedEventArgs) Handles button_F_grilled_watermelon.Click
        F_Show_Ingredients("烤西瓜", "Grilled Watermelon", "F_grilled_watermelon", 1, 12.5, 7.5, 8, "NoDLC", "FC_Fruit", "×1", "把西瓜放在篝火上烤制。食用后5秒降低40温度。")
    End Sub

    Private Sub button_F_halved_coconut_click(sender As Object, e As RoutedEventArgs) Handles button_F_halved_coconut.Click
        F_Show_Ingredients("半个椰子", "Halved Coconut", "F_halved_coconut", 1, 4.6875, 0, 10, "SW", "FC_Fruit", "×1", "用砍刀把椰子劈开。")
    End Sub

    Private Sub button_F_roasted_coconut_click(sender As Object, e As RoutedEventArgs) Handles button_F_roasted_coconut.Click
        F_Show_Ingredients("烤椰子", "Roasted Coconut", "F_roasted_coconut", 1, 9.375, 0, 10, "SW", "FC_Fruit", "×1", "把半个椰子放在篝火上烤制。")
    End Sub

    REM ------------------食材(蛋类)-------------------
    Private Sub button_F_egg_click(sender As Object, e As RoutedEventArgs) Handles button_F_egg.Click
        F_Show_Ingredients("蛋", "Egg", "F_egg", 0, 9.375, 0, 10, "NoDLC", "FC_Eggs", "×1", "喂养被囚禁的鸟任何肉类或肉类食品(除了生的怪兽肉)获得，也可以在企鹅筑巢区发现，在船难版砍倒丛林树也会掉落。")
    End Sub

    Private Sub button_F_cooked_egg_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_egg.Click
        F_Show_Ingredients("煎蛋", "Cooked Egg", "F_cooked_egg", 0, 12.5, 0, 6, "NoDLC", "FC_Eggs", "×1", "把蛋放在篝火上烤制。")
    End Sub

    Private Sub button_F_tallbird_egg_click(sender As Object, e As RoutedEventArgs) Handles button_F_tallbird_egg.Click
        F_Show_Ingredients("高脚鸟蛋", "Tallbird Egg", "F_tallbird_egg", 3, 25, 0, 1000, "NoDLC", "FC_Eggs", "×4", "高脚鸟巢穴获得。")
    End Sub

    Private Sub button_F_fried_tallbird_egg_click(sender As Object, e As RoutedEventArgs) Handles button_F_fried_tallbird_egg.Click
        F_Show_Ingredients("油煎高脚鸟蛋", "Fried Tallbird Egg", "F_fried_tallbird_egg", 0, 37.5, 0, 6, "NoDLC", "FC_Eggs", "×4", "把高脚鸟蛋放在篝火上烤制。")
    End Sub

    Private Sub button_F_doydoy_egg_click(sender As Object, e As RoutedEventArgs) Handles button_F_doydoy_egg.Click
        F_Show_Ingredients("渡渡鸟蛋", "Doydoy Egg", "F_doydoy_egg", 3, 25, 0, 10, "SW", "FC_Eggs", "×1", "渡渡鸟巢穴获得。")
    End Sub

    Private Sub button_F_fried_doydoy_egg_click(sender As Object, e As RoutedEventArgs) Handles button_F_fried_doydoy_egg.Click
        F_Show_Ingredients("渡渡鸟煎蛋", "Fried DoyDoy Egg", "F_fried_doydoy_egg", 0, 37.5, 0, 6, "SW", "FC_Eggs", "×1", "把渡渡鸟蛋放在篝火上烤制。")
    End Sub

    REM ------------------食材(其他)-------------------
    Private Sub button_F_butterfly_wing_click(sender As Object, e As RoutedEventArgs) Handles button_F_butterfly_wing.Click
        F_Show_Ingredients("蝴蝶翅膀", "Butterfly Wing", "F_butterfly_wing", 8, 9.375, 0, 6, "NoDLC", "F_butterfly_wing", "×1", "猎杀蝴蝶获得。")
    End Sub

    Private Sub button_F_butter_click(sender As Object, e As RoutedEventArgs) Handles button_F_butter.Click
        F_Show_Ingredients("蝴蝶黄油", "Butter", "F_butter", 40, 25, 0, 40, "NoDLC", "FC_Dairy_product", "×1", "猎杀蝴蝶2%概率获得。")
    End Sub

    Private Sub button_F_honey_click(sender As Object, e As RoutedEventArgs) Handles button_F_honey.Click
        F_Show_Ingredients("蜂蜜", "Honey", "F_honey", 3, 9.375, 0, 40, "NoDLC", "FC_Sweetener", "×1", "猎杀蜜蜂或拆除蜂巢获得。")
    End Sub

    Private Sub button_F_honeycomb_click(sender As Object, e As RoutedEventArgs) Handles button_F_honeycomb.Click
        F_Show_Ingredients("蜂巢", "Honeycomb", "F_honeycomb", 0, 0, 0, 1000, "NoDLC", "FC_Sweetener", "×1", "拆除蜂巢获得。")
    End Sub

    Private Sub button_F_lichen_click(sender As Object, e As RoutedEventArgs) Handles button_F_lichen.Click
        F_Show_Ingredients("苔藓", "Lichen", "F_lichen", 3, 12, -5, 2, "NoDLC", "FC_Vegetables", "×1", "洞穴采摘。")
    End Sub

    Private Sub button_F_mandrake_click(sender As Object, e As RoutedEventArgs) Handles button_F_mandrake.Click
        F_Show_Ingredients("曼德拉草", "Mandrake", "F_mandrake", 60, 75, 0, 1000, "NoDLC", "FC_Vegetables", "×1", "白天在草原(曼德拉平原)或洞穴拔起曼德拉草获得。")
    End Sub

    Private Sub button_F_electric_milk_click(sender As Object, e As RoutedEventArgs) Handles button_F_electric_milk.Click
        F_Show_Ingredients("电羊奶", "Electric Milk", "F_electric_milk", 3, 12.5, 5, 6, "ROG", "FC_Dairy_product", "×1", "猎杀带电的伏特山羊获得。")
    End Sub

    Private Sub button_F_ice_click(sender As Object, e As RoutedEventArgs) Handles button_F_ice.Click
        F_Show_Ingredients("冰块", "Ice", "F_ice", 0.5, 2.3, 0, 3, "NoDLC", "F_ice", "×1", "挖迷你冰川获得。食用后7.5秒降低40温度。")
    End Sub

    Private Sub button_F_roasted_birchnut_click(sender As Object, e As RoutedEventArgs) Handles button_F_roasted_birchnut.Click
        F_Show_Ingredients("烤坚果", "Roasted Birchnut", "F_roasted_birchnut", 1, 9.375, 0, 6, "ROG", "NULL", "填充物×1", "把坚果放在篝火上烤制。")
    End Sub

    Private Sub button_F_twigs_click(sender As Object, e As RoutedEventArgs) Handles button_F_twigs.Click
        F_Show_Ingredients("树枝", "Twigs", "G_twigs", 0, 0, 0, 1000, "NoDLC", "G_twigs", "×1", "砍伐多枝的树、红树林或采集树苗获得。")
    End Sub

    Private Sub button_F_butterfly_wing_sw_click(sender As Object, e As RoutedEventArgs) Handles button_F_butterfly_wing_sw.Click
        F_Show_Ingredients("蝴蝶翅膀(船难)", "Butterfly Wing(SW)", "F_butterfly_wing_sw", 8, 9.375, 0, 6, "SW", "F_butterfly_wing", "×1", "猎杀蝴蝶获得。")
    End Sub

    Private Sub button_F_coffee_beans_click(sender As Object, e As RoutedEventArgs) Handles button_F_coffee_beans.Click
        F_Show_Ingredients("咖啡豆", "Coffee Beans", "F_coffee_beans", 0, 9.375, 0, 6, "SW", "FC_Fruit", "×0.5", "咖啡树采摘。")
    End Sub

    Private Sub button_F_roasted_coffee_beans_click(sender As Object, e As RoutedEventArgs) Handles button_F_roasted_coffee_beans.Click
        F_Show_Ingredients("烘咖啡豆", "Roasted Coffee Beans", "F_roasted_coffee_beans", 0, 9.375, -5, 15, "SW", "FC_Fruit", "×1", "把咖啡豆放在篝火上烤制。")
    End Sub

    REM ------------------左侧面板(非食材)------------------
    Private Sub F_Show_Uningredients(F_Name As String, F_EnName As String, F_picture As String, F_HealthValue As Single, F_HungerValue As Single, F_SanityValue As Single, F_PerishValue As Single, F_FoodValue As String, F_DLC As String, F_Introduce As String)
        REM ------------------初始化------------------
        Canvas_FoodLeft.Height = 604
        FL_image_F_DLC.Visibility = Visibility.Collapsed
        FL_image_Portable_Crock_Pot.Visibility = Visibility.Collapsed
        TextBlock_F_Cooktime.Visibility = Visibility.Collapsed
        TextBlock_F_priority.Visibility = Visibility.Collapsed
        Image_F_PBB_Cooktime.Visibility = Visibility.Collapsed
        Image_F_PBB_Priority.Visibility = Visibility.Collapsed
        Image_F_PB_Cooktime.Visibility = Visibility.Collapsed
        Image_F_PB_Priority.Visibility = Visibility.Collapsed
        TextBlock_F_CooktimeValue.Visibility = Visibility.Collapsed
        TextBlock_F_PriorityValue.Visibility = Visibility.Collapsed
        TextBlock_F_CooKRequirements.Visibility = Visibility.Collapsed
        button_F_FoodNeed_1.Visibility = Visibility.Collapsed
        button_F_FoodNeed_or.Visibility = Visibility.Collapsed
        button_F_FoodNeed_2.Visibility = Visibility.Collapsed
        button_F_FoodNeed_3.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodNeed_1.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodNeed_or.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodNeed_2.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodNeed_3.Visibility = Visibility.Collapsed
        TextBlock_F_CookFillerRestrictions.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_No.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_LessThanOne_1.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_LessThanOne_2.Visibility = Visibility.Collapsed
        FL_TextBlock_Restrictions_Compare.Visibility = Visibility.Collapsed
        button_F_Restrictions_1.Visibility = Visibility.Collapsed
        button_F_Restrictions_2.Visibility = Visibility.Collapsed
        button_F_Restrictions_3.Visibility = Visibility.Collapsed
        button_F_Restrictions_4.Visibility = Visibility.Collapsed
        button_F_Restrictions_5.Visibility = Visibility.Collapsed
        button_F_Restrictions_6.Visibility = Visibility.Collapsed
        button_F_Restrictions_7.Visibility = Visibility.Collapsed
        button_F_Restrictions_Compare.Visibility = Visibility.Collapsed
        button_F_FoodAttribute_1.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodAttribute_Fish.Visibility = Visibility.Collapsed
        TextBlock_F_FoodAttribute.Visibility = Visibility.Collapsed
        button_F_FoodAttribute_2.Visibility = Visibility.Collapsed
        FL_TextBlock_FoodAttribute.Visibility = Visibility.Collapsed
        TextBlock_F_FoodValue.Visibility = Visibility.Collapsed
        TextBlock_F_HealthValue.Foreground = Brushes.Black
        TextBlock_F_HungerValue.Foreground = Brushes.Black
        TextBlock_F_SanityValue.Foreground = Brushes.Black
        TextBlock_F_Recommend.Visibility = Visibility.Collapsed
        F_WrapPanel_Recommend_1.Visibility = Visibility.Collapsed
        F_WrapPanel_Recommend_2.Visibility = Visibility.Collapsed
        REM ------------------食物名字------------------
        FL_textBlock_FoodName.Text = F_Name
        FL_textBlock_FoodName.UpdateLayout()
        Dim F_N_MarginLeft As Integer
        F_N_MarginLeft = (Canvas_FoodLeft.ActualWidth - FL_textBlock_FoodName.ActualWidth) / 2
        Dim F_N_T As New Thickness()
        F_N_T.Top = 80
        F_N_T.Left = F_N_MarginLeft
        FL_textBlock_FoodName.Margin = F_N_T

        FL_textBlock_FoodEnName.Text = F_EnName
        FL_textBlock_FoodEnName.UpdateLayout()
        Dim F_EnN_MarginLeft As Integer
        F_EnN_MarginLeft = (Canvas_FoodLeft.ActualWidth - FL_textBlock_FoodEnName.ActualWidth) / 2
        Dim F_EnN_T As New Thickness()
        F_EnN_T.Top = 100
        F_EnN_T.Left = F_EnN_MarginLeft
        FL_textBlock_FoodEnName.Margin = F_EnN_T
        REM ------------------食物图片------------------
        FL_image_FoodPicture.Source = Picture_Short_Name(Res_Short_Name(F_picture))
        REM ------------------食物属性------------------
        TextBlock_F_HealthValue.Text = F_HealthValue
        TextBlock_F_HungerValue.Text = F_HungerValue
        TextBlock_F_SanityValue.Text = F_SanityValue
        If F_PerishValue = 1000 Then
            TextBlock_F_PerishValue.Text = "∞"
        Else
            TextBlock_F_PerishValue.Text = F_PerishValue
        End If

        If F_HealthValue >= 0 Then
            Image_F_PB_Health.Width = F_HealthValue
        Else
            Image_F_PB_Health.Width = 0
            TextBlock_F_HealthValue.Foreground = Brushes.Red
        End If
        If F_HungerValue >= 0 Then
            Image_F_PB_Hunger.Width = F_HungerValue / 1.5
        Else
            Image_F_PB_Hunger.Width = 0
            TextBlock_F_HungerValue.Foreground = Brushes.Red
        End If
        If F_SanityValue >= 0 Then
            Image_F_PB_Sanity.Width = F_SanityValue / 0.5
        Else
            Image_F_PB_Sanity.Width = 0
            TextBlock_F_SanityValue.Foreground = Brushes.Red
        End If
        If F_PerishValue <= 20 Then
            Image_F_PB_Perish.Width = F_PerishValue / 0.2
        Else
            Image_F_PB_Perish.Width = 100
        End If
        REM ------------------食物荤素------------------
        If F_FoodValue <> "" Then
            TextBlock_F_FoodValue.Text = "食物属性：" + F_FoodValue
            TextBlock_F_FoodValue.Visibility = Visibility.Visible
        End If

        REM ------------------食物DLC-------------------
        If F_DLC = "ROG" Then
            FL_image_F_DLC.Visibility = Visibility.Visible
            FL_image_F_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf F_DLC = "SW" Then
            FL_image_F_DLC.Visibility = Visibility.Visible
            FL_image_F_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        Else
            FL_image_F_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------食物简介-------------------
        TextBlock_F_Introduce.Text = F_Introduce
        TextBlock_F_Introduce.Height = SetTextBlockHeight(F_Introduce, 14)
        TextBlock_F_Introduce.SetValue(Canvas.TopProperty, CDbl(335))
        REM ------------------关闭食物种类-------------------
        FL_CLOSE()
    End Sub

    REM ------------------非食材-------------------
    Private Sub button_F_batilisk_wing_click(sender As Object, e As RoutedEventArgs) Handles button_F_batilisk_wing.Click
        F_Show_Uningredients("黑蝙蝠翅膀", "Batilisk Wing", "F_batilisk_wing", 3, 12.5, -10, 6, "荤", "NoDLC", "狩猎蝙蝠获得。")
    End Sub

    Private Sub button_F_cooked_batilisk_wing_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_batilisk_wing.Click
        F_Show_Uningredients("煮熟的黑蝙蝠翅膀", "Cooked Batilisk Wing", "F_cooked_batilisk_wing", 8, 18.75, 0, 10, "荤", "NoDLC", "把黑蝙蝠翅膀放在篝火上烤制。")
    End Sub

    Private Sub button_F_leafy_meat_click(sender As Object, e As RoutedEventArgs) Handles button_F_leafy_meat.Click
        F_Show_Uningredients("多叶的肉", "Leafy Meat", "F_leafy_meat", 0, 12.5, -10, 6, "荤", "NoDLC", "猎杀食人花获得或者直接摘取。")
    End Sub

    Private Sub button_F_cooked_leafy_meat_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_leafy_meat.Click
        F_Show_Uningredients("煮熟的多叶的肉", "Cooked Leafy Meat", "F_cooked_leafy_meat", 1, 18.75, 0, 10, "荤", "NoDLC", "把多叶的肉放在篝火上烤制。")
    End Sub

    Private Sub button_F_cooked_mandrake_click(sender As Object, e As RoutedEventArgs) Handles button_F_cooked_mandrake.Click
        F_Show_Uningredients("熟曼德拉草", "Cooked Mandrake", "F_cooked_mandrake", 100, 150, 0, 1000, "素", "NoDLC", "把曼德拉草放在篝火上烤制。")
    End Sub

    Private Sub button_F_petals_click(sender As Object, e As RoutedEventArgs) Handles button_F_petals.Click
        F_Show_Uningredients("花瓣", "Petals", "F_petals", 1, 0, 0, 6, "素", "NoDLC", "野外采摘(采摘时也加5点精神)。")
    End Sub

    Private Sub button_F_dark_petals_click(sender As Object, e As RoutedEventArgs) Handles button_F_dark_petals.Click
        F_Show_Uningredients("恶魔花瓣", "Dark Petals", "F_dark_petals", 0, 0, -5, 6, "素", "NoDLC", "野外采摘(采摘时也减5点精神)。")
    End Sub

    Private Sub button_F_deerclops_eyeball_click(sender As Object, e As RoutedEventArgs) Handles button_F_deerclops_eyeball.Click
        F_Show_Uningredients("巨鹿眼球", "Deerclops Eyeball", "F_deerclops_eyeball", 60, 75, -15, 1000, "荤", "NoDLC", "击杀独眼巨鹿获得。")
    End Sub

    Private Sub button_F_foliage_click(sender As Object, e As RoutedEventArgs) Handles button_F_foliage.Click
        F_Show_Uningredients("蕨叶", "Foliage", "F_foliage", 1, 0, 0, 6, "素", "NoDLC", "洞穴采摘蕨类植物获得。")
    End Sub

    Private Sub button_F_gears_click(sender As Object, e As RoutedEventArgs) Handles button_F_gears.Click
        F_Show_Uningredients("齿轮(仅机器人)", "Gears", "F_gears", 60, 75, 50, 1000, "", "NoDLC", "击杀发条生物获得。")
    End Sub

    Private Sub button_F_glommers_goop_click(sender As Object, e As RoutedEventArgs) Handles button_F_glommers_goop.Click
        F_Show_Uningredients("格洛姆粘液", "Glommer's Goop", "F_glommer's_goop", 40, 9.375, -50, 1000, "素", "NoDLC", "格罗姆每过一段时间会生成一个，被击杀也会掉落。")
    End Sub

    Private Sub button_F_phlegm_click(sender As Object, e As RoutedEventArgs) Handles button_F_phlegm.Click
        F_Show_Uningredients("痰", "Phlegm", "F_phlegm", 0, 12.5, -15, 1000, "素", "NoDLC", "击杀钢羊获得。")
    End Sub

    Private Sub button_F_glow_berry_click(sender As Object, e As RoutedEventArgs) Handles button_F_glow_berry.Click
        F_Show_Uningredients("发光浆果", "Glow Berry", "F_glow_berry", 10, 25, -15, 10, "素", "NoDLC", "猎杀深渊蠕虫获得，放在地上有小范围照明效果，食用有较大范围照明效果。")
    End Sub

    Private Sub button_F_lesser_glow_berry_click(sender As Object, e As RoutedEventArgs) Handles button_F_lesser_glow_berry.Click
        F_Show_Uningredients("小发光浆果", "Lesser Glow Berry", "F_lesser_glow_berry", 3, 12.5, -10, 10, "素", "NoDLC", "奇怪的植物采摘，放在地上有小范围照明效果，食用有较大范围照明效果。")
    End Sub

    Private Sub button_F_guardians_horn_click(sender As Object, e As RoutedEventArgs) Handles button_F_guardians_horn.Click
        F_Show_Uningredients("守卫者的角", "Guardian's Horn", "F_guardian's_horn", 60, 75, -15, 1000, "荤", "NoDLC", "狩猎远古守护者获得。")
    End Sub

    Private Sub button_F_hatching_tallbird_egg_click(sender As Object, e As RoutedEventArgs) Handles button_F_hatching_tallbird_egg.Click
        F_Show_Uningredients("孵化的高脚鸟蛋", "Hatching Tallbird Egg", "F_hatching_tallbird_egg", 3, 25, 0, 1000, "荤", "NoDLC", "黄昏或晚上把高脚鸟蛋放在篝火旁获得。")
    End Sub

    Private Sub button_F_koalefant_trunk_click(sender As Object, e As RoutedEventArgs) Handles button_F_koalefant_trunk.Click
        F_Show_Uningredients("红色象鼻", "Koalefant Trunk", "F_koalefant_trunk", 30, 37.5, 0, 6, "荤", "NoDLC", "猎杀非冬季考拉象获得。")
    End Sub

    Private Sub button_F_winter_koalefant_trunk_click(sender As Object, e As RoutedEventArgs) Handles button_F_winter_koalefant_trunk.Click
        F_Show_Uningredients("蓝色象鼻", "Winter Koalefant Trunk", "F_winter_koalefant_trunk", 30, 37.5, 0, 6, "荤", "NoDLC", "猎杀冬季冬考拉象获得。")
    End Sub

    Private Sub button_F_koalefant_trunk_steak_click(sender As Object, e As RoutedEventArgs) Handles button_F_koalefant_trunk_steak.Click
        F_Show_Uningredients("烤熟象鼻", "Koalefant Trunk Steak", "F_koalefant_trunk_steak", 40, 75, 0, 15, "荤", "NoDLC", "把红色象鼻或蓝色象鼻放在篝火上烤制。")
    End Sub

    Private Sub button_F_light_bulb_click(sender As Object, e As RoutedEventArgs) Handles button_F_light_bulb.Click
        F_Show_Uningredients("荧光果", "Light Bulb", "F_light_bulb", 1, 0, 0, 6, "素", "NoDLC", "荧光草采摘，放在地上或带在身上有小范围照明效果。")
    End Sub

    Private Sub button_F_rotten_egg_click(sender As Object, e As RoutedEventArgs) Handles button_F_rotten_egg.Click
        F_Show_Uningredients("腐烂的蛋", "Rotten Egg", "F_rotten_egg", -1, -10, 0, 1000, "素", "NoDLC", "把蛋放在地上十天获得。")
    End Sub

    Private Sub button_F_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_seeds.Click
        F_Show_Uningredients("种子", "Seeds", "F_seeds", 0, 4.6875, 0, 40, "素", "NoDLC", "种子或所有农作物喂食被囚禁的鸟有几率获得(联机版里给被囚禁的鸟喂食种子不会获得种子，而是有几率获得鸟屎)。")
    End Sub

    Private Sub button_F_toasted_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_toasted_seeds.Click
        F_Show_Uningredients("烤种子", "Toasted Seeds", "F_toasted_seeds", 1, 4.6875, 0, 10, "素", "NoDLC", "把种子放在篝火上烤制。")
    End Sub

    Private Sub button_F_carrot_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_carrot_seeds.Click
        F_Show_Uningredients("胡萝卜种子", "Carrot Seeds", "F_carrot_seeds", 0.5, 9.375, 0, 40, "素", "NoDLC", "胡萝卜喂食被囚禁的鸟换取(有几率额外获得一个)。")
    End Sub

    Private Sub button_F_corn_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_corn_seeds.Click
        F_Show_Uningredients("玉米种子", "Corn Seeds", "F_corn_seeds", 0.5, 9.375, 0, 40, "素", "NoDLC", "玉米喂食被囚禁的鸟换取(有几率额外获得一个)。")
    End Sub

    Private Sub button_F_dragon_fruit_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_dragon_fruit_seeds.Click
        F_Show_Uningredients("火龙果种子", "Dragon Fruit Seeds", "F_dragon_fruit_seeds", 0.5, 9.375, 0, 40, "素", "NoDLC", "火龙果喂食被囚禁的鸟换取(有几率额外获得一个)。")
    End Sub

    Private Sub button_F_durian_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_durian_seeds.Click
        F_Show_Uningredients("榴莲种子", "Durian Seeds", "F_durian_seeds", 0.5, 9.375, 0, 40, "素", "NoDLC", "榴莲喂食被囚禁的鸟换取(有几率额外获得一个)。")
    End Sub

    Private Sub button_F_eggplant_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_eggplant_seeds.Click
        F_Show_Uningredients("茄子种子", "Eggplant Seeds", "F_eggplant_seeds", 0.5, 9.375, 0, 40, "素", "NoDLC", "茄子喂食被囚禁的鸟换取(有几率额外获得一个)。")
    End Sub

    Private Sub button_F_pomegranate_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_pomegranate_seeds.Click
        F_Show_Uningredients("石榴种子", "Pomegranate Seeds", "F_pomegranate_seeds", 0.5, 9.375, 0, 40, "素", "NoDLC", "石榴喂食被囚禁的鸟换取(有几率额外获得一个)。")
    End Sub

    Private Sub button_F_pumpkin_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_pumpkin_seeds.Click
        F_Show_Uningredients("南瓜种子", "Pumpkin Seeds", "F_pumpkin_seeds", 0.5, 9.375, 0, 40, "素", "NoDLC", "南瓜喂食被囚禁的鸟换取(有几率额外获得一个)。")
    End Sub

    Private Sub button_F_watermelon_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_watermelon_seeds.Click
        F_Show_Uningredients("西瓜种子", "Watermelon Seeds", "F_watermelon_seeds", 0.5, 9.375, 0, 40, "素", "NoDLC", "西瓜喂食被囚禁的鸟换取(有几率额外获得一个)。")
    End Sub

    Private Sub button_F_sweet_potato_seeds_click(sender As Object, e As RoutedEventArgs) Handles button_F_sweet_potato_seeds.Click
        F_Show_Uningredients("甘薯种子", "Sweet Potato Seeds", "F_sweet_potato_seeds", 0.5, 9.375, 0, 40, "素", "SW", "甘薯喂食被囚禁的鸟换取(有几率额外获得一个)。")
    End Sub

    Private Sub button_F_dead_swordfish_click(sender As Object, e As RoutedEventArgs) Handles button_F_dead_swordfish.Click
        F_Show_Uningredients("死旗鱼", "Dead Swordfish", "F_dead_swordfish", 1, 25, 0, 6, "荤", "SW", "狩猎旗鱼获得。")
    End Sub

    Private Sub button_F_blubber_click(sender As Object, e As RoutedEventArgs) Handles button_F_blubber.Click
        F_Show_Uningredients("鲸油", "Blubber", "F_blubber", 10, 10, 0, 10, "荤", "SW", "白鲸尸体、蓝鲸尸体获得。")
    End Sub

    Private Sub button_F_brainy_matter_click(sender As Object, e As RoutedEventArgs) Handles button_F_brainy_matter.Click
        F_Show_Uningredients("聪明豆", "Brainy Matter", "F_brainy_matter", -10, 10, 50, 1, "素", "SW", "聪明芽采摘。")
    End Sub

    Private Sub button_F_dead_wobster_click(sender As Object, e As RoutedEventArgs) Handles button_F_dead_wobster.Click
        F_Show_Uningredients("死龙虾", "Dead Wobster", "F_dead_wobster", 1, 12.5, 0, 3, "荤", "SW", "龙虾过两天变成死龙虾。")
    End Sub

    Private Sub button_F_delicious_wobster_click(sender As Object, e As RoutedEventArgs) Handles button_F_delicious_wobster.Click
        F_Show_Uningredients("美味龙虾", "Delicious Wobster", "F_delicious_wobster", 1, 12.5, 0, 6, "荤", "SW", "把死龙虾放在篝火上烤制。")
    End Sub

    Private Sub button_F_dragoon_heart_click(sender As Object, e As RoutedEventArgs) Handles button_F_dragoon_heart.Click
        F_Show_Uningredients("龙人心", "Dragoon Heart", "F_dragoon_heart", 11, 25, -10, 10, "荤", "SW", "击杀龙骑士或摧毁龙人巢获得，放在地上有小范围照明效果，食用有较大范围照明效果(短时间内逐渐缩小)，并且会持续增加精神(夜晚也会增加)。")
    End Sub

    Private Sub button_F_hail_click(sender As Object, e As RoutedEventArgs) Handles button_F_hail.Click
        F_Show_Uningredients("冰雹", "Hail", "F_hail", 0, 0, 0, 1, "", "SW", "船难版风季从飓风中掉落，四个可合成一个冰块，不可食用。")
    End Sub

    Private Sub button_F_coconut_click(sender As Object, e As RoutedEventArgs) Handles button_F_coconut.Click
        F_Show_Uningredients("椰子", "Coconut", "F_coconut", 0, 0, 0, 20, "", "SW", "砍到椰子树获得。砍椰子树的过程中也有几率会掉下椰子。")
    End Sub

    Private Sub button_F_spoiled_fish_click(sender As Object, e As RoutedEventArgs) Handles button_F_spoiled_fish.Click
        F_Show_Uningredients("变质的鱼", "Spoiled Fish", "F_spoiled_fish", 0, 0, 0, 1000, "素", "SW", "船难中死狗鱼、死旗鱼等腐烂后变成变质的鱼，不可食用。")
    End Sub

    Private Sub button_F_rot_click(sender As Object, e As RoutedEventArgs) Handles button_F_rot.Click
        F_Show_Uningredients("腐烂食物", "Rot", "F_rot", -1, -10, 0, 1000, "素", "NoDLC", "一般食物腐烂后都会变成腐烂食物，吃下去大概会觉得很恶心吧。")
    End Sub

    Private Sub button_F_eye_of_the_tiger_shark_click(sender As Object, e As RoutedEventArgs) Handles button_F_eye_of_the_tiger_shark.Click
        F_Show_Uningredients("虎鲨之眼", "Eye of the Tiger Shark", "F_eye_of_the_tiger_shark", 60, 75, -15, 1000, "荤", "SW", "击杀虎鲨获得。")
    End Sub

    REM 食物DLC检测初始化
    Private Sub F_DLC_Check_initialization()
        REM 食谱
        button_F_banana_pop.Visibility = Visibility.Collapsed
        button_F_bisque.Visibility = Visibility.Collapsed
        button_F_california_roll.Visibility = Visibility.Collapsed
        button_F_ceviche.Visibility = Visibility.Collapsed
        button_F_coffee.Visibility = Visibility.Collapsed
        button_F_fresh_fruit_crepes.Visibility = Visibility.Collapsed
        button_F_jelly_O_pop.Visibility = Visibility.Collapsed
        button_F_lobster_bisque.Visibility = Visibility.Collapsed
        button_F_lobster_dinner.Visibility = Visibility.Collapsed
        button_F_monster_tartare.Visibility = Visibility.Collapsed
        button_F_mussel_bouillabaise.Visibility = Visibility.Collapsed
        button_F_seafood_gumbo.Visibility = Visibility.Collapsed
        button_F_shark_fin_soup.Visibility = Visibility.Collapsed
        button_F_surf_n_turf.Visibility = Visibility.Collapsed
        button_F_sweet_potato_souffle.Visibility = Visibility.Collapsed
        REM 肉类
        button_F_moleworm.Visibility = Visibility.Collapsed
        button_F_tropical_fish.Visibility = Visibility.Collapsed
        button_F_fish_morsel.Visibility = Visibility.Collapsed
        button_F_cooked_fish_morsel.Visibility = Visibility.Collapsed
        button_F_jellyfish.Visibility = Visibility.Collapsed
        button_F_dead_jellyfish.Visibility = Visibility.Collapsed
        button_F_cooked_jellyfish.Visibility = Visibility.Collapsed
        button_F_dried_jellyfish.Visibility = Visibility.Collapsed
        button_F_limpets.Visibility = Visibility.Collapsed
        button_F_cooked_limpets.Visibility = Visibility.Collapsed
        button_F_mussel.Visibility = Visibility.Collapsed
        button_F_cooked_mussel.Visibility = Visibility.Collapsed
        button_F_dead_dogfish.Visibility = Visibility.Collapsed
        button_F_dead_wobster.Visibility = Visibility.Collapsed
        button_F_delicious_wobster.Visibility = Visibility.Collapsed
        button_F_dragoon_heart.Visibility = Visibility.Collapsed
        button_F_wobster.Visibility = Visibility.Collapsed
        button_F_raw_fish.Visibility = Visibility.Collapsed
        button_F_fish_steak.Visibility = Visibility.Collapsed
        button_F_shark_fin.Visibility = Visibility.Collapsed
        REM 蔬菜
        button_F_cactus_flesh.Visibility = Visibility.Collapsed
        button_F_cooked_cactus_flesh.Visibility = Visibility.Collapsed
        button_F_cactus_flower.Visibility = Visibility.Collapsed
        button_F_sweet_potato.Visibility = Visibility.Collapsed
        button_F_cooked_sweet_potato.Visibility = Visibility.Collapsed
        button_F_seaweed.Visibility = Visibility.Collapsed
        button_F_roasted_seaweed.Visibility = Visibility.Collapsed
        button_F_dried_seaweed.Visibility = Visibility.Collapsed
        REM 水果
        button_F_juicy_berries.Visibility = Visibility.Collapsed
        button_F_roasted_juicy_berries.Visibility = Visibility.Collapsed
        button_F_roasted_coconut.Visibility = Visibility.Collapsed
        button_F_halved_coconut.Visibility = Visibility.Collapsed
        REM 蛋类
        button_F_doydoy_egg.Visibility = Visibility.Collapsed
        button_F_fried_doydoy_egg.Visibility = Visibility.Collapsed
        REM 其他
        button_F_butterfly_wing.Visibility = Visibility.Collapsed
        button_F_lichen.Visibility = Visibility.Collapsed
        button_F_electric_milk.Visibility = Visibility.Collapsed
        button_F_ice.Visibility = Visibility.Collapsed
        button_F_roasted_birchnut.Visibility = Visibility.Collapsed
        button_F_butterfly_wing_sw.Visibility = Visibility.Collapsed
        button_F_coffee_beans.Visibility = Visibility.Collapsed
        button_F_roasted_coffee_beans.Visibility = Visibility.Collapsed
        REM 非食材
        button_F_phlegm.Visibility = Visibility.Collapsed
        button_F_watermelon_seeds.Visibility = Visibility.Collapsed
        button_F_sweet_potato_seeds.Visibility = Visibility.Collapsed
        button_F_dead_swordfish.Visibility = Visibility.Collapsed
        button_F_blubber.Visibility = Visibility.Collapsed
        button_F_brainy_matter.Visibility = Visibility.Collapsed
        button_F_hail.Visibility = Visibility.Collapsed
        button_F_coconut.Visibility = Visibility.Collapsed
        button_F_spoiled_fish.Visibility = Visibility.Collapsed
        button_F_eye_of_the_tiger_shark.Visibility = Visibility.Collapsed
    End Sub

    Private Sub F_DLC_ROG_SHOW()
        REM 食谱
        REM 肉类
        button_F_moleworm.Visibility = Visibility.Visible
        REM 蔬菜
        button_F_cactus_flesh.Visibility = Visibility.Visible
        button_F_cooked_cactus_flesh.Visibility = Visibility.Visible
        button_F_cactus_flower.Visibility = Visibility.Visible
        REM 水果
        REM 其他
        button_F_butterfly_wing.Visibility = Visibility.Visible
        button_F_lichen.Visibility = Visibility.Visible
        button_F_electric_milk.Visibility = Visibility.Visible
        button_F_ice.Visibility = Visibility.Visible
        button_F_roasted_birchnut.Visibility = Visibility.Visible
        REM 非食材
        button_F_watermelon_seeds.Visibility = Visibility.Visible
    End Sub

    Private Sub F_DLC_SW_SHOW()
        REM 食谱
        button_F_banana_pop.Visibility = Visibility.Visible
        button_F_bisque.Visibility = Visibility.Visible
        button_F_california_roll.Visibility = Visibility.Visible
        button_F_ceviche.Visibility = Visibility.Visible
        button_F_coffee.Visibility = Visibility.Visible
        button_F_fresh_fruit_crepes.Visibility = Visibility.Visible
        button_F_jelly_O_pop.Visibility = Visibility.Visible
        button_F_lobster_bisque.Visibility = Visibility.Visible
        button_F_lobster_dinner.Visibility = Visibility.Visible
        button_F_monster_tartare.Visibility = Visibility.Visible
        button_F_mussel_bouillabaise.Visibility = Visibility.Visible
        button_F_seafood_gumbo.Visibility = Visibility.Visible
        button_F_shark_fin_soup.Visibility = Visibility.Visible
        button_F_surf_n_turf.Visibility = Visibility.Visible
        button_F_sweet_potato_souffle.Visibility = Visibility.Visible
        REM 肉类
        button_F_tropical_fish.Visibility = Visibility.Visible
        button_F_fish_morsel.Visibility = Visibility.Visible
        button_F_cooked_fish_morsel.Visibility = Visibility.Visible
        button_F_jellyfish.Visibility = Visibility.Visible
        button_F_dead_jellyfish.Visibility = Visibility.Visible
        button_F_cooked_jellyfish.Visibility = Visibility.Visible
        button_F_dried_jellyfish.Visibility = Visibility.Visible
        button_F_limpets.Visibility = Visibility.Visible
        button_F_cooked_limpets.Visibility = Visibility.Visible
        button_F_mussel.Visibility = Visibility.Visible
        button_F_cooked_mussel.Visibility = Visibility.Visible
        button_F_dead_dogfish.Visibility = Visibility.Visible
        button_F_dead_wobster.Visibility = Visibility.Visible
        button_F_delicious_wobster.Visibility = Visibility.Visible
        button_F_dragoon_heart.Visibility = Visibility.Visible
        button_F_wobster.Visibility = Visibility.Visible
        button_F_raw_fish.Visibility = Visibility.Visible
        button_F_fish_steak.Visibility = Visibility.Visible
        button_F_shark_fin.Visibility = Visibility.Visible
        REM 蔬菜
        button_F_sweet_potato.Visibility = Visibility.Visible
        button_F_cooked_sweet_potato.Visibility = Visibility.Visible
        button_F_seaweed.Visibility = Visibility.Visible
        button_F_roasted_seaweed.Visibility = Visibility.Visible
        button_F_dried_seaweed.Visibility = Visibility.Visible
        REM 水果
        button_F_roasted_coconut.Visibility = Visibility.Visible
        button_F_halved_coconut.Visibility = Visibility.Visible
        REM 蛋类
        button_F_doydoy_egg.Visibility = Visibility.Visible
        button_F_fried_doydoy_egg.Visibility = Visibility.Visible
        REM 其他
        button_F_butterfly_wing_sw.Visibility = Visibility.Visible
        button_F_coffee_beans.Visibility = Visibility.Visible
        button_F_roasted_coffee_beans.Visibility = Visibility.Visible
        REM 非食材
        button_F_sweet_potato_seeds.Visibility = Visibility.Visible
        button_F_dead_swordfish.Visibility = Visibility.Visible
        button_F_blubber.Visibility = Visibility.Visible
        button_F_brainy_matter.Visibility = Visibility.Visible
        button_F_hail.Visibility = Visibility.Visible
        button_F_coconut.Visibility = Visibility.Visible
        button_F_spoiled_fish.Visibility = Visibility.Visible
        button_F_eye_of_the_tiger_shark.Visibility = Visibility.Visible
    End Sub

    Private Sub F_DLC_DST_SHOW()
        REM 食谱
        REM 肉类
        button_F_moleworm.Visibility = Visibility.Visible
        REM 蔬菜
        button_F_cactus_flesh.Visibility = Visibility.Visible
        button_F_cooked_cactus_flesh.Visibility = Visibility.Visible
        button_F_cactus_flower.Visibility = Visibility.Visible
        REM 水果
        button_F_juicy_berries.Visibility = Visibility.Visible
        button_F_roasted_juicy_berries.Visibility = Visibility.Visible
        REM 其他
        button_F_butterfly_wing.Visibility = Visibility.Visible
        button_F_lichen.Visibility = Visibility.Visible
        button_F_electric_milk.Visibility = Visibility.Visible
        button_F_ice.Visibility = Visibility.Visible
        button_F_roasted_birchnut.Visibility = Visibility.Visible
        REM 非食材
        button_F_phlegm.Visibility = Visibility.Visible
        button_F_watermelon_seeds.Visibility = Visibility.Visible
    End Sub

    REM 食物DLC检测
    Private Sub F_DLC_Check()
        F_DLC_Check_initialization()
        Dim F_ROG_SW_DST As SByte
        Dim F_ROG__ As SByte
        Dim F_SW__ As SByte
        Dim F_DST__ As SByte
        If checkBox_F_DLC_ROG.IsChecked = True Then
            F_ROG__ = 1
        Else
            F_ROG__ = 0
        End If
        If checkBox_F_DLC_SW.IsChecked = True Then
            F_SW__ = 2
        Else
            F_SW__ = 0
        End If
        If checkBox_F_DLC_DST.IsChecked = True Then
            F_DST__ = 4
        Else
            F_DST__ = 0
        End If
        F_ROG_SW_DST = F_ROG__ + F_SW__ + F_DST__
        If F_ROG_SW_DST = 0 Then
            MsgBox("至少选择一项！")
            checkBox_F_DLC_ROG.IsChecked = True
            F_DLC_Check()
        Else
            Select Case F_ROG_SW_DST
                Case 1
                    F_DLC_ROG_SHOW()
                    WrapPanel_F_recipe.Height = 250
                    WrapPanel_F_meats.Height = 170
                    WrapPanel_F_others.Height = 90
                    WrapPanel_F_no_fc.Height = 330
                    WrapPanel_Food.Height = 1500
                    Reg_Write("Food", 1)
                Case 2
                    F_DLC_SW_SHOW()
                    WrapPanel_F_recipe.Height = 410
                    WrapPanel_F_meats.Height = 250
                    WrapPanel_F_others.Height = 90
                    WrapPanel_F_no_fc.Height = 330
                    WrapPanel_Food.Height = 1820
                    Reg_Write("Food", 2)
                Case 3
                    F_DLC_ROG_SHOW()
                    F_DLC_SW_SHOW()
                    WrapPanel_F_recipe.Height = 410
                    WrapPanel_F_meats.Height = 330
                    WrapPanel_F_others.Height = 170
                    WrapPanel_F_no_fc.Height = 410
                    WrapPanel_Food.Height = 1980
                    Reg_Write("Food", 3)
                Case 4
                    F_DLC_DST_SHOW()
                    WrapPanel_F_recipe.Height = 250
                    WrapPanel_F_meats.Height = 170
                    WrapPanel_F_others.Height = 90
                    WrapPanel_F_no_fc.Height = 330
                    WrapPanel_Food.Height = 1500
                    Reg_Write("Food", 4)
                Case 5
                    F_DLC_ROG_SHOW()
                    F_DLC_DST_SHOW()
                    WrapPanel_F_recipe.Height = 250
                    WrapPanel_F_meats.Height = 170
                    WrapPanel_F_others.Height = 90
                    WrapPanel_F_no_fc.Height = 330
                    WrapPanel_Food.Height = 1500
                    Reg_Write("Food", 5)
                Case 6
                    F_DLC_SW_SHOW()
                    F_DLC_DST_SHOW()
                    WrapPanel_F_recipe.Height = 410
                    WrapPanel_F_meats.Height = 330
                    WrapPanel_F_others.Height = 170
                    WrapPanel_F_no_fc.Height = 410
                    WrapPanel_Food.Height = 1980
                    Reg_Write("Food", 6)
                Case 7
                    F_DLC_ROG_SHOW()
                    F_DLC_SW_SHOW()
                    F_DLC_DST_SHOW()
                    WrapPanel_F_recipe.Height = 410
                    WrapPanel_F_meats.Height = 330
                    WrapPanel_F_others.Height = 170
                    WrapPanel_F_no_fc.Height = 410
                    WrapPanel_Food.Height = 1980
                    Reg_Write("Food", 7)
            End Select
        End If
    End Sub

    Private Sub checkBox_F_DLC_ROG_click(sender As Object, e As RoutedEventArgs) Handles checkBox_F_DLC_ROG.Click
        F_DLC_Check()
    End Sub

    Private Sub FL_button_F_DLC_ROG_click(sender As Object, e As RoutedEventArgs) Handles FL_button_F_DLC_ROG.Click
        If checkBox_F_DLC_ROG.IsChecked = True Then
            checkBox_F_DLC_ROG.IsChecked = False
        Else
            checkBox_F_DLC_ROG.IsChecked = True
        End If
        checkBox_F_DLC_ROG_click(Nothing, Nothing)
    End Sub

    Private Sub checkBox_F_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles checkBox_F_DLC_SW.Click
        F_DLC_Check()
    End Sub

    Private Sub FL_button_F_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles FL_button_F_DLC_SW.Click
        If checkBox_F_DLC_SW.IsChecked = True Then
            checkBox_F_DLC_SW.IsChecked = False
        Else
            checkBox_F_DLC_SW.IsChecked = True
        End If
        checkBox_F_DLC_SW_click(Nothing, Nothing)
    End Sub

    Private Sub checkBox_F_DLC_DST_click(sender As Object, e As RoutedEventArgs) Handles checkBox_F_DLC_DST.Click
        F_DLC_Check()
    End Sub

    Private Sub FL_button_F_DLC_DST_click(sender As Object, e As RoutedEventArgs) Handles FL_button_F_DLC_DST.Click
        If checkBox_F_DLC_DST.IsChecked = True Then
            checkBox_F_DLC_DST.IsChecked = False
        Else
            checkBox_F_DLC_DST.IsChecked = True
        End If
        checkBox_F_DLC_DST_click(Nothing, Nothing)
    End Sub

    REM ------------------食物类别------------------
    '关闭按钮
    Private Sub FL_btn_close_M_click(sender As Object, e As RoutedEventArgs) Handles FL_btn_close_M.Click
        Canvas_FoodLeft_Meats.Visibility = Visibility.Collapsed
    End Sub

    Private Sub FL_btn_close_MM_click(sender As Object, e As RoutedEventArgs) Handles FL_btn_close_MM.Click
        Canvas_FoodLeft_Monster_Meats.Visibility = Visibility.Collapsed
    End Sub

    Private Sub FL_btn_close_F_click(sender As Object, e As RoutedEventArgs) Handles FL_btn_close_F.Click
        Canvas_FoodLeft_Fishes.Visibility = Visibility.Collapsed
    End Sub

    Private Sub FL_btn_close_V_click(sender As Object, e As RoutedEventArgs) Handles FL_btn_close_V.Click
        Canvas_FoodLeft_Vegetables.Visibility = Visibility.Collapsed
    End Sub

    Private Sub FL_btn_close_Fr_click(sender As Object, e As RoutedEventArgs) Handles FL_btn_close_Fr.Click
        Canvas_FoodLeft_Fruit.Visibility = Visibility.Collapsed
    End Sub

    Private Sub FL_btn_close_O_click(sender As Object, e As RoutedEventArgs) Handles FL_btn_close_O.Click
        Canvas_FoodLeft_Others.Visibility = Visibility.Collapsed
    End Sub

    '左侧面板出可选食材面板
    Private Sub FL_CLOSE()
        Canvas_FoodLeft_Meats.Visibility = Visibility.Collapsed
        Canvas_FoodLeft_Monster_Meats.Visibility = Visibility.Collapsed
        Canvas_FoodLeft_Fishes.Visibility = Visibility.Collapsed
        Canvas_FoodLeft_Vegetables.Visibility = Visibility.Collapsed
        Canvas_FoodLeft_Fruit.Visibility = Visibility.Collapsed
        Canvas_FoodLeft_Others.Visibility = Visibility.Collapsed
    End Sub

    Private Sub FC_Select(VariableName As String, FirstChange As String)
        FL_CLOSE()
        ButtonJump(VariableName, FirstChange)
    End Sub

    Private Sub button_F_FoodNeed_1_click(sender As Object, e As RoutedEventArgs) Handles button_F_FoodNeed_1.Click
        FC_Select(F_FoodNeed_1, "F")
    End Sub

    Private Sub button_F_FoodNeed_or_click(sender As Object, e As RoutedEventArgs) Handles button_F_FoodNeed_or.Click
        FC_Select(F_FoodNeed_or, "F")
    End Sub

    Private Sub button_F_FoodNeed_2_click(sender As Object, e As RoutedEventArgs) Handles button_F_FoodNeed_2.Click
        FC_Select(F_FoodNeed_2, "F")
    End Sub

    Private Sub button_F_FoodNeed_3_click(sender As Object, e As RoutedEventArgs) Handles button_F_FoodNeed_3.Click
        FC_Select(F_FoodNeed_3, "F")
    End Sub

    Private Sub button_F_Restrictions_1_click(sender As Object, e As RoutedEventArgs) Handles button_F_Restrictions_1.Click
        FC_Select(F_Restrictions_1, "F")
    End Sub

    Private Sub button_F_Restrictions_2_click(sender As Object, e As RoutedEventArgs) Handles button_F_Restrictions_2.Click
        FC_Select(F_Restrictions_2, "F")
    End Sub

    Private Sub button_F_Restrictions_3_click(sender As Object, e As RoutedEventArgs) Handles button_F_Restrictions_3.Click
        FC_Select(F_Restrictions_3, "F")
    End Sub

    Private Sub button_F_Restrictions_4_click(sender As Object, e As RoutedEventArgs) Handles button_F_Restrictions_4.Click
        FC_Select(F_Restrictions_4, "F")
    End Sub

    Private Sub button_F_Restrictions_5_click(sender As Object, e As RoutedEventArgs) Handles button_F_Restrictions_5.Click
        FC_Select(F_Restrictions_5, "F")
    End Sub

    Private Sub button_F_Restrictions_6_click(sender As Object, e As RoutedEventArgs) Handles button_F_Restrictions_6.Click
        FC_Select(F_Restrictions_6, "F")
    End Sub

    Private Sub button_F_Restrictions_7_click(sender As Object, e As RoutedEventArgs) Handles button_F_Restrictions_7.Click
        FC_Select(F_Restrictions_7, "F")
    End Sub

    Private Sub button_F_Restrictions_Compare_click(sender As Object, e As RoutedEventArgs) Handles button_F_Restrictions_Compare.Click
        FC_Select(F_Restrictions_Compare, "F")
    End Sub

    Private Sub button_F_FoodAttribute_1_click(sender As Object, e As RoutedEventArgs) Handles button_F_FoodAttribute_1.Click
        FC_Select(F_Ingredients_1, "F")
    End Sub

    Private Sub button_F_FoodAttribute_2_click(sender As Object, e As RoutedEventArgs) Handles button_F_FoodAttribute_2.Click
        FC_Select(F_Ingredients_2, "F")
    End Sub

    '可选食材面板跳转
    Private Sub button_F_Meats_1_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_1.Click
        button_F_meat_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_2_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_2.Click
        button_F_cooked_meat_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_3_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_3.Click
        button_F_jerky_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_4_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_4.Click
        button_F_monster_meat_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_5_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_5.Click
        button_F_cooked_monster_meat_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_6_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_6.Click
        button_F_monster_jerky_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_7_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_7.Click
        button_F_morsel_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_8_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_8.Click
        button_F_cooked_morsel_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_9_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_9.Click
        button_F_small_jerky_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_10_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_10.Click
        button_F_drumstick_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_11_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_11.Click
        button_F_fried_drumstick_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_12_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_12.Click
        button_F_frog_legs_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_13_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_13.Click
        button_F_cooked_frog_legs_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_14_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_14.Click
        button_F_fish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_15_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_15.Click
        button_F_cooked_fish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_16_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_16.Click
        button_F_eel_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_17_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_17.Click
        button_F_cooked_eel_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_18_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_18.Click
        button_F_moleworm_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_19_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_19.Click
        button_F_tropical_fish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_20_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_20.Click
        button_F_dead_dogfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_21_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_21.Click
        button_F_raw_fish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_22_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_22.Click
        button_F_fish_steak_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Meats_23_click(sender As Object, e As RoutedEventArgs) Handles button_F_Meats_23.Click
        button_F_shark_fin_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Monster_Meats_1_click(sender As Object, e As RoutedEventArgs) Handles button_F_Monster_Meats_1.Click
        button_F_monster_meat_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Monster_Meats_2_click(sender As Object, e As RoutedEventArgs) Handles button_F_Monster_Meats_2.Click
        button_F_cooked_monster_meat_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Monster_Meats_3_click(sender As Object, e As RoutedEventArgs) Handles button_F_Monster_Meats_3.Click
        button_F_monster_jerky_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Monster_Meats_4_click(sender As Object, e As RoutedEventArgs) Handles button_F_Monster_Meats_4.Click
        button_F_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Monster_Meats_5_click(sender As Object, e As RoutedEventArgs) Handles button_F_Monster_Meats_5.Click
        button_F_dead_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Monster_Meats_6_click(sender As Object, e As RoutedEventArgs) Handles button_F_Monster_Meats_6.Click
        button_F_cooked_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Monster_Meats_7_click(sender As Object, e As RoutedEventArgs) Handles button_F_Monster_Meats_7.Click
        button_F_dried_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Monster_Meats_8_click(sender As Object, e As RoutedEventArgs) Handles button_F_Monster_Meats_8.Click
        button_F_durian_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Monster_Meats_9_click(sender As Object, e As RoutedEventArgs) Handles button_F_Monster_Meats_9.Click
        button_F_extra_smelly_durian_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_1_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_1.Click
        button_F_fish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_2_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_2.Click
        button_F_cooked_fish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_3_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_3.Click
        button_F_eel_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_4_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_4.Click
        button_F_cooked_eel_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_5_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_5.Click
        button_F_limpets_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_6_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_6.Click
        button_F_cooked_limpets_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_7_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_7.Click
        button_F_tropical_fish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_8_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_8.Click
        button_F_fish_morsel_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_9_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_9.Click
        button_F_cooked_fish_morsel_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_10_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_10.Click
        button_F_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_11_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_11.Click
        button_F_dead_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_12_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_12.Click
        button_F_cooked_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_13_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_13.Click
        button_F_dried_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_14_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_14.Click
        button_F_mussel_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_15_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_15.Click
        button_F_cooked_mussel_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_16_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_16.Click
        button_F_dead_dogfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_17_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_17.Click
        button_F_wobster_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_18_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_18.Click
        button_F_raw_fish_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_19_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_19.Click
        button_F_fish_steak_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Fishes_20_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fishes_20.Click
        button_F_shark_fin_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_1_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_1.Click
        button_F_blue_cap_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_2_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_2.Click
        button_F_cooked_blue_cap_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_3_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_3.Click
        button_F_green_cap_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_4_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_4.Click
        button_F_cooked_green_cap_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_5_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_5.Click
        button_F_red_cap_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_6_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_6.Click
        button_F_cooked_red_cap_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_7_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_7.Click
        button_F_eggplant_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_8_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_8.Click
        button_F_braised_eggplant_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_9_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_9.Click
        button_F_carrot_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_10_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_10.Click
        button_F_roasted_carrot_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_11_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_11.Click
        button_F_corn_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_12_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_12.Click
        button_F_popcorn_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_13_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_13.Click
        button_F_pumpkin_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_14_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_14.Click
        button_F_hot_pumpkin_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_15_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_15.Click
        button_F_cactus_flesh_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_16_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_16.Click
        button_F_cooked_cactus_flesh_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_17_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_17.Click
        button_F_cactus_flower_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_18_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_18.Click
        button_F_sweet_potato_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_19_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_19.Click
        button_F_cooked_sweet_potato_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_20_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_20.Click
        button_F_seaweed_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_21_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_21.Click
        button_F_roasted_seaweed_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_22_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_22.Click
        button_F_dried_seaweed_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_23_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_23.Click
        button_F_lichen_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_Vegetables_24_click(sender As Object, e As RoutedEventArgs) Handles button_F_Vegetables_24.Click
        button_F_mandrake_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_1_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_1.Click
        button_F_juicy_berries_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_2_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_2.Click
        button_F_roasted_juicy_berries_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_3_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_3.Click
        button_F_berries_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_4_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_4.Click
        button_F_roasted_berrie_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_5_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_5.Click
        button_F_banana_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_6_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_6.Click
        button_F_cooked_banana_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_7_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_7.Click
        button_F_dragon_fruit_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_8_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_8.Click
        button_F_prepared_dragon_fruit_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_9_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_9.Click
        button_F_durian_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_10_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_10.Click
        button_F_extra_smelly_durian_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_11_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_11.Click
        button_F_pomegranate_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_12_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_12.Click
        button_F_sliced_pomegranate_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_13_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_13.Click
        button_F_watermelon_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_14_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_14.Click
        button_F_grilled_watermelon_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_15_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_15.Click
        button_F_halved_coconut_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_16_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_16.Click
        button_F_roasted_coconut_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_17_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_17.Click
        button_F_coffee_beans_click(Nothing, Nothing)
    End Sub

    Private Sub button_F_button_F_Fruit_18_click(sender As Object, e As RoutedEventArgs) Handles button_F_Fruit_18.Click
        button_F_roasted_coffee_beans_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_1_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_1.Click
        button_F_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_2_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_2.Click
        button_F_dead_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_3_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_3.Click
        button_F_cooked_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_4_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_4.Click
        button_F_dried_jellyfish_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_5_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_5.Click
        button_F_egg_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_6_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_6.Click
        button_F_cooked_egg_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_7_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_7.Click
        button_F_tallbird_egg_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_8_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_8.Click
        button_F_fried_tallbird_egg_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_9_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_9.Click
        button_F_doydoy_egg_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_10_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_10.Click
        button_F_fried_doydoy_egg_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_11_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_11.Click
        button_F_butter_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_12_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_12.Click
        button_F_electric_milk_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_13_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_13.Click
        button_F_honey_click(Nothing, Nothing)
    End Sub

    Private Sub button_button_F_Others_14_click(sender As Object, e As RoutedEventArgs) Handles button_F_Others_14.Click
        button_F_honeycomb_click(Nothing, Nothing)
    End Sub

    REM ------------------左侧面板(科技)------------------
    Private Sub S_Show(S_Name As String, S_EnName As String, S_picture As String, S_DLC As String, S_DLC_ROG As SByte, S_DLC_SW As SByte, S_DLC_DST As SByte, S_image_Need_1 As String, S_textblock_Need_1 As String, S_image_Need_2 As String, S_textblock_Need_2 As String, S_image_Need_3 As String, S_textblock_Need_3 As String, S_Deblocking_Science As SByte, S_Deblocking_Character As String, S_Introduce As String)
        REM ------------------初始化------------------
        button_S_Science_2.Visibility = Visibility.Collapsed
        SL_TextBlock_ScienceNeed_2.Visibility = Visibility.Collapsed
        button_S_Science_3.Visibility = Visibility.Collapsed
        SL_TextBlock_ScienceNeed_3.Visibility = Visibility.Collapsed
        SL_TextBlock_ScienceDeblocking_Science.Visibility = Visibility.Collapsed
        SL_TextBlock_ScienceDeblocking_Character.Visibility = Visibility.Collapsed
        Canvas_ScienceLeft_DryingRack.Visibility = Visibility.Collapsed
        If S_Name = "晒肉架" Then
            button_S_DryingRack_table.Visibility = Visibility.Visible
        Else
            button_S_DryingRack_table.Visibility = Visibility.Collapsed
        End If
        REM ------------------科技名字------------------
        SL_textBlock_ScienceName.Text = S_Name
        SL_textBlock_ScienceName.UpdateLayout()
        Dim S_N_MarginLeft As Integer
        S_N_MarginLeft = (Canvas_ScienceLeft.ActualWidth - SL_textBlock_ScienceName.ActualWidth) / 2
        Dim S_N_T As New Thickness()
        S_N_T.Top = 80
        S_N_T.Left = S_N_MarginLeft
        SL_textBlock_ScienceName.Margin = S_N_T

        SL_textBlock_ScienceEnName.Text = S_EnName
        SL_textBlock_ScienceEnName.UpdateLayout()
        Dim S_EnN_MarginLeft As Integer
        S_EnN_MarginLeft = (Canvas_ScienceLeft.ActualWidth - SL_textBlock_ScienceEnName.ActualWidth) / 2
        Dim S_EnN_T As New Thickness()
        S_EnN_T.Top = 100
        S_EnN_T.Left = S_EnN_MarginLeft
        SL_textBlock_ScienceEnName.Margin = S_EnN_T
        REM ------------------科技图片------------------
        SL_image_SciencePicture.Source = Picture_Short_Name(Res_Short_Name(S_picture))
        REM ------------------科技需求-------------------
        S_ScienceNeed_1 = S_image_Need_1
        SL_image_ScienceNeed_1.Source = Picture_Short_Name(Res_Short_Name(S_image_Need_1))
        SL_TextBlock_ScienceNeed_1.Text = S_textblock_Need_1
        If S_textblock_Need_2 <> "" Then
            S_ScienceNeed_2 = S_image_Need_2
            SL_image_ScienceNeed_2.Source = Picture_Short_Name(Res_Short_Name(S_image_Need_2))
            SL_TextBlock_ScienceNeed_2.Text = S_textblock_Need_2
            button_S_Science_2.Visibility = Visibility.Visible
            SL_TextBlock_ScienceNeed_2.Visibility = Visibility.Visible
        Else
            S_ScienceNeed_2 = ""
        End If
        If S_textblock_Need_3 <> "" Then
            S_ScienceNeed_3 = S_image_Need_3
            SL_image_ScienceNeed_3.Source = Picture_Short_Name(Res_Short_Name(S_image_Need_3))
            SL_TextBlock_ScienceNeed_3.Text = S_textblock_Need_3
            button_S_Science_3.Visibility = Visibility.Visible
            SL_TextBlock_ScienceNeed_3.Visibility = Visibility.Visible
        Else
            S_ScienceNeed_3 = ""
        End If
        REM ------------------科技DLC-------------------
        If S_DLC = "ROG" Then
            SL_image_S_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf S_DLC = "SW" Then
            SL_image_S_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf S_DLC = "DST" Then
            SL_image_S_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            SL_image_S_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------存在版本-------------------
        SL_textBlock_S_DLC_1.Foreground = Brushes.Black
        SL_textBlock_S_DLC_2.Foreground = Brushes.Black
        SL_textBlock_S_DLC_3.Foreground = Brushes.Black
        If S_DLC_ROG = 0 Then
            SL_textBlock_S_DLC_1.Foreground = Brushes.Silver
        End If
        If S_DLC_SW = 0 Then
            SL_textBlock_S_DLC_2.Foreground = Brushes.Silver
        End If
        If S_DLC_DST = 0 Then
            SL_textBlock_S_DLC_3.Foreground = Brushes.Silver
        End If
        REM ------------------科技简介-------------------
        TextBlock_S_Introduce.Text = S_Introduce
        REM ------------------科技解锁-------------------
        SL_TextBlock_ScienceDeblocking_Science.Visibility = Visibility.Visible
        Select Case S_Deblocking_Science
            Case 0
                SL_TextBlock_ScienceDeblocking_Science.Visibility = Visibility.Collapsed
            Case 1
                SL_TextBlock_ScienceDeblocking_Science.Text = "科学机器"
                SL_TextBlock_ScienceDeblocking_Science.Foreground = Brushes.Orange
            Case 2
                SL_TextBlock_ScienceDeblocking_Science.Text = "炼金术引擎"
                SL_TextBlock_ScienceDeblocking_Science.Foreground = Brushes.Crimson
            Case 3
                SL_TextBlock_ScienceDeblocking_Science.Text = "魔法帽子"
                SL_TextBlock_ScienceDeblocking_Science.Foreground = Brushes.PaleGreen
            Case 4
                SL_TextBlock_ScienceDeblocking_Science.Text = "暗影操纵仪"
                SL_TextBlock_ScienceDeblocking_Science.Foreground = Brushes.DarkOrchid
            Case 5
                SL_TextBlock_ScienceDeblocking_Science.Text = "破碎的远古遗迹"
                SL_TextBlock_ScienceDeblocking_Science.Foreground = Brushes.SkyBlue
            Case 6
                SL_TextBlock_ScienceDeblocking_Science.Text = "远古遗迹"
                SL_TextBlock_ScienceDeblocking_Science.Foreground = Brushes.Cyan
            Case 7
                SL_TextBlock_ScienceDeblocking_Science.Text = "暗影秘典"
                SL_TextBlock_ScienceDeblocking_Science.Foreground = Brushes.DarkSlateBlue
            Case 8
                SL_TextBlock_ScienceDeblocking_Science.Text = "灵子分解器"
                SL_TextBlock_ScienceDeblocking_Science.Foreground = Brushes.PaleGreen
            Case 9
                SL_TextBlock_ScienceDeblocking_Science.Text = "黑曜石工作台"
                SL_TextBlock_ScienceDeblocking_Science.Foreground = Brushes.SaddleBrown
        End Select
        SL_TextBlock_ScienceDeblocking_Science.Text = SL_TextBlock_ScienceDeblocking_Science.Text & "解锁"
        SL_TextBlock_ScienceDeblocking_Science.UpdateLayout()
        SL_TextBlock_ScienceDeblocking_Science.SetValue(Canvas.LeftProperty, CDbl((Canvas_ScienceLeft.ActualWidth - SL_TextBlock_ScienceDeblocking_Science.ActualWidth) / 2))
        REM ------------------科技人物解锁-------------------
        If S_Deblocking_Character <> "" Then
            SL_TextBlock_ScienceDeblocking_Character.Visibility = Visibility.Visible
            SL_TextBlock_ScienceDeblocking_Character.Text = S_Deblocking_Character & "解锁"
            SL_TextBlock_ScienceDeblocking_Character.UpdateLayout()
            SL_TextBlock_ScienceDeblocking_Character.SetValue(Canvas.LeftProperty, CDbl((Canvas_ScienceLeft.ActualWidth - SL_TextBlock_ScienceDeblocking_Character.ActualWidth) / 2))
        End If
    End Sub

    Private Sub S_ScienceNeed_Select(VariableName As String)
        Select Case VariableName
            Case "S_gold_nugget"
                ButtonJump(VariableName, "G")
            Case "G_parrot"
                ButtonJump(VariableName, "A")
            Case "F_moleworm"
                ButtonJump(VariableName, "A")
            Case "G_bioluminescence"
                ButtonJump(VariableName, "A")
            Case "G_health"
            Case "G_sanity"
            Case Else
                ButtonJump(VariableName)
        End Select
    End Sub

    Private Sub button_S_Science_1_click(sender As Object, e As RoutedEventArgs) Handles button_S_Science_1.Click
        S_ScienceNeed_Select(S_ScienceNeed_1)
    End Sub

    Private Sub button_S_Science_2_click(sender As Object, e As RoutedEventArgs) Handles button_S_Science_2.Click
        S_ScienceNeed_Select(S_ScienceNeed_2)
    End Sub

    Private Sub button_S_Science_3_click(sender As Object, e As RoutedEventArgs) Handles button_S_Science_3.Click
        S_ScienceNeed_Select(S_ScienceNeed_3)
    End Sub


    REM ------------------科技(工具)-------------------
    Private Sub button_S_axe_click(sender As Object, e As RoutedEventArgs) Handles button_S_axe.Click
        S_Show("斧头", "Axe", "S_axe", "NoDLC", 1, 1, 1, "G_twigs", "×1", "G_flint", "×1", "", "", 0, "", "前期伐木获得木头的重要工具，木材主要用于生火，后期建设也需要大量木板。")
    End Sub

    Private Sub button_S_goldenaxe_click(sender As Object, e As RoutedEventArgs) Handles button_S_goldenaxe.Click
        S_Show("金斧头", "Luxury Axe", "S_goldenaxe", "NoDLC", 1, 1, 1, "G_twigs", "×2", "S_gold_nugget", "×2", "", "", 2, "", "耐久度是普通斧头的四倍。尽情地砍吧，少年！")
    End Sub

    Private Sub button_S_machete_click(sender As Object, e As RoutedEventArgs) Handles button_S_machete.Click
        S_Show("砍刀", "Machete", "S_machete", "SW", 0, 1, 0, "G_twigs", "×1", "G_flint", "×3", "", "", 0, "", "用来砍竹子和藤蔓丛的必要工具，还可以切开鲸鱼的肚子，也可以用作武器。")
    End Sub

    Private Sub button_S_luxury_machete_click(sender As Object, e As RoutedEventArgs) Handles button_S_Luxury_machete.Click
        S_Show("黄金砍刀", "Luxury Machete", "S_luxury_machete", "SW", 0, 1, 0, "G_twigs", "×4", "S_gold_nugget", "×2", "", "", 2, "", "耐久度是普通砍刀的四倍。砍砍砍！")
    End Sub

    Private Sub button_S_pickaxe_click(sender As Object, e As RoutedEventArgs) Handles button_S_pickaxe.Click
        S_Show("鹤嘴锄", "Pickaxe", "S_pickaxe", "NoDLC", 1, 1, 1, "G_twigs", "×2", "G_flint", "×2", "", "", 0, "", "中后期发展离不开矿石，单靠拾取的矿石是不够的，拿起鹤嘴锄去开采吧。")
    End Sub

    Private Sub button_S_goldenpickaxe_click(sender As Object, e As RoutedEventArgs) Handles button_S_goldenpickaxe.Click
        S_Show("黄金鹤嘴锄", "Opulent Pickaxe", "S_goldenpickaxe", "NoDLC", 1, 1, 1, "G_twigs", "×4", "S_gold_nugget", "×2", "", "", 2, "", "耐久度是普通鹤嘴锄的四倍。用它去征服矿区！")
    End Sub

    Private Sub button_S_shovel_click(sender As Object, e As RoutedEventArgs) Handles button_S_shovel.Click
        S_Show("铁铲", "Shovel", "S_shovel", "NoDLC", 1, 1, 1, "G_twigs", "×2", "G_flint", "×2", "", "", 1, "", "前期移植作物的工具，要坚持走可持续发展道路，两下三下奔小康，铲子也能把树桩铲掉。")
    End Sub

    Private Sub button_S_goldenshovel_click(sender As Object, e As RoutedEventArgs) Handles button_S_goldenshovel.Click
        S_Show("黄金铁铲", "Regal Shovel", "S_goldenshovel", "NoDLC", 1, 1, 1, "G_twigs", "×4", "S_gold_nugget", "×2", "", "", 2, "", "耐久度是普通铲子的四倍。人手一把去挖坟！")
    End Sub

    Private Sub button_S_hammer_click(sender As Object, e As RoutedEventArgs) Handles button_S_hammer.Click
        S_Show("锤子", "Hammer", "S_hammer", "NoDLC", 1, 1, 1, "G_twigs", "×3", "G_rocks", "×3", "G_cut_grass", "×6", 0, "", "用于破坏建筑，收回50%以上的建材，一般可以去拆野外的建筑获得资源。")
    End Sub

    Private Sub button_S_pitchfork_click(sender As Object, e As RoutedEventArgs) Handles button_S_pitchfork.Click
        S_Show("草叉", "Pitchfork", "S_pitchfork", "NoDLC", 1, 1, 1, "G_twigs", "×2", "G_flint", "×2", "", "", 1, "", "可以把地皮铲起来，后期改造基地非常有用。尽早给基地铺上鹅卵石的路吧。")
    End Sub

    Private Sub button_S_razor_click(sender As Object, e As RoutedEventArgs) Handles button_S_razor.Click
        S_Show("剃刀", "Razor", "S_razor", "NoDLC", 1, 1, 1, "G_twigs", "×2", "G_flint", "×2", "", "", 1, "", "用于刮牛毛，晚上趁牛睡觉可以举着火把刮点毛下来，威尔逊和韦伯的胡子长了你也可以自己刮点下来。")
    End Sub

    Private Sub button_S_saddlehorn_click(sender As Object, e As RoutedEventArgs) Handles button_S_saddlehorn.Click
        S_Show("取鞍器", "Saddlehorn", "S_saddlehorn", "DST", 0, 0, 1, "G_twigs", "×2", "G_boneshard", "×2", "G_jet_feather", "×1", 2, "", "用它来取下鞍再好不过了！")
    End Sub

    Private Sub button_S_saddle_click(sender As Object, e As RoutedEventArgs) Handles button_S_saddle.Click
        S_Show("鞍", "Saddle", "S_saddle", "DST", 0, 0, 1, "G_beefalo_wool", "×4", "G_pig_skin", "×4", "S_gold_nugget", "×4", 2, "", "用它可以骑上牛战斗！不过得记得先喂点草，要不然发怒的牛可不好惹哦！移动速度增加40%。")
    End Sub

    Private Sub button_S_war_saddle_click(sender As Object, e As RoutedEventArgs) Handles button_S_war_saddle.Click
        S_Show("浴血战鞍", "War Saddle", "S_war_saddle", "DST", 0, 0, 1, "A_rabbit", "×4", "G_steel_wool", "×4", "G_log", "×10", 2, "", "比普通鞍增加16点攻击力！但是材料难得，而且骑上牛不能用自己的武器，一般不会使用。移动速度增加25%。")
    End Sub

    Private Sub button_S_glossamer_saddle_click(sender As Object, e As RoutedEventArgs) Handles button_S_glossamer_saddle.Click
        S_Show("闪亮之鞍", "Glossamer Saddle", "S_glossamer_saddle", "DST", 0, 0, 1, "G_living_log", "×2", "G_silk", "×4", "F_butterfly_wing", "×68", 2, "", "如果你有一大堆蝴蝶翅膀时你会怎么做呢？直接吃掉回复血量？现在不用这么做了！新型的鞍诞生了！唯一的效果就是移动速度增加55%。")
    End Sub

    Private Sub button_S_brush_click(sender As Object, e As RoutedEventArgs) Handles button_S_brush.Click
        S_Show("刷子", "Brush", "S_brush", "DST", 0, 0, 1, "G_steel_wool", "×1", "G_walrus_tusk", "×1", "S_gold_nugget", "×2", 2, "", "过一段时间可以给牛刷一次毛，获得一个牛毛！刷完后牛会跟着你哟！")
    End Sub

    Private Sub button_S_salt_lick_click(sender As Object, e As RoutedEventArgs) Handles button_S_salt_lick.Click
        S_Show("舐盐器", "Salt Lick", "S_salt_lick", "DST", 0, 0, 1, "S_boards", "×2", "G_nitre", "×4", "", "", 2, "", "吸引附近的皮弗娄牛、电羊(带电电羊)和考拉象(冬考拉象)。这些生物会一直舔直到耗尽。当皮弗娄牛舔舐盐器时驯化水平不降低。一只皮弗娄牛15天消耗完，电羊(带电电羊)20天，考拉象(冬考拉象)7.5天。")
    End Sub

    REM ------------------科技(照明)-------------------
    Private Sub button_S_campfire_click(sender As Object, e As RoutedEventArgs) Handles button_S_campfire.Click
        S_Show("营火", "Campfire", "S_campfire", "NoDLC", 1, 1, 1, "G_cut_grass", "×3", "G_log", "×2", "", "", 0, "", "素材要求比较低，随时可以设置用于照明、烤制食物、取暖。燃料消耗完后会消失留下灰烬。")
    End Sub

    Private Sub button_S_fire_pit_click(sender As Object, e As RoutedEventArgs) Handles button_S_fire_pit.Click
        S_Show("石头篝火", "Fire Pit", "S_fire_pit", "NoDLC", 1, 1, 1, "G_log", "×2", "G_rocks", "×12", "", "", 0, "", "需要较多的石头，但是燃烧效率和下雨天的影响都比营火优秀，且燃料消耗完后不会消失。")
    End Sub

    Private Sub button_S_willows_lighter_click(sender As Object, e As RoutedEventArgs) Handles button_S_willows_lighter.Click
        S_Show("薇洛的打火机", "Willow's Lighter", "S_willow's_lighter", "DST", 0, 0, 1, "S_rope", "×1", "S_gold_nugget", "×1", "G_petals", "×3", 0, "薇洛", "薇洛出生自带的打火机，联机版有耐久限制，作用与火炬大同小异，不受下雨影响，还能用来烤食物。")
    End Sub

    Private Sub button_S_chiminea_click(sender As Object, e As RoutedEventArgs) Handles button_S_chiminea.Click
        S_Show("室外壁炉", "Chiminea", "S_chiminea", "SW", 0, 1, 0, "S_limestone", "×2", "G_sand", "×2", "G_log", "×2", 0, "", "防风的壁炉，谁用谁知道！它不会让周围的物品燃烧，风季的时候熄灭得也更慢，防风必备！")
    End Sub

    Private Sub button_S_torch_click(sender As Object, e As RoutedEventArgs) Handles button_S_torch.Click
        S_Show("火炬", "Torch", "S_torch", "NoDLC", 1, 1, 1, "G_cut_grass", "×2", "G_twigs", "×2", "", "", 0, "", "夜晚照明，冬季保暖，居家旅行，杀人放火必备神器。下雨会影响火焰燃烧时间。")
    End Sub

    Private Sub button_S_endothermic_fire_click(sender As Object, e As RoutedEventArgs) Handles button_S_endothermic_fire.Click
        S_Show("吸热火堆", "Endothermic Fire", "S_endothermic_fire", "NoDLC", 1, 1, 1, "G_cut_grass", "×3", "G_nitre", "×2", "", "", 1, "", "夏天外出必备冰火降温，和营火一样燃料消耗完后会消失。")
    End Sub

    Private Sub button_S_endothermic_fire_pit_click(sender As Object, e As RoutedEventArgs) Handles button_S_endothermic_fire_pit.Click
        S_Show("吸热篝火", "Endothermic Fire Pit", "S_endothermic_fire_pit", "NoDLC", 1, 1, 1, "G_nitre", "×2", "S_cut_stone", "×4", "S_electrical_doodad", "×2", 2, "", "需要较多的石头，但是燃烧效率和下雨天的影响都比冰火优秀，且燃料消耗完后不会消失。")
    End Sub

    Private Sub button_S_obsidian_fire_pit_click(sender As Object, e As RoutedEventArgs) Handles button_S_obsidian_fire_pit.Click
        S_Show("黑曜石火堆", "Obsidian Fire Pit", "S_obsidian_fire_pit", "SW", 0, 1, 0, "G_log", "×3", "G_obsidian", "×8", "", "", 2, "", "比营火的燃烧效率要高三倍，而且光照范围更大，免疫大风，不过会被淹没。")
    End Sub

    Private Sub button_S_miner_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_miner_hat.Click
        S_Show("矿工帽", "Miner Hat", "S_miner_hat", "NoDLC", 1, 1, 1, "G_straw_hat", "×1", "S_gold_nugget", "×1", "G_fireflies", "×1", 2, "", "大范围照明工具，优点在于你能腾出手来干活，缺点就是萤火虫是不可再生资源。可以用萤火虫、荧光果、粘滑含糊虫添加燃料。")
    End Sub

    Private Sub button_S_moggles_click(sender As Object, e As RoutedEventArgs) Handles button_S_moggles.Click
        S_Show("鼹鼠帽", "Moggles", "S_moggles", "ROG", 1, 0, 1, "F_moleworm", "×2", "S_electrical_doodad", "×2", "F_glow_berry", "×1", 2, "", "戴上鼹鼠帽就像戴上了夜视仪，但是千万别在白天戴上鼹鼠帽，别怪我没提醒你哟！可以用发光浆果添加燃料。")
    End Sub

    Private Sub button_S_pumpkin_lantern_click(sender As Object, e As RoutedEventArgs) Handles button_S_pumpkin_lantern.Click
        S_Show("南瓜灯", "Pumpkin Lantern", "S_pumpkin_lantern", "ROG", 1, 0, 1, "F_pumpkin", "×1", "G_fireflies", "×1", "", "", 2, "", "丢出来成为一个微弱的光源，持续时间1~2天。由于太暗，所以没人用，打烂它可以放出萤火虫。")
    End Sub

    Private Sub button_S_lantern_click(sender As Object, e As RoutedEventArgs) Handles button_S_lantern.Click
        S_Show("提灯", "Lantern", "S_lantern_1", "ROG", 1, 0, 1, "G_twigs", "×3", "G_cut_grass", "×2", "G_light_bulb", "×2", 2, "", "大范围照明工具，可以放置在地下，右键开关，下洞穴带着它是个不错的选择。可以用萤火虫、荧光果、粘滑含糊虫添加燃料。")
    End Sub

    Private Sub button_S_bottle_lantern_click(sender As Object, e As RoutedEventArgs) Handles button_S_bottle_lantern.Click
        S_Show("水瓶提灯", "Bottle Lantern", "S_bottle_lantern", "SW", 0, 1, 0, "S_empty_bottle", "×1", "G_bioluminescence", "×2", "", "", 2, "", "船难版的提灯。可以用萤火虫、荧光果、粘滑含糊虫添加燃料。")
    End Sub

    Private Sub button_S_boat_torch_click(sender As Object, e As RoutedEventArgs) Handles button_S_boat_torch.Click
        S_Show("船载火炬", "Boat Torch", "S_boat_torch", "SW", 0, 1, 0, "G_twigs", "×2", "S_torch", "×1", "", "", 1, "", "顾名思义，船上用的火炬，不能给木筏和竹筏用，照明范围和火炬一样。")
    End Sub

    Private Sub button_S_boat_lantern_click(sender As Object, e As RoutedEventArgs) Handles button_S_boat_lantern.Click
        S_Show("船灯", "Boat Lantern", "S_boat_lantern", "SW", 0, 1, 0, "S_empty_bottle", "×1", "G_twigs", "×2", "G_fireflies", "×1", 1, "", "装在船上的水瓶提灯，照明范围很大。可以用萤火虫、荧光果、粘滑含糊虫添加燃料。")
    End Sub

    REM ------------------科技(航海)-------------------
    Private Sub button_S_surfboard_click(sender As Object, e As RoutedEventArgs) Handles button_S_surfboard.Click
        S_Show("冲浪板", "Surfboard", "S_surfboard", "SW", 0, 1, 0, "S_boards", "×1", "G_seashell", "×2", "", "", 0, "瓦拉尼", "瓦拉尼出生自带的冲浪板，是竹筏速度的1.3倍，手中可以拿工具，只有100点耐久度。骑乘波浪时会增加一点精神和五倍速度。可以冲浪！")
    End Sub

    Private Sub button_S_lucky_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_lucky_hat.Click
        S_Show("幸运帽", "Lucky Hat", "S_lucky_hat", "SW", 0, 1, 0, "S_cloth", "×3", "G_boneshard", "×4", "G_dubloons", "×10", 0, "伍德莱格", "伍德莱格出生自带的幸运帽，每800秒(约1天半，基于耐久)产生一个X标记的宝藏。如果不是由玩家穿戴则只有66%几率产生X标记。")
    End Sub

    Private Sub button_S_the_sea_legs_click(sender As Object, e As RoutedEventArgs) Handles button_S_the_sea_legs.Click
        S_Show("海腿号", "The 'Sea Legs'", "S_the_'sea_legs'", "SW", 0, 1, 0, "S_boat_cannon", "×1", "S_boards", "×4", "G_dubloons", "×4", 0, "伍德莱格", "伍德莱格出生自带的材料可以建造一艘海腿号，拥有500点耐久，自带船帆和无限使用的船载加农炮(只有50的伤害)。航行速度：6。")
    End Sub

    Private Sub button_S_log_raft_click(sender As Object, e As RoutedEventArgs) Handles button_S_log_raft.Click
        S_Show("木筏", "Log Raft", "S_log_raft", "SW", 0, 1, 0, "G_log", "×6", "G_cut_grass", "×4", "", "", 0, "", "拥有150点耐久度的最基本的船。航行速度：4。")
    End Sub

    Private Sub button_S_raft_click(sender As Object, e As RoutedEventArgs) Handles button_S_raft.Click
        S_Show("竹筏", "Raft", "S_raft", "SW", 0, 1, 0, "G_bamboo_patch", "×4", "G_vine", "×3", "", "", 0, "", "拥有150点耐久度，速度比木筏略快一些。航行速度：5。")
    End Sub

    Private Sub button_S_row_boat_click(sender As Object, e As RoutedEventArgs) Handles button_S_row_boat.Click
        S_Show("划艇", "Row Boat", "S_row_boat", "SW", 0, 1, 0, "S_boards", "×3", "G_vine", "×4", "", "", 1, "", "拥有250点耐久度，速度比竹筏更快一些，最重要的是可以装上武器/船灯和船帆了。航行速度：6。")
    End Sub

    Private Sub button_S_cargo_boat_click(sender As Object, e As RoutedEventArgs) Handles button_S_cargo_boat.Click
        S_Show("货船", "Cargo Boat", "S_cargo_boat", "SW", 0, 1, 0, "S_boards", "×6", "S_rope", "×3", "", "", 2, "", "拥有300点耐久度，可以装备武器/船灯和船帆，还有额外的六个物品栏。航行速度：5。")
    End Sub

    Private Sub button_S_armoured_boat_click(sender As Object, e As RoutedEventArgs) Handles button_S_armoured_boat.Click
        S_Show("装甲船", "Armoured Boat", "S_armoured_boat", "SW", 0, 1, 0, "S_boards", "×6", "S_rope", "×3", "G_seashell", "×10", 2, "", "拥有500点耐久度，耐久度最高的船，可以装备武器/船灯和船帆。航行速度：6。")
    End Sub

    Private Sub button_S_boat_repair_kit_click(sender As Object, e As RoutedEventArgs) Handles button_S_boat_repair_kit.Click
        S_Show("修船套件", "Boat Repair Kit", "S_boat_repair_kit", "SW", 0, 1, 0, "S_boards", "×2", "G_stinger", "×2", "S_rope", "×2", 1, "", "每次使用修复船100点耐久度，可以使用三次。")
    End Sub

    Private Sub button_S_thatch_sail_click(sender As Object, e As RoutedEventArgs) Handles button_S_thatch_sail.Click
        S_Show("草帆", "Thatch Sail", "S_thatch_sail", "SW", 0, 1, 0, "G_bamboo_patch", "×2", "G_vine", "×2", "G_palm_leaf", "×4", 1, "", "可以提高20%的航速，耐久性最低。只需要科学机器就可以建造。")
    End Sub

    Private Sub button_S_cloth_sail_click(sender As Object, e As RoutedEventArgs) Handles button_S_cloth_sail.Click
        S_Show("布帆", "Cloth Sail", "S_cloth_sail", "SW", 0, 1, 0, "G_bamboo_patch", "×2", "S_rope", "×2", "S_cloth", "×2", 2, "", "可以提高30%的航速，耐久性中等，需要炼金术引擎才能建造。")
    End Sub

    Private Sub button_S_snakeskin_sail_click(sender As Object, e As RoutedEventArgs) Handles button_S_snakeskin_sail.Click
        S_Show("蛇皮帆", "Snakeskin Sail", "S_snakeskin_sail", "SW", 0, 1, 0, "G_log", "×4", "S_rope", "×2", "G_snakeskin", "×2", 2, "", "耐久性最高，然而速度在草帆和布帆之间，比较适合长途旅行。")
    End Sub

    Private Sub button_S_feather_lite_sail_click(sender As Object, e As RoutedEventArgs) Handles button_S_feather_lite_sail.Click
        S_Show("简版羽帆", "Feather Lite Sail", "S_feather_lite_sail", "SW", 0, 1, 0, "G_bamboo_patch", "×2", "S_rope", "×2", "G_doydoy_feather", "×4", 1, "", "可以提高40%的航速，但是耐久性和草帆相当。")
    End Sub

    Private Sub button_S_iron_wind_click(sender As Object, e As RoutedEventArgs) Handles button_S_iron_wind.Click
        S_Show("铁风牌发动机", "Iron Wind", "S_iron_wind", "SW", 0, 1, 0, "G_turbine_blades", "×1", "S_electrical_doodad", "×1", "S_gold_nugget", "×2", 2, "", "可以提高50%的航速，耐久性也最高，然而为了获得材料涡轮叶片必须打败豹卷风。只能通过齿轮修复，一个齿轮修复25%耐久度。")
    End Sub

    Private Sub button_S_boat_cannon_click(sender As Object, e As RoutedEventArgs) Handles button_S_boat_cannon.Click
        S_Show("船载加农炮", "Boat Cannon", "S_boat_cannon", "SW", 0, 1, 0, "G_log", "×5", "S_gunpowder", "×4", "F_coconut", "×6", 1, "", "每炮都能造成100点伤害，总共可以射出15炮。")
    End Sub

    Private Sub button_S_sea_trap_click(sender As Object, e As RoutedEventArgs) Handles button_S_sea_trap.Click
        S_Show("海洋陷阱", "Sea Trap", "S_sea_trap", "SW", 0, 1, 0, "G_palm_leaf", "×4", "S_empty_bottle", "×2", "A_jellyfish", "×1", 1, "", "捉龙虾就靠它！")
    End Sub

    Private Sub button_S_trawl_net_click(sender As Object, e As RoutedEventArgs) Handles button_S_trawl_net.Click
        S_Show("拖网", "Trawl Net", "S_trawl_net", "SW", 0, 1, 0, "S_rope", "×3", "G_bamboo_patch", "×2", "", "", 1, "", "挂在船后拖一段时间就可以随机获得九样东西。")
    End Sub

    Private Sub button_S_spyglass_click(sender As Object, e As RoutedEventArgs) Handles button_S_spyglass.Click
        S_Show("望远镜", "Spyglass", "S_spyglass", "SW", 0, 1, 0, "S_empty_bottle", "×1", "G_pig_skin", "×1", "S_gold_nugget", "×1", 1, "", "通过点击右键看到远处的东西。距离受到时间的影响，白天最远，在船上或者风季距离都会减小。可以发现X标记的宝箱。")
    End Sub

    Private Sub button_S_super_spyglass_click(sender As Object, e As RoutedEventArgs) Handles button_S_super_spyglass.Click
        S_Show("EX望远镜", "Super Spyglass", "S_super_spyglass", "SW", 0, 1, 0, "S_spyglass", "×1", "F_eye_of_the_tiger_shark", "×1", "S_gold_nugget", "×1", 1, "", "距离是望远镜的两倍，可以被大风吹走。")
    End Sub

    Private Sub button_S_captain_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_captain_hat.Click
        S_Show("船长帽", "Captain Hat", "S_captain_hat", "SW", 0, 1, 0, "F_seaweed", "×1", "G_boneshard", "×1", "S_straw_hat", "×1", 1, "", "戴上会使船的航行耐久度消耗减半。当个船长去冒险！")
    End Sub

    Private Sub button_S_pirate_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_pirate_hat.Click
        S_Show("海盗帽", "Pirate Hat", "S_pirate_hat", "SW", 0, 1, 0, "G_boneshard", "×2", "S_rope", "×1", "G_silk", "×2", 1, "", "航行时戴上它会增加地图显示范围，缓慢恢复精神。")
    End Sub

    Private Sub button_S_life_jacket_click(sender As Object, e As RoutedEventArgs) Handles button_S_life_jacket.Click
        S_Show("救生衣", "Life Jacket", "S_life_jacket", "SW", 0, 1, 0, "S_cloth", "×2", "G_vine", "×2", "S_empty_bottle", "×3", 1, "", "穿上救生衣，不怕翻船！当船的耐久度降为0后，屏幕会逐渐变黑，玩家出现在最近的船难上，并且拥有足够的草和木头制作木筏。")
    End Sub

    Private Sub button_S_buoy_click(sender As Object, e As RoutedEventArgs) Handles button_S_buoy.Click
        S_Show("浮标", "Buoy", "S_buoy", "SW", 0, 1, 0, "S_empty_bottle", "×1", "G_bamboo_patch", "×4", "G_bioluminescence", "×2", 1, "", "放在海上提供比火炬更亮的光，不需要添加燃料。")
    End Sub

    REM ------------------科技(生存)-------------------
    Private Sub button_S_chef_pouch_click(sender As Object, e As RoutedEventArgs) Handles button_S_chef_pouch.Click
        S_Show("厨师的袋子", "Chef Pouch", "S_chef_pouch", "SW", 0, 1, 0, "S_cloth", "×1", "S_rope", "×1", "", "", 0, "沃利", "沃利特有的8格隔热包！作用和冰箱类似可以让食物变质变缓慢，但是和冰箱不同的是不能阻止冰块变质，也不能冷却热能石。")
    End Sub

    Private Sub button_S_telltale_heart_click(sender As Object, e As RoutedEventArgs) Handles button_S_telltale_heart.Click
        S_Show("救赎之心", "Telltale Heart", "S_telltale_heart", "DST", 0, 0, 1, "G_cut_grass", "×3", "G_spider_gland", "×1", "G_health", "-40", 0, "", "相对于肉块雕像属于比较容易制作的救人道具，救人的玩家回复80点精神，被复活的玩家损失25%最大生命上限。")
    End Sub

    Private Sub button_S_healing_salve_click(sender As Object, e As RoutedEventArgs) Handles button_S_healing_salve.Click
        S_Show("治疗药膏", "Healing Salve", "S_healing_salve", "NoDLC", 1, 1, 1, "G_ash", "×2", "G_rocks", "×1", "G_spider_gland", "×1", 1, "", "可以恢复20生命，但是寻找素材比较麻烦，如果我有蜘蛛腺体，还是直接使用(恢复8生命)方便点。")
    End Sub

    Private Sub button_S_honey_poultice_click(sender As Object, e As RoutedEventArgs) Handles button_S_honey_poultice.Click
        S_Show("蜂蜜药膏", "honey Poultice", "S_honey_poultice", "NoDLC", 1, 1, 1, "S_papyrus", "×1", "F_honey", "×2", "", "", 2, "", "可以恢复30生命，只要控制了沼泽地的芦苇便能大面积生产，它没有保鲜度，所以要备一些在宝箱。")
    End Sub

    Private Sub button_S_booster_shot_click(sender As Object, e As RoutedEventArgs) Handles button_S_booster_shot.Click
        S_Show("AGV试验药", "Booster Shot", "S_booster_shot", "DST", 0, 0, 1, "F_rot", "×8", "G_nitre", "×2", "G_stinger", "×1", 2, "", "可以恢复通过救赎之心、恶魔之门和肉块雕像复活而损失的生命上限，可谓是神器！现在的配方也很容易制作，再也不用担心血上限的问题了。")
    End Sub

    Private Sub button_S_bernie_click(sender As Object, e As RoutedEventArgs) Handles button_S_bernie.Click
        S_Show("伯尼", "Bernie", "S_bernie_1", "DST", 0, 0, 1, "G_beard_hair", "×2", "G_beefalo_wool", "×2", "G_silk", "×2", 0, "薇洛", "薇洛出生自带的小熊伯尼，精神低的时候放在地上可以吸引暗影怪的仇恨。")
    End Sub

    Private Sub button_S_trap_click(sender As Object, e As RoutedEventArgs) Handles button_S_trap.Click
        S_Show("陷阱", "Trap", "S_trap", "NoDLC", 1, 1, 1, "G_twigs", "×2", "G_cut_grass", "×6", "", "", 0, "", "可以用来抓些兔子什么的，材料简单，不需要科技，前期可以很方便地获得小肉。可多次使用。")
    End Sub

    Private Sub button_S_bird_trap_click(sender As Object, e As RoutedEventArgs) Handles button_S_bird_trap.Click
        S_Show("捕鸟器", "Bird Trap", "S_bird_trap", "NoDLC", 1, 1, 1, "G_twigs", "×3", "G_silk", "×4", "", "", 1, "", "顾名思义，捕鸟的陷阱。可多次使用。")
    End Sub

    Private Sub button_S_bug_net_click(sender As Object, e As RoutedEventArgs) Handles button_S_bug_net.Click
        S_Show("捕虫网", "Bug Net", "S_bug_net", "NoDLC", 1, 1, 1, "G_twigs", "×4", "G_silk", "×2", "S_rope", "×1", 1, "", "可以用来抓蝴蝶、蜜蜂、萤火虫。可使用10次。最好先造个原型体，然后到哪都可以抓虫子了。")
    End Sub

    Private Sub button_S_fishing_rod_click(sender As Object, e As RoutedEventArgs) Handles button_S_fishing_rod.Click
        S_Show("钓竿", "Fishing Rod", "S_fishing_rod", "NoDLC", 1, 1, 1, "G_twigs", "×2", "G_silk", "×2", "", "", 1, "", "人人都知道鱼排是补血神器，素材简单，回血又多。你还在等什么,还不弄个杆子去钓鱼，不用鱼饵哟。")
    End Sub

    Private Sub button_S_silly_monkey_ball_click(sender As Object, e As RoutedEventArgs) Handles button_S_silly_monkey_ball.Click
        S_Show("傻猴子球", "Silly Monkey Ball", "S_silly_monkey_ball", "SW", 0, 1, 0, "G_snakeskin", "×2", "F_banana", "×1", "S_rope", "×2", 2, "", "傻猴子们，玩球儿去~")
    End Sub

    Private Sub button_S_tropical_parasol_click(sender As Object, e As RoutedEventArgs) Handles button_S_tropical_parasol.Click
        S_Show("热带遮阳伞", "Tropical Parasol", "S_tropical_parasol", "SW", 0, 1, 0, "G_twigs", "×4", "G_palm_leaf", "×3", "G_petals", "×6", 0, "", "船难版的漂亮太阳伞，防止潮湿和过热。无论是否装备上都会消耗耐久。")
    End Sub

    Private Sub button_S_pretty_parasol_click(sender As Object, e As RoutedEventArgs) Handles button_S_pretty_parasol.Click
        S_Show("漂亮太阳伞", "Pretty Parasol", "S_pretty_parasol", "ROG", 1, 0, 1, "G_twigs", "×4", "G_cut_grass", "×3", "G_petals", "×6", 0, "", "夏天可以稍微防止高温，由于占用装备栏，一般不太使用。")
    End Sub

    Private Sub button_S_umbrella_click(sender As Object, e As RoutedEventArgs) Handles button_S_umbrella.Click
        S_Show("雨伞", "Umbrella", "S_umbrella", "NoDLC", 1, 1, 1, "G_twigs", "×6", "G_pig_skin", "×1", "G_silk", "×2", 1, "", "降低下雨对玩家的影响，有些文艺青年也拿它当武器使。")
    End Sub

    Private Sub button_S_anti_venom_click(sender As Object, e As RoutedEventArgs) Handles button_S_anti_venom.Click
        S_Show("抗蛇毒血清", "Anti Venom", "S_anti_venom", "SW", 0, 1, 0, "G_venom_gland", "×1", "F_seaweed", "×3", "G_coral", "×2", 1, "", "中毒了用什么？抗蛇毒血清！治疗那种不舒服的中毒感。")
    End Sub

    Private Sub button_S_waterballoon_click(sender As Object, e As RoutedEventArgs) Handles button_S_waterballoon.Click
        S_Show("水球", "WaterBalloon", "S_waterballoon", "DST", 0, 0, 1, "G_mosquito_sack", "×2", "F_ice", "×1", "", "", 1, "", "卧槽，这不是我小时候玩的那个嘛！无论燃烧得多旺的篝火都能一次性熄灭，还能给着火的树灭火！如果被砸中会增加20点湿度。")
    End Sub

    Private Sub button_S_pile_o_balloons_click(sender As Object, e As RoutedEventArgs) Handles button_S_pile_o_balloons.Click
        S_Show("一堆气球", "Pile o' Balloons", "S_pile_o'_balloons", "DST", 0, 0, 1, "S_waterballoon", "×4", "", "", "", "", 0, "韦斯", "韦斯出生自带的一堆气球，用途嘛...当然是吹气球啦！每次吹气球会掉5点精神，气球被击破会连环引爆，然而伤害低的可怜。")
    End Sub

    Private Sub button_S_compass_click(sender As Object, e As RoutedEventArgs) Handles button_S_compass.Click
        S_Show("指南针", "Compass", "S_compass", "NoDLC", 1, 1, 1, "S_gold_nugget", "×1", "G_flint", "×1", "", "", 0, "", "毫！无！卵！用！")
    End Sub

    Private Sub button_S_thermal_stone_click(sender As Object, e As RoutedEventArgs) Handles button_S_thermal_stone.Click
        S_Show("热能石", "Thermal Stone", "S_thermal_stone", "NoDLC", 1, 1, 1, "G_rocks", "×10", "S_pickaxe", "×1", "G_flint", "×3", 2, "", "靠近篝火可以吸收热量，供玩家在冬季保暖，有一定热度时戴在身上还可以发光.放在冰箱里可以放热，供玩家夏季乘凉。")
    End Sub

    Private Sub button_S_thatch_pack_click(sender As Object, e As RoutedEventArgs) Handles button_S_thatch_pack.Click
        S_Show("茅草包", "Thatch Pack", "S_thatch_pack", "SW", 0, 1, 0, "G_palm_leaf", "×4", "", "", "", "", 0, "", "只能增加4格物品栏，大概没有更小的包了。唯一的好处是不需要科技就能做。")
    End Sub

    Private Sub button_S_backpack_click(sender As Object, e As RoutedEventArgs) Handles button_S_backpack.Click
        S_Show("背包", "Backpack", "S_backpack", "NoDLC", 1, 1, 1, "G_cut_grass", "×4", "G_twigs", "×4", "", "", 1, "", "前期就能制作，能增加8格物品栏，相信所有喜欢旅行喜欢探险的玩家都会装备吧。")
    End Sub

    Private Sub button_S_piggyback_click(sender As Object, e As RoutedEventArgs) Handles button_S_piggyback.Click
        S_Show("小猪包", "Piggyback", "S_piggyback", "NoDLC", 1, 1, 1, "G_pig_skin", "×4", "G_silk", "×6", "S_rope", "×2", 2, "", "小猪包能增加12格物品栏，但是会减少主角的移动速度，毕竟不是LV的包包，不是很实用。")
    End Sub

    Private Sub button_S_straw_roll_click(sender As Object, e As RoutedEventArgs) Handles button_S_straw_roll.Click
        S_Show("稻草卷", "Straw Roll", "S_straw_roll", "NoDLC", 1, 1, 1, "G_cut_grass", "×6", "S_rope", "×1", "", "", 1, "", "睡觉可以度过晚上，并且恢复精神，由于材料非常好找，自己精神低的时候就别硬撑了。")
    End Sub

    Private Sub button_S_fur_roll_click(sender As Object, e As RoutedEventArgs) Handles button_S_fur_roll.Click
        S_Show("毛皮铺盖", "Fur roll", "S_fur_roll", "ROG", 1, 0, 1, "S_straw_roll", "×1", "G_bunny_puff", "×2", "", "", 2, "", "和稻草卷一样是睡觉用的，相比之下毛皮铺盖还能额外恢复30生命值，使用次数也增加到了3次。")
    End Sub

    Private Sub button_S_tent_click(sender As Object, e As RoutedEventArgs) Handles button_S_tent.Click
        S_Show("帐篷", "Tent", "S_tent", "NoDLC", 1, 1, 1, "G_silk", "×6", "G_twigs", "×4", "S_rope", "×3", 2, "", "供玩家睡觉，可使用6次，可同时恢复生命和精神。用过5次之后建议用锤子砸烂并回收部分建材。")
    End Sub

    Private Sub button_S_siesta_lean_to_click(sender As Object, e As RoutedEventArgs) Handles button_S_siesta_lean_to.Click
        S_Show("简易小木棚", "Siesta Lean-to", "S_siesta_lean-to", "NoDLC", 1, 1, 1, "G_silk", "×2", "S_boards", "×4", "S_rope", "×3", 2, "", "白天睡觉恢复精神和生命，可使用6次，消耗的饥饿比帐篷低，可谓是恢复神器。")
    End Sub

    Private Sub button_S_palm_leaf_hut_click(sender As Object, e As RoutedEventArgs) Handles button_S_palm_leaf_hut.Click
        S_Show("椰叶小屋", "Palm Leaf Hut", "S_palm_leaf_hut", "SW", 0, 1, 0, "G_palm_leaf", "×4", "G_bamboo_patch", "×4", "S_rope", "×3", 2, "", "待在椰叶小屋下，不会被雨淋，不会过热。小屋没有耐久，尽管其外表脆弱，然而不会受到大风的影响。小屋的保护区域比看起来要大一些。")
    End Sub

    Private Sub button_S_whirly_fan_click(sender As Object, e As RoutedEventArgs) Handles button_S_whirly_fan.Click
        S_Show("旋风扇", "Whirly Fan", "S_whirly_fan", "DST", 0, 0, 1, "G_twigs", "×3", "G_petals", "×1", "", "", 0, "", "大风车吱呀吱扭扭滴转~ 什么，你问为什么不转？跑起来吖！")
    End Sub

    Private Sub button_S_luxury_fan_click(sender As Object, e As RoutedEventArgs) Handles button_S_luxury_fan.Click
        S_Show("豪华风扇", "Luxury Fan", "S_luxury_fan", "ROG", 1, 0, 1, "G_down_feather", "×5", "G_cut_reeds", "×2", "S_rope", "×2", 2, "", "扇一下会让玩家和周围温度降低50°，但最低不会低于2.5°。会扑灭周围的火！")
    End Sub

    Private Sub button_S_tropical_fan_click(sender As Object, e As RoutedEventArgs) Handles button_S_tropical_fan.Click
        S_Show("热带风扇", "Tropical Fan", "S_tropical_fan", "SW", 0, 1, 0, "G_doydoy_feather", "×5", "G_cut_reeds", "×2", "S_rope", "×2", 2, "", "扇一下会让玩家温度降低50°，但最低不会低于5°。会扑灭周围的火！可以使用15次。")
    End Sub

    Private Sub button_S_insulated_pack_click(sender As Object, e As RoutedEventArgs) Handles button_S_insulated_pack.Click
        S_Show("隔热包", "Insulated Pack", "S_insulated_pack", "ROG", 1, 0, 1, "S_thick_fur", "×1", "G_gears", "×3", "S_electrical_doodad", "×3", 2, "", "只有6个格子的背包，但是作用和冰箱类似可以让食物变质变缓慢，但是和冰箱不同的是不能阻止冰块变质，也不能冷却热能石。")
    End Sub

    Private Sub button_S_sea_sack_click(sender As Object, e As RoutedEventArgs) Handles button_S_sea_sack.Click
        S_Show("海上麻袋", "Sea Sack", "S_sea_sack", "SW", 0, 1, 0, "G_cut_reeds", "×5", "G_vine", "×2", "G_shark_gills", "×1", 2, "", "船难版的隔热包。只有6个格子的背包，但是作用和冰箱类似可以让食物变质变缓慢，但是和冰箱不同的是不能阻止冰块变质，也不能冷却热能石。")
    End Sub

    Private Sub button_S_doydoy_nest_click(sender As Object, e As RoutedEventArgs) Handles button_S_doydoy_nest.Click
        S_Show("渡渡鸟巢", "Doydoy Nest", "S_doydoy_nest", "SW", 0, 1, 0, "G_twigs", "×8", "G_doydoy_feather", "×2", "G_manure", "×4", 2, "", "建造一个渡渡鸟巢来孵小渡渡鸟~")
    End Sub

    REM ------------------科技(食物)-------------------
    Private Sub button_S_mussel_stick_click(sender As Object, e As RoutedEventArgs) Handles button_S_mussel_stick.Click
        S_Show("采贝器", "Mussel Stick", "S_mussel_stick", "SW", 0, 1, 0, "G_bamboo_patch", "×2", "G_vine", "×1", "F_seaweed", "×1", 1, "", "插在贻贝床上可以采贝，时间越久采的贻贝越多，最多六个。")
    End Sub

    Private Sub button_S_basic_farm_click(sender As Object, e As RoutedEventArgs) Handles button_S_basic_farm.Click
        S_Show("基本农场", "Basic Farm", "S_basic_farm", "NoDLC", 1, 1, 1, "G_cut_grass", "×8", "G_manure", "×4", "G_log", "×4", 1, "", "素材要求比较低，前期可以弄几个来种植蔬菜。")
    End Sub

    Private Sub button_S_improved_farm_click(sender As Object, e As RoutedEventArgs) Handles button_S_improved_farm.Click
        S_Show("高级农场", "Improved Farm", "S_improved_farm", "NoDLC", 1, 1, 1, "G_cut_grass", "×10", "G_manure", "×6", "G_rocks", "×4", 2, "", "需要大量的素材，但是它是值得的，因为它的生长速度和施肥效率都是基本农场的2倍。")
    End Sub

    Private Sub button_S_bucket_o_poop_click(sender As Object, e As RoutedEventArgs) Handles button_S_bucket_o_poop.Click
        S_Show("便便篮", "Bucket-o-poop", "S_bucket-o-poop", "NoDLC", 1, 1, 1, "G_manure", "×3", "G_boneshard", "×2", "G_log", "×4", 1, "", "三个便便变成十个便便的戏法。")
    End Sub

    Private Sub button_S_bee_box_click(sender As Object, e As RoutedEventArgs) Handles button_S_bee_box.Click
        S_Show("蜂箱", "Bee Box", "S_bee_box", "NoDLC", 1, 1, 1, "S_boards", "×2", "F_honeycomb", "×1", "A_bee", "×4", 1, "", "可自行养殖蜜蜂生产蜂蜜，旁边需要用蝴蝶种植一些花朵蜜蜂采蜜才有效率，收蜜时小心蜜蜂攻击。")
    End Sub

    Private Sub button_S_drying_rack_click(sender As Object, e As RoutedEventArgs) Handles button_S_drying_rack.Click
        S_Show("晒肉架", "Drying Rack", "S_drying_rack", "NoDLC", 1, 1, 1, "G_twigs", "×3", "G_charcoal", "×2", "S_rope", "×3", 1, "", "可以把肉类风干，大肉和蝙蝠翅膀需要2天，其他肉需要1天，下雨暂停风干过程。")
    End Sub

    Private Sub button_S_DryingRack_table_click(sender As Object, e As RoutedEventArgs) Handles button_S_DryingRack_table.Click
        Canvas_ScienceLeft_DryingRack.Visibility = Visibility.Visible
    End Sub

    Private Sub SL_btn_close_DryingRack_click(sender As Object, e As RoutedEventArgs) Handles SL_btn_close_DryingRack.Click
        Canvas_ScienceLeft_DryingRack.Visibility = Visibility.Collapsed
    End Sub

    Private Sub button_S_Meats_1_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_1.Click
        ButtonJump("F_meat")
    End Sub

    Private Sub button_S_Meats_2_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_2.Click
        ButtonJump("F_monster_meat")
    End Sub

    Private Sub button_S_Meats_3_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_3.Click
        ButtonJump("F_morsel")
    End Sub

    Private Sub button_S_Meats_4_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_4.Click
        ButtonJump("F_drumstick")
    End Sub

    Private Sub button_S_Meats_5_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_5.Click
        ButtonJump("F_frog_legs")
    End Sub

    Private Sub button_S_Meats_6_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_6.Click
        ButtonJump("F_fish")
    End Sub

    Private Sub button_S_Meats_7_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_7.Click
        ButtonJump("F_eel")
    End Sub

    Private Sub button_S_Meats_8_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_8.Click
        ButtonJump("F_batilisk_wing")
    End Sub

    Private Sub button_S_Meats_9_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_9.Click
        ButtonJump("F_tropical_fish")
    End Sub

    Private Sub button_S_Meats_10_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_10.Click
        ButtonJump("F_fish_morsel")
    End Sub

    Private Sub button_S_Meats_11_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_11.Click
        ButtonJump("F_dead_jellyfish")
    End Sub

    Private Sub button_S_Meats_12_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_12.Click
        ButtonJump("F_dead_dogfish")
    End Sub

    Private Sub button_S_Meats_13_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_13.Click
        ButtonJump("F_raw_fish")
    End Sub

    Private Sub button_S_Meats_14_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_14.Click
        ButtonJump("F_dead_swordfish")
    End Sub

    Private Sub button_S_Meats_15_click(sender As Object, e As RoutedEventArgs) Handles button_S_Meats_15.Click
        ButtonJump("F_seaweed")
    End Sub

    Private Sub button_S_Jerky_1_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_1.Click
        ButtonJump("F_jerky")
    End Sub

    Private Sub button_S_Jerky_2_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_2.Click
        ButtonJump("F_monster_jerky")
    End Sub

    Private Sub button_S_Jerky_3_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_3.Click
        ButtonJump("F_small_jerky")
    End Sub

    Private Sub button_S_Jerky_4_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_4.Click
        ButtonJump("F_small_jerky")
    End Sub

    Private Sub button_S_Jerky_5_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_5.Click
        ButtonJump("F_small_jerky")
    End Sub

    Private Sub button_S_Jerky_6_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_6.Click
        ButtonJump("F_small_jerky")
    End Sub

    Private Sub button_S_Jerky_7_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_7.Click
        ButtonJump("F_small_jerky")
    End Sub

    Private Sub button_S_Jerky_8_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_8.Click
        ButtonJump("F_small_jerky")
    End Sub

    Private Sub button_S_Jerky_9_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_9.Click
        ButtonJump("F_small_jerky")
    End Sub

    Private Sub button_S_Jerky_10_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_10.Click
        ButtonJump("F_small_jerky")
    End Sub

    Private Sub button_S_Jerky_11_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_11.Click
        ButtonJump("F_dried_jellyfish")
    End Sub

    Private Sub button_S_Jerky_12_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_12.Click
        ButtonJump("F_jerky")
    End Sub

    Private Sub button_S_Jerky_13_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_13.Click
        ButtonJump("F_jerky")
    End Sub

    Private Sub button_S_Jerky_14_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_14.Click
        ButtonJump("F_jerky")
    End Sub

    Private Sub button_S_Jerky_15_click(sender As Object, e As RoutedEventArgs) Handles button_S_Jerky_15.Click
        ButtonJump("F_dried_seaweed")
    End Sub

    Private Sub button_S_crock_pot_click(sender As Object, e As RoutedEventArgs) Handles button_S_crock_pot.Click
        S_Show("烹煮锅", "Crock Pot", "S_crock_pot", "NoDLC", 1, 1, 1, "S_cut_stone", "×3", "G_charcoal", "×6", "G_twigs", "×6", 1, "", "允许玩家把4份食材烹饪成料理，料理配方见食物表。")
    End Sub

    Private Sub button_S_ice_box_click(sender As Object, e As RoutedEventArgs) Handles button_S_ice_box.Click
        S_Show("冰箱", "Ice Box", "S_ice_box", "NoDLC", 1, 1, 1, "S_gold_nugget", "×2", "G_gears", "×1", "S_cut_stone", "×1", 2, "", "拥有9格的储存功能，只允许储藏食物和热能石，放进去的食物腐烂速度延缓50%。")
    End Sub

    REM ------------------科技(科学)-------------------
    Private Sub button_S_science_machine_click(sender As Object, e As RoutedEventArgs) Handles button_S_science_machine.Click
        S_Show("科学机器", "Science Machine", "S_science_machine", "NoDLC", 1, 1, 1, "S_gold_nugget", "×1", "G_log", "×4", "G_rocks", "×4", 0, "", "靠近后为玩家解锁一级科技，制作成功后，当前科技将永久解锁。")
    End Sub

    Private Sub button_S_alchemy_engine_click(sender As Object, e As RoutedEventArgs) Handles button_S_alchemy_engine.Click
        S_Show("炼金术引擎", "Alchemy Engine", "S_alchemy_engine", "NoDLC", 1, 1, 1, "S_boards", "×4", "S_cut_stone", "×2", "S_electrical_doodad", "×2", 1, "", "靠近后为玩家解锁二级科技，制作成功后，当前科技将永久解锁。")
    End Sub

    Private Sub button_S_electrical_doodad_click(sender As Object, e As RoutedEventArgs) Handles button_S_electrical_doodad.Click
        S_Show("电器元件", "Electrical Doodad", "S_electrical_doodad", "NoDLC", 1, 1, 1, "S_gold_nugget", "×2", "S_cut_stone", "×1", "", "", 1, "", "高级素材，进一步合成更多道具装备。")
    End Sub

    Private Sub button_S_divining_rod_click(sender As Object, e As RoutedEventArgs) Handles button_S_divining_rod.Click
        S_Show("魔杖", "Divining Rod", "S_divining_rod", "NoDLC", 1, 1, 1, "G_twigs", "×1", "G_nightmare_fuel", "×4", "G_gears", "×1", 2, "", "魔棒是用于定位和寻找麦斯威尔之门的道具，在联机版里貌似没有用处。")
    End Sub

    Private Sub button_S_thermal_measurer_click(sender As Object, e As RoutedEventArgs) Handles button_S_thermal_measurer.Click
        S_Show("温度测量器", "Thermal Measurer", "S_thermal_measurer", "NoDLC", 1, 1, 1, "S_boards", "×2", "S_gold_nugget", "×2", "", "", 1, "", "显示当前温度，判断夏季和冬季来临时间。")
    End Sub

    Private Sub button_S_rainometer_click(sender As Object, e As RoutedEventArgs) Handles button_S_rainometer.Click
        S_Show("雨量计", "Rainometer", "S_rainometer", "NoDLC", 1, 1, 1, "S_boards", "×2", "S_gold_nugget", "×2", "S_rope", "×2", 1, "", "显示当前湿度，判断下雨和下雪的来临时间。")
    End Sub

    Private Sub button_S_gunpowder_click(sender As Object, e As RoutedEventArgs) Handles button_S_gunpowder.Click
        S_Show("火药", "Gunpowder", "S_gunpowder", "NoDLC", 1, 1, 1, "F_rotten_egg", "×1", "G_charcoal", "×1", "G_nitre", "×1", 2, "", "点燃后爆炸，小范围造成200点伤害，这足以秒杀玩家，高数量伤害会叠加，对付移动速度慢的怪好用。")
    End Sub

    Private Sub button_S_lightning_rod_click(sender As Object, e As RoutedEventArgs) Handles button_S_lightning_rod.Click
        S_Show("避雷针", "Lightning Rod", "S_lightning_rod", "NoDLC", 1, 1, 1, "S_gold_nugget", "×4", "S_cut_stone", "×1", "", "", 1, "", "引导雷电，使其不会劈到周围可燃物上，如果避雷针被雷电劈中，它连续几天都有静电且会发光。")
    End Sub

    Private Sub button_S_ice_flingomatic_click(sender As Object, e As RoutedEventArgs) Handles button_S_ice_flingomatic.Click
        S_Show("雪球发射器", "Ice Flingomatic", "S_ice_flingomatic", "NoDLC", 1, 1, 1, "G_gears", "×2", "F_ice", "×15", "S_electrical_doodad", "×2", 2, "", "雪球发射机可以扑灭任何冒烟或者着火的物体并且降温，包括篝火、暗夜照明灯。任何燃料物质可以为其补充燃料。可以让农作物夏天也能收成。")
    End Sub

    Private Sub button_S_ice_maker_3000_click(sender As Object, e As RoutedEventArgs) Handles button_S_ice_maker_3000.Click
        S_Show("造冰机 3000 plus", "Ice Maker 3000", "S_ice_maker_3000", "SW", 0, 1, 0, "S_thermal_stone", "×1", "G_bamboo_patch", "×5", "S_electrical_doodad", "×2", 2, "", "用燃料换冰。每30秒产生一个冰，最多3个，一旦开始生产就不能停下，燃料添加过多也不会有多余的冰。被建造时会持续生产45秒(即一个半冰)。剩余30秒时若燃料不够则会浪费剩余的燃料。")
    End Sub

    Private Sub button_S_accomploshrine_click(sender As Object, e As RoutedEventArgs) Handles button_S_accomploshrine.Click
        S_Show("神灯", "Accomploshrine", "S_accomploshrine", "NoDLC", 1, 1, 1, "S_gold_nugget", "×10", "S_cut_stone", "×1", "G_gears", "×6", 2, "", "仅存在于PSN/XBOX版本的东西，唯一目的就是增加一项成就。每激活25次就会放一次焰火，激活725次放更大的焰火并且获得成就——...and despair!")
    End Sub

    REM ------------------科技(战斗)-------------------
    Private Sub button_S_battle_spear_click(sender As Object, e As RoutedEventArgs) Handles button_S_battle_spear.Click
        S_Show("战斗长矛", "Battle Spear", "S_battle_spear", "DST", 1, 1, 1, "G_twigs", "×2", "G_flint", "×2", "S_gold_nugget", "×2", 0, "维京女", "维京女出生自带的战斗长矛，攻击力比长矛要高一些，制作材料也比较简单，适合人手一把。")
    End Sub

    Private Sub button_S_battle_helm_click(sender As Object, e As RoutedEventArgs) Handles button_S_battle_helm.Click
        S_Show("战斗头盔", "Battle Helm", "S_battle_helm", "DST", 1, 1, 1, "S_gold_nugget", "×2", "G_rocks", "×2", "", "", 0, "维京女", "维京女出生自带的战斗头盔，防御力不错，野建议人手一个并多备一些，消耗得比较快。")
    End Sub

    Private Sub button_S_spear_click(sender As Object, e As RoutedEventArgs) Handles button_S_spear.Click
        S_Show("长矛", "Spear", "S_spear", "NoDLC", 1, 1, 1, "G_twigs", "×2", "S_rope", "×1", "G_flint", "×1", 1, "", "若小伙伴中没有维京女，就只能用长矛将就一下了，比较不错的前期武器。")
    End Sub

    Private Sub button_S_poison_spear_click(sender As Object, e As RoutedEventArgs) Handles button_S_poison_spear.Click
        S_Show("毒矛", "Poison Spear", "S_poison_spear", "SW", 0, 1, 0, "S_spear", "×1", "G_venom_gland", "×1", "", "", 1, "", "和长矛的攻击力相当，中毒的敌人攻速移速将会变慢，攻击力降低，然而收获的食材保鲜度将下降为原本的一半。")
    End Sub

    Private Sub button_S_ham_bat_click(sender As Object, e As RoutedEventArgs) Handles button_S_ham_bat.Click
        S_Show("火腿球棒", "Ham Bat", "S_ham_bat", "NoDLC", 1, 1, 1, "G_pig_skin", "×1", "G_twigs", "×2", "G_meat", "×2", 2, "", "需要两个大肉，不够长矛来得实在，不过攻击力还可以。攻击力随着新鲜度而变化，最高59.5，然后逐渐下降到30，最后变成腐烂食物。")
    End Sub

    Private Sub button_S_morning_star_click(sender As Object, e As RoutedEventArgs) Handles button_S_morning_star.Click
        S_Show("晨星", "Morning Star", "S_morning_star", "ROG", 1, 0, 1, "G_volt_goat_horn", "×1", "S_electrical_doodad", "×2", "G_nitre", "×2", 2, "", "晨星拿在手上持续掉耐久。攻击干燥生物伤害为43.35，攻击潮湿生物伤害为72.25，拿着会发光。攻击电羊会帮其充电，杀死获得电羊奶。")
    End Sub

    Private Sub button_S_tail_o_three_cats_click(sender As Object, e As RoutedEventArgs) Handles button_S_tail_o_three_cats.Click
        S_Show("三尾猫的教诲", "Tail o' Three Cats", "S_tail_o'_three_cats", "DST", 0, 0, 1, "G_coontail", "×3", "G_tentacle_spots", "×3", "", "", 2, "", "中距离攻击武器，攻击力27，攻击生物有可能产生小火花或大火花，如果打出大火花会平息生物对你的仇恨(例如牛)。")
    End Sub

    Private Sub button_S_grass_suit_click(sender As Object, e As RoutedEventArgs) Handles button_S_grass_suit.Click
        S_Show("草地盔甲", "Grass Suit", "S_grass_suit", "ROG", 1, 0, 1, "G_cut_grass", "×10", "G_twigs", "×2", "", "", 0, "", "前期的防具，穿了还不如不穿，防御很低，耐久度也不高，只是素材要求比较低而已，实用性很差。")
    End Sub

    Private Sub button_S_log_suit_click(sender As Object, e As RoutedEventArgs) Handles button_S_log_suit.Click
        S_Show("木制盔甲", "Log Suit", "S_log_suit", "NoDLC", 1, 1, 1, "G_log", "×8", "S_rope", "×2", "", "", 1, "", "虽然防御不是最高的，但是配合猪皮足球头盔使用你就能单挑巨鹿了，素材也不是很难找，万金油防具。")
    End Sub

    Private Sub button_S_marble_suit_click(sender As Object, e As RoutedEventArgs) Handles button_S_marble_suit.Click
        S_Show("大理石盔甲", "Marble Suit", "S_marble_suit", "ROG", 1, 0, 1, "G_marble", "×12", "S_rope", "×4", "", "", 2, "", "接近于无敌的防具，但是素材比较难找，大理石比较稀少，采完了只能等洞穴地震了，而且会减速30%。")
    End Sub

    Private Sub button_S_seashell_suit_click(sender As Object, e As RoutedEventArgs) Handles button_S_seashell_suit.Click
        S_Show("海贝盔甲", "Seashell Suit", "S_seashell_suit", "SW", 0, 1, 0, "G_seashell", "×10", "F_seaweed", "×2", "S_rope", "×1", 1, "", "吸收大部分的伤害，防止被毒蛇、毒蚊、蜘蛛战士攻击时中毒。")
    End Sub

    Private Sub button_S_limestone_suit_click(sender As Object, e As RoutedEventArgs) Handles button_S_limestone_suit.Click
        S_Show("石灰石盔甲", "Limestone Suit", "S_limestone_suit", "SW", 0, 1, 0, "S_limestone", "×3", "S_rope", "×2", "", "", 2, "", "吸收大量伤害，每分钟回复2点精神，但是会减少10%的速度。")
    End Sub

    Private Sub button_S_cactus_armour_click(sender As Object, e As RoutedEventArgs) Handles button_S_cactus_armour.Click
        S_Show("象仙人掌盔甲", "Cactus Armour", "S_cactus_armour", "SW", 0, 1, 0, "G_cactus_spike", "×3", "S_log_suit", "×1", "", "", 2, "", "防御力很高，每次受到攻击都会反弹17点伤害，免疫火。")
    End Sub

    Private Sub button_S_football_helmet_click(sender As Object, e As RoutedEventArgs) Handles button_S_football_helmet.Click
        S_Show("猪皮足球头盔", "Football Helmet", "S_football_helmet", "NoDLC", 1, 1, 1, "G_pig_skin", "×1", "S_rope", "×1", "", "", 2, "", "防御优异的帽子，素材也容易寻找，可惜需要炼金机器才能解锁，配合其他防具让你拥有极高的防御。")
    End Sub

    Private Sub button_S_horned_helmet_click(sender As Object, e As RoutedEventArgs) Handles button_S_horned_helmet.Click
        S_Show("角状头盔", "Horned Helmet", "S_horned_helmet", "SW", 0, 1, 0, "G_horn", "×1", "G_seashell", "×4", "S_rope", "×1", 2, "", "防御力极高，有一定防水功效，还能防止被毒蛇、毒蚊、蜘蛛战士攻击时中毒。")
    End Sub

    Private Sub button_S_blow_dart_click(sender As Object, e As RoutedEventArgs) Handles button_S_blow_dart.Click
        S_Show("吹箭", "Blow Dart", "S_blow_dart", "NoDLC", 1, 1, 1, "G_cut_reeds", "×2", "G_hound's_tooth", "×1", "G_azure_feather", "×1", 1, "", "一次性武器，伤害最高且远程的武器，吃饱的大力士可以一箭秒掉海象。")
    End Sub

    Private Sub button_S_sleep_dart_click(sender As Object, e As RoutedEventArgs) Handles button_S_sleep_dart.Click
        S_Show("麻醉吹箭", "Sleep Dart", "S_sleep_dart", "NoDLC", 1, 1, 1, "G_cut_reeds", "×2", "G_stinger", "×1", "G_jet_feather", "×1", 1, "", "一次性武器，远程催眠怪物，如果怪物体型较大，你可能要使用多次才能成功催眠。")
    End Sub

    Private Sub button_S_fire_dart_click(sender As Object, e As RoutedEventArgs) Handles button_S_fire_dart.Click
        S_Show("燃烧吹箭", "Fire Dart", "S_fire_dart", "NoDLC", 1, 1, 1, "G_cut_reeds", "×2", "G_charcoal", "×1", "G_crimson_feather", "×1", 1, "", "一次性武器，远程点燃并伤害怪物，怪物被烧死后掉落的肉类都是被烤熟的，其他战利品变成灰烬。")
    End Sub

    Private Sub button_S_poison_dart_click(sender As Object, e As RoutedEventArgs) Handles button_S_poison_dart.Click
        S_Show("毒镖", "Poison Dart", "S_poison_dart", "SW", 0, 1, 0, "G_cut_reeds", "×2", "G_venom_gland", "×1", "G_jet_feather", "×1", 1, "", "一次性武器，在两分钟内每十秒毒两点血，共造成二十六点伤害。如果生物被毒死食材会变得不新鲜。")
    End Sub

    Private Sub button_S_boomerang_click(sender As Object, e As RoutedEventArgs) Handles button_S_boomerang.Click
        S_Show("回旋镖", "Boomerang", "S_boomerang", "NoDLC", 1, 1, 1, "S_boards", "×2", "G_silk", "×1", "G_charcoal", "×1", 2, "", "比较普通的远程武器，飞出并返回，须按空格接住，否则伤到自己并损失2倍耐久度。")
    End Sub

    Private Sub button_S_bee_mine_click(sender As Object, e As RoutedEventArgs) Handles button_S_bee_mine.Click
        S_Show("蜜蜂地雷", "Bee Mine", "S_bee_mine", "NoDLC", 1, 1, 1, "S_boards", "×2", "A_bee", "×4", "G_flint", "×1", 1, "", "敌人踩上去会爆炸，出现4只蜜蜂进行攻击，如果周围有蜂箱蜂巢其他蜜蜂也会协同攻击。")
    End Sub

    Private Sub button_S_tooth_trap_click(sender As Object, e As RoutedEventArgs) Handles button_S_tooth_trap.Click
        S_Show("狗牙陷阱", "Tooth Trap", "S_tooth_trap", "NoDLC", 1, 1, 1, "G_log", "×1", "S_rope", "×1", "G_hound's_tooth", "×1", 2, "", "伤害是长矛的2倍，数量多时会非常恐怖，基地简直固若金汤，冬天刷海象父子就能获得大量狗牙。")
    End Sub

    Private Sub button_S_scalemail_click(sender As Object, e As RoutedEventArgs) Handles button_S_scalemail.Click
        S_Show("鳞甲", "Scalemail", "S_scalemail", "ROG", 1, 0, 1, "G_scales", "×1", "S_log_suit", "×1", "G_pig_skin", "×3", 2, "", "吸收70%物理伤害，非战斗状态每分钟恢复3.3精神，同时免疫火伤害，而敌人攻击玩家时会被引燃。")
    End Sub

    Private Sub button_S_weather_pain_click(sender As Object, e As RoutedEventArgs) Handles button_S_weather_pain.Click
        S_Show("旋风", "Weather Pain", "S_weather_pain", "ROG", 1, 0, 1, "G_down_feather", "×10", "G_volt_goat_horn", "×1", "G_gears", "×1", 2, "", "威力超强的远程武器，可以破坏与之接触的物体，使用时对目标发射一股旋风，并在施放点来回弹跳几次。总伤害大约140。")
    End Sub

    Private Sub button_S_coconade_click(sender As Object, e As RoutedEventArgs) Handles button_S_coconade.Click
        S_Show("椰壳炸弹", "Coconade", "S_coconade", "SW", 0, 1, 0, "F_coconut", "×1", "S_gunpowder", "×1", "S_rope", "×1", 1, "", "可以放在地上点燃又可以点燃后丢出去的炸弹，造成250点伤害。")
    End Sub

    Private Sub button_S_spear_gun_click(sender As Object, e As RoutedEventArgs) Handles button_S_spear_gun.Click
        S_Show("矛枪", "Spear Gun", "S_spear_gun", "SW", 0, 1, 0, "G_bamboo_patch", "×3", "A_jellyfish", "×1", "", "", 1, "", "可以装载长矛、战斗长矛、毒矛、黑曜石矛，射出去造成三倍伤害。射出的矛需重新拾起填装。一个矛枪可以使用八次。")
    End Sub

    Private Sub button_S_cutlass_supreme_click(sender As Object, e As RoutedEventArgs) Handles button_S_cutlass_supreme.Click
        S_Show("旗鱼短剑", "Cutlass Supreme", "S_cutlass_supreme", "SW", 0, 1, 0, "G_dead_swordfish", "×1", "S_gold_nugget", "×2", "G_twigs", "×1", 2, "", "拥有高额的伤害，对豹卷风还能造成1.5倍伤害加成。攻击力虽高可是需要旗鱼。")
    End Sub

    REM ------------------科技(建筑)-------------------
    Private Sub button_S_spider_eggs_click(sender As Object, e As RoutedEventArgs) Handles button_S_spider_eggs.Click
        S_Show("蜘蛛卵", "Spider Eggs", "S_spider_eggs", "DST", 0, 0, 1, "G_silk", "×12", "G_spider_gland", "×6", "S_papyrus", "×6", 0, "韦伯", "韦伯出生自带的蜘蛛卵，需要的材料实在有点多，而且并没有什么用。")
    End Sub

    Private Sub button_S_chest_click(sender As Object, e As RoutedEventArgs) Handles button_S_chest.Click
        S_Show("宝箱", "Chest", "S_chest", "NoDLC", 1, 1, 1, "S_boards", "×3", "", "", "", "", 1, "", "拥有3×3格的储物箱子。")
    End Sub

    Private Sub button_S_sign_click(sender As Object, e As RoutedEventArgs) Handles button_S_sign.Click
        S_Show("路标牌", "Sign", "S_sign", "NoDLC", 1, 1, 1, "S_boards", "×1", "", "", "", "", 1, "", "放置后会显示在小地图上，可以标记一些重要地点。同时也是fast travel MOD的载体。")
    End Sub

    Private Sub button_S_directional_sign_click(sender As Object, e As RoutedEventArgs) Handles button_S_directional_sign.Click
        S_Show("路标", "Directional Sign", "S_directional_sign", "DST", 0, 0, 1, "S_boards", "×1", "", "", "", "", 1, "", "建造的时候切换视角可以改变指向的方向。")
    End Sub

    Private Sub button_S_hay_wall_click(sender As Object, e As RoutedEventArgs) Handles button_S_hay_wall.Click
        S_Show("干草墙", "Hay Wall", "S_hay_wall", "NoDLC", 1, 1, 1, "G_cut_grass", "×4", "G_twigs", "×2", "", "", 1, "", "可放置围墙，每份50生命，可叠加放置，最高100，生命太少没什么用，一般起装饰作用或者防火鸡什么的。")
    End Sub

    Private Sub button_S_wood_wall_click(sender As Object, e As RoutedEventArgs) Handles button_S_wood_wall.Click
        S_Show("木墙", "Wood Wall", "S_wood_wall", "NoDLC", 1, 1, 1, "S_boards", "×2", "S_rope", "×1", "", "", 1, "", "可放置围墙，每份100生命，可叠加放置，最高200，生命一般，素材好找，防野狗、巨鹿都是不错的选择。")
    End Sub

    Private Sub button_S_stone_wall_click(sender As Object, e As RoutedEventArgs) Handles button_S_stone_wall.Click
        S_Show("石墙", "Stone Wall", "S_stone_wall", "NoDLC", 1, 1, 1, "S_cut_stone", "×2", "", "", "", "", 2, "", "可放置围墙，每份200生命，可叠加放置，最高400，和前两种墙相比，生命值挺高，且不能被点燃。")
    End Sub

    Private Sub button_S_limestone_wall_click(sender As Object, e As RoutedEventArgs) Handles button_S_limestone_wall.Click
        S_Show("石灰石墙", "Limestone Wall", "S_limestone_wall", "SW", 0, 1, 0, "S_limestone", "×2", "", "", "", "", 2, "", "可放置围墙，每份250生命，可叠加放置，最高500，珊瑚还算比较多，有时间的话可以造一些。")
    End Sub

    Private Sub button_S_moon_rock_wall_click(sender As Object, e As RoutedEventArgs) Handles button_S_moon_rock_wall.Click
        S_Show("月岩墙", "Moon Rock Wall", "S_moon_rock_wall", "DST", 0, 0, 1, "G_moon_rock", "×12", "", "", "", "", 2, "", "可放置围墙，每份300生命，可叠加放置，最高600，由于其材料难得，一般不建造这种墙。")
    End Sub

    Private Sub button_S_wardrobe_click(sender As Object, e As RoutedEventArgs) Handles button_S_wardrobe.Click
        S_Show("衣柜", "Wardrobe", "S_wardrobe", "DST", 0, 0, 1, "S_boards", "×4", "S_rope", "×3", "", "", 2, "", "顾名思义，换衣服用的啦！")
    End Sub

    Private Sub button_S_pig_house_click(sender As Object, e As RoutedEventArgs) Handles button_S_pig_house.Click
        S_Show("猪舍", "Pig House", "S_pig_house", "ROG", 1, 0, 1, "S_boards", "×4", "S_cut_stone", "×3", "G_pig_skin", "×4", 2, "", "建造一个猪舍，和野外的一样，合理运用地理位置，将会使自己基地多了一群守卫。")
    End Sub

    Private Sub button_S_rabbit_hutch_click(sender As Object, e As RoutedEventArgs) Handles button_S_rabbit_hutch.Click
        S_Show("兔子窝", "Rabbit Hutch", "S_rabbit_hutch", "ROG", 1, 0, 1, "S_boards", "×4", "G_carrot", "×10", "G_bunny_puff", "×4", 2, "", "建造一个兔子窝，和野外的一样，合理运用地理位置，将会使自己基地多了一群守卫。")
    End Sub

    Private Sub button_S_wildbore_house_click(sender As Object, e As RoutedEventArgs) Handles button_S_wildbore_house.Click
        S_Show("荒野小屋", "Wildbore House", "S_wildbore_house", "SW", 0, 1, 0, "G_bamboo_patch", "×8", "G_palm_leaf", "5", "G_pig_skin", "×4", 2, "", "建造一个荒野小屋，类似猪舍，不过当你拆荒野小屋的时候野猪会试图保护它。")
    End Sub

    Private Sub button_S_prime_ape_hut_click(sender As Object, e As RoutedEventArgs) Handles button_S_prime_ape_hut.Click
        S_Show("猿猴小屋", "Prime Ape Hut", "S_prime_ape_hut", "SW", 0, 1, 0, "G_twigs", "×10", "F_banana", "×3", "G_manure", "×4", 2, "", "建造一个猿猴小屋，最多产生四只猿。猿被击杀后新的猿将会在短时间内重生。")
    End Sub

    Private Sub button_S_dragoon_den_click(sender As Object, e As RoutedEventArgs) Handles button_S_dragoon_den.Click
        S_Show("龙人巢", "Dragoon Den", "S_dragoon_den", "SW", 0, 1, 0, "G_dragoon_heart", "×1", "G_rocks", "×5", "G_obsidian", "×4", 2, "", "建造一个龙人巢，产生3到4只龙人。不能用锤子摧毁，只能用火药或椰壳炸弹炸毁。")
    End Sub

    Private Sub button_S_birdcage_click(sender As Object, e As RoutedEventArgs) Handles button_S_birdcage.Click
        S_Show("鸟笼", "Birdcage", "S_birdcage", "NoDLC", 1, 1, 1, "S_papyrus", "×2", "S_gold_nugget", "×6", "F_seeds", "×2", 2, "", "可以把捕捉到的小鸟放置在鸟笼内，可以用作物与其兑换种子，用肉类兑换鸟蛋。")
    End Sub

    Private Sub button_S_cobblestones_click(sender As Object, e As RoutedEventArgs) Handles button_S_cobblestones.Click
        If checkBox_S_DLC_SW.IsChecked = False Then
            S_Show("卵石路", "Cobblestones", "S_cobblestones", "NoDLC", 1, 1, 1, "G_rocky_turf", "×1", "S_boards", "×1", "", "", 2, "", "可放置地皮，主角和其他生物在卵石路上行走，都会增加50%移动速度，尽早为自己基地铺上吧。")
        Else
            S_Show("卵石路", "Cobblestones", "S_cobblestones", "NoDLC", 1, 1, 1, "G_magma_turf", "×1", "S_boards", "×1", "", "", 2, "", "可放置地皮，主角和其他生物在卵石路上行走，都会增加50%移动速度，尽早为自己基地铺上吧。")
        End If
    End Sub

    Private Sub button_S_wooden_flooring_click(sender As Object, e As RoutedEventArgs) Handles button_S_wooden_flooring.Click
        S_Show("木质地板", "Wooden Flooring", "S_wooden_flooring", "NoDLC", 1, 1, 1, "S_boards", "×1", "", "", "", "", 2, "", "可放置地皮，仅有装饰性。")
    End Sub

    Private Sub button_S_checkered_flooring_click(sender As Object, e As RoutedEventArgs) Handles button_S_checkered_flooring.Click
        S_Show("方格地板", "Checkered Flooring", "S_checkered_flooring", "ROG", 1, 0, 1, "G_marble", "×1", "", "", "", "", 2, "", "可放置地皮，仅有装饰性。")
    End Sub

    Private Sub button_S_carpeted_flooring_click(sender As Object, e As RoutedEventArgs) Handles button_S_carpeted_flooring.Click
        S_Show("地毯地板", "Carpeted Flooring", "S_carpeted_flooring", "ROG", 1, 0, 1, "S_boards", "×1", "G_beefalo_wool", "×1", "", "", 2, "", "可放置地皮，仅有装饰性。")
    End Sub

    Private Sub button_S_scaled_flooring_click(sender As Object, e As RoutedEventArgs) Handles button_S_scaled_flooring.Click
        S_Show("龙鳞地板", "Scaled Flooring", "S_scaled_flooring", "DST", 0, 0, 1, "G_scales", "×1", "S_cut_stone", "×2", "", "", 2, "", "可放置地皮，能阻止火势的蔓延。材料极其难得。")
    End Sub

    Private Sub button_S_snakeskin_rug_click(sender As Object, e As RoutedEventArgs) Handles button_S_snakeskin_rug.Click
        S_Show("蛇皮地毯", "Snakeskin Rug", "S_snakeskin_rug", "SW", 0, 1, 0, "G_snakeskin", "×2", "S_cloth", "×1", "", "", 2, "", "可放置地皮，不会产生水坑，然而无法阻止潮汐和洪水。")
    End Sub

    Private Sub button_S_potted_fern_click(sender As Object, e As RoutedEventArgs) Handles button_S_potted_fern.Click
        S_Show("盆栽的蕨类植物", "Potted Fern", "S_potted_fern", "ROG", 1, 0, 1, "G_foliage", "×5", "G_broken_shell", "×1", "", "", 2, "", "仅有装饰作用，且其材料仅有洞穴或风滚草才能获得。")
    End Sub

    Private Sub button_S_scaled_chest_click(sender As Object, e As RoutedEventArgs) Handles button_S_scaled_chest.Click
        S_Show("鳞片箱子", "Scaled Chest", "S_scaled_chest", "ROG", 1, 0, 1, "G_scales", "×1", "S_boards", "×4", "S_gold_nugget", "×10", 2, "", "拥有3×4格的储物箱子，可以防火。材料极其难得。")
    End Sub

    Private Sub button_S_sandbag_click(sender As Object, e As RoutedEventArgs) Handles button_S_sandbag.Click
        S_Show("沙袋", "SandBag", "S_sandbag", "SW", 0, 1, 0, "S_cloth", "×2", "G_sand", "×3", "", "", 1, "", "阻止洪水、填充水坑就靠它！每份200点生命。")
    End Sub

    Private Sub button_S_sand_castle_click(sender As Object, e As RoutedEventArgs) Handles button_S_sand_castle.Click
        S_Show("沙雕", "Sand Castle", "S_sand_castle", "SW", 0, 1, 0, "G_sand", "×4", "G_palm_leaf", "×2", "G_seashell", "×3", 0, "", "沙雕嘛，当然只能建在沙滩上啦！站在周围会增加精神，只会持续一天。一天结束后会变成沙子、贝壳和椰树叶各一个。")
    End Sub

    REM ------------------科技(合成)-------------------
    Private Sub button_S_rope_click(sender As Object, e As RoutedEventArgs) Handles button_S_rope.Click
        S_Show("绳索", "Rope", "S_rope", "NoDLC", 1, 1, 1, "G_cut_grass", "×3", "", "", "", "", 1, "", "高级素材，可进一步合成更多道具装备。")
    End Sub

    Private Sub button_S_boards_click(sender As Object, e As RoutedEventArgs) Handles button_S_boards.Click
        S_Show("木板", "Boards", "S_boards", "NoDLC", 1, 1, 1, "G_log", "×4", "", "", "", "", 1, "", "高级素材，可进一步合成更多道具，或者建造建筑。")
    End Sub

    Private Sub button_S_cut_stone_click(sender As Object, e As RoutedEventArgs) Handles button_S_cut_stone.Click
        S_Show("石砖块", "Cut Stone", "S_cut_stone", "NoDLC", 1, 1, 1, "G_rocks", "×3", "", "", "", "", 1, "", "高级素材，一些建造需要用到石砖块。")
    End Sub

    Private Sub button_S_papyrus_click(sender As Object, e As RoutedEventArgs) Handles button_S_papyrus.Click
        S_Show("莎草纸", "Papyrus", "S_papyrus", "NoDLC", 1, 1, 1, "G_cut_reeds", "×4", "", "", "", "", 1, "", "高级素材，可进一步合成其他道具。维克波顿制作书籍必备素材。")
    End Sub

    Private Sub button_S_thick_fur_click(sender As Object, e As RoutedEventArgs) Handles button_S_thick_fur.Click
        S_Show("厚皮毛", "Thick Fur", "S_thick_fur", "DST", 0, 0, 1, "G_fur_tuft", "×90", "", "", "", "", 2, "", "高级素材，用来制作熊皮背心，一次制造三个。")
    End Sub

    Private Sub button_S_cloth_click(sender As Object, e As RoutedEventArgs) Handles button_S_cloth.Click
        S_Show("布", "Cloth", "S_cloth", "SW", 0, 1, 0, "G_bamboo_patch", "×3", "", "", "", "", 1, "", "高级素材，可以做一些衣物。")
    End Sub

    Private Sub button_S_limestone_click(sender As Object, e As RoutedEventArgs) Handles button_S_limestone.Click
        S_Show("石灰石", "Limestone", "S_limestone", "SW", 0, 1, 0, "G_coral", "×3", "", "", "", "", 1, "", "高级素材，一些建筑要用到石灰石。")
    End Sub

    Private Sub button_S_gold_nugget_click(sender As Object, e As RoutedEventArgs) Handles button_S_gold_nugget.Click
        S_Show("金块", "Gold Nugget", "S_gold_nugget", "SW", 0, 1, 0, "G_dubloons", "×3", "", "", "", "", 1, "", "船难中金块不多，可以用金币合成一些。")
    End Sub

    Private Sub button_S_ice_click(sender As Object, e As RoutedEventArgs) Handles button_S_ice.Click
        S_Show("冰", "Ice", "S_ice", "SW", 0, 1, 0, "G_hail", "×4", "", "", "", "", 2, "", "船难中的冰比较缺少，可以用冰雹合成。")
    End Sub

    Private Sub button_S_empty_bottle_click(sender As Object, e As RoutedEventArgs) Handles button_S_empty_bottle.Click
        S_Show("空瓶子", "Empty Bottle", "S_empty_bottle", "SW", 0, 1, 0, "G_sand", "×3", "", "", "", "", 2, "", "高级素材，可以制作灯等。")
    End Sub

    Private Sub button_S_nightmare_fuel_click(sender As Object, e As RoutedEventArgs) Handles button_S_nightmare_fuel.Click
        S_Show("噩梦燃料", "Nightmare Fuel", "S_nightmare_fuel", "NoDLC", 1, 1, 1, "G_dark_petals", "×3", "", "", "", "", 3, "", "高级素材，可以用来制作梦魇装备，或者为暗影灯添加燃料。")
    End Sub

    Private Sub button_S_purple_gem_click(sender As Object, e As RoutedEventArgs) Handles button_S_purple_gem.Click
        S_Show("紫宝石", "Purple Gem", "S_purple_gem", "NoDLC", 1, 1, 1, "G_red_gem", "×1", "G_blue_gem", "×1", "", "", 3, "", "高级素材，可以用来合成暗影操纵仪等道具。")
    End Sub

    REM ------------------科技(魔法)-------------------
    Private Sub button_S_abigails_flower_click(sender As Object, e As RoutedEventArgs) Handles button_S_abigails_flower.Click
        S_Show("Abigail之花", "Abigail's Flower", "S_abigail's_flower", "DST", 0, 0, 1, "G_petals", "×6", "G_nightmare_fuel", "×1", "", "", 0, "温蒂", "温蒂出生自带的Abigail之花，当花开时放在地上并杀死一只生物(暗影怪不算)可召唤妹妹阿比盖尔。")
    End Sub

    Private Sub button_S_prestihatitator_click(sender As Object, e As RoutedEventArgs) Handles button_S_prestihatitator.Click
        S_Show("魔法帽子", "Prestihatitator", "S_prestihatitator", "ROG", 1, 0, 1, "A_rabbit", "×4", "S_boards", "×4", "S_top_hat", "×1", 1, "", "靠近后为玩家解锁一级魔法，制作成功后，当前魔法将永久解锁。")
    End Sub

    Private Sub button_S_prestihatitator_SW_click(sender As Object, e As RoutedEventArgs) Handles button_S_prestihatitator_SW.Click
        S_Show("灵子分解器", "Prestihatitator(SW)", "S_prestihatitator_SW", "SW", 0, 1, 0, "G_parrot", "×1", "S_boards", "×4", "G_pirate_hat", "×1", 1, "", "靠近后为玩家解锁一级魔法，制作成功后，当前魔法将永久解锁。")
    End Sub

    Private Sub button_S_shadow_manipulator_click(sender As Object, e As RoutedEventArgs) Handles button_S_shadow_manipulator.Click
        S_Show("暗影操纵仪", "Shadow Manipulator", "S_shadow_manipulator", "NoDLC", 1, 1, 1, "G_living_log", "×3", "G_purple_gem", "×1", "G_nightmare_fuel", "×7", 3, "", "靠近后为玩家解锁二级魔法，制作成功后，当前魔法将永久解锁。")
    End Sub

    Private Sub button_S_meat_effigy_click(sender As Object, e As RoutedEventArgs) Handles button_S_meat_effigy.Click
        If checkBox_S_DLC_DST.IsChecked = False Then
            S_Show("肉块雕像", "Meat Effigy", "S_meat_effigy", "NoDLC", 1, 1, 1, "S_boards", "×4", "G_beard_hair", "×4", "F_cooked_meat", "×4", 3, "", "死亡后点击右上角 激活雕像 即可在此复活，每造一个都会减少40生命上限，一般造一个在家里就好了。")
        Else
            S_Show("肉块雕像", "Meat Effigy", "S_meat_effigy", "NoDLC", 1, 1, 1, "S_boards", "×4", "G_beard_hair", "×4", "G_health", "-40", 3, "", "死亡后点击右上角 激活雕像 即可在此复活，每造一个都会减少40生命上限，一般造一个在家里就好了。")
        End If
    End Sub

    Private Sub button_S_pan_flute_click(sender As Object, e As RoutedEventArgs) Handles button_S_pan_flute.Click
        S_Show("排箫", "Pan Flute", "S_pan_flute", "NoDLC", 1, 1, 1, "G_cut_reeds", "×5", "F_mandrake", "×1", "S_rope", "×1", 3, "", "能让周围所有生物睡着的神器，由于制作材料需要曼德拉草，一般还是捡到的比较多。")
    End Sub

    Private Sub button_S_dripple_pipes_click(sender As Object, e As RoutedEventArgs) Handles button_S_dripple_pipes.Click
        S_Show("滴水的长管", "Dripple Pipes", "S_dripple_pipes", "SW", 0, 1, 0, "G_horn", "×1", "G_nightmare_fuel", "×2", "S_rope", "×1", 8, "", "冬天使用会下雪，其他时候使用会下小雨。改变天气的奇妙物品。")
    End Sub

    Private Sub button_S_old_bell_click(sender As Object, e As RoutedEventArgs) Handles button_S_old_bell.Click
        S_Show("旧钟", "Old Bell", "S_old_bell", "ROG", 1, 0, 0, "G_glommer's_wings", "×1", "G_glommer's_flower", "×1", "", "", 0, "", "蓝图需用鹤嘴锄砸格罗姆雕像获得。使用后过几秒会有一个长满鳞片的大脚，能砍伐树木、挖矿等，造成一千点伤害。在洞穴使用则会引发地震。")
        SL_TextBlock_ScienceDeblocking_Science.Visibility = Visibility.Visible
        SL_TextBlock_ScienceDeblocking_Science.Text = "蓝图解锁"
        SL_TextBlock_ScienceDeblocking_Science.Foreground = Brushes.CadetBlue
        SL_TextBlock_ScienceDeblocking_Science.UpdateLayout()
        SL_TextBlock_ScienceDeblocking_Science.SetValue(Canvas.LeftProperty, CDbl((Canvas_ScienceLeft.ActualWidth - SL_TextBlock_ScienceDeblocking_Science.ActualWidth) / 2))
    End Sub

    Private Sub button_S_one_man_band_click(sender As Object, e As RoutedEventArgs) Handles button_S_one_man_band.Click
        S_Show("独奏乐器", "One-man Band", "S_one-man_band", "NoDLC", 1, 1, 1, "S_gold_nugget", "×2", "G_nightmare_fuel", "×4", "G_pig_skin", "×2", 3, "", "牺牲精神来吸引猪人，也是没什么用，同时吸引十几个猪人能让你精神瞬间见底。")
    End Sub

    Private Sub button_S_night_light_click(sender As Object, e As RoutedEventArgs) Handles button_S_night_light.Click
        S_Show("暗夜照明灯", "Night Light", "S_night_light", "NoDLC", 1, 1, 1, "S_gold_nugget", "×8", "G_nightmare_fuel", "×2", "G_red_gem", "×1", 3, "", "添加噩梦燃料便会燃烧，一个就能烧很久，但是太靠近暗夜照明灯会掉精神，因此这货并不是很好用。")
    End Sub

    Private Sub button_S_night_armour_click(sender As Object, e As RoutedEventArgs) Handles button_S_night_armour.Click
        S_Show("暗影之甲", "Night Armour", "S_night_armour", "NoDLC", 1, 1, 1, "G_nightmare_fuel", "×5", "S_papyrus", "×3", "", "", 4, "", "防御力极高，但是会快速消耗掉你的精神，加上噩梦燃料不太好刷，装备的实用性就不是很高了。")
    End Sub

    Private Sub button_S_dark_sword_click(sender As Object, e As RoutedEventArgs) Handles button_S_dark_sword.Click
        S_Show("暗夜剑", "Dark Sword", "S_dark_sword", "NoDLC", 1, 1, 1, "G_nightmare_fuel", "×5", "G_living_log", "×1", "", "", 4, "", "装备后会快速失去精神，但是其伤害可是杠杠的。")
    End Sub

    Private Sub button_S_bat_bat_click(sender As Object, e As RoutedEventArgs) Handles button_S_bat_bat.Click
        S_Show("蝙蝠斧棍", "Bat Bat", "S_bat_bat", "ROG", 1, 0, 1, "F_batilisk_wing", "×5", "G_living_log", "×2", "G_purple_gem", "×1", 4, "", "虽然攻击会失去精神，但是能吸6.8生命，防具给力时这把武器完全能让你硬顶着压力打怪。")
    End Sub

    Private Sub button_S_belt_of_hunger_click(sender As Object, e As RoutedEventArgs) Handles button_S_belt_of_hunger.Click
        S_Show("饥饿腰带", "Belt of Hunger", "S_belt_of_hunger", "ROG", 1, 0, 1, "G_slurper_pelt", "×6", "S_rope", "×2", "G_nightmare_fuel", "×2", 4, "", "由缀食者之皮制作而成，具有神秘力量，玩家饥饿速度将变成默认的60%。")
    End Sub

    Private Sub button_S_life_giving_amulet_click(sender As Object, e As RoutedEventArgs) Handles button_S_life_giving_amulet.Click
        S_Show("提神护符", "Life Giving Amulet", "S_life_giving_amulet", "NoDLC", 1, 1, 1, "S_gold_nugget", "×3", "G_nightmare_fuel", "×2", "G_red_gem", "×1", 3, "", "它除了死后复活之外也能让玩家缓慢地把饥饿转化为生命，同时精神也能缓慢恢复。")
    End Sub

    Private Sub button_S_chilled_amulet_click(sender As Object, e As RoutedEventArgs) Handles button_S_chilled_amulet.Click
        S_Show("寒冰护符", "Chilled Amulet", "S_chilled_amulet", "NoDLC", 1, 1, 1, "S_gold_nugget", "×3", "G_blue_gem", "×1", "", "", 3, "", "持续恢复少量精神，任何试图攻击你的生物都会受到冰冻效果，同时你自己也会被其寒气所影响。")
    End Sub

    Private Sub button_S_nightmare_amulet_click(sender As Object, e As RoutedEventArgs) Handles button_S_nightmare_amulet.Click
        S_Show("梦魇护符", "Nightmare Amulet", "S_nightmare_amulet", "NoDLC", 1, 1, 1, "S_gold_nugget", "×6", "G_nightmare_fuel", "×4", "G_purple_gem", "×2", 4, "", "穿上后会立刻陷入噩梦状态(精神为0)，每分钟掉3.33精神，取下后会恢复到实际精神值。过方尖碑变得很容易。")
    End Sub

    Private Sub button_S_fire_staff_click(sender As Object, e As RoutedEventArgs) Handles button_S_fire_staff.Click
        S_Show("火魔杖", "Fire Staff", "S_fire_staff", "NoDLC", 1, 1, 1, "G_nightmare_fuel", "×2", "S_spear", "×1", "G_red_gem", "×1", 4, "", "允许玩家远程点燃怪物，以及某些生物巢穴，可以用来焚烧胡蜂巢和蜘蛛巢，烧皮弗娄牛效果也不错。")
    End Sub

    Private Sub button_S_ice_staff_click(sender As Object, e As RoutedEventArgs) Handles button_S_ice_staff.Click
        S_Show("冰魔杖", "Ice Staff", "S_ice_staff", "NoDLC", 1, 1, 1, "S_spear", "×1", "G_blue_gem", "×1", "", "", 3, "", "可以远程冻结怪物，怪物体型越大就需要越多的攻击来冻结。冰杖没有伤害，攻击冻结怪物会解冻。")
    End Sub

    Private Sub button_S_telelocator_staff_click(sender As Object, e As RoutedEventArgs) Handles button_S_telelocator_staff.Click
        S_Show("传送权杖", "Telelocator Staff", "S_telelocator_staff", "NoDLC", 1, 1, 1, "G_nightmare_fuel", "×4", "G_living_log", "×2", "G_purple_gem", "×2", 4, "", "装备后右键即可传送，会到达随机位置，如果有已激活的传送核心则传送到传送核心。每次使用都会降低50精神。")
    End Sub

    Private Sub button_S_telelocator_focus_click(sender As Object, e As RoutedEventArgs) Handles button_S_telelocator_focus.Click
        S_Show("传送核心", "Telelocator Focus", "S_telelocator_focus", "NoDLC", 1, 1, 1, "G_nightmare_fuel", "×4", "G_living_log", "×4", "S_gold_nugget", "×8", 4, "", "放置三枚紫宝石后开始工作，使用传送权杖的时候就会固定传送到该位置，由于成本太高，不太常用。")
    End Sub

    Private Sub button_S_seaworthy_click(sender As Object, e As RoutedEventArgs) Handles button_S_seaworthy.Click
        S_Show("海之船送", "Seaworthy", "S_Seaworthy", "ROG", 1, 0, 0, "G_nightmare_fuel", "×4", "G_living_log", "×4", "T_sea_worther", "×1", 4, "", "从船难版通过海之船送穿越后，魔法科技就会多出一个海之船送，用以穿越回船难。其中沃尔特海需要用锤子砸掉沉船获得。")
    End Sub

    REM ------------------科技(衣物)-------------------
    Private Sub button_S_sewing_kit_click(sender As Object, e As RoutedEventArgs) Handles button_S_sewing_kit.Click
        S_Show("缝纫工具包", "Sewing Kit", "S_sewing_kit", "NoDLC", 1, 1, 1, "G_log", "×1", "G_silk", "×8", "G_hound's_tooth", "×2", 2, "", "可以修复非防御性的衣服和帽子，由于不同的装备耐久度最大值不一样，修复的百分比也不会相同。")
    End Sub

    Private Sub button_S_garland_click(sender As Object, e As RoutedEventArgs) Handles button_S_garland.Click
        S_Show("花环", "Garland", "S_garland", "NoDLC", 1, 1, 1, "G_petals", "×12", "", "", "", "", 0, "", "前期就能制作，只能稍微恢复一下精神，用处不大。")
    End Sub

    Private Sub button_S_straw_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_straw_hat.Click
        S_Show("草帽", "Straw Hat", "S_straw_hat", "NoDLC", 1, 1, 1, "G_cut_grass", "×12", "", "", "", "", 0, "", "没什么用的帽子，带上去也不能让你变成路飞。它能用来进一步制作矿工帽或丛林帽。")
    End Sub

    Private Sub button_S_top_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_top_hat.Click
        S_Show("高礼帽", "Top Hat", "S_top_hat", "NoDLC", 1, 1, 1, "G_silk", "×6", "", "", "", "", 1, "", "前期可以稍微用来恢复点精神，但是太慢了，不如采花睡觉来得实在，因此它很少人用。")
    End Sub

    Private Sub button_S_rain_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_rain_hat.Click
        S_Show("雨帽", "Rain Hat", "S_rain_hat", "ROG", 1, 0, 1, "F_moleworm", "×2", "S_straw_hat", "×1", "G_boneshard", "×1", 2, "", "比较容易制作，不过挡雨效果不佳，配上雨衣更佳哦！")
    End Sub

    Private Sub button_S_rabbit_earmuffs_click(sender As Object, e As RoutedEventArgs) Handles button_S_rabbit_earmuffs.Click
        S_Show("兔毛耳套", "Rabbit Earmuffs", "S_rabbit_earmuffs", "ROG", 1, 0, 1, "A_rabbit", "×2", "G_twigs", "×1", "", "", 1, "", "做这个帽子要抓两只活的小兔兔，由于制作手法极其残忍，属性又不高，所以没有什么人用。")
    End Sub

    Private Sub button_S_beefalo_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_beefalo_hat.Click
        S_Show("牛帽", "Beefalo Hat", "S_beefalo_hat", "ROG", 1, 0, 1, "G_beefalo_wool", "×8", "G_beefalo_horn", "×1", "", "", 1, "", "这个厉害了，牛群发情期能保护你不受伤害。重要的是它在帽子中保暖系数是最高的。")
    End Sub

    Private Sub button_S_winter_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_winter_hat.Click
        S_Show("寒冬帽", "Winter Hat", "S_winter_hat", "ROG", 1, 0, 1, "G_beefalo_wool", "×4", "G_silk", "×4", "", "", 2, "", "很多人喜欢用它过冬，难道只是因为它样子好看，保暖系数高吗？其实也不高，牛帽那才叫高。")
    End Sub

    Private Sub button_S_cat_cap_click(sender As Object, e As RoutedEventArgs) Handles button_S_cat_cap.Click
        S_Show("猫帽", "Cat Cap", "S_cat_cap", "ROG", 1, 0, 1, "G_coontail", "×4", "G_silk", "×4", "", "", 2, "", "属性和寒冬帽基本相同，就是制作材料不是牛毛而是猫尾。")
    End Sub

    Private Sub button_S_fashion_melon_click(sender As Object, e As RoutedEventArgs) Handles button_S_fashion_melon.Click
        S_Show("时尚西瓜帽", "Fashion Melon", "S_fashion_melon", "NoDLC", 1, 1, 1, "F_watermelon", "×1", "G_twigs", "×3", "", "", 1, "", "夏天可以稍微防止高温，不过腐烂速度实在是有点快。")
    End Sub

    Private Sub button_S_ice_cube_click(sender As Object, e As RoutedEventArgs) Handles button_S_ice_cube.Click
        S_Show("冰块", "Ice Cube", "S_ice_cube", "NoDLC", 1, 1, 1, "S_electrical_doodad", "×2", "S_rope", "×4", "F_ice", "×10", 2, "", "制作材料倒是一点都不困难，只要提前在冰箱里准备好冰块就行了。不过带上后湿度会增加，速度也会降低10%，比较头痛。")
    End Sub

    Private Sub button_S_brain_of_thought_click(sender As Object, e As RoutedEventArgs) Handles button_S_brain_of_thought.Click
        S_Show("智慧帽", "Brain of Thought", "S_brain_of_thought", "SW", 0, 1, 0, "F_brainy_matter", "×1", "A_jellyfish", "×1", "S_rope", "×2", 2, "", "谁最聪明吖？戴上它可以制作四次任何可合成的物品而不需要科技，但是不会解锁该物品的配方。")
    End Sub

    Private Sub button_S_shark_tooth_crown_click(sender As Object, e As RoutedEventArgs) Handles button_S_shark_tooth_crown.Click
        S_Show("鲨齿王冠", "Shark Tooth Crown", "S_shark_tooth_crown", "SW", 0, 1, 0, "G_hound's_tooth", "×5", "S_gold_nugget", "×1", "", "", 1, "", "在船上戴上它每分钟恢复6.6精神。金灿灿的牙齿高贵身份显露无疑。")
    End Sub

    Private Sub button_S_beekeeper_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_beekeeper_hat.Click
        S_Show("养蜂人的帽子", "Beekeeper Hat", "S_beekeeper_hat", "NoDLC", 1, 1, 1, "G_silk", "×8", "S_rope", "×1", "", "", 2, "", "只能防御蜜蜂攻击，和猪皮足球头盔防御系数一样，由于1个猪皮不比8个蜘蛛网难找，所以不太常用。")
    End Sub

    Private Sub button_S_feather_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_feather_hat.Click
        S_Show("羽毛帽", "Feather Hat", "S_feather_hat", "NoDLC", 1, 1, 1, "G_jet_feather", "×3", "G_crimson_feather", "×2", "G_tentacle_spots", "×2", 2, "", "恢复少量精神，能吸引更多的鸟着陆，然而靠近鸟还是会飞走。")
    End Sub

    Private Sub button_S_bush_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_bush_hat.Click
        S_Show("丛林帽", "Bush Hat", "S_bush_hat", "NoDLC", 1, 1, 1, "S_straw_hat", "×1", "G_crimson_feather", "×1", "G_berry_bush", "×1", 2, "", "右键变成浆果丛隐藏自己，对已经发现你的怪物没有作用，除了卖萌毫无用处的帽子，感觉在掩耳盗铃。")
    End Sub

    Private Sub button_S_snakeskin_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_snakeskin_hat.Click
        S_Show("蛇鳞帽", "Snakeskin Hat", "S_snakeskin_hat", "SW", 0, 1, 0, "G_snakeskin", "×1", "S_straw_hat", "×1", "G_boneshard", "×1", 2, "", "防止70%的湿度增加，还能防雷防水母触电，简直就是戴在头上的避雷针！")
    End Sub

    Private Sub button_S_snakeskin_jacket_click(sender As Object, e As RoutedEventArgs) Handles button_S_snakeskin_jacket.Click
        S_Show("蛇鳞上衣", "Snakeskin Jacket", "S_snakeskin_jacket", "SW", 0, 1, 0, "G_snakeskin", "×2", "G_vine", "×2", "G_boneshard", "×2", 1, "", "和蛇鳞帽效果一样，另外还具有较差的保暖功能，和兔毛耳套差不多。")
    End Sub

    Private Sub button_S_rain_coat_click(sender As Object, e As RoutedEventArgs) Handles button_S_rain_coat.Click
        S_Show("雨衣", "Rain Coat", "S_rain_coat", "ROG", 1, 0, 1, "G_tentacle_spots", "×2", "S_rope", "×2", "G_boneshard", "×2", 1, "", "超级防雨防雷神器，绝佳！")
    End Sub

    Private Sub button_S_blubber_suit_click(sender As Object, e As RoutedEventArgs) Handles button_S_blubber_suit.Click
        S_Show("鲸脂套装", "Blubber Suit", "S_blubber_suit", "SW", 0, 1, 0, "F_blubber", "×4", "S_cloth", "×2", "G_palm_leaf", "×2", 2, "", "完全免疫水和雨！保暖功效和牛帽一样！")
    End Sub

    Private Sub button_S_dapper_vest_click(sender As Object, e As RoutedEventArgs) Handles button_S_dapper_vest.Click
        S_Show("小巧背心", "Dapper Vest", "S_dapper_vest", "ROG", 1, 0, 1, "G_hound's_tooth", "×8", "G_silk", "×6", "", "", 2, "", "看上去很华丽，但是精神加成和保暖系数都不算很高，而且耐久度也比较低。")
    End Sub

    Private Sub button_S_breezy_vest_click(sender As Object, e As RoutedEventArgs) Handles button_S_breezy_vest.Click
        S_Show("夏日背心", "Breezy Vest", "S_breezy_vest", "ROG", 1, 0, 1, "G_koalefant_trunk", "×1", "G_silk", "×8", "", "", 2, "", "素材要求不高，同时属性也比较中庸，如果我有一个象鼻肯定烤来吃爽快一点。注意虽然叫夏日背心但却是冬天保暖用的。")
    End Sub

    Private Sub button_S_puffy_vest_click(sender As Object, e As RoutedEventArgs) Handles button_S_puffy_vest.Click
        S_Show("寒冬背心", "Puffy Vest", "S_puffy_vest", "ROG", 1, 0, 1, "G_winter_koalefant_trunk", "×1", "G_silk", "×8", "G_beefalo_wool", "×2", 2, "", "保暖效果比较优异，开了mod的话就不用担心装备栏不够的问题了，穿上它吧！")
    End Sub

    Private Sub button_S_summer_frest_click(sender As Object, e As RoutedEventArgs) Handles button_S_summer_frest.Click
        S_Show("夏季背心", "Summer Frest", "S_summer_frest", "NoDLC", 1, 1, 1, "S_rope", "×1", "G_crimson_feather", "×3", "G_pig_skin", "×2", 1, "", "夏天可以凉快一些，属性略低于花纹衬衫。")
    End Sub

    Private Sub button_S_floral_shirt_click(sender As Object, e As RoutedEventArgs) Handles button_S_floral_shirt.Click
        If checkBox_S_DLC_ROG.IsChecked = False And checkBox_S_DLC_DST.IsChecked = False Then
            S_Show("花纹衬衫", "Floral Shirt", "S_floral_shirt", "NoDLC", 1, 1, 1, "S_papyrus", "×3", "G_silk", "×3", "G_petals", "×5", 2, "", "夏天用来减缓温度上升的衣服，虽然效果不大，总比带负面效果的冰块要好一点吧。不能用缝纫工具包修补。在船难版里制作材料仙人掌花改为花瓣。")
        Else
            S_Show("花纹衬衫", "Floral Shirt", "S_floral_shirt", "NoDLC", 1, 1, 1, "S_papyrus", "×3", "G_silk", "×3", "F_cactus_flower", "×5", 2, "", "夏天用来减缓温度上升的衣服，虽然效果不大，总比带负面效果的冰块要好一点吧。不能用缝纫工具包修补。在船难版里制作材料仙人掌花改为花瓣。")
        End If
    End Sub

    Private Sub button_S_walking_cane_click(sender As Object, e As RoutedEventArgs) Handles button_S_walking_cane.Click
        S_Show("步行手杖", "Walking Cane", "S_walking_cane", "ROG", 1, 0, 1, "S_gold_nugget", "×2", "G_walrus_tusk", "×1", "G_twigs", "×4", 2, "", "装备后增加25%移动速度。也可以拿来攻击，虽然伤害只有17，但是没有耐久度不必担心它会坏。")
    End Sub

    Private Sub button_S_hibearnation_vest_click(sender As Object, e As RoutedEventArgs) Handles button_S_hibearnation_vest.Click
        S_Show("熊皮背心", "Hibearnation Vest", "S_hibearnation_vest", "ROG", 1, 0, 1, "S_thick_fur", "×1", "S_dapper_vest", "×1", "S_rope", "×2", 2, "", "保暖效果在衣服里是最好的，而且每分钟回复4.5精神，减缓25%的饥饿速度。但是制作材料厚皮毛需要打死秋季BOSS比尔格才能得到，而且和牛帽效果不叠加，故用处不大。")
    End Sub

    Private Sub button_S_eyebrella_click(sender As Object, e As RoutedEventArgs) Handles button_S_eyebrella.Click
        S_Show("眼球伞", "Eyebrella", "S_eyebrella", "ROG", 1, 0, 1, "F_deerclops_eyeball", "×1", "G_twigs", "×15", "G_boneshard", "×4", 2, "", "防雨神器！而且不会占用武器栏，十分好用，制作材料也只需要在第一个冬天打死独眼巨鹿就可以获得。")
    End Sub

    Private Sub button_S_dumbrella_click(sender As Object, e As RoutedEventArgs) Handles button_S_dumbrella.Click
        S_Show("双层伞帽", "Dumbrella", "S_dumbrella", "SW", 0, 1, 0, "G_shark_gills", "×2", "S_umbrella", "×1", "S_straw_hat", "×1", 2, "", "像眼球伞一样防水防雷还延缓过热，可以空出双手来工作，非常方便。")
    End Sub

    Private Sub button_S_windbreaker_click(sender As Object, e As RoutedEventArgs) Handles button_S_windbreaker.Click
        S_Show("风衣", "Windbreaker", "S_windbreaker", "SW", 0, 1, 0, "F_blubber", "×2", "S_cloth", "×1", "S_rope", "×1", 2, "", "20%防水，并且防止被大风吹跑，穿上后还会减小被豹卷风卷影响的半径。")
    End Sub

    Private Sub button_S_particulate_purifier_click(sender As Object, e As RoutedEventArgs) Handles button_S_particulate_purifier.Click
        S_Show("粉尘净化器", "Particulate Purifier", "S_particulate_purifier", "SW", 0, 1, 0, "S_empty_bottle", "×2", "G_coral", "×3", "A_jellyfish", "×1", 2, "", "戴上它就可以免疫毒洞和恶臭魔鬼鱼的毒！机器人戴上它大概是为了臭美一下吧？")
    End Sub

    Private Sub button_S_sleek_hat_click(sender As Object, e As RoutedEventArgs) Handles button_S_sleek_hat.Click
        S_Show("星芒头盔", "Sleek Hat", "S_sleek_hat", "SW", 0, 1, 0, "G_shark_fin", "×1", "G_vine", "×2", "F_coconut", "×1", 2, "", "增加25%的移动速度，和步行手杖一样。还能抵御些许风！")
    End Sub

    REM ------------------科技(远古)-------------------
    Private Sub button_S_thulecite_click(sender As Object, e As RoutedEventArgs) Handles button_S_thulecite.Click
        S_Show("铥矿", "Thulecite", "S_thulecite", "ROG", 1, 0, 1, "G_thulecite_fragments", "×6", "", "", "", "", 5, "", "制作大部分远古物品的基础材料，可在洞穴远古遗迹附近获得。")
    End Sub

    Private Sub button_S_thulecite_wall_click(sender As Object, e As RoutedEventArgs) Handles button_S_thulecite_wall.Click
        S_Show("铥矿墙壁", "Thulecite Wall", "S_thulecite_wall", "ROG", 1, 0, 1, "S_thulecite", "×6", "", "", "", "", 5, "", "可放置围墙，每份400生命，可叠加放置，最高800，生命最多的墙，素材难得，用锤子击破只能获得铥矿石碎片。")
    End Sub

    Private Sub button_S_thulecite_medallion_click(sender As Object, e As RoutedEventArgs) Handles button_S_thulecite_medallion.Click
        S_Show("铥矿奖章", "Thulecite Medallion", "S_thulecite_medallion", "ROG", 1, 0, 1, "S_thulecite", "×2", "G_nightmare_fuel", "×2", "", "", 5, "", "用于检查古代力量的徽章，颜色发红就代表力量觉醒，和指南针一样是同类别的废物。")
    End Sub

    Private Sub button_S_the_lazy_forager_click(sender As Object, e As RoutedEventArgs) Handles button_S_the_lazy_forager.Click
        S_Show("懒惰的强盗", "The Lazy Forager", "S_the_lazy_forager", "ROG", 1, 0, 1, "S_thulecite", "×2", "G_nightmare_fuel", "×3", "G_orange_gem", "×1", 6, "", "自动拾取地上的物品，并不是瞬间拾取，有一点点间隔，由于有使用次数，一般情况就别用了。")
    End Sub

    Private Sub button_S_magiluminescence_click(sender As Object, e As RoutedEventArgs) Handles button_S_magiluminescence.Click
        S_Show("魔力之光", "Magiluminescence", "S_magiluminescence", "ROG", 1, 0, 1, "S_thulecite", "×2", "G_nightmare_fuel", "×3", "G_yellow_gem", "×1", 6, "", "装备后会发光，类似矿工帽，解放双手和帽子。")
    End Sub

    Private Sub button_S_construction_amulet_click(sender As Object, e As RoutedEventArgs) Handles button_S_construction_amulet.Click
        S_Show("建造护符", "Construction Amulet", "S_construction_amulet", "ROG", 1, 0, 1, "S_thulecite", "×2", "G_nightmare_fuel", "×3", "G_green_gem", "×1", 5, "", "装备后建设和制作需要的材料会减半，只能使用5次，自己斟酌着使用。")
    End Sub

    Private Sub button_S_the_lazy_explorer_click(sender As Object, e As RoutedEventArgs) Handles button_S_the_lazy_explorer.Click
        S_Show("懒惰的探索者", "The Lazy Explorer", "S_the_lazy_explorer", "ROG", 1, 0, 1, "G_nightmare_fuel", "×2", "S_walking_cane", "×1", "G_orange_gem", "×2", 6, "", "使用后会向目标瞬移，平常只会增加25%的移动速度，但是被怪物包围，或者被河道或者障碍阻挡道路，就好用了。每次使用消耗15点精神。")
    End Sub

    Private Sub button_S_star_callers_staff_click(sender As Object, e As RoutedEventArgs) Handles button_S_star_callers_staff.Click
        S_Show("星星呼唤者权杖", "Star Caller's Staff", "S_star_caller's_staff", "ROG", 1, 0, 1, "G_nightmare_fuel", "×4", "G_living_log", "×2", "G_yellow_gem", "×2", 5, "", "向目标发射光球，持续照明一段时间，可以用来烹调食物。其实不太实用，施展需要吟唱时间，光球也无法移动。每次使用消耗20点精神。")
    End Sub

    Private Sub button_S_deconstruction_staff_click(sender As Object, e As RoutedEventArgs) Handles button_S_deconstruction_staff.Click
        S_Show("毁灭权杖", "Deconstruction Staff", "S_deconstruction_staff", "ROG", 1, 0, 1, "G_nightmare_fuel", "×4", "G_living_log", "×2", "G_green_gem", "×2", 5, "", "可以分解道具和建筑，但不会分解出宝石，如果被分解对象有折损，就无法获得100%原素材。每次使用消耗20点精神。")
    End Sub

    Private Sub button_S_pickaxe_1_click(sender As Object, e As RoutedEventArgs) Handles button_S_pickaxe_1.Click
        S_Show("摘/斧头", "Pick/Axe", "S_pickaxe_1", "ROG", 1, 0, 1, "S_goldenaxe", "×1", "S_goldenpickaxe", "×1", "S_thulecite", "×2", 6, "", "金斧头和黄金鹤嘴锄的结合体，又能伐木又能采矿，攻击伤害和大部分农具一样，因此有点华而不实。")
    End Sub

    Private Sub button_S_thulecite_crown_click(sender As Object, e As RoutedEventArgs) Handles button_S_thulecite_crown.Click
        S_Show("铥矿石皇冠", "Thulecite Crown", "S_thulecite_crown", "ROG", 1, 0, 1, "S_thulecite", "×4", "G_nightmare_fuel", "×4", "", "", 6, "", "吸收90%的物理伤害，当被击中时有1/3几率创造一个持续4秒免疫火、雷电的力场，cd9秒。")
    End Sub

    Private Sub button_S_thulecite_suit_click(sender As Object, e As RoutedEventArgs) Handles button_S_thulecite_suit.Click
        S_Show("铥矿甲胄", "Thulecite Suit", "S_thulecite_suit", "ROG", 1, 0, 1, "S_thulecite", "×6", "G_nightmare_fuel", "×4", "", "", 6, "", "吸收90%的伤害并且每分钟恢复3.3精神。就是制作素材有点多，果断装备建设护符后制作。")
    End Sub

    Private Sub button_S_thulecite_club_click(sender As Object, e As RoutedEventArgs) Handles button_S_thulecite_club.Click
        S_Show("铥矿棒", "Thulecite Club", "S_thulecite_club", "ROG", 1, 0, 1, "G_living_log", "×3", "S_thulecite", "×4", "G_nightmare_fuel", "×4", 6, "", "攻击和耐久都非常优异，攻击有概率产出魂触手助攻，但是铥矿石比较宝贵，要合理使用。")
    End Sub

    Private Sub button_S_houndius_shootius_click(sender As Object, e As RoutedEventArgs) Handles button_S_houndius_shootius.Click
        S_Show("恒迪尤斯·舒提尤斯", "Houndius Shootius", "S_houndius_shootius", "ROG", 1, 0, 1, "F_deerclops_eyeball", "×1", "F_guardian's_horn", "×1", "S_thulecite", "×5", 6, "", "允许玩家部署恒迪尤斯·舒提尤斯炮台，它将是一个强大的防御型建筑，拥有1000点血量和65点攻击，每秒会自动恢复12点生命。靠得很近的时候会减少精神。")
    End Sub

    REM ------------------科技(书)-------------------
    Private Sub button_S_birds_of_the_world_click(sender As Object, e As RoutedEventArgs) Handles button_S_birds_of_the_world.Click
        S_Show("世界上的鸟", "Birds Of The World", "S_birds_of_the_world", "NoDLC", 1, 1, 1, "S_papyrus", "×2", "G_egg", "×2", "", "", 0, "维克波顿", "维克波顿周围将出现大量的鸟类，阅读书籍将会降低50精神，维克波顿专有，可使用5次。")
    End Sub

    Private Sub button_S_applied_horticulture_click(sender As Object, e As RoutedEventArgs) Handles button_S_applied_horticulture.Click
        S_Show("应用园艺学", "Applied Horticulture", "S_applied_horticulture", "NoDLC", 1, 1, 1, "S_papyrus", "×2", "F_seeds", "×1", "G_manure", "×1", 0, "维克波顿", "让周围的植物迅速生长(需先施肥，冬天只对农场有效)，阅读书籍将会降低33精神，维克波顿专有，可使用5次。")
    End Sub

    Private Sub button_S_sleepytime_stories_click(sender As Object, e As RoutedEventArgs) Handles button_S_sleepytime_stories.Click
        S_Show("睡前的故事", "Sleepytime Stories", "S_sleepytime_stories", "NoDLC", 1, 1, 1, "S_papyrus", "×2", "G_nightmare_fuel", "×2", "", "", 3, "维克波顿", "让周围生物进入梦乡，阅读书籍将会降低33精神，维克波顿专有，可使用5次。")
    End Sub

    Private Sub button_S_the_end_is_nigh_click(sender As Object, e As RoutedEventArgs) Handles button_S_the_end_is_nigh.Click
        S_Show("快要结束了！", "The End Is Nigh!", "S_the_end_is_nigh!", "NoDLC", 1, 1, 1, "S_papyrus", "×2", "G_red_gem", "×1", "", "", 4, "维克波顿", "维克波顿周围将出现大量闪电，阅读书籍将会降低33精神，维克波顿专有，可使用5次。")
    End Sub

    Private Sub button_S_on_tentacles_click(sender As Object, e As RoutedEventArgs) Handles button_S_on_tentacles.Click
        S_Show("在触手上", "On Tentacles", "S_on_tentacles", "ROG", 1, 0, 1, "S_papyrus", "×2", "G_tentacle_spots", "×1", "", "", 2, "维克波顿", "维克波顿周围将出现3只触手，阅读书籍将会降低50精神，维克波顿专有，可使用5次。")
    End Sub

    Private Sub button_S_joy_of_volcanology_click(sender As Object, e As RoutedEventArgs) Handles button_S_joy_of_volcanology.Click
        S_Show("欢乐的火山", "Joy Of Volcanology", "S_joy_of_volcanology", "SW", 0, 1, 0, "S_papyrus", "×2", "G_obsidian", "×2", "", "", 3, "维克波顿", "在玩家位置落下四个龙人蛋(不会保存完整)。阅读书籍不会降低精神，维克波顿专有，可使用5次。")
    End Sub

    REM ------------------科技(暗影巫术)-------------------
    Private Sub button_S_codex_umbra_click(sender As Object, e As RoutedEventArgs) Handles button_S_codex_umbra.Click
        S_Show("暗影秘典", "Codex Umbra", "S_codex_umbra", "DST", 0, 0, 1, "S_papyrus", "×2", "G_nightmare_fuel", "×2", "G_health", "-50", 0, "麦克斯韦", "麦克斯韦专属秘典！把它放到地上并且站在其周围(书呈翻开状)才能解锁暗影科技。")
    End Sub

    Private Sub button_S_shadow_logger_click(sender As Object, e As RoutedEventArgs) Handles button_S_shadow_logger.Click
        S_Show("暗影伐木工", "Shadow Logger", "S_shadow_logger", "DST", 0, 0, 1, "G_nightmare_fuel", "×2", "S_axe", "×1", "G_sanity", "-20%", 7, "麦克斯韦", "麦克斯韦耗费20%(即40点)的精神(上限)召唤出暗影伐木工，跟随走动，会自动搜寻周围树木砍伐。拥有1点生命值。")
    End Sub

    Private Sub button_S_shadow_miner_click(sender As Object, e As RoutedEventArgs) Handles button_S_shadow_miner.Click
        S_Show("暗影矿工", "Shadow Miner", "S_shadow_miner", "DST", 0, 0, 1, "G_nightmare_fuel", "×2", "S_pickaxe", "×1", "G_sanity", "-20%", 7, "麦克斯韦", "麦克斯韦耗费20%(即40点)的精神(上限)召唤出暗影矿工，跟随走动，会自动搜寻周围石矿开采。拥有1点生命值。")
    End Sub

    Private Sub button_S_shadow_digger_click(sender As Object, e As RoutedEventArgs) Handles button_S_shadow_digger.Click
        S_Show("暗影挖掘者", "Shadow Digger", "S_shadow_digger", "DST", 0, 0, 1, "G_nightmare_fuel", "×2", "S_shovel", "×1", "G_sanity", "-20%", 7, "麦克斯韦", "麦克斯韦耗费20%(即40点)的精神(上限)召唤出暗影挖掘者，跟随走动，会自动搜寻周围树桩挖掘(不会挖浆果灌木丛等)。拥有1点的生命值。")
    End Sub

    Private Sub button_S_shadow_duelist_click(sender As Object, e As RoutedEventArgs) Handles button_S_shadow_duelist.Click
        S_Show("暗影斗士", "Shadow Duelist", "S_shadow_duelist", "DST", 0, 0, 1, "G_nightmare_fuel", "×2", "S_spear", "×1", "G_sanity", "-35%", 7, "麦克斯韦", "麦克斯韦耗费35%(即70点)的精神(上限)召唤出暗影伐斗士，跟随走动，会自动搜寻周围生物猎杀(40攻击力)。拥有100点生命值。")
    End Sub

    REM ------------------科技(火山)-------------------
    Private Sub button_S_obsidian_machete_click(sender As Object, e As RoutedEventArgs) Handles button_S_obsidian_machete.Click
        S_Show("黑曜石砍刀", "Obsidian Machete", "S_obsidian_machete", "SW", 0, 1, 0, "S_machete", "×1", "G_obsidian", "×3", "G_dragoon_heart", "×1", 9, "", "挥砍更有效率。拥有29点攻击力，如果充能能打出连击。除了风季，会逐渐充能，能量和第二击伤害(最高29)成正比。过度充能会导致攻击的目标燃烧。充能还会发光发热，会导致过热。")
    End Sub

    Private Sub button_S_obsidian_axe_click(sender As Object, e As RoutedEventArgs) Handles button_S_obsidian_axe.Click
        S_Show("黑曜石斧", "Obsidian Axe", "S_obsidian_axe", "SW", 0, 1, 0, "S_axe", "×1", "G_obsidian", "×2", "G_dragoon_heart", "×1", 9, "", "砍树效率是普通斧头的2.5倍。和黑曜石砍刀一样会充能。")
    End Sub

    Private Sub button_S_obsidian_spear_click(sender As Object, e As RoutedEventArgs) Handles button_S_obsidian_spear.Click
        S_Show("黑曜石矛", "Obsidian Spear", "S_obsidian_spear", "SW", 0, 1, 0, "S_spear", "×1", "G_obsidian", "×3", "G_dragoon_heart", "×1", 9, "", "拥有51点攻击力，充满能后攻击将会达到102点。")
    End Sub

    Private Sub button_S_volcano_staff_click(sender As Object, e As RoutedEventArgs) Handles button_S_volcano_staff.Click
        S_Show("熔岩法杖", "Volcano Staff", "S_volcano_staff", "SW", 0, 1, 0, "S_fire_staff", "×1", "G_obsidian", "×4", "G_dragoon_heart", "×1", 9, "", "施放符咒会导致下龙人蛋雨，不同于天然的火山喷发，没有机会保留完整的龙人蛋。")
    End Sub

    Private Sub button_S_obsidian_armour_click(sender As Object, e As RoutedEventArgs) Handles button_S_obsidian_armour.Click
        S_Show("黑曜石盔甲", "Obsidian Armour", "S_obsidian_armour", "SW", 0, 1, 0, "S_log_suit", "×1", "G_obsidian", "×5", "G_dragoon_heart", "×1", 9, "", "类似鳞甲，然而不会恢复精神，而且耐久度也只有鳞甲的四分之三。")
    End Sub

    Private Sub button_S_obsidian_coconade_click(sender As Object, e As RoutedEventArgs) Handles button_S_obsidian_coconade.Click
        S_Show("黑曜石炸弹", "Obsidian Coconade", "S_obsidian_coconade", "SW", 0, 1, 0, "S_coconade", "×3", "G_obsidian", "×3", "G_dragoon_heart", "×1", 9, "", "类似椰壳炸弹，不过威力更大，能造成340点伤害，而且一次制造三个。")
    End Sub

    Private Sub button_S_howling_conch_click(sender As Object, e As RoutedEventArgs) Handles button_S_howling_conch.Click
        S_Show("呼啸的海螺", "Howling Conch", "S_howling_conch", "SW", 0, 1, 0, "G_obsidian", "×4", "S_purple_gem", "×1", "G_magic_seal", "×1", 9, "", "使用后会开始持续半天的强风，如果在巨人的统治里使用则会变成降雨。")
    End Sub

    Private Sub button_S_sail_stick_click(sender As Object, e As RoutedEventArgs) Handles button_S_sail_stick.Click
        S_Show("帆棍", "Sail Stick", "S_sail_stick", "SW", 0, 1, 0, "G_obsidian", "×2", "G_nightmare_fuel", "×3", "G_magic_seal", "×1", 9, "", "改变风向的神器道具，装备着帆棍无论怎么走都是顺风！")
    End Sub

    REM 科技DLC检测初始化
    Private Sub S_DLC_Check_initialization()
        REM 工具
        button_S_glossamer_saddle.Visibility = Visibility.Collapsed
        button_S_saddlehorn.Visibility = Visibility.Collapsed
        button_S_saddle.Visibility = Visibility.Collapsed
        button_S_war_saddle.Visibility = Visibility.Collapsed
        button_S_brush.Visibility = Visibility.Collapsed
        button_S_machete.Visibility = Visibility.Collapsed
        button_S_Luxury_machete.Visibility = Visibility.Collapsed
        button_S_salt_lick.Visibility = Visibility.Collapsed
        REM 照明
        button_S_pumpkin_lantern.Visibility = Visibility.Collapsed
        button_S_lantern.Visibility = Visibility.Collapsed
        button_S_willows_lighter.Visibility = Visibility.Collapsed
        button_S_chiminea.Visibility = Visibility.Collapsed
        button_S_moggles.Visibility = Visibility.Collapsed
        button_S_obsidian_fire_pit.Visibility = Visibility.Collapsed
        button_S_bottle_lantern.Visibility = Visibility.Collapsed
        button_S_boat_torch.Visibility = Visibility.Collapsed
        button_S_boat_lantern.Visibility = Visibility.Collapsed
        REM 航海
        TextBlock_S_nautical.Visibility = Visibility.Collapsed
        WrapPanel_S_nautical.Visibility = Visibility.Collapsed
        REM 生存
        button_S_fur_roll.Visibility = Visibility.Collapsed
        button_S_telltale_heart.Visibility = Visibility.Collapsed
        button_S_booster_shot.Visibility = Visibility.Collapsed
        button_S_bernie.Visibility = Visibility.Collapsed
        button_S_waterballoon.Visibility = Visibility.Collapsed
        button_S_pile_o_balloons.Visibility = Visibility.Collapsed
        button_S_pretty_parasol.Visibility = Visibility.Collapsed
        button_S_whirly_fan.Visibility = Visibility.Collapsed
        button_S_luxury_fan.Visibility = Visibility.Collapsed
        button_S_insulated_pack.Visibility = Visibility.Collapsed
        button_S_chef_pouch.Visibility = Visibility.Collapsed
        button_S_silly_monkey_ball.Visibility = Visibility.Collapsed
        button_S_tropical_parasol.Visibility = Visibility.Collapsed
        button_S_anti_venom.Visibility = Visibility.Collapsed
        button_S_thatch_pack.Visibility = Visibility.Collapsed
        button_S_palm_leaf_hut.Visibility = Visibility.Collapsed
        button_S_tropical_fan.Visibility = Visibility.Collapsed
        button_S_sea_sack.Visibility = Visibility.Collapsed
        button_S_doydoy_nest.Visibility = Visibility.Collapsed
        REM 食物
        button_S_mussel_stick.Visibility = Visibility.Collapsed
        REM 科学
        button_S_ice_maker_3000.Visibility = Visibility.Collapsed
        REM 战斗
        button_S_grass_suit.Visibility = Visibility.Collapsed
        button_S_marble_suit.Visibility = Visibility.Collapsed
        button_S_morning_star.Visibility = Visibility.Collapsed
        button_S_scalemail.Visibility = Visibility.Collapsed
        button_S_weather_pain.Visibility = Visibility.Collapsed
        button_S_tail_o_three_cats.Visibility = Visibility.Collapsed
        button_S_poison_spear.Visibility = Visibility.Collapsed
        button_S_seashell_suit.Visibility = Visibility.Collapsed
        button_S_limestone_suit.Visibility = Visibility.Collapsed
        button_S_cactus_armour.Visibility = Visibility.Collapsed
        button_S_horned_helmet.Visibility = Visibility.Collapsed
        button_S_poison_dart.Visibility = Visibility.Collapsed
        button_S_coconade.Visibility = Visibility.Collapsed
        button_S_spear_gun.Visibility = Visibility.Collapsed
        button_S_cutlass_supreme.Visibility = Visibility.Collapsed
        REM 建筑
        button_S_potted_fern.Visibility = Visibility.Collapsed
        button_S_pig_house.Visibility = Visibility.Collapsed
        button_S_rabbit_hutch.Visibility = Visibility.Collapsed
        button_S_checkered_flooring.Visibility = Visibility.Collapsed
        button_S_carpeted_flooring.Visibility = Visibility.Collapsed
        button_S_scaled_flooring.Visibility = Visibility.Collapsed
        button_S_spider_eggs.Visibility = Visibility.Collapsed
        button_S_directional_sign.Visibility = Visibility.Collapsed
        button_S_moon_rock_wall.Visibility = Visibility.Collapsed
        button_S_wardrobe.Visibility = Visibility.Collapsed
        button_S_scaled_chest.Visibility = Visibility.Collapsed
        button_S_limestone_wall.Visibility = Visibility.Collapsed
        button_S_wildbore_house.Visibility = Visibility.Collapsed
        button_S_prime_ape_hut.Visibility = Visibility.Collapsed
        button_S_dragoon_den.Visibility = Visibility.Collapsed
        button_S_snakeskin_rug.Visibility = Visibility.Collapsed
        button_S_sandbag.Visibility = Visibility.Collapsed
        button_S_sand_castle.Visibility = Visibility.Collapsed
        REM 合成
        button_S_thick_fur.Visibility = Visibility.Collapsed
        button_S_cloth.Visibility = Visibility.Collapsed
        button_S_limestone.Visibility = Visibility.Collapsed
        button_S_gold_nugget.Visibility = Visibility.Collapsed
        button_S_ice.Visibility = Visibility.Collapsed
        button_S_empty_bottle.Visibility = Visibility.Collapsed
        REM 魔法
        button_S_bat_bat.Visibility = Visibility.Collapsed
        button_S_belt_of_hunger.Visibility = Visibility.Collapsed
        button_S_old_bell.Visibility = Visibility.Collapsed
        button_S_prestihatitator.Visibility = Visibility.Collapsed
        button_S_abigails_flower.Visibility = Visibility.Collapsed
        button_S_prestihatitator_SW.Visibility = Visibility.Collapsed
        button_S_dripple_pipes.Visibility = Visibility.Collapsed
        button_S_seaworthy.Visibility = Visibility.Collapsed
        REM 衣物
        button_S_rain_hat.Visibility = Visibility.Collapsed
        button_S_rabbit_earmuffs.Visibility = Visibility.Collapsed
        button_S_beefalo_hat.Visibility = Visibility.Collapsed
        button_S_winter_hat.Visibility = Visibility.Collapsed
        button_S_cat_cap.Visibility = Visibility.Collapsed
        button_S_rain_coat.Visibility = Visibility.Collapsed
        button_S_dapper_vest.Visibility = Visibility.Collapsed
        button_S_breezy_vest.Visibility = Visibility.Collapsed
        button_S_puffy_vest.Visibility = Visibility.Collapsed
        button_S_walking_cane.Visibility = Visibility.Collapsed
        button_S_hibearnation_vest.Visibility = Visibility.Collapsed
        button_S_eyebrella.Visibility = Visibility.Collapsed
        button_S_brain_of_thought.Visibility = Visibility.Collapsed
        button_S_shark_tooth_crown.Visibility = Visibility.Collapsed
        button_S_snakeskin_hat.Visibility = Visibility.Collapsed
        button_S_snakeskin_jacket.Visibility = Visibility.Collapsed
        button_S_blubber_suit.Visibility = Visibility.Collapsed
        button_S_dumbrella.Visibility = Visibility.Collapsed
        button_S_windbreaker.Visibility = Visibility.Collapsed
        button_S_particulate_purifier.Visibility = Visibility.Collapsed
        button_S_sleek_hat.Visibility = Visibility.Collapsed
        REM 书
        button_S_on_tentacles.Visibility = Visibility.Collapsed
        button_S_joy_of_volcanology.Visibility = Visibility.Collapsed
        REM 远古
        TextBlock_S_ancient.Visibility = Visibility.Collapsed
        WrapPanel_S_ancient.Visibility = Visibility.Collapsed
        REM 暗影巫术
        TextBlock_S_shadow.Visibility = Visibility.Collapsed
        WrapPanel_S_shadow.Visibility = Visibility.Collapsed
        REM 火山
        TextBlock_S_volcano.Visibility = Visibility.Collapsed
        WrapPanel_S_volcano.Visibility = Visibility.Collapsed
    End Sub

    Private Sub S_DLC_ROG_SHOW()
        REM 照明
        button_S_pumpkin_lantern.Visibility = Visibility.Visible
        button_S_lantern.Visibility = Visibility.Visible
        button_S_moggles.Visibility = Visibility.Visible
        REM 生存
        button_S_fur_roll.Visibility = Visibility.Visible
        button_S_pretty_parasol.Visibility = Visibility.Visible
        button_S_luxury_fan.Visibility = Visibility.Visible
        button_S_insulated_pack.Visibility = Visibility.Visible
        REM 战斗
        button_S_grass_suit.Visibility = Visibility.Visible
        button_S_marble_suit.Visibility = Visibility.Visible
        button_S_morning_star.Visibility = Visibility.Visible
        button_S_scalemail.Visibility = Visibility.Visible
        button_S_weather_pain.Visibility = Visibility.Visible
        REM 建筑
        button_S_potted_fern.Visibility = Visibility.Visible
        button_S_pig_house.Visibility = Visibility.Visible
        button_S_rabbit_hutch.Visibility = Visibility.Visible
        button_S_checkered_flooring.Visibility = Visibility.Visible
        button_S_carpeted_flooring.Visibility = Visibility.Visible
        button_S_scaled_chest.Visibility = Visibility.Visible
        REM 魔法
        button_S_bat_bat.Visibility = Visibility.Visible
        button_S_belt_of_hunger.Visibility = Visibility.Visible
        button_S_old_bell.Visibility = Visibility.Visible
        button_S_prestihatitator.Visibility = Visibility.Visible
        button_S_seaworthy.Visibility = Visibility.Visible
        REM 衣物
        button_S_rain_hat.Visibility = Visibility.Visible
        button_S_rabbit_earmuffs.Visibility = Visibility.Visible
        button_S_beefalo_hat.Visibility = Visibility.Visible
        button_S_winter_hat.Visibility = Visibility.Visible
        button_S_cat_cap.Visibility = Visibility.Visible
        button_S_rain_coat.Visibility = Visibility.Visible
        button_S_dapper_vest.Visibility = Visibility.Visible
        button_S_breezy_vest.Visibility = Visibility.Visible
        button_S_puffy_vest.Visibility = Visibility.Visible
        button_S_walking_cane.Visibility = Visibility.Visible
        button_S_hibearnation_vest.Visibility = Visibility.Visible
        button_S_eyebrella.Visibility = Visibility.Visible
        REM 书
        button_S_on_tentacles.Visibility = Visibility.Visible
        REM 远古
        TextBlock_S_ancient.Visibility = Visibility.Visible
        WrapPanel_S_ancient.Visibility = Visibility.Visible
    End Sub

    Private Sub S_DLC_SW_SHOW()
        REM 工具
        button_S_machete.Visibility = Visibility.Visible
        button_S_Luxury_machete.Visibility = Visibility.Visible
        REM 照明
        button_S_chiminea.Visibility = Visibility.Visible
        button_S_obsidian_fire_pit.Visibility = Visibility.Visible
        button_S_bottle_lantern.Visibility = Visibility.Visible
        button_S_boat_torch.Visibility = Visibility.Visible
        button_S_boat_lantern.Visibility = Visibility.Visible
        REM 航海
        TextBlock_S_nautical.Visibility = Visibility.Visible
        WrapPanel_S_nautical.Visibility = Visibility.Visible
        REM 生存
        button_S_chef_pouch.Visibility = Visibility.Visible
        button_S_silly_monkey_ball.Visibility = Visibility.Visible
        button_S_tropical_parasol.Visibility = Visibility.Visible
        button_S_anti_venom.Visibility = Visibility.Visible
        button_S_thatch_pack.Visibility = Visibility.Visible
        button_S_palm_leaf_hut.Visibility = Visibility.Visible
        button_S_tropical_fan.Visibility = Visibility.Visible
        button_S_sea_sack.Visibility = Visibility.Visible
        button_S_doydoy_nest.Visibility = Visibility.Visible
        REM 食物
        button_S_mussel_stick.Visibility = Visibility.Visible
        REM 科学
        button_S_ice_maker_3000.Visibility = Visibility.Visible
        REM 战斗
        button_S_poison_spear.Visibility = Visibility.Visible
        button_S_seashell_suit.Visibility = Visibility.Visible
        button_S_limestone_suit.Visibility = Visibility.Visible
        button_S_cactus_armour.Visibility = Visibility.Visible
        button_S_horned_helmet.Visibility = Visibility.Visible
        button_S_poison_dart.Visibility = Visibility.Visible
        button_S_coconade.Visibility = Visibility.Visible
        button_S_spear_gun.Visibility = Visibility.Visible
        button_S_cutlass_supreme.Visibility = Visibility.Visible
        REM 建筑
        button_S_limestone_wall.Visibility = Visibility.Visible
        button_S_wildbore_house.Visibility = Visibility.Visible
        button_S_prime_ape_hut.Visibility = Visibility.Visible
        button_S_dragoon_den.Visibility = Visibility.Visible
        button_S_snakeskin_rug.Visibility = Visibility.Visible
        button_S_sandbag.Visibility = Visibility.Visible
        button_S_sand_castle.Visibility = Visibility.Visible
        REM 合成
        button_S_cloth.Visibility = Visibility.Visible
        button_S_limestone.Visibility = Visibility.Visible
        button_S_gold_nugget.Visibility = Visibility.Visible
        button_S_ice.Visibility = Visibility.Visible
        button_S_empty_bottle.Visibility = Visibility.Visible
        REM 魔法
        button_S_prestihatitator_SW.Visibility = Visibility.Visible
        button_S_dripple_pipes.Visibility = Visibility.Visible
        REM 衣物
        button_S_brain_of_thought.Visibility = Visibility.Visible
        button_S_shark_tooth_crown.Visibility = Visibility.Visible
        button_S_snakeskin_hat.Visibility = Visibility.Visible
        button_S_snakeskin_jacket.Visibility = Visibility.Visible
        button_S_blubber_suit.Visibility = Visibility.Visible
        button_S_dumbrella.Visibility = Visibility.Visible
        button_S_windbreaker.Visibility = Visibility.Visible
        button_S_particulate_purifier.Visibility = Visibility.Visible
        button_S_sleek_hat.Visibility = Visibility.Visible
        REM 书
        button_S_joy_of_volcanology.Visibility = Visibility.Visible
        REM 火山
        TextBlock_S_volcano.Visibility = Visibility.Visible
        WrapPanel_S_volcano.Visibility = Visibility.Visible
    End Sub

    Private Sub S_DLC_DST_SHOW()
        REM 工具
        button_S_glossamer_saddle.Visibility = Visibility.Visible
        button_S_saddlehorn.Visibility = Visibility.Visible
        button_S_saddle.Visibility = Visibility.Visible
        button_S_war_saddle.Visibility = Visibility.Visible
        button_S_brush.Visibility = Visibility.Visible
        button_S_salt_lick.Visibility = Visibility.Visible
        REM 照明
        button_S_pumpkin_lantern.Visibility = Visibility.Visible
        button_S_lantern.Visibility = Visibility.Visible
        button_S_willows_lighter.Visibility = Visibility.Visible
        button_S_moggles.Visibility = Visibility.Visible
        REM 生存
        button_S_fur_roll.Visibility = Visibility.Visible
        button_S_telltale_heart.Visibility = Visibility.Visible
        button_S_booster_shot.Visibility = Visibility.Visible
        button_S_bernie.Visibility = Visibility.Visible
        button_S_waterballoon.Visibility = Visibility.Visible
        button_S_pile_o_balloons.Visibility = Visibility.Visible
        button_S_pretty_parasol.Visibility = Visibility.Visible
        button_S_whirly_fan.Visibility = Visibility.Visible
        button_S_luxury_fan.Visibility = Visibility.Visible
        button_S_insulated_pack.Visibility = Visibility.Visible
        REM 战斗
        button_S_grass_suit.Visibility = Visibility.Visible
        button_S_marble_suit.Visibility = Visibility.Visible
        button_S_morning_star.Visibility = Visibility.Visible
        button_S_scalemail.Visibility = Visibility.Visible
        button_S_weather_pain.Visibility = Visibility.Visible
        button_S_tail_o_three_cats.Visibility = Visibility.Visible
        REM 建筑
        button_S_potted_fern.Visibility = Visibility.Visible
        button_S_pig_house.Visibility = Visibility.Visible
        button_S_rabbit_hutch.Visibility = Visibility.Visible
        button_S_checkered_flooring.Visibility = Visibility.Visible
        button_S_carpeted_flooring.Visibility = Visibility.Visible
        button_S_scaled_flooring.Visibility = Visibility.Visible
        button_S_spider_eggs.Visibility = Visibility.Visible
        button_S_directional_sign.Visibility = Visibility.Visible
        button_S_moon_rock_wall.Visibility = Visibility.Visible
        button_S_wardrobe.Visibility = Visibility.Visible
        button_S_scaled_chest.Visibility = Visibility.Visible
        REM 合成
        button_S_thick_fur.Visibility = Visibility.Visible
        REM 魔法
        button_S_bat_bat.Visibility = Visibility.Visible
        button_S_belt_of_hunger.Visibility = Visibility.Visible
        button_S_prestihatitator.Visibility = Visibility.Visible
        button_S_abigails_flower.Visibility = Visibility.Visible
        REM 衣物
        button_S_rain_hat.Visibility = Visibility.Visible
        button_S_rabbit_earmuffs.Visibility = Visibility.Visible
        button_S_beefalo_hat.Visibility = Visibility.Visible
        button_S_winter_hat.Visibility = Visibility.Visible
        button_S_cat_cap.Visibility = Visibility.Visible
        button_S_rain_coat.Visibility = Visibility.Visible
        button_S_dapper_vest.Visibility = Visibility.Visible
        button_S_breezy_vest.Visibility = Visibility.Visible
        button_S_puffy_vest.Visibility = Visibility.Visible
        button_S_walking_cane.Visibility = Visibility.Visible
        button_S_hibearnation_vest.Visibility = Visibility.Visible
        button_S_eyebrella.Visibility = Visibility.Visible
        REM 书
        button_S_on_tentacles.Visibility = Visibility.Visible
        REM 远古
        TextBlock_S_ancient.Visibility = Visibility.Visible
        WrapPanel_S_ancient.Visibility = Visibility.Visible
        REM 暗影巫术
        TextBlock_S_shadow.Visibility = Visibility.Visible
        WrapPanel_S_shadow.Visibility = Visibility.Visible
    End Sub

    REM 科技DLC检测
    Private Sub S_DLC_Check()

        Dim S_ROG_SW_DST As SByte
        Dim S_ROG__ As SByte
        Dim S_SW__ As SByte
        Dim S_DST__ As SByte
        If checkBox_S_DLC_ROG.IsChecked = True Then
            S_ROG__ = 1
        Else
            S_ROG__ = 0
        End If
        If checkBox_S_DLC_SW.IsChecked = True Then
            S_SW__ = 2
        Else
            S_SW__ = 0
        End If
        If checkBox_S_DLC_DST.IsChecked = True Then
            S_DST__ = 4
        Else
            S_DST__ = 0
        End If
        S_ROG_SW_DST = S_ROG__ + S_SW__ + S_DST__
        If S_ROG_SW_DST = 0 Then
            MsgBox("至少选择一项！")
            checkBox_S_DLC_ROG.IsChecked = True
            S_DLC_Check()
        Else
            S_DLC_Check_initialization()
            Select Case S_ROG_SW_DST
                Case 1
                    S_DLC_ROG_SHOW()
                    WrapPanel_S_Tools.Height = 90
                    WrapPanel_S_light.Height = 90
                    WrapPanel_S_survival.Height = 170
                    WrapPanel_S_fight.Height = 170
                    WrapPanel_S_structures.Height = 170
                    WrapPanel_S_refine.Height = 90
                    WrapPanel_S_dress.Height = 250
                    WrapPanel_S_ancient.Height = 170
                    WrapPanel_Science.Height = 2223.8
                    Reg_Write("Science", 1)
                Case 2
                    S_DLC_SW_SHOW()
                    WrapPanel_S_Tools.Height = 90
                    WrapPanel_S_light.Height = 90
                    WrapPanel_S_nautical.Height = 250
                    WrapPanel_S_survival.Height = 250
                    WrapPanel_S_fight.Height = 170
                    WrapPanel_S_structures.Height = 170
                    WrapPanel_S_refine.Height = 90
                    WrapPanel_S_dress.Height = 170
                    WrapPanel_S_volcano.Height = 90
                    WrapPanel_Science.Height = 2409.2
                    Reg_Write("Science", 2)
                Case 3
                    S_DLC_ROG_SHOW()
                    S_DLC_SW_SHOW()
                    WrapPanel_S_Tools.Height = 90
                    WrapPanel_S_light.Height = 170
                    WrapPanel_S_nautical.Height = 250
                    WrapPanel_S_survival.Height = 250
                    WrapPanel_S_fight.Height = 250
                    WrapPanel_S_structures.Height = 170
                    WrapPanel_S_refine.Height = 90
                    WrapPanel_S_dress.Height = 250
                    WrapPanel_S_ancient.Height = 170
                    WrapPanel_S_volcano.Height = 90
                    WrapPanel_Science.Height = 2889.6
                    Reg_Write("Science", 3)
                Case 4
                    S_DLC_DST_SHOW()
                    WrapPanel_S_Tools.Height = 170
                    WrapPanel_S_light.Height = 90
                    WrapPanel_S_survival.Height = 250
                    WrapPanel_S_fight.Height = 170
                    WrapPanel_S_structures.Height = 170
                    WrapPanel_S_refine.Height = 90
                    WrapPanel_S_dress.Height = 250
                    WrapPanel_S_ancient.Height = 170
                    WrapPanel_S_shadow.Height = 90
                    WrapPanel_Science.Height = 2489.2
                    Reg_Write("Science", 4)
                Case 5
                    S_DLC_ROG_SHOW()
                    S_DLC_DST_SHOW()
                    WrapPanel_S_Tools.Height = 170
                    WrapPanel_S_light.Height = 90
                    WrapPanel_S_survival.Height = 250
                    WrapPanel_S_fight.Height = 170
                    WrapPanel_S_structures.Height = 170
                    WrapPanel_S_refine.Height = 90
                    WrapPanel_S_dress.Height = 250
                    WrapPanel_S_ancient.Height = 170
                    WrapPanel_S_shadow.Height = 90
                    WrapPanel_Science.Height = 2489.2
                    Reg_Write("Science", 5)
                Case 6
                    S_DLC_SW_SHOW()
                    S_DLC_DST_SHOW()
                    WrapPanel_S_Tools.Height = 170
                    WrapPanel_S_light.Height = 170
                    WrapPanel_S_nautical.Height = 250
                    WrapPanel_S_survival.Height = 250
                    WrapPanel_S_fight.Height = 250
                    WrapPanel_S_structures.Height = 250
                    WrapPanel_S_refine.Height = 170
                    WrapPanel_S_dress.Height = 250
                    WrapPanel_S_ancient.Height = 170
                    WrapPanel_S_shadow.Height = 90
                    WrapPanel_S_volcano.Height = 90
                    WrapPanel_Science.Height = 3260
                    Reg_Write("Science", 6)
                Case 7
                    S_DLC_ROG_SHOW()
                    S_DLC_SW_SHOW()
                    S_DLC_DST_SHOW()
                    WrapPanel_S_Tools.Height = 170
                    WrapPanel_S_light.Height = 170
                    WrapPanel_S_nautical.Height = 250
                    WrapPanel_S_survival.Height = 250
                    WrapPanel_S_fight.Height = 250
                    WrapPanel_S_structures.Height = 250
                    WrapPanel_S_refine.Height = 170
                    WrapPanel_S_dress.Height = 250
                    WrapPanel_S_ancient.Height = 170
                    WrapPanel_S_shadow.Height = 90
                    WrapPanel_S_volcano.Height = 90
                    WrapPanel_Science.Height = 3260
                    Reg_Write("Science", 7)
            End Select
        End If
    End Sub
    Private Sub checkBox_S_DLC_ROG_click(sender As Object, e As RoutedEventArgs) Handles checkBox_S_DLC_ROG.Click
        S_DLC_Check()
    End Sub

    Private Sub SL_button_S_DLC_ROG_click(sender As Object, e As RoutedEventArgs) Handles SL_button_S_DLC_ROG.Click
        If checkBox_S_DLC_ROG.IsChecked = True Then
            checkBox_S_DLC_ROG.IsChecked = False
        Else
            checkBox_S_DLC_ROG.IsChecked = True
        End If
        checkBox_S_DLC_ROG_click(Nothing, Nothing)
    End Sub

    Private Sub checkBox_S_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles checkBox_S_DLC_SW.Click
        S_DLC_Check()
    End Sub

    Private Sub SL_button_S_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles SL_button_S_DLC_SW.Click
        If checkBox_S_DLC_SW.IsChecked = True Then
            checkBox_S_DLC_SW.IsChecked = False
        Else
            checkBox_S_DLC_SW.IsChecked = True
        End If
        checkBox_S_DLC_SW_click(Nothing, Nothing)
    End Sub

    Private Sub checkBox_S_DLC_DST_click(sender As Object, e As RoutedEventArgs) Handles checkBox_S_DLC_DST.Click
        S_DLC_Check()
    End Sub

    Private Sub SL_button_S_DLC_DST_click(sender As Object, e As RoutedEventArgs) Handles SL_button_S_DLC_DST.Click
        If checkBox_S_DLC_DST.IsChecked = True Then
            checkBox_S_DLC_DST.IsChecked = False
        Else
            checkBox_S_DLC_DST.IsChecked = True
        End If
        checkBox_S_DLC_DST_click(Nothing, Nothing)
    End Sub

    REM ------------------添加食材按钮-------------------
    REM ------------------食材(肉类)-------------------
    Private Sub button_CS_eel_click(sender As Object, e As RoutedEventArgs) Handles button_CS_eel.Click
        CS_Add("F_eel")
    End Sub

    Private Sub button_CS_cooked_eel_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_eel.Click
        CS_Add("F_cooked_eel")
    End Sub

    Private Sub button_CS_fish_click(sender As Object, e As RoutedEventArgs) Handles button_CS_fish.Click
        CS_Add("F_fish")
    End Sub

    Private Sub button_CS_cooked_fish_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_fish.Click
        CS_Add("F_cooked_fish")
    End Sub

    Private Sub button_CS_frog_legs_click(sender As Object, e As RoutedEventArgs) Handles button_CS_frog_legs.Click
        CS_Add("F_frog_legs")
    End Sub

    Private Sub button_CS_cooked_frog_legs_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_frog_legs.Click
        CS_Add("F_cooked_frog_legs")
    End Sub

    Private Sub button_CS_meat_click(sender As Object, e As RoutedEventArgs) Handles button_CS_meat.Click
        CS_Add("F_meat")
    End Sub

    Private Sub button_CS_cooked_meat_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_meat.Click
        CS_Add("F_cooked_meat")
    End Sub

    Private Sub button_CS_jerky_click(sender As Object, e As RoutedEventArgs) Handles button_CS_jerky.Click
        CS_Add("F_jerky")
    End Sub

    Private Sub button_CS_monster_meat_click(sender As Object, e As RoutedEventArgs) Handles button_CS_monster_meat.Click
        CS_Add("F_monster_meat")
    End Sub

    Private Sub button_CS_cooked_monster_meat_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_monster_meat.Click
        CS_Add("F_cooked_monster_meat")
    End Sub

    Private Sub button_CS_monster_jerky_click(sender As Object, e As RoutedEventArgs) Handles button_CS_monster_jerky.Click
        CS_Add("F_monster_jerky")
    End Sub

    Private Sub button_CS_morsel_click(sender As Object, e As RoutedEventArgs) Handles button_CS_morsel.Click
        CS_Add("F_morsel")
    End Sub

    Private Sub button_CS_cooked_morsel_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_morsel.Click
        CS_Add("F_cooked_morsel")
    End Sub

    Private Sub button_CS_small_jerky_click(sender As Object, e As RoutedEventArgs) Handles button_CS_small_jerky.Click
        CS_Add("F_small_jerky")
    End Sub

    Private Sub button_CS_drumstick_click(sender As Object, e As RoutedEventArgs) Handles button_CS_drumstick.Click
        CS_Add("F_drumstick")
    End Sub

    Private Sub button_CS_fried_drumstick_click(sender As Object, e As RoutedEventArgs) Handles button_CS_fried_drumstick.Click
        CS_Add("F_fried_drumstick")
    End Sub

    Private Sub button_CS_moleworm_click(sender As Object, e As RoutedEventArgs) Handles button_CS_moleworm.Click
        CS_Add("F_moleworm")
    End Sub

    Private Sub button_CS_tropical_fish_click(sender As Object, e As RoutedEventArgs) Handles button_CS_tropical_fish.Click
        CS_Add("F_tropical_fish")
    End Sub

    Private Sub button_CS_fish_morsel_click(sender As Object, e As RoutedEventArgs) Handles button_CS_fish_morsel.Click
        CS_Add("F_fish_morsel")
    End Sub

    Private Sub button_CS_cooked_fish_morsel_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_fish_morsel.Click
        CS_Add("F_cooked_fish_morsel")
    End Sub

    Private Sub button_CS_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_CS_jellyfish.Click
        CS_Add("F_jellyfish")
    End Sub

    Private Sub button_CS_dead_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_CS_dead_jellyfish.Click
        CS_Add("F_dead_jellyfish")
    End Sub

    Private Sub button_CS_cooked_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_jellyfish.Click
        CS_Add("F_cooked_jellyfish")
    End Sub

    Private Sub button_CS_dried_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_CS_dried_jellyfish.Click
        CS_Add("F_dried_jellyfish")
    End Sub

    Private Sub button_CS_limpets_click(sender As Object, e As RoutedEventArgs) Handles button_CS_limpets.Click
        CS_Add("F_limpets")
    End Sub

    Private Sub button_CS_cooked_limpets_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_limpets.Click
        CS_Add("F_cooked_limpets")
    End Sub

    Private Sub button_CS_mussel_click(sender As Object, e As RoutedEventArgs) Handles button_CS_mussel.Click
        CS_Add("F_mussel")
    End Sub

    Private Sub button_CS_cooked_mussel_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_mussel.Click
        CS_Add("F_cooked_mussel")
    End Sub

    Private Sub button_CS_dead_dogfish_click(sender As Object, e As RoutedEventArgs) Handles button_CS_dead_dogfish.Click
        CS_Add("F_dead_dogfish")
    End Sub

    Private Sub button_CS_wobster_click(sender As Object, e As RoutedEventArgs) Handles button_CS_wobster.Click
        CS_Add("F_wobster")
    End Sub

    Private Sub button_CS_raw_fish_click(sender As Object, e As RoutedEventArgs) Handles button_CS_raw_fish.Click
        CS_Add("F_raw_fish")
    End Sub

    Private Sub button_CS_fish_steak_click(sender As Object, e As RoutedEventArgs) Handles button_CS_fish_steak.Click
        CS_Add("F_fish_steak")
    End Sub

    Private Sub button_CS_shark_fin_click(sender As Object, e As RoutedEventArgs) Handles button_CS_shark_fin.Click
        CS_Add("F_shark_fin")
    End Sub

    REM ------------------食材(蔬菜)-------------------
    Private Sub button_CS_blue_cap_click(sender As Object, e As RoutedEventArgs) Handles button_CS_blue_cap.Click
        CS_Add("F_blue_cap")
    End Sub

    Private Sub button_CS_cooked_blue_cap_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_blue_cap.Click
        CS_Add("F_cooked_blue_cap")
    End Sub

    Private Sub button_CS_green_cap_click(sender As Object, e As RoutedEventArgs) Handles button_CS_green_cap.Click
        CS_Add("F_green_cap")
    End Sub

    Private Sub button_CS_cooked_green_cap_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_green_cap.Click
        CS_Add("F_cooked_green_cap")
    End Sub

    Private Sub button_CS_red_cap_click(sender As Object, e As RoutedEventArgs) Handles button_CS_red_cap.Click
        CS_Add("F_red_cap")
    End Sub

    Private Sub button_CS_cooked_red_cap_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_red_cap.Click
        CS_Add("F_cooked_red_cap")
    End Sub

    Private Sub button_CS_eggplant_click(sender As Object, e As RoutedEventArgs) Handles button_CS_eggplant.Click
        CS_Add("F_eggplant")
    End Sub

    Private Sub button_CS_braised_eggplant_click(sender As Object, e As RoutedEventArgs) Handles button_CS_braised_eggplant.Click
        CS_Add("F_braised_eggplant")
    End Sub

    Private Sub button_CS_carrot_click(sender As Object, e As RoutedEventArgs) Handles button_CS_carrot.Click
        CS_Add("F_carrot")
    End Sub

    Private Sub button_CS_roasted_carrot_click(sender As Object, e As RoutedEventArgs) Handles button_CS_roasted_carrot.Click
        CS_Add("F_roasted_carrot")
    End Sub

    Private Sub button_CS_corn_click(sender As Object, e As RoutedEventArgs) Handles button_CS_corn.Click
        CS_Add("F_corn")
    End Sub

    Private Sub button_CS_popcorn_click(sender As Object, e As RoutedEventArgs) Handles button_CS_popcorn.Click
        CS_Add("F_popcorn")
    End Sub

    Private Sub button_CS_pumpkin_click(sender As Object, e As RoutedEventArgs) Handles button_CS_pumpkin.Click
        CS_Add("F_pumpkin")
    End Sub

    Private Sub button_CS_hot_pumpkin_click(sender As Object, e As RoutedEventArgs) Handles button_CS_hot_pumpkin.Click
        CS_Add("F_hot_pumpkin")
    End Sub

    Private Sub button_CS_cactus_flesh_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cactus_flesh.Click
        CS_Add("F_cactus_flesh")
    End Sub

    Private Sub button_CS_cooked_cactus_flesh_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_cactus_flesh.Click
        CS_Add("F_cooked_cactus_flesh")
    End Sub

    Private Sub button_CS_cactus_flower_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cactus_flower.Click
        CS_Add("F_cactus_flower")
    End Sub

    Private Sub button_CS_sweet_potato_click(sender As Object, e As RoutedEventArgs) Handles button_CS_sweet_potato.Click
        CS_Add("F_sweet_potato")
    End Sub

    Private Sub button_CS_cooked_sweet_potato_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_sweet_potato.Click
        CS_Add("F_cooked_sweet_potato")
    End Sub

    Private Sub button_CS_seaweed_click(sender As Object, e As RoutedEventArgs) Handles button_CS_seaweed.Click
        CS_Add("F_seaweed")
    End Sub

    Private Sub button_CS_roasted_seaweed_click(sender As Object, e As RoutedEventArgs) Handles button_CS_roasted_seaweed.Click
        CS_Add("F_roasted_seaweed")
    End Sub

    Private Sub button_CS_dried_seaweed_click(sender As Object, e As RoutedEventArgs) Handles button_CS_dried_seaweed.Click
        CS_Add("F_dried_seaweed")
    End Sub

    REM ------------------食材(水果)-------------------
    Private Sub button_CS_banana_click(sender As Object, e As RoutedEventArgs) Handles button_CS_banana.Click
        CS_Add("F_banana")
    End Sub

    Private Sub button_CS_cooked_banana_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_banana.Click
        CS_Add("F_cooked_banana")
    End Sub

    Private Sub button_CS_juicy_berries_click(sender As Object, e As RoutedEventArgs) Handles button_CS_juicy_berries.Click
        CS_Add("F_juicy_berries")
    End Sub

    Private Sub button_CS_roasted_juicy_berries_click(sender As Object, e As RoutedEventArgs) Handles button_CS_roasted_juicy_berries.Click
        CS_Add("F_roasted_juicy_berries")
    End Sub

    Private Sub button_CS_berries_click(sender As Object, e As RoutedEventArgs) Handles button_CS_berries.Click
        CS_Add("F_berries")
    End Sub

    Private Sub button_CS_roasted_berrie_click(sender As Object, e As RoutedEventArgs) Handles button_CS_roasted_berrie.Click
        CS_Add("F_roasted_berrie")
    End Sub

    Private Sub button_CS_dragon_fruit_click(sender As Object, e As RoutedEventArgs) Handles button_CS_dragon_fruit.Click
        CS_Add("F_dragon_fruit")
    End Sub

    Private Sub button_CS_prepared_dragon_fruit_click(sender As Object, e As RoutedEventArgs) Handles button_CS_prepared_dragon_fruit.Click
        CS_Add("F_prepared_dragon_fruit")
    End Sub

    Private Sub button_CS_durian_click(sender As Object, e As RoutedEventArgs) Handles button_CS_durian.Click
        CS_Add("F_durian")
    End Sub

    Private Sub button_CS_extra_smelly_durian_click(sender As Object, e As RoutedEventArgs) Handles button_CS_extra_smelly_durian.Click
        CS_Add("F_extra_smelly_durian")
    End Sub

    Private Sub button_CS_pomegranate_click(sender As Object, e As RoutedEventArgs) Handles button_CS_pomegranate.Click
        CS_Add("F_pomegranate")
    End Sub

    Private Sub button_CS_sliced_pomegranate_click(sender As Object, e As RoutedEventArgs) Handles button_CS_sliced_pomegranate.Click
        CS_Add("F_sliced_pomegranate")
    End Sub

    Private Sub button_CS_watermelon_click(sender As Object, e As RoutedEventArgs) Handles button_CS_watermelon.Click
        CS_Add("F_watermelon")
    End Sub

    Private Sub button_CS_grilled_watermelon_click(sender As Object, e As RoutedEventArgs) Handles button_CS_grilled_watermelon.Click
        CS_Add("F_grilled_watermelon")
    End Sub

    Private Sub button_CS_halved_coconut_click(sender As Object, e As RoutedEventArgs) Handles button_CS_halved_coconut.Click
        CS_Add("F_halved_coconut")
    End Sub

    Private Sub button_CS_roasted_coconut_click(sender As Object, e As RoutedEventArgs) Handles button_CS_roasted_coconut.Click
        CS_Add("F_roasted_coconut")
    End Sub

    REM ------------------食材(蛋类)-------------------
    Private Sub button_CS_egg_click(sender As Object, e As RoutedEventArgs) Handles button_CS_egg.Click
        CS_Add("F_egg")
    End Sub

    Private Sub button_CS_cooked_egg_click(sender As Object, e As RoutedEventArgs) Handles button_CS_cooked_egg.Click
        CS_Add("F_cooked_egg")
    End Sub

    Private Sub button_CS_tallbird_egg_click(sender As Object, e As RoutedEventArgs) Handles button_CS_tallbird_egg.Click
        CS_Add("F_tallbird_egg")
    End Sub

    Private Sub button_CS_fried_tallbird_egg_click(sender As Object, e As RoutedEventArgs) Handles button_CS_fried_tallbird_egg.Click
        CS_Add("F_fried_tallbird_egg")
    End Sub

    Private Sub button_CS_doydoy_egg_click(sender As Object, e As RoutedEventArgs) Handles button_CS_doydoy_egg.Click
        CS_Add("F_doydoy_egg")
    End Sub

    Private Sub button_CS_fried_doydoy_egg_click(sender As Object, e As RoutedEventArgs) Handles button_CS_fried_doydoy_egg.Click
        CS_Add("F_fried_doydoy_egg")
    End Sub

    REM ------------------食材(其他)-------------------
    Private Sub button_CS_butterfly_wing_click(sender As Object, e As RoutedEventArgs) Handles button_CS_butterfly_wing.Click
        CS_Add("F_butterfly_wing")
    End Sub

    Private Sub button_CS_butter_click(sender As Object, e As RoutedEventArgs) Handles button_CS_butter.Click
        CS_Add("F_butter")
    End Sub

    Private Sub button_CS_honey_click(sender As Object, e As RoutedEventArgs) Handles button_CS_honey.Click
        CS_Add("F_honey")
    End Sub

    Private Sub button_CS_honeycomb_click(sender As Object, e As RoutedEventArgs) Handles button_CS_honeycomb.Click
        CS_Add("F_honeycomb")
    End Sub

    Private Sub button_CS_lichen_click(sender As Object, e As RoutedEventArgs) Handles button_CS_lichen.Click
        CS_Add("F_lichen")
    End Sub

    Private Sub button_CS_mandrake_click(sender As Object, e As RoutedEventArgs) Handles button_CS_mandrake.Click
        CS_Add("F_mandrake")
    End Sub

    Private Sub button_CS_electric_milk_click(sender As Object, e As RoutedEventArgs) Handles button_CS_electric_milk.Click
        CS_Add("F_electric_milk")
    End Sub

    Private Sub button_CS_ice_click(sender As Object, e As RoutedEventArgs) Handles button_CS_ice.Click
        CS_Add("F_ice")
    End Sub

    Private Sub button_CS_roasted_birchnut_click(sender As Object, e As RoutedEventArgs) Handles button_CS_roasted_birchnut.Click
        CS_Add("F_roasted_birchnut")
    End Sub

    Private Sub button_CS_twigs_click(sender As Object, e As RoutedEventArgs) Handles button_CS_twigs.Click
        CS_Add("G_twigs")
    End Sub

    Private Sub button_CS_butterfly_wing_sw_click(sender As Object, e As RoutedEventArgs) Handles button_CS_butterfly_wing_sw.Click
        CS_Add("F_butterfly_wing_sw")
    End Sub

    Private Sub button_CS_coffee_beans_click(sender As Object, e As RoutedEventArgs) Handles button_CS_coffee_beans.Click
        CS_Add("F_coffee_beans")
    End Sub

    Private Sub button_CS_roasted_coffee_beans_click(sender As Object, e As RoutedEventArgs) Handles button_CS_roasted_coffee_beans.Click
        CS_Add("F_roasted_coffee_beans")
    End Sub

    REM 是否启用便携式烹饪锅
    Private Sub checkBox_CS_PortableCrockPot_click() Handles checkBox_CS_PortableCrockPot.Click
        If checkBox_CS_PortableCrockPot.IsChecked = True Then
            Reg_Write("CS_PortableCrockPot", 0)
            checkBox_CS_CrockPot.IsChecked = False
            CS_image_CrockPot.Source = Picture_Short_Name(Res_Short_Name("CP_PortableCrockPot"))
            CS_PortableCrockPot = True
        Else
            Reg_Write("CS_PortableCrockPot", 1)
            checkBox_CS_CrockPot.IsChecked = True
            CS_image_CrockPot.Source = Picture_Short_Name(Res_Short_Name("CP_CrockPot"))
            CS_PortableCrockPot = False
        End If
    End Sub

    Private Sub checkBox_CS_CrockPot_click() Handles checkBox_CS_CrockPot.Click
        If checkBox_CS_CrockPot.IsChecked = True Then
            Reg_Write("CS_PortableCrockPot", 1)
            checkBox_CS_PortableCrockPot.IsChecked = False
            CS_image_CrockPot.Source = Picture_Short_Name(Res_Short_Name("CP_CrockPot"))
            CS_PortableCrockPot = False
        Else
            Reg_Write("CS_PortableCrockPot", 0)
            checkBox_CS_PortableCrockPot.IsChecked = True
            CS_image_CrockPot.Source = Picture_Short_Name(Res_Short_Name("CP_PortableCrockPot"))
            CS_PortableCrockPot = True
        End If
    End Sub

    REM 添加食材
    Private Sub CS_Add(Name As String)
        If CS_Recipe_1 = "" Then
            CS_Recipe_1 = Name
            CS_image_Food_1.Source = Picture_Short_Name(Res_Short_Name(Name))
        ElseIf CS_Recipe_2 = "" Then
            CS_Recipe_2 = Name
            CS_image_Food_2.Source = Picture_Short_Name(Res_Short_Name(Name))
        ElseIf CS_Recipe_3 = "" Then
            CS_Recipe_3 = Name
            CS_image_Food_3.Source = Picture_Short_Name(Res_Short_Name(Name))
        ElseIf CS_Recipe_4 = "" Then
            CS_Recipe_4 = Name
            CS_image_Food_4.Source = Picture_Short_Name(Res_Short_Name(Name))
            'Else
            '    MsgBox("食材已满，不可添加！")
        End If
        If CS_Recipe_1 <> "" And CS_Recipe_2 <> "" And CS_Recipe_3 <> "" And CS_Recipe_4 <> "" And checkBox_CS_AutoCalculation.IsChecked = True Then
            CS_CrockPotCalculation()
        End If
    End Sub

    REM 删除食材
    Private Sub button_CS_Food_1_click(sender As Object, e As RoutedEventArgs) Handles button_CS_Food_1.Click
        CS_Recipe_1 = ""
        CS_image_Food_1.Source = Picture_Short_Name()
    End Sub

    Private Sub button_CS_Food_2_click(sender As Object, e As RoutedEventArgs) Handles button_CS_Food_2.Click
        CS_Recipe_2 = ""
        CS_image_Food_2.Source = Picture_Short_Name()
    End Sub

    Private Sub button_CS_Food_3_click(sender As Object, e As RoutedEventArgs) Handles button_CS_Food_3.Click
        CS_Recipe_3 = ""
        CS_image_Food_3.Source = Picture_Short_Name()
    End Sub

    Private Sub button_CS_Food_4_click(sender As Object, e As RoutedEventArgs) Handles button_CS_Food_4.Click
        CS_Recipe_4 = ""
        CS_image_Food_4.Source = Picture_Short_Name()
    End Sub

    REM 食材属性统计
    Private Sub CS_RecipeStatistics(Name As String)
        Select Case Name
            Case "F_eel"
                CS_FT_Fishes += 1
                CS_FT_Eel += 1
                CS_FT_Meats += 0.5
            Case "F_cooked_eel"
                CS_FT_Fishes += 1
                CS_FT_Eel += 1
                CS_FT_Meats += 0.5
            Case "F_fish"
                CS_FT_Fishes += 1
                CS_FT_Meats += 0.5
            Case "F_cooked_fish"
                CS_FT_Fishes += 1
                CS_FT_Meats += 0.5
            Case "F_frog_legs"
                CS_FT_Meats += 0.5
                CS_FT_FrogLegs += 1
            Case "F_cooked_frog_legs"
                CS_FT_Meats += 0.5
                CS_FT_FrogLegs += 1
            Case "F_meat"
                CS_FT_Meats += 1
            Case "F_cooked_meat"
                CS_FT_Meats += 1
            Case "F_jerky"
                CS_FT_Meats += 1
            Case "F_monster_meat"
                CS_FT_Meats += 1
                CS_FT_MonsterFoods += 1
            Case "F_cooked_monster_meat"
                CS_FT_Meats += 1
                CS_FT_MonsterFoods += 1
            Case "F_monster_jerky"
                CS_FT_Meats += 1
                CS_FT_MonsterFoods += 1
            Case "F_morsel"
                CS_FT_Meats += 0.5
            Case "F_cooked_morsel"
                CS_FT_Meats += 0.5
            Case "F_small_jerky"
                CS_FT_Meats += 0.5
            Case "F_drumstick"
                CS_FT_Meats += 0.5
                CS_FT_Drumstick += 1
            Case "F_fried_drumstick"
                CS_FT_Meats += 0.5
                CS_FT_Drumstick += 1
            Case "F_moleworm"
                CS_FT_Moleworm += 1
            Case "F_tropical_fish"
                CS_FT_Meats += 0.5
                CS_FT_Fishes += 1
            Case "F_fish_morsel"
                CS_FT_Fishes += 0.5
            Case "F_cooked_fish_morsel"
                CS_FT_Fishes += 0.5
            Case "F_jellyfish"
                CS_FT_Fishes += 1
                CS_FT_MonsterFoods += 1
                CS_FT_Jellyfish += 1
            Case "F_dead_jellyfish"
                CS_FT_Fishes += 1
                CS_FT_MonsterFoods += 1
            Case "F_cooked_jellyfish"
                CS_FT_Fishes += 1
                CS_FT_MonsterFoods += 1
            Case "F_dried_jellyfish"
                CS_FT_Fishes += 1
                CS_FT_MonsterFoods += 1
            Case "F_limpets"
                CS_FT_Fishes += 0.5
                CS_FT_Limpets += 1
            Case "F_cooked_limpets"
                CS_FT_Fishes += 0.5
            Case "F_mussel"
                CS_FT_Fishes += 0.5
                CS_FT_Mussel += 1
            Case "F_cooked_mussel"
                CS_FT_Fishes += 0.5
                CS_FT_Mussel += 1
            Case "F_dead_dogfish"
                CS_FT_Fishes += 1
                CS_FT_Meats += 0.5
            Case "F_dead_swordfish"
                CS_FT_Fishes += 1
                CS_FT_Meats += 0.5
            Case "F_wobster"
                CS_FT_Fishes += 2
                CS_FT_Wobster += 1
            Case "F_raw_fish"
                CS_FT_Fishes += 1
                CS_FT_Meats += 0.5
            Case "F_fish_steak"
                CS_FT_Fishes += 1
                CS_FT_Meats += 0.5
            Case "F_shark_fin"
                CS_FT_Fishes += 1
                CS_FT_Meats += 0.5
                CS_FT_SharkFin += 1
            Case "F_blue_cap"
                CS_FT_Vegetables += 0.5
            Case "F_cooked_blue_cap"
                CS_FT_Vegetables += 0.5
            Case "F_green_cap"
                CS_FT_Vegetables += 0.5
            Case "F_cooked_green_cap"
                CS_FT_Vegetables += 0.5
            Case "F_red_cap"
                CS_FT_Vegetables += 0.5
            Case "F_cooked_red_cap"
                CS_FT_Vegetables += 0.5
            Case "F_eggplant"
                CS_FT_Vegetables += 1
                CS_FT_Eggplant += 1
            Case "F_braised_eggplant"
                CS_FT_Vegetables += 1
                CS_FT_Eggplant += 1
            Case "F_carrot"
                CS_FT_Vegetables += 1
            Case "F_roasted_carrot"
                CS_FT_Vegetables += 1
            Case "F_corn"
                CS_FT_Vegetables += 1
                CS_FT_Corn += 1
            Case "F_popcorn"
                CS_FT_Vegetables += 1
                CS_FT_Corn += 1
            Case "F_pumpkin"
                CS_FT_Vegetables += 1
                CS_FT_Pumpkin += 1
            Case "F_hot_pumpkin"
                CS_FT_Vegetables += 1
                CS_FT_Pumpkin += 1
            Case "F_cactus_flesh"
                CS_FT_Vegetables += 1
                CS_FT_CactusFlesh += 1
            Case "F_cooked_cactus_flesh"
                CS_FT_Vegetables += 1
            Case "F_cactus_flower"
                CS_FT_Vegetables += 1
                CS_FT_CactusFlower += 1
            Case "F_sweet_potato"
                CS_FT_Vegetables += 1
                CS_FT_SweetPotato += 1
            Case "F_cooked_sweet_potato"
                CS_FT_Vegetables += 1
            Case "F_seaweed"
                CS_FT_Vegetables += 0.5
                CS_FT_Seaweed += 1
            Case "F_roasted_seaweed"
                CS_FT_Vegetables += 0.5
            Case "F_dried_seaweed"
                CS_FT_Vegetables += 0.5
            Case "F_banana"
                CS_FT_Fruit += 1
                CS_FT_Banana += 1
            Case "F_cooked_banana"
                CS_FT_Fruit += 1
            Case "F_juicy_berries"
                CS_FT_Fruit += 0.5
            Case "F_roasted_juicy_berries"
                CS_FT_Fruit += 0.5
            Case "F_berries"
                CS_FT_Fruit += 0.5
                CS_FT_Berries += 1
            Case "F_roasted_berrie"
                CS_FT_Fruit += 0.5
                CS_FT_Berries += 1
            Case "F_dragon_fruit"
                CS_FT_Fruit += 1
                CS_FT_DragonFruit += 1
            Case "F_prepared_dragon_fruit"
                CS_FT_Fruit += 1
                CS_FT_DragonFruit += 1
            Case "F_durian"
                CS_FT_Fruit += 1
                CS_FT_MonsterFoods += 1
            Case "F_extra_smelly_durian"
                CS_FT_Fruit += 1
                CS_FT_MonsterFoods += 1
            Case "F_pomegranate"
                CS_FT_Fruit += 1
            Case "F_sliced_pomegranate"
                CS_FT_Fruit += 1
            Case "F_watermelon"
                CS_FT_Fruit += 1
                CS_FT_Watermelon += 1
            Case "F_grilled_watermelon"
                CS_FT_Fruit += 1
            Case "F_halved_coconut"
                CS_FT_Fruit += 1
            Case "F_roasted_coconut"
                CS_FT_Fruit += 1
            Case "F_egg"
                CS_FT_Eggs += 1
            Case "F_cooked_egg"
                CS_FT_Eggs += 1
            Case "F_tallbird_egg"
                CS_FT_Eggs += 4
            Case "F_fried_tallbird_egg"
                CS_FT_Eggs += 4
            Case "F_doydoy_egg"
                CS_FT_Eggs += 1
            Case "F_fried_doydoy_egg"
                CS_FT_Eggs += 1
            Case "F_butterfly_wing"
                CS_FT_Butterfly_wings += 1
            Case "F_butter"
                CS_FT_DairyProduct += 1
                CS_FT_Butter += 1
            Case "F_honey"
                CS_FT_Sweetener += 1
                CS_FT_Honey += 1
            Case "F_honeycomb"
                CS_FT_Sweetener += 1
            Case "F_lichen"
                CS_FT_Vegetables += 1
                CS_FT_Lichen += 1
            Case "F_mandrake"
                CS_FT_Vegetables += 1
                CS_FT_Mandrake += 1
            Case "F_electric_milk"
                CS_FT_DairyProduct += 1
            Case "F_ice"
                CS_FT_Ice += 1
            Case "F_roasted_birchnut"
                CS_FT_RoastedBirchnut += 1
            Case "G_twigs"
                CS_FT_Twigs += 1
            Case "F_butterfly_wing_sw"
                CS_FT_Butterfly_wings += 1
            Case "F_coffee_beans"
                CS_FT_Fruit += 0.5
            Case "F_roasted_coffee_beans"
                CS_FT_Fruit += 1
                CS_FT_RoastedCoffeeBeans += 1
        End Select
    End Sub

    Private Sub checkBox_CS_AutoCalculation_click(sender As Object, e As RoutedEventArgs) Handles checkBox_CS_AutoCalculation.Click
        If checkBox_CS_AutoCalculation.IsChecked = True Then
            Reg_Write("CS_AutoCalculation", 1)
        Else
            Reg_Write("CS_AutoCalculation", 0)
        End If
    End Sub

    Private Sub checkBox_CS_AutoClean_click(sender As Object, e As RoutedEventArgs) Handles checkBox_CS_AutoClean.Click
        If checkBox_CS_AutoClean.IsChecked = True Then
            Reg_Write("CS_AutoClean", 1)
        Else
            Reg_Write("CS_AutoClean", 0)
        End If
    End Sub

    REM 烹饪
    Private Sub button_CS_CrockPot_click(sender As Object, e As RoutedEventArgs) Handles button_CS_CrockPot.Click
        CS_CrockPotCalculation()
    End Sub

    Private Sub button_CS_Switch_Left_click(sender As Object, e As RoutedEventArgs) Handles button_CS_Switch_Left.Click
        button_CS_Switch_Right.IsEnabled = True
        If FoodIndex <> 0 Then
            FoodIndex -= 1
            If FoodIndex = 0 Then
                button_CS_Switch_Left.IsEnabled = False
            End If
            CS_F_name = CrockPotList(FoodIndex)
            Select Case CrockPotList(FoodIndex)
                Case "新鲜水果薄饼"
                    CS_image_Food_5_Source("F_fresh_fruit_crepes")
                Case "怪物鞑靼"
                    CS_image_Food_5_Source("F_monster_tartare")
                Case "贝类淡菜汤"
                    CS_image_Food_5_Source("F_mussel_bouillabaise")
                Case "薯蛋奶酥"
                    CS_image_Food_5_Source("F_sweet_potato_souffle")
                Case "龙虾浓汤"
                    CS_image_Food_5_Source("F_lobster_bisque")
                Case "汤"
                    CS_image_Food_5_Source("F_bisque")
                Case "咖啡"
                    CS_image_Food_5_Source("F_coffee")
                Case "海鲜牛排"
                    CS_image_Food_5_Source("F_surf_'n'_turf")
                Case "龙虾正餐"
                    CS_image_Food_5_Source("F_lobster_dinner")
                Case "香蕉冰淇淋"
                    CS_image_Food_5_Source("F_banana_pop")
                Case "加州卷"
                    CS_image_Food_5_Source("F_california_roll")
                Case "果冻冰淇淋"
                    CS_image_Food_5_Source("F_jelly-O_pop")
                Case "橘汁腌鱼"
                    CS_image_Food_5_Source("F_ceviche")
                Case "鱼翅汤"
                    CS_image_Food_5_Source("F_shark_fin_soup")
                Case "海鲜汤"
                    CS_image_Food_5_Source("F_seafood_gumbo")
                Case "鼹鼠鳄梨酱"
                    CS_image_Food_5_Source("F_guacamole")
                Case "花瓣沙拉"
                    CS_image_Food_5_Source("F_flower_salad")
                Case "冰淇淋"
                    CS_image_Food_5_Source("F_ice_cream")
                Case "西瓜冰"
                    CS_image_Food_5_Source("F_melonsicle")
                Case "水果杂烩"
                    CS_image_Food_5_Source("F_trail_mix")
                Case "辣椒酱"
                    CS_image_Food_5_Source("F_spicy_chili")
                Case "鳗鱼"
                    CS_image_Food_5_Source("F_unagi")
                Case "南瓜饼"
                    CS_image_Food_5_Source("F_pumpkin_cookie")
                Case "芝士蛋糕"
                    CS_image_Food_5_Source("F_powdercake")
                Case "曼德拉汤"
                    CS_image_Food_5_Source("F_mandrake_soup")
                Case "炸鱼条"
                    CS_image_Food_5_Source("F_fishsticks")
                Case "玉米饼包炸鱼"
                    CS_image_Food_5_Source("F_fish_tacos")
                Case "培根煎蛋"
                    CS_image_Food_5_Source("F_bacon_and_eggs")
                Case "火鸡正餐"
                    CS_image_Food_5_Source("F_turkey_dinner")
                Case "太妃糖"
                    CS_image_Food_5_Source("F_taffy")
                Case "华夫饼"
                    CS_image_Food_5_Source("F_waffles")
                Case "怪物千层饼"
                    CS_image_Food_5_Source("F_monster_lasagna")
                Case "饺子"
                    CS_image_Food_5_Source("F_pierogi")
                Case "肉串"
                    CS_image_Food_5_Source("F_kabobs")
                Case "蜜汁火腿"
                    CS_image_Food_5_Source("F_honey_ham")
                Case "甜蜜金砖"
                    CS_image_Food_5_Source("F_honey_nuggets")
                Case "奶油松饼"
                    CS_image_Food_5_Source("F_butter_muffin")
                Case "青蛙圆面包三明治"
                    CS_image_Food_5_Source("F_froggle_bunwich")
                Case "火龙果派"
                    CS_image_Food_5_Source("F_dragonpie")
                Case "香酥茄盒"
                    CS_image_Food_5_Source("F_stuffed_eggplant")
                Case "蔬菜杂烩"
                    CS_image_Food_5_Source("F_ratatouille")
                Case "果酱蜜饯"
                    CS_image_Food_5_Source("F_fist_full_of_jam")
                Case "水果拼盘"
                    CS_image_Food_5_Source("F_fruit_medley")
                Case "肉汤"
                    CS_image_Food_5_Source("F_meaty_stew")
                Case "肉丸"
                    CS_image_Food_5_Source("F_meatballs")
            End Select
            REM 显示食物名称
            TextBlock_CS_FoodName.Text = CS_F_name
            TextBlock_CS_FoodName.UpdateLayout()
            Dim CS_FoodName_MarginLeft As Integer
            CS_FoodName_MarginLeft = (Canvas_CookingSimulatorLeft.ActualWidth - TextBlock_CS_FoodName.ActualWidth) / 2
            Dim CS_FoodName_T As New Thickness()
            CS_FoodName_T.Top = 555
            CS_FoodName_T.Left = CS_FoodName_MarginLeft
            TextBlock_CS_FoodName.Margin = CS_FoodName_T
        End If
    End Sub

    Private Sub button_CS_Switch_Right_click(sender As Object, e As RoutedEventArgs) Handles button_CS_Switch_Right.Click
        button_CS_Switch_Left.IsEnabled = True
        If FoodIndex <> CrockPotListIndex Then
            FoodIndex += 1
            If FoodIndex = CrockPotListIndex Then
                button_CS_Switch_Right.IsEnabled = False
            End If
            CS_F_name = CrockPotList(FoodIndex)
            Select Case CrockPotList(FoodIndex)
                Case "新鲜水果薄饼"
                    CS_image_Food_5_Source("F_fresh_fruit_crepes")
                Case "怪物鞑靼"
                    CS_image_Food_5_Source("F_monster_tartare")
                Case "贝类淡菜汤"
                    CS_image_Food_5_Source("F_mussel_bouillabaise")
                Case "薯蛋奶酥"
                    CS_image_Food_5_Source("F_sweet_potato_souffle")
                Case "龙虾浓汤"
                    CS_image_Food_5_Source("F_lobster_bisque")
                Case "汤"
                    CS_image_Food_5_Source("F_bisque")
                Case "咖啡"
                    CS_image_Food_5_Source("F_coffee")
                Case "海鲜牛排"
                    CS_image_Food_5_Source("F_surf_'n'_turf")
                Case "龙虾正餐"
                    CS_image_Food_5_Source("F_lobster_dinner")
                Case "香蕉冰淇淋"
                    CS_image_Food_5_Source("F_banana_pop")
                Case "加州卷"
                    CS_image_Food_5_Source("F_california_roll")
                Case "果冻冰淇淋"
                    CS_image_Food_5_Source("F_jelly-O_pop")
                Case "橘汁腌鱼"
                    CS_image_Food_5_Source("F_ceviche")
                Case "鱼翅汤"
                    CS_image_Food_5_Source("F_shark_fin_soup")
                Case "海鲜汤"
                    CS_image_Food_5_Source("F_seafood_gumbo")
                Case "鼹鼠鳄梨酱"
                    CS_image_Food_5_Source("F_guacamole")
                Case "花瓣沙拉"
                    CS_image_Food_5_Source("F_flower_salad")
                Case "冰淇淋"
                    CS_image_Food_5_Source("F_ice_cream")
                Case "西瓜冰"
                    CS_image_Food_5_Source("F_melonsicle")
                Case "水果杂烩"
                    CS_image_Food_5_Source("F_trail_mix")
                Case "辣椒酱"
                    CS_image_Food_5_Source("F_spicy_chili")
                Case "鳗鱼"
                    CS_image_Food_5_Source("F_unagi")
                Case "南瓜饼"
                    CS_image_Food_5_Source("F_pumpkin_cookie")
                Case "芝士蛋糕"
                    CS_image_Food_5_Source("F_powdercake")
                Case "曼德拉汤"
                    CS_image_Food_5_Source("F_mandrake_soup")
                Case "炸鱼条"
                    CS_image_Food_5_Source("F_fishsticks")
                Case "玉米饼包炸鱼"
                    CS_image_Food_5_Source("F_fish_tacos")
                Case "培根煎蛋"
                    CS_image_Food_5_Source("F_bacon_and_eggs")
                Case "火鸡正餐"
                    CS_image_Food_5_Source("F_turkey_dinner")
                Case "太妃糖"
                    CS_image_Food_5_Source("F_taffy")
                Case "华夫饼"
                    CS_image_Food_5_Source("F_waffles")
                Case "怪物千层饼"
                    CS_image_Food_5_Source("F_monster_lasagna")
                Case "饺子"
                    CS_image_Food_5_Source("F_pierogi")
                Case "肉串"
                    CS_image_Food_5_Source("F_kabobs")
                Case "蜜汁火腿"
                    CS_image_Food_5_Source("F_honey_ham")
                Case "甜蜜金砖"
                    CS_image_Food_5_Source("F_honey_nuggets")
                Case "奶油松饼"
                    CS_image_Food_5_Source("F_butter_muffin")
                Case "青蛙圆面包三明治"
                    CS_image_Food_5_Source("F_froggle_bunwich")
                Case "火龙果派"
                    CS_image_Food_5_Source("F_dragonpie")
                Case "香酥茄盒"
                    CS_image_Food_5_Source("F_stuffed_eggplant")
                Case "蔬菜杂烩"
                    CS_image_Food_5_Source("F_ratatouille")
                Case "果酱蜜饯"
                    CS_image_Food_5_Source("F_fist_full_of_jam")
                Case "水果拼盘"
                    CS_image_Food_5_Source("F_fruit_medley")
                Case "肉汤"
                    CS_image_Food_5_Source("F_meaty_stew")
                Case "肉丸"
                    CS_image_Food_5_Source("F_meatballs")
            End Select
            REM 显示食物名称
            TextBlock_CS_FoodName.Text = CS_F_name
            TextBlock_CS_FoodName.UpdateLayout()
            Dim CS_FoodName_MarginLeft As Integer
            CS_FoodName_MarginLeft = (Canvas_CookingSimulatorLeft.ActualWidth - TextBlock_CS_FoodName.ActualWidth) / 2
            Dim CS_FoodName_T As New Thickness()
            CS_FoodName_T.Top = 555
            CS_FoodName_T.Left = CS_FoodName_MarginLeft
            TextBlock_CS_FoodName.Margin = CS_FoodName_T
        End If
    End Sub

    REM 向食物列表添加食物
    Private Sub CS_CrockPotListAddFood(FoodName As String, FoodPriority As SByte)
        If FoodPriority >= CrockPotMaxPriority Then
            CrockPotMaxPriority = FoodPriority
            CrockPotListIndex += 1
            ReDim Preserve CrockPotList(CrockPotListIndex)
            CrockPotList(CrockPotListIndex) = FoodName
        End If
    End Sub

    REM 烹饪结果图片
    Private Sub CS_image_Food_5_Source(source As String)
        CS_image_Food_5.Source = Picture_Short_Name(Res_Short_Name(source))
    End Sub

    Private Sub CS_CrockPotCalculation()
        REM 判断食材是否足够
        If CS_Recipe_1 = "" Or CS_Recipe_2 = "" Or CS_Recipe_3 = "" Or CS_Recipe_4 = "" Then
            MsgBox("食材不足，请添加！")
            Exit Sub
        End If
        FoodIndex = 0
        REM 食物列表初始化
        Erase CrockPotList
        ReDim CrockPotList(0)
        CrockPotListIndex = -1
        CrockPotMaxPriority = -128
        REM 食材属性初始化
        CS_FT_Banana = 0
        CS_FT_Berries = 0
        CS_FT_Butter = 0
        CS_FT_Butterfly_wings = 0
        CS_FT_CactusFlesh = 0
        CS_FT_CactusFlower = 0
        CS_FT_Corn = 0
        CS_FT_DairyProduct = 0
        CS_FT_DragonFruit = 0
        CS_FT_Drumstick = 0
        CS_FT_Eel = 0
        CS_FT_Eggplant = 0
        CS_FT_Eggs = 0
        CS_FT_Fishes = 0
        CS_FT_FrogLegs = 0
        CS_FT_Fruit = 0
        CS_FT_Honey = 0
        CS_FT_Ice = 0
        CS_FT_Jellyfish = 0
        CS_FT_Lichen = 0
        CS_FT_Limpets = 0
        CS_FT_Mandrake = 0
        CS_FT_Meats = 0
        CS_FT_Moleworm = 0
        CS_FT_MonsterFoods = 0
        CS_FT_Mussel = 0
        CS_FT_Pumpkin = 0
        CS_FT_RoastedBirchnut = 0
        CS_FT_RoastedCoffeeBeans = 0
        CS_FT_Seaweed = 0
        CS_FT_SharkFin = 0
        CS_FT_Sweetener = 0
        CS_FT_SweetPotato = 0
        CS_FT_Twigs = 0
        CS_FT_Vegetables = 0
        CS_FT_Watermelon = 0
        CS_FT_Wobster = 0
        REM 属性统计
        CS_RecipeStatistics(CS_Recipe_1)
        CS_RecipeStatistics(CS_Recipe_2)
        CS_RecipeStatistics(CS_Recipe_3)
        CS_RecipeStatistics(CS_Recipe_4)
        REM 烹饪
        '------------------------SW------------------------
        If CS_ROG_SW_DST = 2 Or CS_ROG_SW_DST = 3 Or CS_ROG_SW_DST = 6 Or CS_ROG_SW_DST = 7 Then
            If CS_PortableCrockPot = True Then '便携式烹饪锅的四种食物
                If CS_FT_Fruit >= 2 And CS_FT_Butter >= 1 And CS_FT_Honey >= 1 Then
                    CS_CrockPotListAddFood("新鲜水果薄饼", 30)
                End If
                If CS_FT_MonsterFoods >= 2 And CS_FT_Eggs >= 1 And CS_FT_Vegetables >= 0.5 Then
                    CS_CrockPotListAddFood("怪物鞑靼", 30)
                End If
                If CS_FT_Mussel >= 2 And CS_FT_Vegetables >= 2 Then
                    CS_CrockPotListAddFood("贝类淡菜汤", 30)
                End If
                If CS_FT_SweetPotato >= 2 And CS_FT_Eggs >= 2 Then
                    CS_CrockPotListAddFood("薯蛋奶酥", 30)
                End If
            End If
            If CS_FT_Wobster >= 1 And CS_FT_Ice >= 1 Then
                CS_CrockPotListAddFood("龙虾浓汤", 30)
            End If
            If CS_FT_Limpets >= 3 And CS_FT_Ice >= 1 Then
                CS_CrockPotListAddFood("汤", 30)
            End If
            If CS_FT_RoastedCoffeeBeans >= 3 And (CS_FT_RoastedCoffeeBeans = 4 Or CS_FT_Sweetener = 1 Or CS_FT_DairyProduct = 1) Then
                CS_CrockPotListAddFood("咖啡", 30)
            End If
            If CS_FT_Meats >= 2.5 And CS_FT_Fishes >= 1.5 And CS_FT_Ice = 0 Then
                CS_CrockPotListAddFood("海鲜牛排", 30)
            End If
            If CS_FT_Wobster >= 1 And CS_FT_Butter >= 1 And CS_FT_Meats = 0 And CS_FT_Ice = 0 Then
                CS_CrockPotListAddFood("龙虾正餐", 25)
            End If
            If CS_FT_Banana >= 1 And CS_FT_Ice >= 1 And CS_FT_Twigs >= 1 And CS_FT_Meats = 0 And CS_FT_Fishes = 0 Then
                CS_CrockPotListAddFood("香蕉冰淇淋", 20)
            End If
            If CS_FT_Fishes >= 1 And CS_FT_Seaweed = 2 Then
                CS_CrockPotListAddFood("加州卷", 20)
            End If
            If CS_FT_Jellyfish >= 1 And CS_FT_Ice >= 1 And CS_FT_Twigs >= 1 Then
                CS_CrockPotListAddFood("果冻冰淇淋", 20)
            End If
            If CS_FT_Fishes >= 2 And CS_FT_Ice >= 1 Then
                CS_CrockPotListAddFood("橘汁腌鱼", 20)
            End If
            If CS_FT_SharkFin >= 1 Then
                CS_CrockPotListAddFood("鱼翅汤", 20)
            End If
            If CS_FT_Fishes >= 2.5 Then
                CS_CrockPotListAddFood("海鲜汤", 10)
            End If
        End If
        '------------------------其他------------------------
        If CS_FT_CactusFlesh >= 1 And CS_FT_Moleworm >= 1 And CS_FT_Fruit = 0 Then
            CS_CrockPotListAddFood("鼹鼠鳄梨酱", 10)
        End If
        If CS_FT_CactusFlower >= 1 And CS_FT_Vegetables >= 2 And CS_FT_Fruit = 0 And CS_FT_Meats = 0 And CS_FT_Eggs = 0 And CS_FT_Sweetener = 0 And CS_FT_Twigs = 0 Then
            CS_CrockPotListAddFood("花瓣沙拉", 10)
        End If
        If CS_FT_DairyProduct >= 1 And CS_FT_Ice >= 1 And CS_FT_Sweetener >= 1 And CS_FT_Meats = 0 And CS_FT_Eggs = 0 And CS_FT_Vegetables = 0 And CS_FT_Twigs = 0 Then
            CS_CrockPotListAddFood("冰淇淋", 10)
        End If
        If CS_FT_Watermelon >= 1 And CS_FT_Ice >= 1 And CS_FT_Twigs >= 1 And CS_FT_Meats = 0 And CS_FT_Eggs = 0 And CS_FT_Vegetables = 0 Then
            CS_CrockPotListAddFood("西瓜冰", 10)
        End If
        If CS_FT_RoastedBirchnut >= 1 And CS_FT_Berries >= 1 And CS_FT_Fruit >= 1 And CS_FT_Meats = 0 And CS_FT_Eggs = 0 And CS_FT_Vegetables = 0 And CS_FT_Sweetener = 0 Then
            CS_CrockPotListAddFood("水果杂烩", 10)
        End If
        If CS_FT_Vegetables >= 1.5 And CS_FT_Meats >= 1.5 Then
            CS_CrockPotListAddFood("辣椒酱", 10)
        End If
        If CS_FT_Eel >= 1 And CS_FT_Lichen >= 1 Then
            CS_CrockPotListAddFood("鳗鱼", 20)
        End If
        If CS_FT_Pumpkin >= 1 And CS_FT_Sweetener >= 2 Then
            CS_CrockPotListAddFood("南瓜饼", 10)
        End If
        If CS_FT_Corn >= 1 And CS_FT_Honey >= 1 And CS_FT_Twigs >= 1 Then
            CS_CrockPotListAddFood("芝士蛋糕", 10)
        End If
        If CS_FT_Mandrake >= 1 Then
            CS_CrockPotListAddFood("曼德拉汤", 10)
        End If
        If CS_FT_Fishes >= 0.5 And CS_FT_Twigs = 1 Then
            CS_CrockPotListAddFood("炸鱼条", 10)
        End If
        If CS_FT_Fishes >= 0.5 And CS_FT_Corn >= 1 Then
            CS_CrockPotListAddFood("玉米饼包炸鱼", 10)
        End If
        If CS_FT_Meats >= 1.5 And CS_FT_Eggs >= 2 And CS_FT_Vegetables = 0 Then
            CS_CrockPotListAddFood("培根煎蛋", 10)
        End If
        If CS_FT_Drumstick >= 2 And CS_FT_Meats >= 1.5 And (CS_FT_Vegetables >= 0.5 Or CS_FT_Fruit >= 0.5) Then
            CS_CrockPotListAddFood("火鸡正餐", 10)
        End If
        If CS_FT_Sweetener >= 3 And CS_FT_Meats = 0 Then
            CS_CrockPotListAddFood("太妃糖", 10)
        End If
        If CS_FT_Butter >= 1 And CS_FT_Eggs >= 1 And CS_FT_Berries >= 1 Then
            CS_CrockPotListAddFood("华夫饼", 10)
        End If
        If CS_FT_MonsterFoods >= 2 And CS_FT_Twigs = 0 Then
            CS_CrockPotListAddFood("怪物千层饼", 10)
        End If
        If CS_FT_Eggs >= 1 And CS_FT_Meats >= 0.5 And CS_FT_Vegetables >= 0.5 And CS_FT_Twigs = 0 Then
            CS_CrockPotListAddFood("饺子", 5)
        End If
        If CS_FT_Meats >= 0.5 And CS_FT_Twigs = 1 And CS_FT_MonsterFoods <= 1 Then
            CS_CrockPotListAddFood("肉串", 5)
        End If
        If CS_FT_Meats >= 2 And CS_FT_Honey >= 1 And CS_FT_Twigs = 0 Then
            CS_CrockPotListAddFood("蜜汁火腿", 2)
        End If
        If CS_FT_Meats >= 0.5 And CS_FT_Meats < 2 And CS_FT_Honey >= 1 And CS_FT_Twigs = 0 Then
            CS_CrockPotListAddFood("甜蜜金砖", 2)
        End If
        If CS_FT_Butterfly_wings >= 1 And CS_FT_Vegetables >= 0.5 And CS_FT_Meats = 0 Then
            CS_CrockPotListAddFood("奶油松饼", 1)
        End If
        If CS_FT_FrogLegs >= 1 And CS_FT_Vegetables >= 0.5 Then
            CS_CrockPotListAddFood("青蛙圆面包三明治", 1)
        End If
        If CS_FT_DragonFruit >= 1 And CS_FT_Meats = 0 Then
            CS_CrockPotListAddFood("火龙果派", 1)
        End If
        If CS_FT_Eggplant >= 1 And CS_FT_Vegetables >= 0.5 Then
            CS_CrockPotListAddFood("香酥茄盒", 1)
        End If
        If CS_FT_Vegetables >= 0.5 And CS_FT_Meats = 0 And CS_FT_Twigs = 0 Then
            CS_CrockPotListAddFood("蔬菜杂烩", 0)
        End If
        If CS_FT_Fruit >= 0.5 And CS_FT_Meats = 0 And CS_FT_Vegetables = 0 Then
            If CS_FT_Fruit < 3 Then
                CS_CrockPotListAddFood("果酱蜜饯", 0)
            Else
                If CS_FT_Twigs = 0 Then
                    CS_CrockPotListAddFood("果酱蜜饯", 0)
                    CS_CrockPotListAddFood("水果拼盘", 0)
                Else
                    CS_CrockPotListAddFood("水果拼盘", 0)
                End If
            End If
        End If
        If CS_FT_Meats >= 3 And CS_FT_Twigs = 0 Then
            CS_CrockPotListAddFood("肉汤", 0)
        End If
        If CS_FT_Meats >= 0.5 And CS_FT_Meats < 3 And CS_FT_Twigs = 0 Then
            CS_CrockPotListAddFood("肉丸", -1)
        End If

        REM 食物判断
        If CrockPotListIndex = -1 Then
            CS_F_name = "湿腻焦糊"
            CS_image_Food_5_Source("F_wet_goop")
        Else
            CS_F_name = CrockPotList(0)
            Select Case CrockPotList(0)
                Case "新鲜水果薄饼"
                    CS_image_Food_5_Source("F_fresh_fruit_crepes")
                Case "怪物鞑靼"
                    CS_image_Food_5_Source("F_monster_tartare")
                Case "贝类淡菜汤"
                    CS_image_Food_5_Source("F_mussel_bouillabaise")
                Case "薯蛋奶酥"
                    CS_image_Food_5_Source("F_sweet_potato_souffle")
                Case "龙虾浓汤"
                    CS_image_Food_5_Source("F_lobster_bisque")
                Case "汤"
                    CS_image_Food_5_Source("F_bisque")
                Case "咖啡"
                    CS_image_Food_5_Source("F_coffee")
                Case "海鲜牛排"
                    CS_image_Food_5_Source("F_surf_'n'_turf")
                Case "龙虾正餐"
                    CS_image_Food_5_Source("F_lobster_dinner")
                Case "香蕉冰淇淋"
                    CS_image_Food_5_Source("F_banana_pop")
                Case "加州卷"
                    CS_image_Food_5_Source("F_california_roll")
                Case "果冻冰淇淋"
                    CS_image_Food_5_Source("F_jelly-O_pop")
                Case "橘汁腌鱼"
                    CS_image_Food_5_Source("F_ceviche")
                Case "鱼翅汤"
                    CS_image_Food_5_Source("F_shark_fin_soup")
                Case "海鲜汤"
                    CS_image_Food_5_Source("F_seafood_gumbo")
                Case "鼹鼠鳄梨酱"
                    CS_image_Food_5_Source("F_guacamole")
                Case "花瓣沙拉"
                    CS_image_Food_5_Source("F_flower_salad")
                Case "冰淇淋"
                    CS_image_Food_5_Source("F_ice_cream")
                Case "西瓜冰"
                    CS_image_Food_5_Source("F_melonsicle")
                Case "水果杂烩"
                    CS_image_Food_5_Source("F_trail_mix")
                Case "辣椒酱"
                    CS_image_Food_5_Source("F_spicy_chili")
                Case "鳗鱼"
                    CS_image_Food_5_Source("F_unagi")
                Case "南瓜饼"
                    CS_image_Food_5_Source("F_pumpkin_cookie")
                Case "芝士蛋糕"
                    CS_image_Food_5_Source("F_powdercake")
                Case "曼德拉汤"
                    CS_image_Food_5_Source("F_mandrake_soup")
                Case "炸鱼条"
                    CS_image_Food_5_Source("F_fishsticks")
                Case "玉米饼包炸鱼"
                    CS_image_Food_5_Source("F_fish_tacos")
                Case "培根煎蛋"
                    CS_image_Food_5_Source("F_bacon_and_eggs")
                Case "火鸡正餐"
                    CS_image_Food_5_Source("F_turkey_dinner")
                Case "太妃糖"
                    CS_image_Food_5_Source("F_taffy")
                Case "华夫饼"
                    CS_image_Food_5_Source("F_waffles")
                Case "怪物千层饼"
                    CS_image_Food_5_Source("F_monster_lasagna")
                Case "饺子"
                    CS_image_Food_5_Source("F_pierogi")
                Case "肉串"
                    CS_image_Food_5_Source("F_kabobs")
                Case "蜜汁火腿"
                    CS_image_Food_5_Source("F_honey_ham")
                Case "甜蜜金砖"
                    CS_image_Food_5_Source("F_honey_nuggets")
                Case "奶油松饼"
                    CS_image_Food_5_Source("F_butter_muffin")
                Case "青蛙圆面包三明治"
                    CS_image_Food_5_Source("F_froggle_bunwich")
                Case "火龙果派"
                    CS_image_Food_5_Source("F_dragonpie")
                Case "香酥茄盒"
                    CS_image_Food_5_Source("F_stuffed_eggplant")
                Case "蔬菜杂烩"
                    CS_image_Food_5_Source("F_ratatouille")
                Case "果酱蜜饯"
                    CS_image_Food_5_Source("F_fist_full_of_jam")
                Case "水果拼盘"
                    CS_image_Food_5_Source("F_fruit_medley")
                Case "肉汤"
                    CS_image_Food_5_Source("F_meaty_stew")
                Case "肉丸"
                    CS_image_Food_5_Source("F_meatballs")
            End Select
        End If

        REM 选择按钮显示判断
        If CrockPotListIndex < 1 Then
            button_CS_Switch_Left.Visibility = Visibility.Collapsed
            button_CS_Switch_Right.Visibility = Visibility.Collapsed
        Else
            button_CS_Switch_Left.Visibility = Visibility.Visible
            button_CS_Switch_Right.Visibility = Visibility.Visible
            button_CS_Switch_Left.IsEnabled = False
            button_CS_Switch_Right.IsEnabled = True
        End If

        REM 显示食物名称
        TextBlock_CS_FoodName.Text = CS_F_name
        TextBlock_CS_FoodName.UpdateLayout()
        Dim CS_FoodName_MarginLeft As Integer
        CS_FoodName_MarginLeft = (Canvas_CookingSimulatorLeft.ActualWidth - TextBlock_CS_FoodName.ActualWidth) / 2
        Dim CS_FoodName_T As New Thickness()
        CS_FoodName_T.Top = 555
        CS_FoodName_T.Left = CS_FoodName_MarginLeft
        TextBlock_CS_FoodName.Margin = CS_FoodName_T
        REM 自动清空材料
        If checkBox_CS_AutoClean.IsChecked = True Then
            CS_Recipe_1 = ""
            CS_Recipe_2 = ""
            CS_Recipe_3 = ""
            CS_Recipe_4 = ""
            CS_image_Food_1.Source = Picture_Short_Name()
            CS_image_Food_2.Source = Picture_Short_Name()
            CS_image_Food_3.Source = Picture_Short_Name()
            CS_image_Food_4.Source = Picture_Short_Name()
        End If
    End Sub

    Private Sub button_CS_Food_1_5_click(sender As Object, e As RoutedEventArgs) Handles button_CS_Food_1_5.Click
        If CS_F_name <> "" Then
            LeftTabItem_Food.IsSelected = True
            Select Case CS_F_name
                Case "海鲜牛排"
                    Call button_F_surf_n_turf_click(Nothing, Nothing)
                Case "龙虾浓汤"
                    Call button_F_lobster_bisque_click(Nothing, Nothing)
                Case "咖啡"
                    Call button_F_coffee_click(Nothing, Nothing)
                Case "薯蛋奶酥"
                    Call button_F_sweet_potato_souffle_click(Nothing, Nothing)
                Case "怪物鞑靼"
                    Call button_F_monster_tartare_click(Nothing, Nothing)
                Case "贝类淡菜汤"
                    Call button_F_mussel_bouillabaise_click(Nothing, Nothing)
                Case "新鲜水果薄饼"
                    Call button_F_fresh_fruit_crepes_click(Nothing, Nothing)
                Case "汤"
                    Call button_F_bisque_click(Nothing, Nothing)
                Case "龙虾正餐"
                    Call button_F_lobster_dinner_click(Nothing, Nothing)
                Case "香蕉冰淇淋"
                    Call button_F_banana_pop_click(Nothing, Nothing)
                Case "鱼翅汤"
                    Call button_F_shark_fin_soup_click(Nothing, Nothing)
                Case "鳗鱼"
                    Call button_F_unagi_click(Nothing, Nothing)
                Case "橘汁腌鱼"
                    Call button_F_ceviche_click(Nothing, Nothing)
                Case "加州卷"
                    Call button_F_california_roll_click(Nothing, Nothing)
                Case "果冻冰淇淋"
                    Call button_F_jelly_O_pop_click(Nothing, Nothing)
                Case "海鲜汤"
                    Call button_F_seafood_gumbo_click(Nothing, Nothing)
                Case "南瓜饼"
                    Call button_F_pumpkin_cookie_click(Nothing, Nothing)
                Case "芝士蛋糕"
                    Call button_F_powdercake_click(Nothing, Nothing)
                Case "炸鱼条"
                    Call button_F_fishsticks_click(Nothing, Nothing)
                Case "玉米饼包炸鱼"
                    Call button_F_fish_tacos_click(Nothing, Nothing)
                Case "培根煎蛋"
                    Call button_F_bacon_and_eggs_click(Nothing, Nothing)
                Case "火鸡正餐"
                    Call button_F_turkey_dinner_click(Nothing, Nothing)
                Case "太妃糖"
                    Call button_F_taffy_click(Nothing, Nothing)
                Case "华夫饼"
                    Call button_F_waffles_click(Nothing, Nothing)
                Case "鼹鼠鳄梨酱"
                    Call button_F_guacamole_click(Nothing, Nothing)
                Case "花瓣沙拉"
                    Call button_F_flower_salad_click(Nothing, Nothing)
                Case "冰淇淋"
                    Call button_F_ice_cream_click(Nothing, Nothing)
                Case "西瓜冰"
                    Call button_F_melonsicle_click(Nothing, Nothing)
                Case "水果杂烩"
                    Call button_F_trail_mix_click(Nothing, Nothing)
                Case "辣椒酱"
                    Call button_F_spicy_chili_click(Nothing, Nothing)
                Case "曼德拉汤"
                    Call button_F_mandrake_soup_click(Nothing, Nothing)
                Case "怪物千层饼"
                    Call button_F_monster_lasagna_click(Nothing, Nothing)
                Case "饺子"
                    Call button_F_pierogi_click(Nothing, Nothing)
                Case "肉串"
                    Call button_F_kabobs_click(Nothing, Nothing)
                Case "蜜汁火腿"
                    Call button_F_honey_ham_click(Nothing, Nothing)
                Case "甜蜜金砖"
                    Call button_F_honey_nuggets_click(Nothing, Nothing)
                Case "奶油松饼"
                    Call button_F_butter_muffin_click(Nothing, Nothing)
                Case "青蛙圆面包三明治"
                    Call button_F_froggle_bunwich_click(Nothing, Nothing)
                Case "火龙果派"
                    Call button_F_dragonpie_click(Nothing, Nothing)
                Case "香酥茄盒"
                    Call button_F_stuffed_eggplant_click(Nothing, Nothing)
                Case "蔬菜杂烩"
                    Call button_F_ratatouille_click(Nothing, Nothing)
                Case "果酱蜜饯"
                    Call button_F_fist_full_of_jam_click(Nothing, Nothing)
                Case "水果拼盘"
                    Call button_F_fruit_medley_click(Nothing, Nothing)
                Case "肉汤"
                    Call button_F_meaty_stew_click(Nothing, Nothing)
                Case "肉丸"
                    Call button_F_meatballs_click(Nothing, Nothing)
                Case "湿腻焦糊"
                    Call button_F_wet_goop_click(Nothing, Nothing)
            End Select
        End If
    End Sub

    REM 食物DLC检测初始化
    Private Sub CS_DLC_Check_initialization()
        REM 肉类
        button_CS_tropical_fish.Visibility = Visibility.Collapsed
        button_CS_fish_morsel.Visibility = Visibility.Collapsed
        button_CS_cooked_fish_morsel.Visibility = Visibility.Collapsed
        button_CS_jellyfish.Visibility = Visibility.Collapsed
        button_CS_dead_jellyfish.Visibility = Visibility.Collapsed
        button_CS_cooked_jellyfish.Visibility = Visibility.Collapsed
        button_CS_dried_jellyfish.Visibility = Visibility.Collapsed
        button_CS_moleworm.Visibility = Visibility.Collapsed
        button_CS_limpets.Visibility = Visibility.Collapsed
        button_CS_cooked_limpets.Visibility = Visibility.Collapsed
        button_CS_mussel.Visibility = Visibility.Collapsed
        button_CS_cooked_mussel.Visibility = Visibility.Collapsed
        button_CS_dead_dogfish.Visibility = Visibility.Collapsed
        button_CS_wobster.Visibility = Visibility.Collapsed
        button_CS_raw_fish.Visibility = Visibility.Collapsed
        button_CS_fish_steak.Visibility = Visibility.Collapsed
        button_CS_shark_fin.Visibility = Visibility.Collapsed
        REM 蔬菜
        button_CS_cactus_flesh.Visibility = Visibility.Collapsed
        button_CS_cooked_cactus_flesh.Visibility = Visibility.Collapsed
        button_CS_cactus_flower.Visibility = Visibility.Collapsed
        button_CS_sweet_potato.Visibility = Visibility.Collapsed
        button_CS_cooked_sweet_potato.Visibility = Visibility.Collapsed
        button_CS_seaweed.Visibility = Visibility.Collapsed
        button_CS_roasted_seaweed.Visibility = Visibility.Collapsed
        button_CS_dried_seaweed.Visibility = Visibility.Collapsed
        REM 水果
        button_CS_juicy_berries.Visibility = Visibility.Collapsed
        button_CS_roasted_juicy_berries.Visibility = Visibility.Collapsed
        button_CS_roasted_coconut.Visibility = Visibility.Collapsed
        button_CS_halved_coconut.Visibility = Visibility.Collapsed
        REM 蛋类
        button_CS_doydoy_egg.Visibility = Visibility.Collapsed
        button_CS_fried_doydoy_egg.Visibility = Visibility.Collapsed
        REM 其他
        button_CS_electric_milk.Visibility = Visibility.Collapsed
        button_CS_roasted_birchnut.Visibility = Visibility.Collapsed
        button_CS_butterfly_wing_sw.Visibility = Visibility.Collapsed
        button_CS_coffee_beans.Visibility = Visibility.Collapsed
        button_CS_roasted_coffee_beans.Visibility = Visibility.Collapsed
    End Sub

    Private Sub CS_DLC_ROG_SHOW()
        REM 肉类
        button_CS_moleworm.Visibility = Visibility.Visible
        REM 蔬菜
        button_CS_cactus_flesh.Visibility = Visibility.Visible
        button_CS_cooked_cactus_flesh.Visibility = Visibility.Visible
        button_CS_cactus_flower.Visibility = Visibility.Visible
        REM 水果
        REM 其他
        button_CS_electric_milk.Visibility = Visibility.Visible
        button_CS_roasted_birchnut.Visibility = Visibility.Visible
    End Sub

    Private Sub CS_DLC_SW_SHOW()
        REM 肉类
        button_CS_tropical_fish.Visibility = Visibility.Visible
        button_CS_fish_morsel.Visibility = Visibility.Visible
        button_CS_cooked_fish_morsel.Visibility = Visibility.Visible
        button_CS_jellyfish.Visibility = Visibility.Visible
        button_CS_dead_jellyfish.Visibility = Visibility.Visible
        button_CS_cooked_jellyfish.Visibility = Visibility.Visible
        button_CS_dried_jellyfish.Visibility = Visibility.Visible
        button_CS_limpets.Visibility = Visibility.Visible
        button_CS_cooked_limpets.Visibility = Visibility.Visible
        button_CS_mussel.Visibility = Visibility.Visible
        button_CS_cooked_mussel.Visibility = Visibility.Visible
        button_CS_dead_dogfish.Visibility = Visibility.Visible
        button_CS_wobster.Visibility = Visibility.Visible
        button_CS_raw_fish.Visibility = Visibility.Visible
        button_CS_fish_steak.Visibility = Visibility.Visible
        button_CS_shark_fin.Visibility = Visibility.Visible
        REM 蔬菜
        button_CS_sweet_potato.Visibility = Visibility.Visible
        button_CS_cooked_sweet_potato.Visibility = Visibility.Visible
        button_CS_seaweed.Visibility = Visibility.Visible
        button_CS_roasted_seaweed.Visibility = Visibility.Visible
        button_CS_dried_seaweed.Visibility = Visibility.Visible
        REM 水果
        button_CS_roasted_coconut.Visibility = Visibility.Visible
        button_CS_halved_coconut.Visibility = Visibility.Visible
        REM 蛋类
        button_CS_doydoy_egg.Visibility = Visibility.Visible
        button_CS_fried_doydoy_egg.Visibility = Visibility.Visible
        REM 其他
        button_CS_butterfly_wing_sw.Visibility = Visibility.Visible
        button_CS_coffee_beans.Visibility = Visibility.Visible
        button_CS_roasted_coffee_beans.Visibility = Visibility.Visible
    End Sub

    Private Sub CS_DLC_DST_SHOW()
        REM 肉类
        button_CS_moleworm.Visibility = Visibility.Visible
        REM 蔬菜
        button_CS_cactus_flesh.Visibility = Visibility.Visible
        button_CS_cooked_cactus_flesh.Visibility = Visibility.Visible
        button_CS_cactus_flower.Visibility = Visibility.Visible
        REM 水果
        button_CS_juicy_berries.Visibility = Visibility.Visible
        button_CS_roasted_juicy_berries.Visibility = Visibility.Visible
        REM 其他
        button_CS_electric_milk.Visibility = Visibility.Visible
        button_CS_roasted_birchnut.Visibility = Visibility.Visible
    End Sub

    REM 食物DLC检测
    Private Sub CS_DLC_Check()
        CS_DLC_Check_initialization()

        Dim CS_ROG__ As SByte
        Dim CS_SW__ As SByte
        Dim CS_DST__ As SByte
        If checkBox_CS_DLC_ROG.IsChecked = True Then
            CS_ROG__ = 1
        Else
            CS_ROG__ = 0
        End If
        If checkBox_CS_DLC_SW.IsChecked = True Then
            CS_SW__ = 2
        Else
            CS_SW__ = 0
        End If
        If checkBox_CS_DLC_DST.IsChecked = True Then
            CS_DST__ = 4
        Else
            CS_DST__ = 0
        End If
        CS_ROG_SW_DST = CS_ROG__ + CS_SW__ + CS_DST__
        If CS_ROG_SW_DST = 0 Then
            MsgBox("至少选择一项！")
            checkBox_CS_DLC_ROG.IsChecked = True
            CS_DLC_Check()
        Else
            Select Case CS_ROG_SW_DST
                Case 1
                    CS_DLC_ROG_SHOW()
                    WrapPanel_CS_meats.Height = 170
                    WrapPanel_CS_others.Height = 90
                    WrapPanel_CookingSimulator.Height = 940
                    Reg_Write("Cooking_Simulator", 1)
                Case 2
                    CS_DLC_SW_SHOW()
                    WrapPanel_CS_meats.Height = 250
                    WrapPanel_CS_others.Height = 90
                    WrapPanel_CookingSimulator.Height = 1020
                    Reg_Write("Cooking_Simulator", 2)
                Case 3
                    CS_DLC_ROG_SHOW()
                    CS_DLC_SW_SHOW()
                    WrapPanel_CS_meats.Height = 330
                    WrapPanel_CS_others.Height = 170
                    WrapPanel_CookingSimulator.Height = 1180
                    Reg_Write("Cooking_Simulator", 3)
                Case 4
                    CS_DLC_DST_SHOW()
                    WrapPanel_CS_meats.Height = 170
                    WrapPanel_CS_others.Height = 90
                    WrapPanel_CookingSimulator.Height = 940
                    Reg_Write("Cooking_Simulator", 4)
                Case 5
                    CS_DLC_ROG_SHOW()
                    CS_DLC_DST_SHOW()
                    WrapPanel_CS_meats.Height = 170
                    WrapPanel_CS_others.Height = 90
                    WrapPanel_CookingSimulator.Height = 940
                    Reg_Write("Cooking_Simulator", 5)
                Case 6
                    CS_DLC_SW_SHOW()
                    CS_DLC_DST_SHOW()
                    WrapPanel_CS_meats.Height = 330
                    WrapPanel_CS_others.Height = 170
                    WrapPanel_CookingSimulator.Height = 1180
                    Reg_Write("Cooking_Simulator", 6)
                Case 7
                    CS_DLC_ROG_SHOW()
                    CS_DLC_SW_SHOW()
                    CS_DLC_DST_SHOW()
                    WrapPanel_CS_meats.Height = 330
                    WrapPanel_CS_others.Height = 170
                    WrapPanel_CookingSimulator.Height = 1180
                    Reg_Write("Cooking_Simulator", 7)
            End Select
        End If
    End Sub

    Private Sub CS_CHECKBOX_()
        button_CS_Switch_Left.Visibility = Visibility.Collapsed
        button_CS_Switch_Right.Visibility = Visibility.Collapsed
        CS_Recipe_1 = ""
        CS_Recipe_2 = ""
        CS_Recipe_3 = ""
        CS_Recipe_4 = ""
        CS_image_Food_1.Source = Picture_Short_Name()
        CS_image_Food_2.Source = Picture_Short_Name()
        CS_image_Food_3.Source = Picture_Short_Name()
        CS_image_Food_4.Source = Picture_Short_Name()
        CS_image_Food_5.Source = Picture_Short_Name()
        TextBlock_CS_FoodName.Text = ""
        CS_F_name = ""
    End Sub

    Private Sub button_CS_Reset_click(sender As Object, e As RoutedEventArgs) Handles button_CS_Reset.Click
        CS_CHECKBOX_()
    End Sub

    Private Sub checkBox_CS_DLC_ROG_click(sender As Object, e As RoutedEventArgs) Handles checkBox_CS_DLC_ROG.Click
        CS_CHECKBOX_()
        CS_DLC_Check()
    End Sub

    Private Sub FL_button_CS_DLC_ROG_click(sender As Object, e As RoutedEventArgs) Handles FL_button_CS_DLC_ROG.Click
        If checkBox_CS_DLC_ROG.IsChecked = True Then
            checkBox_CS_DLC_ROG.IsChecked = False
        Else
            checkBox_CS_DLC_ROG.IsChecked = True
        End If
        checkBox_CS_DLC_ROG_click(Nothing, Nothing)
    End Sub

    Private Sub checkBox_CS_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles checkBox_CS_DLC_SW.Click
        CS_CHECKBOX_()
        CS_DLC_Check()
    End Sub

    Private Sub FL_button_CS_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles FL_button_CS_DLC_SW.Click
        If checkBox_CS_DLC_SW.IsChecked = True Then
            checkBox_CS_DLC_SW.IsChecked = False
        Else
            checkBox_CS_DLC_SW.IsChecked = True
        End If
        checkBox_CS_DLC_SW_click(Nothing, Nothing)
    End Sub

    Private Sub checkBox_CS_DLC_DST_click(sender As Object, e As RoutedEventArgs) Handles checkBox_CS_DLC_DST.Click
        CS_CHECKBOX_()
        CS_DLC_Check()
    End Sub

    Private Sub FL_button_CS_DLC_DST_click(sender As Object, e As RoutedEventArgs) Handles FL_button_CS_DLC_DST.Click
        If checkBox_CS_DLC_DST.IsChecked = True Then
            checkBox_CS_DLC_DST.IsChecked = False
        Else
            checkBox_CS_DLC_DST.IsChecked = True
        End If
        checkBox_CS_DLC_DST_click(Nothing, Nothing)
    End Sub

    REM ------------------左侧面板(生物)------------------
    Private Sub A_Show(A_Name As String, A_EnName As String, A_picture As String, A_DLC As String, A_DLC_ROG As SByte, A_DLC_SW As SByte, A_DLC_DST As SByte, A_Health As Single, A_HealthDST As Single, A_Attack As Single, A_AttackPeriod As Single, A_AttackRange As Single, A_Speed As Single, A_SpeedRun As Single, A_Criticality As Single, A_Sanity As Single, A_ActiveAttack As Boolean, A_TeamWork As Boolean, A_Introduce As String, A_LootHeight As Integer, A_SpecialAbilityHeight As Integer)
        REM -----------------特殊生物判定---------------
        ScrollViewer_AnimalLeft.Visibility = Visibility.Visible
        ScrollViewer_AnimalLeft_Krampus.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Apackim_baggims.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_PigKing.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Yaarctopus.Visibility = Visibility.Collapsed
        If A_Name = "坎普斯" Or A_Name = "船难坎普斯" Then
            button_A_Krampus_table.Visibility = Visibility.Visible
        Else
            button_A_Krampus_table.Visibility = Visibility.Collapsed
        End If
        If A_Name = "鹈鹕" Or A_Name = "胖鹈鹕" Then
            button_A_Apackim_baggims_table.Visibility = Visibility.Visible
        Else
            button_A_Apackim_baggims_table.Visibility = Visibility.Collapsed
        End If
        REM ------------------生物名字------------------
        AL_textBlock_AnimalName.Text = A_Name
        AL_textBlock_AnimalName.UpdateLayout()
        Dim A_N_MarginLeft As Integer
        A_N_MarginLeft = (264 - AL_textBlock_AnimalName.ActualWidth) / 2
        Dim A_N_T As New Thickness()
        A_N_T.Top = 110
        A_N_T.Left = A_N_MarginLeft
        AL_textBlock_AnimalName.Margin = A_N_T

        AL_textBlock_AnimalEnName.Text = A_EnName
        AL_textBlock_AnimalEnName.UpdateLayout()
        Dim A_EnN_MarginLeft As Integer
        A_EnN_MarginLeft = (264 - AL_textBlock_AnimalEnName.ActualWidth) / 2
        Dim A_EnN_T As New Thickness()
        A_EnN_T.Top = 130
        A_EnN_T.Left = A_EnN_MarginLeft
        AL_textBlock_AnimalEnName.Margin = A_EnN_T
        REM ------------------生物图片------------------
        AL_image_AnimalPicture.Source = Picture_Short_Name(Res_Short_Name(A_picture))
        REM ------------------生物DLC-------------------
        If A_DLC = "ROG" Then
            AL_image_A_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf A_DLC = "SW" Then
            AL_image_A_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf A_DLC = "DST" Then
            AL_image_A_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            AL_image_A_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------生物属性------------------
        If A_Health = 0 Then
            TextBlock_A_HealthValue.Text = "无"
        Else
            TextBlock_A_HealthValue.Text = A_Health
        End If
        If A_HealthDST = 0 Then
            TextBlock_A_HealthDSTValue.Text = "无"
        Else
            TextBlock_A_HealthDSTValue.Text = A_HealthDST
        End If
        If A_Attack = 0 Then
            TextBlock_A_AttackValue.Text = "无"
        Else
            TextBlock_A_AttackValue.Text = A_Attack
        End If
        If A_AttackPeriod = 0 Then
            TextBlock_A_AttackPeriodValue.Text = "无"
        Else
            TextBlock_A_AttackPeriodValue.Text = A_AttackPeriod
        End If
        If A_AttackRange = 0 Then
            TextBlock_A_AttackRangeValue.Text = "无"
        Else
            TextBlock_A_AttackRangeValue.Text = A_AttackRange
        End If
        If A_Speed = 0 Then
            TextBlock_A_SpeedValue.Text = "无"
        Else
            TextBlock_A_SpeedValue.Text = A_Speed
        End If
        If A_SpeedRun = 0 Then
            TextBlock_A_SpeedRunValue.Text = "无"
        Else
            TextBlock_A_SpeedRunValue.Text = A_SpeedRun
        End If
        If A_Criticality = 0 Then
            TextBlock_A_CriticalityValue.Text = "无"
        Else
            TextBlock_A_CriticalityValue.Text = A_Criticality
        End If
        If A_Sanity = 0 Then
            TextBlock_A_SanityValue.Text = "无"
        Else
            TextBlock_A_SanityValue.Text = A_Sanity
        End If
        If A_Sanity < 0 Then
            TextBlock_A_SanityValue.Foreground = Brushes.Red
        Else
            TextBlock_A_SanityValue.Foreground = Brushes.Black
        End If

        Image_A_PB_Health.Width = A_Health * 0.051
        If A_HealthDST > 10000 Then
            Image_A_PB_HealthDST.Width = 155
        Else
            Image_A_PB_HealthDST.Width = A_HealthDST * 0.0155
        End If
        Image_A_PB_Attack.Width = A_Attack * 1.0333
        If A_AttackPeriod > 7.5 Then
            Image_A_PB_AttackPeriod.Width = 155
        Else
            Image_A_PB_AttackPeriod.Width = A_AttackPeriod * 20.6666
        End If
        Image_A_PB_AttackRange.Width = A_AttackRange * 6.2
        Image_A_PB_Speed.Width = A_Speed * 15.5
        Image_A_PB_SpeedRun.Width = A_SpeedRun * 9.1176
        Image_A_PB_Criticality.Width = A_Criticality * 15.5
        Dim A_Sanity_T As New Thickness()
        If A_Sanity < 0 Then
            Image_A_PB_Sanity.Width = -A_Sanity * 0.3875
            A_Sanity_T.Top = 0
            A_Sanity_T.Left = 155 - Image_A_PB_Sanity.Width
            Image_A_PB_Sanity.Margin = A_Sanity_T
        ElseIf A_Sanity = 0 Then
            Image_A_PB_Sanity.Width = 0
            A_Sanity_T.Top = 0
            A_Sanity_T.Left = 0
            Image_A_PB_Sanity.Margin = A_Sanity_T
        Else
            Image_A_PB_Sanity.Width = A_Sanity * 6.2
            A_Sanity_T.Top = 0
            A_Sanity_T.Left = 0
            Image_A_PB_Sanity.Margin = A_Sanity_T
        End If
        If A_ActiveAttack = False Then
            Image_A_PB_ActiveAttack.Visibility = Visibility.Collapsed
        Else
            Image_A_PB_ActiveAttack.Visibility = Visibility.Visible
        End If
        If A_TeamWork = False Then
            Image_A_PB_TeamWork.Visibility = Visibility.Collapsed
        Else
            Image_A_PB_TeamWork.Visibility = Visibility.Visible
        End If
        REM ------------------存在版本-------------------
        AL_textBlock_A_DLC_1.Foreground = Brushes.Black
        AL_textBlock_A_DLC_2.Foreground = Brushes.Black
        AL_textBlock_A_DLC_3.Foreground = Brushes.Black
        If A_DLC_ROG = 0 Then
            AL_textBlock_A_DLC_1.Foreground = Brushes.Silver
        End If
        If A_DLC_SW = 0 Then
            AL_textBlock_A_DLC_2.Foreground = Brushes.Silver
        End If
        If A_DLC_DST = 0 Then
            AL_textBlock_A_DLC_3.Foreground = Brushes.Silver
        End If
        REM ------------------生物简介-------------------
        TextBlock_A_Introduce.Text = A_Introduce
        TextBlock_A_Introduce.Height = SetTextBlockHeight(A_Introduce, 20)
        Dim A_IntroduceTop As New Thickness()
        A_IntroduceTop.Left = 10
        Select Case A_SpecialAbilityHeight
            Case 0
                Select Case A_LootHeight
                    Case 0
                        A_IntroduceTop.Top = 460
                    Case 1
                        A_IntroduceTop.Top = 505
                    Case 2
                        A_IntroduceTop.Top = 537
                    Case 3
                        A_IntroduceTop.Top = 569
                    Case 4
                        A_IntroduceTop.Top = 601
                    Case 5
                        A_IntroduceTop.Top = 633
                    Case 6
                        A_IntroduceTop.Top = 665
                    Case 7
                        A_IntroduceTop.Top = 697
                    Case 8
                        A_IntroduceTop.Top = 825
                End Select
            Case 1
                A_IntroduceTop.Top = TextBlock_A_Special_Ability_1.Margin.Top + SetTextBlockHeight(TextBlock_A_Special_Ability_1.Text, 20) + 30
            Case 2
                A_IntroduceTop.Top = TextBlock_A_Special_Ability_2.Margin.Top + SetTextBlockHeight(TextBlock_A_Special_Ability_2.Text, 20) + 30
            Case 3
                A_IntroduceTop.Top = TextBlock_A_Special_Ability_3.Margin.Top + SetTextBlockHeight(TextBlock_A_Special_Ability_3.Text, 20) + 30
            Case 4
                A_IntroduceTop.Top = TextBlock_A_Special_Ability_4.Margin.Top + SetTextBlockHeight(TextBlock_A_Special_Ability_4.Text, 20) + 30
            Case 5
                A_IntroduceTop.Top = TextBlock_A_Special_Ability_5.Margin.Top + SetTextBlockHeight(TextBlock_A_Special_Ability_5.Text, 20) + 30
            Case 6
                A_IntroduceTop.Top = TextBlock_A_Special_Ability_6.Margin.Top + SetTextBlockHeight(TextBlock_A_Special_Ability_6.Text, 20) + 30
        End Select
        TextBlock_A_Introduce.Margin = A_IntroduceTop
        REM ------------------生物左侧面板高度最终确定-------------------
        Canvas_AnimalLeft.Height = TextBlock_A_Introduce.Margin.Top + TextBlock_A_Introduce.Height + 30
        REM ------------------滚动条回到最顶端-------------------
        ScrollViewer_AnimalLeft.ScrollToVerticalOffset(0)
    End Sub

    Public Function A_Loot(Optional LootPicture_1 As String = "", Optional LootNum_1 As String = "", Optional LootPicture_2 As String = "", Optional LootNum_2 As String = "", Optional LootPicture_3 As String = "", Optional LootNum_3 As String = "", Optional LootPicture_4 As String = "", Optional LootNum_4 As String = "", Optional LootPicture_5 As String = "", Optional LootNum_5 As String = "", Optional LootPicture_6 As String = "", Optional LootNum_6 As String = "", Optional LootPicture_7 As String = "", Optional LootNum_7 As String = "", Optional LootPicture_8 As String = "", Optional LootNum_8 As String = "", Optional LootPicture_9 As String = "", Optional LootNum_9 As String = "", Optional LootPicture_10 As String = "", Optional LootNum_10 As String = "", Optional LootPicture_11 As String = "", Optional LootNum_11 As String = "")
        REM ------------------初始化------------------
        TextBlock_A_Loot.Visibility = Visibility.Visible
        LootTrapOrBugNet_1.Visibility = Visibility.Collapsed
        LootTrapOrBugNet_2.Visibility = Visibility.Collapsed
        LootTrapOrBugNet_3.Visibility = Visibility.Collapsed
        LootTrapOrBugNet_4.Visibility = Visibility.Collapsed
        LootTrapOrBugNet_5.Visibility = Visibility.Collapsed
        button_A_LootTrapOrBugNetP_1.Visibility = Visibility.Collapsed
        button_A_LootTrapOrBugNetP_2.Visibility = Visibility.Collapsed
        button_A_LootTrapOrBugNetP_3.Visibility = Visibility.Collapsed
        button_A_LootTrapOrBugNetP_4.Visibility = Visibility.Collapsed
        button_A_LootTrapOrBugNetP_5.Visibility = Visibility.Collapsed
        LootNumT_1.Visibility = Visibility.Collapsed
        LootNumT_2.Visibility = Visibility.Collapsed
        LootNumT_3.Visibility = Visibility.Collapsed
        LootNumT_4.Visibility = Visibility.Collapsed
        LootNumT_5.Visibility = Visibility.Collapsed
        LootNumT_6.Visibility = Visibility.Collapsed
        LootNumT_7.Visibility = Visibility.Collapsed
        LootNumT_8.Visibility = Visibility.Collapsed
        LootNumT_9.Visibility = Visibility.Collapsed
        LootNumT_10.Visibility = Visibility.Collapsed
        LootNumT_11.Visibility = Visibility.Collapsed
        button_A_LootP_1.Visibility = Visibility.Collapsed
        button_A_LootP_2.Visibility = Visibility.Collapsed
        button_A_LootP_3.Visibility = Visibility.Collapsed
        button_A_LootP_4.Visibility = Visibility.Collapsed
        button_A_LootP_5.Visibility = Visibility.Collapsed
        button_A_LootP_6.Visibility = Visibility.Collapsed
        button_A_LootP_7.Visibility = Visibility.Collapsed
        button_A_LootP_8.Visibility = Visibility.Collapsed
        button_A_LootP_9.Visibility = Visibility.Collapsed
        button_A_LootP_10.Visibility = Visibility.Collapsed
        button_A_LootP_11.Visibility = Visibility.Collapsed
        REM ------------------代码------------------
        '判断开始
        If LootPicture_1 = "" And LootNum_1 = "" Then
            TextBlock_A_Loot.Visibility = Visibility.Collapsed
            Return 0
        End If
        '第1个掉落
        If LootPicture_1 <> "" Or LootNum_1 <> "" Then
            A_LootP_Select_1 = LootPicture_1
            button_A_LootP_1.Visibility = Visibility.Visible
            LootP_1.Source = Picture_Short_Name(Res_Short_Name(LootPicture_1))
            If Char.IsNumber(GetChar(LootNum_1, 1)) Then
                LootNumT_1.Visibility = Visibility.Visible
                LootNumT_1.Text = "×" & LootNum_1
            ElseIf LootNum_1 = "捕虫网" Then
                LootTrapOrBugNet_1.Visibility = Visibility.Visible
                button_A_LootTrapOrBugNetP_1.Visibility = Visibility.Visible
                A_LootTrapOrBugNetP_Select_1 = "S_bug_net"
                LootTrapOrBugNetP_1.Source = Picture_Short_Name(Res_Short_Name("S_bug_net"))
            Else
                LootNumT_1.Text = LootNum_1
                LootNumT_1.Visibility = Visibility.Visible
            End If
            '第2个掉落
            If LootPicture_2 <> "" Or LootNum_2 <> "" Then
                A_LootP_Select_2 = LootPicture_2
                button_A_LootP_2.Visibility = Visibility.Visible
                LootP_2.Source = Picture_Short_Name(Res_Short_Name(LootPicture_2))
                If Char.IsNumber(GetChar(LootNum_2, 1)) Then
                    LootNumT_2.Text = "×" & LootNum_2
                    LootNumT_2.Visibility = Visibility.Visible
                ElseIf LootNum_2 = "陷阱" Or LootNum_2 = "捕虫网" Or LootNum_2 = "拖网" Then
                    LootTrapOrBugNet_2.Visibility = Visibility.Visible
                    button_A_LootTrapOrBugNetP_2.Visibility = Visibility.Visible
                    If LootNum_2 = "陷阱" Then
                        A_LootTrapOrBugNetP_Select_2 = "S_trap"
                        LootTrapOrBugNetP_2.Source = Picture_Short_Name(Res_Short_Name("S_trap"))
                    ElseIf LootNum_2 = "捕虫网" Then
                        A_LootTrapOrBugNetP_Select_2 = "S_bug_net"
                        LootTrapOrBugNetP_2.Source = Picture_Short_Name(Res_Short_Name("S_bug_net"))
                    Else
                        A_LootTrapOrBugNetP_Select_2 = "S_trawl_net"
                        LootTrapOrBugNetP_2.Source = Picture_Short_Name(Res_Short_Name("S_trawl_net"))
                    End If
                Else
                    LootNumT_2.Text = LootNum_2
                    LootNumT_2.Visibility = Visibility.Visible
                End If
                '第3个掉落
                If LootPicture_3 <> "" Or LootNum_3 <> "" Then
                    A_LootP_Select_3 = LootPicture_3
                    button_A_LootP_3.Visibility = Visibility.Visible
                    LootP_3.Source = Picture_Short_Name(Res_Short_Name(LootPicture_3))
                    If Char.IsNumber(GetChar(LootNum_3, 1)) Then
                        LootNumT_3.Text = "×" & LootNum_3
                        LootNumT_3.Visibility = Visibility.Visible
                    ElseIf LootNum_3 = "捕虫网" Or LootNum_3 = "捕鸟器" Or LootNum_3 = "铲子" Or LootNum_3 = "锤子" Then
                        LootTrapOrBugNet_3.Visibility = Visibility.Visible
                        button_A_LootTrapOrBugNetP_3.Visibility = Visibility.Visible
                        If LootNum_3 = "捕虫网" Then
                            A_LootTrapOrBugNetP_Select_3 = "S_bug_net"
                            LootTrapOrBugNetP_3.Source = Picture_Short_Name(Res_Short_Name("S_bug_net"))
                        ElseIf LootNum_3 = "铲子" Then
                            A_LootTrapOrBugNetP_Select_3 = "S_shovel"
                            LootTrapOrBugNetP_3.Source = Picture_Short_Name(Res_Short_Name("S_shovel"))
                        ElseIf LootNum_3 = "锤子" Then
                            A_LootTrapOrBugNetP_Select_3 = "S_hammer"
                            LootTrapOrBugNetP_3.Source = Picture_Short_Name(Res_Short_Name("S_hammer"))
                        Else
                            A_LootTrapOrBugNetP_Select_3 = "S_bird_trap"
                            LootTrapOrBugNetP_3.Source = Picture_Short_Name(Res_Short_Name("S_bird_trap"))
                        End If
                    Else
                        LootNumT_3.Text = LootNum_3
                        LootNumT_3.Visibility = Visibility.Visible
                    End If
                    '第4个掉落
                    If LootPicture_4 <> "" Or LootNum_4 <> "" Then
                        A_LootP_Select_4 = LootPicture_4
                        button_A_LootP_4.Visibility = Visibility.Visible
                        LootP_4.Source = Picture_Short_Name(Res_Short_Name(LootPicture_4))
                        If Char.IsNumber(GetChar(LootNum_4, 1)) Then
                            LootNumT_4.Text = "×" & LootNum_4
                            LootNumT_4.Visibility = Visibility.Visible
                        ElseIf LootNum_4 = "陷阱" Or LootNum_4 = "捕鸟器" Then
                            LootTrapOrBugNet_4.Visibility = Visibility.Visible
                            button_A_LootTrapOrBugNetP_4.Visibility = Visibility.Visible
                            If LootNum_4 = "陷阱" Then
                                A_LootTrapOrBugNetP_Select_4 = "S_trap"
                                LootTrapOrBugNetP_4.Source = Picture_Short_Name(Res_Short_Name("S_trap"))
                            Else
                                A_LootTrapOrBugNetP_Select_4 = "S_bird_trap"
                                LootTrapOrBugNetP_4.Source = Picture_Short_Name(Res_Short_Name("S_bird_trap"))
                            End If
                        Else
                            LootNumT_4.Text = LootNum_4
                            LootNumT_4.Visibility = Visibility.Visible
                        End If
                        '第5个掉落
                        If LootPicture_5 <> "" Or LootNum_5 <> "" Then
                            A_LootP_Select_5 = LootPicture_5
                            button_A_LootP_5.Visibility = Visibility.Visible
                            LootP_5.Source = Picture_Short_Name(Res_Short_Name(LootPicture_5))
                            If Char.IsNumber(GetChar(LootNum_5, 1)) Then
                                LootNumT_5.Text = "×" & LootNum_5
                                LootNumT_5.Visibility = Visibility.Visible
                            ElseIf LootNum_5 = "剃刀" Then
                                LootTrapOrBugNet_5.Visibility = Visibility.Visible
                                A_LootTrapOrBugNetP_Select_5 = "S_razor"
                                button_A_LootTrapOrBugNetP_5.Visibility = Visibility.Visible
                                LootTrapOrBugNetP_5.Source = Picture_Short_Name(Res_Short_Name("S_razor"))
                            Else
                                LootNumT_5.Text = LootNum_5
                                LootNumT_5.Visibility = Visibility.Visible
                            End If
                            '第6个掉落
                            If LootPicture_6 <> "" Or LootNum_6 <> "" Then
                                A_LootP_Select_6 = LootPicture_6
                                button_A_LootP_6.Visibility = Visibility.Visible
                                LootNumT_6.Visibility = Visibility.Visible
                                LootP_6.Source = Picture_Short_Name(Res_Short_Name(LootPicture_6))
                                If Char.IsNumber(GetChar(LootNum_6, 1)) Then
                                    LootNumT_6.Text = "×" & LootNum_6
                                Else
                                    LootNumT_6.Text = LootNum_6
                                End If
                                '第7个掉落
                                If LootPicture_7 <> "" Or LootNum_7 <> "" Then
                                    A_LootP_Select_7 = LootPicture_7
                                    button_A_LootP_7.Visibility = Visibility.Visible
                                    LootNumT_7.Visibility = Visibility.Visible
                                    LootP_7.Source = Picture_Short_Name(Res_Short_Name(LootPicture_7))
                                    If Char.IsNumber(GetChar(LootNum_7, 1)) Then
                                        LootNumT_7.Text = "×" & LootNum_7
                                    Else
                                        LootNumT_7.Text = LootNum_7
                                    End If
                                    '第8个掉落
                                    If LootPicture_8 <> "" Or LootNum_8 <> "" Then
                                        button_A_LootP_8.Visibility = Visibility.Visible
                                        LootNumT_8.Visibility = Visibility.Visible
                                        button_A_LootP_9.Visibility = Visibility.Visible
                                        LootNumT_9.Visibility = Visibility.Visible
                                        button_A_LootP_10.Visibility = Visibility.Visible
                                        LootNumT_10.Visibility = Visibility.Visible
                                        button_A_LootP_11.Visibility = Visibility.Visible
                                        LootNumT_11.Visibility = Visibility.Visible
                                        A_LootP_Select_8 = LootPicture_8
                                        A_LootP_Select_9 = LootPicture_9
                                        A_LootP_Select_10 = LootPicture_10
                                        A_LootP_Select_11 = LootPicture_11
                                        LootP_8.Source = Picture_Short_Name(Res_Short_Name(LootPicture_8))
                                        LootP_9.Source = Picture_Short_Name(Res_Short_Name(LootPicture_9))
                                        LootP_10.Source = Picture_Short_Name(Res_Short_Name(LootPicture_10))
                                        LootP_11.Source = Picture_Short_Name(Res_Short_Name(LootPicture_11))
                                        LootNumT_8.Text = "×" & LootNum_8
                                        LootNumT_9.Text = "×" & LootNum_9
                                        LootNumT_10.Text = "×" & LootNum_10
                                        LootNumT_11.Text = "×" & LootNum_11
                                        Return 8
                                    Else
                                        Return 7
                                    End If
                                Else
                                    Return 6
                                End If
                            Else
                                Return 5
                            End If
                        Else
                            Return 4
                        End If
                    Else
                        Return 3
                    End If
                Else
                    Return 2
                End If
            Else
                Return 1
            End If
        End If
    End Function

    Public Function A_SpecialAbility(LootHeight As Integer, Optional SpecialAbility_1 As String = "", Optional SpecialAbility_2 As String = "", Optional SpecialAbility_3 As String = "", Optional SpecialAbility_4 As String = "", Optional SpecialAbility_5 As String = "", Optional SpecialAbility_6 As String = "")
        TextBlock_A_Special_Ability.Visibility = Visibility.Visible
        TextBlock_A_Special_Ability_1.Visibility = Visibility.Collapsed
        TextBlock_A_Special_Ability_2.Visibility = Visibility.Collapsed
        TextBlock_A_Special_Ability_3.Visibility = Visibility.Collapsed
        TextBlock_A_Special_Ability_4.Visibility = Visibility.Collapsed
        TextBlock_A_Special_Ability_5.Visibility = Visibility.Collapsed
        TextBlock_A_Special_Ability_6.Visibility = Visibility.Collapsed

        Dim HeightAdd As Double
        Select Case LootHeight
            Case 0
                HeightAdd = -70
            Case 1
                HeightAdd = 0
            Case 2
                HeightAdd = 32
            Case 3
                HeightAdd = 64
            Case 4
                HeightAdd = 96
            Case 5
                HeightAdd = 128
            Case 6
                HeightAdd = 160
            Case 7
                HeightAdd = 192
            Case 8
                HeightAdd = 320
        End Select

        Dim A_SpecialAbility_Top As New Thickness()
        A_SpecialAbility_Top.Left = 10
        A_SpecialAbility_Top.Top = 510 + HeightAdd
        TextBlock_A_Special_Ability.Margin = A_SpecialAbility_Top

        Dim TextBlockHeight_1 As Double
        Dim TextBlockHeight_2 As Double
        Dim TextBlockHeight_3 As Double
        Dim TextBlockHeight_4 As Double
        Dim TextBlockHeight_5 As Double
        Dim TextBlockHeight_6 As Double
        If SpecialAbility_1 = "" Then
            TextBlock_A_Special_Ability.Visibility = Visibility.Collapsed
            Return 0
        End If
        If SpecialAbility_1 <> "" Then
            TextBlockHeight_1 = A_SATextBlock(TextBlock_A_Special_Ability_1, SpecialAbility_1)
            A_SpecialAbility_Top.Top = TextBlock_A_Special_Ability.Margin.Top + 15
            TextBlock_A_Special_Ability_1.Margin = A_SpecialAbility_Top
            TextBlock_A_Special_Ability_1.TextWrapping = TextWrapping.Wrap
            If SpecialAbility_2 <> "" Then
                TextBlockHeight_2 = A_SATextBlock(TextBlock_A_Special_Ability_2, SpecialAbility_2)
                A_SpecialAbility_Top.Top = TextBlock_A_Special_Ability_1.Margin.Top + TextBlockHeight_1
                TextBlock_A_Special_Ability_2.Margin = A_SpecialAbility_Top
                If SpecialAbility_3 <> "" Then
                    TextBlockHeight_3 = A_SATextBlock(TextBlock_A_Special_Ability_3, SpecialAbility_3)
                    A_SpecialAbility_Top.Top = TextBlock_A_Special_Ability_2.Margin.Top + TextBlockHeight_2
                    TextBlock_A_Special_Ability_3.Margin = A_SpecialAbility_Top
                    If SpecialAbility_4 <> "" Then
                        TextBlockHeight_4 = A_SATextBlock(TextBlock_A_Special_Ability_4, SpecialAbility_4)
                        A_SpecialAbility_Top.Top = TextBlock_A_Special_Ability_3.Margin.Top + TextBlockHeight_3
                        TextBlock_A_Special_Ability_4.Margin = A_SpecialAbility_Top
                        If SpecialAbility_5 <> "" Then
                            TextBlockHeight_5 = A_SATextBlock(TextBlock_A_Special_Ability_5, SpecialAbility_5)
                            A_SpecialAbility_Top.Top = TextBlock_A_Special_Ability_4.Margin.Top + TextBlockHeight_4
                            TextBlock_A_Special_Ability_5.Margin = A_SpecialAbility_Top
                            If SpecialAbility_6 <> "" Then
                                TextBlockHeight_6 = A_SATextBlock(TextBlock_A_Special_Ability_6, SpecialAbility_6)
                                A_SpecialAbility_Top.Top = TextBlock_A_Special_Ability_5.Margin.Top + TextBlockHeight_5
                                TextBlock_A_Special_Ability_6.Margin = A_SpecialAbility_Top
                                Return 6
                            Else
                                Return 5
                            End If
                        Else
                            Return 4
                        End If
                    Else
                        Return 3
                    End If
                Else
                    Return 2
                End If
            Else
                Return 1
            End If
        End If
    End Function

    Public Function A_SATextBlock(TextBlockName As Object, Text As String)
        TextBlockName.Visibility = Visibility.Visible
        Dim TextBlockHeight As Double = SetTextBlockHeight(Text, 20)
        TextBlockName.height = TextBlockHeight
        TextBlockName.text = Text
        Return TextBlockHeight
    End Function

    Private Sub button_A_Krampus_table_click(sender As Object, e As RoutedEventArgs) Handles button_A_Krampus_table.Click
        ScrollViewer_AnimalLeft.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Krampus.Visibility = Visibility.Visible
        ScrollViewer_AnimalLeft_Apackim_baggims.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_PigKing.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Yaarctopus.Visibility = Visibility.Collapsed
    End Sub

    Private Sub button_A_Return_Krampus_click(sender As Object, e As RoutedEventArgs) Handles button_A_Return_Krampus.Click
        ScrollViewer_AnimalLeft.Visibility = Visibility.Visible
        ScrollViewer_AnimalLeft_Krampus.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Apackim_baggims.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_PigKing.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Yaarctopus.Visibility = Visibility.Collapsed
    End Sub

    Private Sub button_A_Apackim_baggims_table_click(sender As Object, e As RoutedEventArgs) Handles button_A_Apackim_baggims_table.Click
        ScrollViewer_AnimalLeft.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Apackim_baggims.Visibility = Visibility.Visible
        ScrollViewer_AnimalLeft_Krampus.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_PigKing.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Yaarctopus.Visibility = Visibility.Collapsed
    End Sub

    Private Sub button_A_Return_Apackim_baggims_click(sender As Object, e As RoutedEventArgs) Handles button_A_Return_Apackim_baggims.Click
        ScrollViewer_AnimalLeft.Visibility = Visibility.Visible
        ScrollViewer_AnimalLeft_Apackim_baggims.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Krampus.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_PigKing.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Yaarctopus.Visibility = Visibility.Collapsed
    End Sub

    Private Sub A_Loot_Select(VariableName As String)
        Select Case VariableName
            Case "S_gold_nugget"
                ButtonJump(VariableName, "G")
            Case "G_parrot"
                ButtonJump(VariableName, "A")
            Case "F_moleworm"
                ButtonJump(VariableName, "A")
            Case "G_bioluminescence"
                ButtonJump(VariableName, "A")
            Case "S_thick_fur"
                ButtonJump(VariableName, "G")
            Case "G_health"
            Case "G_sanity"
            Case Else
                Dim FirstLetter As String = GetChar(VariableName, 1)
                If FirstLetter = "A" Then
                    VariableName = "G" & Mid(VariableName, 2)
                End If
                ButtonJump(VariableName)
        End Select
    End Sub

    Private Sub button_A_LootP_1_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_1.Click
        A_Loot_Select(A_LootP_Select_1)
    End Sub

    Private Sub button_A_LootP_2_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_2.Click
        A_Loot_Select(A_LootP_Select_2)
    End Sub

    Private Sub button_A_LootP_3_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_3.Click
        A_Loot_Select(A_LootP_Select_3)
    End Sub

    Private Sub button_A_LootP_4_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_4.Click
        A_Loot_Select(A_LootP_Select_4)
    End Sub

    Private Sub button_A_LootP_5_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_5.Click
        A_Loot_Select(A_LootP_Select_5)
    End Sub

    Private Sub button_A_LootP_6_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_6.Click
        A_Loot_Select(A_LootP_Select_6)
    End Sub

    Private Sub button_A_LootP_7_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_7.Click
        A_Loot_Select(A_LootP_Select_7)
    End Sub

    Private Sub button_A_LootP_8_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_8.Click
        A_Loot_Select(A_LootP_Select_8)
    End Sub

    Private Sub button_A_LootP_9_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_9.Click
        A_Loot_Select(A_LootP_Select_9)
    End Sub

    Private Sub button_A_LootP_10_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_10.Click
        A_Loot_Select(A_LootP_Select_10)
    End Sub

    Private Sub button_A_LootP_11_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_11.Click
        A_Loot_Select(A_LootP_Select_11)
    End Sub

    Private Sub button_A_LootP_Yaarctopus_1_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_Yaarctopus_1.Click
        A_Loot_Select("G_dubloons")
    End Sub

    Private Sub button_A_LootP_Yaarctopus_2_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_Yaarctopus_2.Click
        A_Loot_Select("F_seaweed")
    End Sub

    Private Sub button_A_LootP_Yaarctopus_3_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_Yaarctopus_3.Click
        A_Loot_Select("G_seashell")
    End Sub

    Private Sub button_A_LootP_Yaarctopus_4_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_Yaarctopus_4.Click
        A_Loot_Select("G_coral")
    End Sub

    Private Sub button_A_LootP_Yaarctopus_5_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_Yaarctopus_5.Click
        A_Loot_Select("F_blubber")
    End Sub

    Private Sub button_A_LootP_Yaarctopus_6_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_Yaarctopus_6.Click
        A_Loot_Select("F_shark_fin")
    End Sub

    Private Sub button_A_LootP_Yaarctopus_7_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootP_Yaarctopus_7.Click
        A_Loot_Select("G_golden_key")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_1_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_1.Click
        A_Loot_Select("F_california_roll")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_3_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_3.Click
        A_Loot_Select("S_thatch_sail")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_4_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_4.Click
        A_Loot_Select("F_seafood_gumbo")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_6_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_6.Click
        A_Loot_Select("S_cloth_sail")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_7_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_7.Click
        A_Loot_Select("F_bisque")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_9_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_9.Click
        A_Loot_Select("S_trawl_net")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_10_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_10.Click
        A_Loot_Select("F_jelly-O_pop")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_12_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_12.Click
        A_Loot_Select("S_sea_trap")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_13_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_13.Click
        A_Loot_Select("F_ceviche")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_15_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_15.Click
        A_Loot_Select("S_spyglass")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_16_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_16.Click
        A_Loot_Select("F_surf_'n'_turf")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_18_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_18.Click
        A_Loot_Select("S_boat_lantern")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_19_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_19.Click
        A_Loot_Select("F_lobster_bisque")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_21_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_21.Click
        A_Loot_Select("S_pirate_hat")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_22_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_22.Click
        A_Loot_Select("F_lobster_dinner")
    End Sub

    Private Sub button_AL_Yaarctopus_exchange_24_click(sender As Object, e As RoutedEventArgs) Handles button_AL_Yaarctopus_exchange_24.Click
        A_Loot_Select("S_boat_cannon")
    End Sub

    Private Sub button_AL_image_Yaarctopus_1_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_1.Click
        A_Loot_Select("T_old_boot")
    End Sub

    Private Sub button_AL_image_Yaarctopus_2_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_2.Click
        A_Loot_Select("T_license_plate")
    End Sub

    Private Sub button_AL_image_Yaarctopus_3_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_3.Click
        A_Loot_Select("T_wine_bottle_candle")
    End Sub

    Private Sub button_AL_image_Yaarctopus_4_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_4.Click
        A_Loot_Select("T_orange_soda")
    End Sub

    Private Sub button_AL_image_Yaarctopus_5_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_5.Click
        A_Loot_Select("T_voodoo_doll")
    End Sub

    Private Sub button_AL_image_Yaarctopus_6_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_6.Click
        A_Loot_Select("T_second_hand_dentures")
    End Sub

    Private Sub button_AL_image_Yaarctopus_7_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_7.Click
        A_Loot_Select("T_brain_cloud_pill")
    End Sub

    Private Sub button_AL_image_Yaarctopus_8_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_8.Click
        A_Loot_Select("T_toy_boat")
    End Sub

    Private Sub button_AL_image_Yaarctopus_9_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_9.Click
        A_Loot_Select("T_ukulele")
    End Sub

    Private Sub button_AL_image_Yaarctopus_10_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_10.Click
        A_Loot_Select("T_ball_and_cup")
    End Sub

    Private Sub button_AL_image_Yaarctopus_11_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_11.Click
        A_Loot_Select("T_gord's_knot")
    End Sub

    Private Sub button_AL_image_Yaarctopus_12_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_12.Click
        A_Loot_Select("T_melty_marbles")
    End Sub

    Private Sub button_AL_image_Yaarctopus_13_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_13.Click
        A_Loot_Select("T_tiny_rocketship")
    End Sub

    Private Sub button_AL_image_Yaarctopus_14_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_14.Click
        A_Loot_Select("T_frazzled_wires")
    End Sub

    Private Sub button_AL_image_Yaarctopus_15_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_15.Click
        A_Loot_Select("T_gnome_1")
    End Sub

    Private Sub button_AL_image_Yaarctopus_16_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_16.Click
        A_Loot_Select("T_lying_robot")
    End Sub

    Private Sub button_AL_image_Yaarctopus_17_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_17.Click
        A_Loot_Select("T_ancient_vase")
    End Sub

    Private Sub button_AL_image_Yaarctopus_18_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_18.Click
        A_Loot_Select("T_broken_AAC_device")
    End Sub

    Private Sub button_AL_image_Yaarctopus_19_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_19.Click
        A_Loot_Select("T_fake_kazoo")
    End Sub

    Private Sub button_AL_image_Yaarctopus_20_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_20.Click
        A_Loot_Select("T_sextant")
    End Sub

    Private Sub button_AL_image_Yaarctopus_21_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_21.Click
        A_Loot_Select("T_one_true_earring")
    End Sub

    Private Sub button_AL_image_Yaarctopus_22_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_22.Click
        A_Loot_Select("T_dessicated_tentacle")
    End Sub

    Private Sub button_AL_image_Yaarctopus_23_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_23.Click
        A_Loot_Select("T_hardened_rubber_bung")
    End Sub

    Private Sub button_AL_image_Yaarctopus_24_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Yaarctopus_24.Click
        A_Loot_Select("T_mismatched_buttons")
    End Sub

    Private Sub button_AL_image_PigKing_1_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_1.Click
        A_Loot_Select("F_egg")
    End Sub

    Private Sub button_AL_image_PigKing_2_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_2.Click
        A_Loot_Select("F_cooked_egg")
    End Sub

    Private Sub button_AL_image_PigKing_3_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_3.Click
        A_Loot_Select("F_morsel")
    End Sub

    Private Sub button_AL_image_PigKing_4_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_4.Click
        A_Loot_Select("F_cooked_morsel")
    End Sub

    Private Sub button_AL_image_PigKing_5_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_5.Click
        A_Loot_Select("F_meat")
    End Sub

    Private Sub button_AL_image_PigKing_6_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_6.Click
        A_Loot_Select("F_cooked_meat")
    End Sub

    Private Sub button_AL_image_PigKing_7_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_7.Click
        A_Loot_Select("F_drumstick")
    End Sub

    Private Sub button_AL_image_PigKing_8_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_8.Click
        A_Loot_Select("F_fried_drumstick")
    End Sub

    Private Sub button_AL_image_PigKing_9_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_9.Click
        A_Loot_Select("F_leafy_meat")
    End Sub

    Private Sub button_AL_image_PigKing_10_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_10.Click
        A_Loot_Select("F_cooked_leafy_meat")
    End Sub

    Private Sub button_AL_image_PigKing_11_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_11.Click
        A_Loot_Select("F_fish")
    End Sub

    Private Sub button_AL_image_PigKing_12_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_12.Click
        A_Loot_Select("F_cooked_fish")
    End Sub

    Private Sub button_AL_image_PigKing_13_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_13.Click
        A_Loot_Select("F_batilisk_wing")
    End Sub

    Private Sub button_AL_image_PigKing_14_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_14.Click
        A_Loot_Select("F_cooked_batilisk_wing")
    End Sub

    Private Sub button_AL_image_PigKing_15_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_15.Click
        A_Loot_Select("F_koalefant_trunk")
    End Sub

    Private Sub button_AL_image_PigKing_16_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_16.Click
        A_Loot_Select("F_winter_koalefant_trunk")
    End Sub

    Private Sub button_AL_image_PigKing_17_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_17.Click
        A_Loot_Select("F_koalefant_trunk_steak")
    End Sub

    Private Sub button_AL_image_PigKing_18_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_18.Click
        A_Loot_Select("F_small_jerky")
    End Sub

    Private Sub button_AL_image_PigKing_19_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_19.Click
        A_Loot_Select("F_jerky")
    End Sub

    Private Sub button_AL_image_PigKing_20_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_20.Click
        A_Loot_Select("F_monster_jerky")
    End Sub

    Private Sub button_AL_image_PigKing_21_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_21.Click
        A_Loot_Select("G_pig_skin")
    End Sub

    Private Sub button_AL_image_PigKing_22_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_22.Click
        A_Loot_Select("G_slurper_pelt")
    End Sub

    Private Sub button_AL_image_PigKing_23_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_23.Click
        A_Loot_Select("T_sea_worther")
    End Sub

    Private Sub button_AL_image_PigKing_24_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_24.Click
        A_Loot_Select("G_bunny_puff")
    End Sub

    Private Sub button_AL_image_PigKing_25_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_25.Click
        A_Loot_Select("T_second_hand_dentures")
    End Sub

    Private Sub button_AL_image_PigKing_26_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_26.Click
        A_Loot_Select("T_sextant")
    End Sub

    Private Sub button_AL_image_PigKing_27_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_27.Click
        A_Loot_Select("T_toy_boat")
    End Sub

    Private Sub button_AL_image_PigKing_28_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_28.Click
        A_Loot_Select("T_bent_spork")
    End Sub

    Private Sub button_AL_image_PigKing_29_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_29.Click
        A_Loot_Select("T_air_unfreshener")
    End Sub

    Private Sub button_AL_image_PigKing_30_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_30.Click
        A_Loot_Select("T_leaky_teacup")
    End Sub

    Private Sub button_AL_image_PigKing_31_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_31.Click
        A_Loot_Select("T_shoe_horn")
    End Sub

    Private Sub button_AL_image_PigKing_32_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_32.Click
        A_Loot_Select("T_one_true_earring")
    End Sub

    Private Sub button_AL_image_PigKing_33_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_33.Click
        A_Loot_Select("T_ball_and_cup")
    End Sub

    Private Sub button_AL_image_PigKing_34_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_34.Click
        A_Loot_Select("T_gord's_knot")
    End Sub

    Private Sub button_AL_image_PigKing_35_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_35.Click
        A_Loot_Select("T_melty_marbles")
    End Sub

    Private Sub button_AL_image_PigKing_36_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_36.Click
        A_Loot_Select("T_old_boot")
    End Sub

    Private Sub button_AL_image_PigKing_37_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_37.Click
        A_Loot_Select("T_tiny_rocketship")
    End Sub

    Private Sub button_AL_image_PigKing_38_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_38.Click
        A_Loot_Select("T_frayed_yarn")
    End Sub

    Private Sub button_AL_image_PigKing_39_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_39.Click
        A_Loot_Select("T_wire_hanger")
    End Sub

    Private Sub button_AL_image_PigKing_40_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_40.Click
        A_Loot_Select("T_white_bishop")
    End Sub

    Private Sub button_AL_image_PigKing_41_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_41.Click
        A_Loot_Select("T_black_bishop")
    End Sub

    Private Sub button_AL_image_PigKing_42_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_42.Click
        A_Loot_Select("T_wine_bottle_candle")
    End Sub

    Private Sub button_AL_image_PigKing_43_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_43.Click
        A_Loot_Select("F_eel")
    End Sub

    Private Sub button_AL_image_PigKing_44_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_44.Click
        A_Loot_Select("F_cooked_eel")
    End Sub

    Private Sub button_AL_image_PigKing_45_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_45.Click
        A_Loot_Select("T_frazzled_wires")
    End Sub

    Private Sub button_AL_image_PigKing_46_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_46.Click
        A_Loot_Select("T_gnome_1")
    End Sub

    Private Sub button_AL_image_PigKing_47_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_47.Click
        A_Loot_Select("T_gnome_2")
    End Sub

    Private Sub button_AL_image_PigKing_48_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_48.Click
        A_Loot_Select("T_lying_robot")
    End Sub

    Private Sub button_AL_image_PigKing_49_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_49.Click
        A_Loot_Select("T_beaten_beater")
    End Sub

    Private Sub button_AL_image_PigKing_50_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_50.Click
        A_Loot_Select("T_fake_kazoo")
    End Sub

    Private Sub button_AL_image_PigKing_51_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_51.Click
        A_Loot_Select("T_toy_trojan_horse")
    End Sub

    Private Sub button_AL_image_PigKing_52_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_52.Click
        A_Loot_Select("T_unbalanced_top")
    End Sub

    Private Sub button_AL_image_PigKing_53_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_53.Click
        A_Loot_Select("T_brain_cloud_pill")
    End Sub

    Private Sub button_AL_image_PigKing_54_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_54.Click
        A_Loot_Select("T_orange_soda")
    End Sub

    Private Sub button_AL_image_PigKing_55_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_55.Click
        A_Loot_Select("T_ukulele")
    End Sub

    Private Sub button_AL_image_PigKing_56_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_56.Click
        A_Loot_Select("T_mismatched_buttons")
    End Sub

    Private Sub button_AL_image_PigKing_57_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_57.Click
        A_Loot_Select("T_soaked_candle")
    End Sub

    Private Sub button_AL_image_PigKing_58_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_58.Click
        A_Loot_Select("T_back_scratcher")
    End Sub

    Private Sub button_AL_image_PigKing_59_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_59.Click
        A_Loot_Select("T_license_plate")
    End Sub

    Private Sub button_AL_image_PigKing_60_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_60.Click
        A_Loot_Select("T_ancient_vase")
    End Sub

    Private Sub button_AL_image_PigKing_61_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_61.Click
        A_Loot_Select("T_dessicated_tentacle")
    End Sub

    Private Sub button_AL_image_PigKing_62_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_62.Click
        A_Loot_Select("T_hardened_rubber_bung")
    End Sub

    Private Sub button_AL_image_PigKing_63_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_63.Click
        A_Loot_Select("T_lucky_cat_jar")
    End Sub

    Private Sub button_AL_image_PigKing_64_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_64.Click
        A_Loot_Select("T_voodoo_doll")
    End Sub

    Private Sub button_AL_image_PigKing_65_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_65.Click
        A_Loot_Select("T_potato_cup")
    End Sub

    Private Sub button_AL_image_PigKing_66_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_PigKing_66.Click
        A_Loot_Select("T_broken_AAC_device")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_1_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_1.Click
        ButtonJump("F_fish")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_2_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_2.Click
        ButtonJump("F_cooked_fish")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_3_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_3.Click
        ButtonJump("F_tropical_fish")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_4_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_4.Click
        ButtonJump("F_limpets")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_5_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_5.Click
        ButtonJump("F_cooked_limpets")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_6_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_6.Click
        ButtonJump("F_wobster")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_7_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_7.Click
        ButtonJump("F_dead_wobster")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_8_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_8.Click
        ButtonJump("F_delicious_wobster")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_9_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_9.Click
        ButtonJump("F_mussel")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_10_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_10.Click
        ButtonJump("F_cooked_mussel")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_11_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_11.Click
        ButtonJump("F_fish_morsel")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_12_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_12.Click
        ButtonJump("F_cooked_fish_morsel")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_13_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_13.Click
        ButtonJump("F_raw_fish")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_14_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_14.Click
        ButtonJump("F_fish_steak")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_15_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_15.Click
        ButtonJump("F_dead_dogfish")
    End Sub

    Private Sub button_AL_image_Apackim_baggims_16_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Apackim_baggims_16.Click
        ButtonJump("F_dead_swordfish")
    End Sub

    Private Sub button_AL_image_Krampus_1_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_1.Click
        ButtonJump("A_glommer")
    End Sub

    Private Sub button_AL_image_Krampus_2_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_2.Click
        ButtonJump("A_seal")
    End Sub

    Private Sub button_AL_image_Krampus_3_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_3.Click
        ButtonJump("A_doydoy")
    End Sub

    Private Sub button_AL_image_Krampus_4_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_4.Click
        ButtonJump("A_blue_whale")
    End Sub

    Private Sub button_AL_image_Krampus_5_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_5.Click
        ButtonJump("A_baby_beefalo")
    End Sub

    Private Sub button_AL_image_Krampus_6_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_6.Click
        ButtonJump("A_smallbird")
    End Sub

    Private Sub button_AL_image_Krampus_7_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_7.Click
        ButtonJump("A_parrot_pirate")
    End Sub

    Private Sub button_AL_image_Krampus_8_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_8.Click
        ButtonJump("A_white_whale")
    End Sub

    Private Sub button_AL_image_Krampus_9_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_9.Click
        ButtonJump("A_catcoon")
    End Sub

    Private Sub button_AL_image_Krampus_10_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_10.Click
        ButtonJump("A_beefalo")
    End Sub

    Private Sub button_AL_image_Krampus_11_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_11.Click
        ButtonJump("A_water_beefalo")
    End Sub

    Private Sub button_AL_image_Krampus_12_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_12.Click
        ButtonJump("A_swordfish")
    End Sub

    Private Sub button_AL_image_Krampus_13_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_13.Click
        ButtonJump("A_pig_man")
    End Sub

    Private Sub button_AL_image_Krampus_14_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_14.Click
        ButtonJump("A_bunnyman")
    End Sub

    Private Sub button_AL_image_Krampus_15_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_15.Click
        ButtonJump("A_beardlord")
    End Sub

    Private Sub button_AL_image_Krampus_16_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_16.Click
        ButtonJump("A_redbird")
    End Sub

    Private Sub button_AL_image_Krampus_17_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_17.Click
        ButtonJump("A_snowbird")
    End Sub

    Private Sub button_AL_image_Krampus_18_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_18.Click
        ButtonJump("A_pengull")
    End Sub

    Private Sub button_AL_image_Krampus_19_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_19.Click
        ButtonJump("A_tallbird")
    End Sub

    Private Sub button_AL_image_Krampus_20_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_20.Click
        ButtonJump("A_teentallbird")
    End Sub

    Private Sub button_AL_image_Krampus_21_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_21.Click
        ButtonJump("A_parrot")
    End Sub

    Private Sub button_AL_image_Krampus_22_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_22.Click
        ButtonJump("A_dogfish")
    End Sub

    Private Sub button_AL_image_Krampus_23_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_23.Click
        ButtonJump("A_bottenosed_ballphin")
    End Sub

    Private Sub button_AL_image_Krampus_24_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_24.Click
        ButtonJump("A_prime_ape")
    End Sub

    Private Sub button_AL_image_Krampus_25_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_25.Click
        ButtonJump("A_wobster")
    End Sub

    Private Sub button_AL_image_Krampus_26_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_26.Click
        ButtonJump("A_rabbit")
    End Sub

    Private Sub button_AL_image_Krampus_27_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_27.Click
        ButtonJump("A_beardling")
    End Sub

    Private Sub button_AL_image_Krampus_28_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_28.Click
        ButtonJump("A_crow")
    End Sub

    Private Sub button_AL_image_Krampus_29_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_29.Click
        ButtonJump("A_bee")
    End Sub

    Private Sub button_AL_image_Krampus_30_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_30.Click
        ButtonJump("A_butterfly")
    End Sub

    Private Sub button_AL_image_Krampus_31_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_31.Click
        ButtonJump("A_moleworm")
    End Sub

    Private Sub button_AL_image_Krampus_32_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_32.Click
        ButtonJump("A_crabbit")
    End Sub

    Private Sub button_AL_image_Krampus_33_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_33.Click
        ButtonJump("A_jellyfish")
    End Sub

    Private Sub button_AL_image_Krampus_34_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_34.Click
        ButtonJump("A_seagull")
    End Sub

    Private Sub button_AL_image_Krampus_35_click(sender As Object, e As RoutedEventArgs) Handles button_AL_image_Krampus_35.Click
        ButtonJump("A_toucan")
    End Sub

    Private Sub button_A_LootTrapOrBugNetP_1_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootTrapOrBugNetP_1.Click
        ButtonJump(A_LootTrapOrBugNetP_Select_1)
    End Sub

    Private Sub button_A_LootTrapOrBugNetP_2_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootTrapOrBugNetP_2.Click
        ButtonJump(A_LootTrapOrBugNetP_Select_2)
    End Sub

    Private Sub button_A_LootTrapOrBugNetP_3_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootTrapOrBugNetP_3.Click
        ButtonJump(A_LootTrapOrBugNetP_Select_3)
    End Sub

    Private Sub button_A_LootTrapOrBugNetP_4_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootTrapOrBugNetP_4.Click
        ButtonJump(A_LootTrapOrBugNetP_Select_4)
    End Sub

    Private Sub button_A_LootTrapOrBugNetP_5_click(sender As Object, e As RoutedEventArgs) Handles button_A_LootTrapOrBugNetP_5.Click
        ButtonJump(A_LootTrapOrBugNetP_Select_5)
    End Sub

    REM ------------------生物(陆地生物)-------------------
    Private Sub button_A_smallbird_click(sender As Object, e As RoutedEventArgs) Handles button_A_smallbird.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "成长为高脚鸟")
        A_Show("高脚鸟宝宝", "Smallbird", "A_smallbird", "NoDLC", 1, 1, 1, 50, 50, 10, 1, 3, 6, 0, 0, 0, False, False, "可爱又脆弱的高脚鸟宝宝，出生于高脚鸟蛋，会跟随玩家，极低的生命值导致它很容易夭折，如果饿了会一直叫，会饿死，可以吃浆果和各种种子。10天后长成青年高脚鸟。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_teentallbird_click(sender As Object, e As RoutedEventArgs) Handles button_A_teentallbird.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "成长为高脚鸟")
        A_Show("青年高脚鸟", "Smallish Tallbird", "A_teentallbird", "NoDLC", 1, 1, 1, 300, 300, 37.5, 2, 3, 6, 6, 1, 0, False, False, "青年高脚鸟会食用任何东西，饥饿值为0的时候会啄玩家(一次2点伤害)，会饿死，食用任何食物都会回复1/3的生命。18天后长成高脚鸟，并且重新变成敌对状态。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_tallbird_click(sender As Object, e As RoutedEventArgs) Handles button_A_tallbird.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("高脚鸟", "Tallbirds", "A_tallbird", "NoDLC", 1, 1, 1, 400, 800, 50, 2, 3, 7, 7, 4, 0, True, True, "通常生长在岩石地带，有几率遇见高脚鸟彩蛋(一堆高脚鸟巢围着一堆矿石)。打高脚鸟时可以风筝它，当然装备够好硬抗也是可以的。当它发现玩家试图偷取高脚鸟蛋的时候会主动攻击玩家。夜里高脚鸟会睡觉，可以趁此机会偷取鸟蛋，几秒种后才会醒来并开始追击。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_pengull_click(sender As Object, e As RoutedEventArgs) Handles button_A_pengull.Click
        Dim LootHeight As Integer = A_Loot("F_drumstick", "1(10%)", "F_morsel", "1(10%)", "G_jet_feather", "1(20%)", "F_egg", "1(如果藏了一个)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "把蛋藏起来")
        A_Show("企鹅", "Pengull", "A_pengull", "NoDLC", 1, 0, 1, 150, 150, 33, 3, 2.5, 2, 2, 3, 0, False, True, "冬天成群出现在海岸边，若攻击其中一只企鹅那么其他的企鹅也会一起滑翔攻击。它们会在一片冰地上下蛋，并且当玩家靠近时会把蛋藏起来。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_gobbler_click(sender As Object, e As RoutedEventArgs) Handles button_A_gobbler.Click
        Dim LootHeight As Integer = A_Loot("F_drumstick", "2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃浆果丛的浆果")
        A_Show("火鸡", "Gobbler", "A_gobbler", "NoDLC", 1, 0, 1, 50, 50, 0, 0, 0, 3, 8, 0, 0, False, False, "采集浆果丛的时候有几率窜出来火鸡，靠近则会逃跑，可以用远程武器击杀。火鸡看到芝士蛋糕就不顾危险地想要去吃它！可以利用这点来做一个火鸡陷阱。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_lureplants_click(sender As Object, e As RoutedEventArgs) Handles button_A_lureplants.Click
        Dim LootHeight As Integer = A_Loot("G_fleshy_bulb", "1", "F_leafy_meat", "1或其他吃下的东西", "", "未消化的东西")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "生成眼树")
        A_Show("食人花", "Lureplants", "A_lureplants", "NoDLC", 1, 1, 1, 300, 300, 0, 0, 0, 0, 0, 0, 0, False, True, "野生食人花的周围会有至多27株眼球草，它们会保护食人花母体。食人花被种植下两天后开始生长眼球草，眼球草吞噬的东西会被传送到食人花母体。食人花会长出诱饵(如果眼球草吞噬了肉类那么诱饵就是这些肉类，否则就是多叶的肉)，若诱饵被取下周围的眼球草会全部消失。食人花冬天进入休眠状态(变成刚种下的样子)，周围的眼球草全部消失。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_eye_plant_click(sender As Object, e As RoutedEventArgs) Handles button_A_eye_plant.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吞噬所有能放在玩家身上的东西")
        A_Show("眼球草", "Eyeplant", "A_eye_plant", "NoDLC", 1, 1, 1, 30, 30, 20, 1, 2.5, 0, 0, 3, 0, True, True, "眼球草会吞噬周围的东西，还会自动攻击想要靠近食人花的生物。虽然多而且攻击力不低，但是血量也不是很高，一般武器最多两下打死一个。眼球草不会生长在卵石路和制作的草皮上。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_rabbit_click(sender As Object, e As RoutedEventArgs) Handles button_A_rabbit.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1", "A_rabbit", "陷阱")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃地上的水果和蔬菜")
        A_Show("兔子", "Rabbit", "A_rabbit", "NoDLC", 1, 0, 1, 25, 25, 0, 0, 0, 5, 5, 0, 0, False, False, "草原上常见的生物，靠近就会逃跑，可以利用这一点来捕捉它，也可以在陷阱里放水果和蔬菜吸引。若挖起兔子窝里面的兔子会跑出来，没有窝的兔子会睡在外面。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_rabbit_winter_click(sender As Object, e As RoutedEventArgs) Handles button_A_rabbit_winter.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1", "A_rabbit_winter", "陷阱")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃地上的水果和蔬菜")
        A_Show("雪兔", "Winter Rabbit", "A_rabbit_winter", "NoDLC", 1, 0, 1, 25, 25, 0, 0, 0, 5, 5, 0, 0, False, False, "普通兔子到了冬天就会变成雪兔，除了外观以外没有任何区别。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_beardling_click(sender As Object, e As RoutedEventArgs) Handles button_A_beardling.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1(40%)", "G_nightmare_fuel", "1(40%)", "G_beard_hair", "1(20%)", "A_beardling", "陷阱")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃地上的水果和蔬菜")
        A_Show("黑兔子", "Rabbit", "A_beardling", "NoDLC", 1, 0, 1, 25, 25, 0, 0, 0, 5, 5, 0, -40, False, False, "精神低于40%(麦克斯韦是30%)的时候看到的兔子都会变成黑兔子，它们已经变成怪物了！当精神上升高于40%的时候又会变回普通的兔子。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_frog_click(sender As Object, e As RoutedEventArgs) Handles button_A_frog.Click
        Dim LootHeight As Integer = A_Loot("F_frog_legs", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "攻击击落玩家背包第一格物品")
        A_Show("青蛙", "Frog", "A_frog", "NoDLC", 1, 0, 1, 100, 100, 10, 1, 3, 4, 8, 2, 0, True, True, "十分恶心的小生物，出现在白天的池塘边，被打一下就掉一样东西，可以绕着打躲避它的攻击。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_mandrake_click(sender As Object, e As RoutedEventArgs) Handles button_A_mandrake.Click
        Dim LootHeight As Integer = A_Loot("F_mandrake", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "在黄昏或夜晚捡起会跟随玩家")
        A_Show("曼德拉草", "Mandrake", "A_mandrake", "NoDLC", 1, 1, 1, 20, 20, 0, 0, 0, 6, 6, 0, 0, False, False, "出现在草地和洞穴的罕见生物，一个世界只会生成2-5个，通常会长在同一片区域。白天拔起时会直接死亡，黄昏或夜晚拔起会跟随玩家直至天亮又重新自己种回地里。直接食用曼德拉草或做成曼德拉汤食用都会导致周围生物睡着。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_catcoon_click(sender As Object, e As RoutedEventArgs) Handles button_A_catcoon.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "1", "G_coontail", "1(33%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "玩礼物(击杀后不掉落)")
        A_Show("猫熊", "Catcoon", "A_catcoon", "NoDLC", 1, 0, 1, 150, 150, 25, 2, 3, 3, 3, 1, 0, False, False, "生长在桦树林的中空树桩内，单机版杀掉一个树桩的9只猫熊就不会再生了。跳跃时的攻击范围为4，经常捕食蝴蝶等小型生物。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_moleworm_click(sender As Object, e As RoutedEventArgs) Handles button_A_moleworm.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1", "", "偷取的任何东西", "F_moleworm", "锤子")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "藏在地下", "偷东西")
        A_Show("鼹鼠", "Moleworm", "A_moleworm", "NoDLC", 1, 0, 1, 25, 25, 0, 0, 0, 2.75, 2.75, 0, 0, False, False, "生活在桦树林的小生物，平常在地下钻来钻去，遇到有东西可以偷就会钻出来，趁这个时候可以用锤子一锤敲晕。击杀鼹鼠后偷取的物品会全部归还。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_prime_ape_click(sender As Object, e As RoutedEventArgs) Handles button_A_prime_ape.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1(50%)", "F_banana", "1(50%)", "G_manure", "1(如果生产了且没抛)", "", "任何捡起且未消耗的物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "偷东西", "抛便便", "喂食香蕉可以做朋友")
        A_Show("猿猴", "Prime Ape", "A_prime_ape", "SW", 0, 1, 0, 125, 0, 20, 2, 3, 7, 7, 3, 0, False, True, "", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_spider_click(sender As Object, e As RoutedEventArgs) Handles button_A_spider.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1(50%)", "G_silk", "1(25%)", "G_spider_gland", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("蜘蛛", "Spider", "A_spider", "NoDLC", 1, 1, 1, 100, 100, 20, 3, 3, 3, 5, 2, -25, True, True, "蜘蛛出生在蜘蛛巢或由蜘蛛女王孵化，当玩家经过蜘蛛网时，会有少量蜘蛛来巡查。黄昏和夜晚蜘蛛会主动出来行动。若攻击了一只蜘蛛，周围的蜘蛛都会来帮忙，蜘蛛的攻击间隔较长，很容易风筝。用陷阱可以直接杀害蜘蛛。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_spider_warrior_click(sender As Object, e As RoutedEventArgs) Handles button_A_spider_warrior.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1(50%)", "G_silk", "1(25%)", "G_spider_gland", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跳跃攻击")
        A_Show("蜘蛛战士", "Spider Warrior", "A_spider_warrior", "NoDLC", 1, 0, 1, 200, 400, 20, 4, 6, 4, 5, 4, -40, True, True, "蜘蛛战士的血量较多，当玩家在蜘蛛网上攻击蜘蛛或直接攻击蜘蛛巢时会出现(前提是蜘蛛巢不是一级的)。它会近程攻击也会跳跃攻击，两种攻击方式循环使用，掌握好节奏也可以无伤击杀。用陷阱同样可以直接杀害战斗蜘蛛。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_spider_warrior_sw_click(sender As Object, e As RoutedEventArgs) Handles button_A_spider_warrior_sw.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1(50%)", "G_silk", "1(25%)", "G_venom_gland", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跳跃攻击", "攻击带毒")
        A_Show("毒蜘蛛", "Spider Warrior", "A_spider_warrior_sw", "SW", 0, 1, 0, 200, 0, 20, 4, 6, 4, 5, 4, -40, True, True, "船难版的战斗蜘蛛，除了攻击带毒没有什么两样。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_fishermerm_click(sender As Object, e As RoutedEventArgs) Handles button_A_fishermerm.Click
        Dim LootHeight As Integer = A_Loot("F_tropical_fish", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("渔人", "Fishermerm", "A_fishermerm", "SW", 0, 1, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "渔人是船难版的鱼人，通常能在沼泽发现，不同的是，它们不会攻击，遇到玩家会逃跑。定期从潮汐池中捞出热带鱼但是并不吃。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_flup_click(sender As Object, e As RoutedEventArgs) Handles button_A_flup.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "G_eyeshot", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("追踪性弹涂鱼", "Flup", "A_flup", "SW", 0, 1, 0, 100, 0, 25, 0, 0, 4, 6.5, 3, 0, True, True, "追踪性弹涂鱼会出现在潮滩，类似于触手，它们会隐藏在泥里，等待生物经过惊吓它们。仔细观察可以看到它们伸出来的眼睛或者气泡，如果预先提防就不容易被攻击。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_merm_click(sender As Object, e As RoutedEventArgs) Handles button_A_merm.Click
        Dim LootHeight As Integer = A_Loot("F_fish", "1", "F_frog_legs", "1", "F_tropical_fish", "1(SW)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃地上的水果和蔬菜")
        A_Show("鱼人", "Merm", "A_merm", "NoDLC", 1, 1, 1, 250, 500, 30, 3, 0, 3, 8, 4, 0, True, True, "鱼人通常能在沼泽的鱼族废墟发现，它们会主动攻击靠近鱼族废墟的生物。经常可以发现它们和触手打起来，可以方便地获得触手尖刺等资源。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_wee_mactusk_click(sender As Object, e As RoutedEventArgs) Handles button_A_wee_mactusk.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("小海象", "Wee MacTusk", "A_wee_mactusk", "NoDLC", 1, 0, 1, 100, 100, 0, 0, 0, 3, 5, 0, 0, False, True, "几乎没有任何威胁的生物，跟在它的父亲后狐假虎威。玩家靠近会逃跑。击杀后也没有任何战利品，一般无视就好了。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_mactusk_click(sender As Object, e As RoutedEventArgs) Handles button_A_mactusk.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "1", "S_blow_dart", "1", "G_walrus_tusk", "1(50%)", "G_tam_o'shanter", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("海象", "Mactusk", "A_mactusk", "NoDLC", 1, 0, 1, 150, 300, 33, 3, 8, 2, 4, 4, 0, True, True, "会远程攻击的烦人生物，通常一个海象巢穴会有一个海象一个小海象和两只蓝色猎犬，玩家靠近时会拉开距离然后攻击。击杀海象的时候最好先干掉两只猎犬然后再击杀海象，或者用远程武器击杀。如果一直追着海象，海象有可能会停下来一段时间，这个时候可以攻击它，也可以逼到角落击杀。杀死后有几率掉落海象牙(制作步行手杖)和贝雷帽(回复精神的神器)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_baby_beefalo_click(sender As Object, e As RoutedEventArgs) Handles button_A_baby_beefalo.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "3", "G_beefalo_wool", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "成长为皮弗娄牛")
        A_Show("牛宝宝", "Baby Beefalo", "A_baby_beefalo", "NoDLC", 1, 0, 1, 300, 300, 0, 0, 0, 2, 9, 0, 0, False, True, "最小型的牛宝宝。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_toddler_beefalo_click(sender As Object, e As RoutedEventArgs) Handles button_A_toddler_beefalo.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "4", "G_beefalo_wool", "2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "成长为皮弗娄牛")
        A_Show("幼年牛", "Toddler Beefalo", "A_baby_beefalo_2", "NoDLC", 1, 0, 1, 300, 300, 0, 0, 0, 2, 9, 0, 0, False, True, "稍微大一点的牛宝宝。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_teen_beefalo_click(sender As Object, e As RoutedEventArgs) Handles button_A_teen_beefalo.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "3", "G_beefalo_wool", "2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "成长为皮弗娄牛")
        A_Show("青年牛", "Teen Beefalo", "A_baby_beefalo_3", "NoDLC", 1, 0, 1, 300, 300, 0, 0, 0, 2, 9, 0, 0, False, True, "即将成型的牛宝宝。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_beefalo_click(sender As Object, e As RoutedEventArgs) Handles button_A_beefalo.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "4", "G_beefalo_wool", "3", "G_beefalo_horn", "1(33%)", "G_manure", "1(周期产生)", "G_beefalo_wool", "剃刀")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "春天生小牛，变成敌对", "可以驯服(DST)")
        A_Show("皮弗娄牛", "Beefalo", "A_beefalo", "NoDLC", 1, 0, 1, 500, 1000, 34, 4, 3, 1.5, 7, 3, 6.25, False, True, "生活在草原的皮弗娄牛，刚到冬天和一整个春天会进入交配期，变成主动攻击生物，春天会生下小牛。夜晚拿剃刀刮牛毛不会吸引到仇恨。只有在与矮胖的家养牛靠的很近的时候才有精神光环。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_volt_goat_click(sender As Object, e As RoutedEventArgs) Handles button_A_volt_goat.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "3", "G_volt_goat_horn", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "变成带电电羊")
        A_Show("电羊", "Volt Goat", "A_volt_goat", "NoDLC", 1, 0, 1, 350, 700, 25, 2, 3, 4, 8, 3, 0, False, False, "电羊被攻击会逃跑。攻击一只电羊不会引起整群电羊围攻，可以击杀大部分电羊，剩下一只赶到家附近用围墙围起，然后就会繁殖。电羊是闪电优先攻击的目标，被闪电或晨星击中会变成带电电羊。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_volt_goat_withelectric_click(sender As Object, e As RoutedEventArgs) Handles button_A_volt_goat_withelectric.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "3", "F_electric_milk", "1", "G_volt_goat_horn", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "攻击带电")
        A_Show("带电电羊", "Volt Goat", "A_volt_goat_withelectric", "NoDLC", 1, 0, 1, 350, 700, 37.5, 2, 3, 4, 8, 5, 0, True, True, "击杀带电电羊会额外获得一个电羊奶。攻击电羊或被电羊攻击均会触电，可以戴着雨帽、雨衣或眼球伞来防电。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_koalefant_click(sender As Object, e As RoutedEventArgs) Handles button_A_koalefant.Click
        Dim LootHeight As Integer = A_Loot("F_koalefant_trunk", "1", "F_meat", "8", "G_manure", "1(周期产生)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("考拉象", "Koalefant", "A_koalefant", "NoDLC", 1, 0, 1, 500, 1000, 50, 4, 0, 1.5, 7, 4, 0, False, True, "在春、夏、秋季沿着动物足迹寻找有可能找到考拉象(另外两种是座狼和钢羊)，在被攻击之前，会一直保持躲避玩家的状态。它和皮弗娄牛一样会周期性掉落便便。击杀后掉落的红色象鼻是制作夏日背心的必要材料。可以用围墙围起，在必要的时候击杀取肉。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_winter_koalefant_click(sender As Object, e As RoutedEventArgs) Handles button_A_winter_koalefant.Click
        Dim LootHeight As Integer = A_Loot("F_winter_koalefant_trunk", "1", "F_meat", "8", "G_manure", "1(周期产生)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("冬考拉象", "Winter Koalefant", "A_winter_koalefant", "NoDLC", 1, 0, 1, 500, 1000, 50, 4, 0, 1.5, 7, 4, 0, False, True, "在冬季沿着动物足迹寻找有可能找到冬考拉象(另外两种是座狼和钢羊)，在被攻击之前，会一直保持躲避玩家的状态。它和皮弗娄牛一样会周期性掉落便便。击杀后掉落的蓝色象鼻是制作寒冬背心的必要材料。可以用围墙围起，在必要的时候击杀取肉。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_pig_man_click(sender As Object, e As RoutedEventArgs) Handles button_A_pig_man.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "1(75%)", "G_pig_skin", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "喂大肉可以做朋友", "月圆之夜变成疯猪")
        A_Show("猪人", "Pig", "A_pig_man", "NoDLC", 1, 0, 1, 250, 250, 33, 3, 3, 3, 5, 2, 25, False, True, "猪人只有白天会出来活动，一到黄昏变会回到猪舍睡觉，若猪舍被砸了会睡在外面。若攻击猪人，它会智能地和玩家战斗。如果喂大肉可以做朋友，会帮你一起做事(打怪、砍树等)，还有回复精神光环。到了月圆之夜则要小心它们变成疯猪(喂食4块怪兽肉也会变成疯猪)。若玩家身上装备了独奏乐队且有猪人朋友时，回复精神光环会变成损失精神光环。给猪人喂食花瓣会拉便便(放在地上的花瓣猪人也会主动食用)，可以通过这种方式刷便便。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_guard_pig_click(sender As Object, e As RoutedEventArgs) Handles button_A_guard_pig.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "1(75%)", "G_pig_skin", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "变成疯猪的转换时间短")
        A_Show("警卫猪", "Guard Pig", "A_guard_pig", "NoDLC", 1, 0, 1, 300, 600, 33, 1.5, 3, 3, 5, 3, 0, True, True, "警卫猪一般出现在猪人火炬附近，一旦发现有生物靠近猪人火炬就会主动出击。虽然攻击力和猪人相同，但是攻击间隔只有猪人的一半。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_werepig_click(sender As Object, e As RoutedEventArgs) Handles button_A_werepig.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "2", "G_pig_skin", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("疯猪", "Werepig", "A_werepig", "NoDLC", 1, 1, 1, 350, 700, 40, 2, 3, 3, 7, 4, -100, True, False, "疯了疯了！一定是疯了！变态的损失精神光环，月圆之夜还是远离这种怪物吧！", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_wildbore_click(sender As Object, e As RoutedEventArgs) Handles button_A_wildbore.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "1(75%)", "G_pig_skin", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "喂大肉可以做朋友")
        A_Show("野猪", "Wildbore", "A_wildbore", "SW", 0, 1, 0, 250, 0, 24, 3, 3, 3, 5, 2, 0, False, True, "船难版的猪人，和猪人不同的是，如果玩家摧毁了它们的家(荒野小屋)，它们会主动攻击玩家。若玩家不停地靠近野猪，会积累烦恼值，当烦恼值到达阈值时会主动攻击玩家，并且在10秒后停止。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_crabbit_click(sender As Object, e As RoutedEventArgs) Handles button_A_crabbit.Click
        Dim LootHeight As Integer = A_Loot("F_fish_morsel", "1", "A_crabbit", "陷阱")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("兔蟹", "Crabbit", "A_crabbit", "SW", 0, 1, 0, 50, 0, 0, 0, 0, 1.5, 5, 0, 0, False, False, "船难版的兔子。与兔子不同的是，被追击太久不会优先回家，而是创造一个流沙把自己埋起来，保护自己免受伤害。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_beardling_sw_click(sender As Object, e As RoutedEventArgs) Handles button_A_beardling_sw.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1(40%)", "G_nightmare_fuel", "1(40%)", "G_beard_hair", "1(20%)", "A_beardling_sw", "陷阱")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("黑兔蟹", "Crabbit", "A_beardling_sw", "SW", 0, 1, 0, 50, 0, 0, 0, 0, 1.5, 5, 0, -40, False, False, "玩家精神低于40%时兔蟹会变成黑兔蟹，掉落物会有所变化，其他行为没有变化。当玩家精神回复到40%以上又会变回兔蟹。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_baby_doydoy_click(sender As Object, e As RoutedEventArgs) Handles button_A_baby_doydoy.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1", "G_doydoy_feather", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃种子", "成长为渡渡鸟")
        A_Show("幼小的渡渡鸟", "Baby Doydoy", "A_baby_doydoy", "SW", 0, 1, 0, 25, 0, 0, 0, 0, 5, 5, 0, 0, False, False, "只吃种子，过两天成长为小渡渡鸟。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_doydoy_child_click(sender As Object, e As RoutedEventArgs) Handles button_A_doydoy_child.Click
        Dim LootHeight As Integer = A_Loot("F_drumstick", "1", "G_doydoy_feather", "2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃种子和蔬菜", "成长为渡渡鸟")
        A_Show("小渡渡鸟", "Doydoy Child", "A_doydoy_child", "SW", 0, 1, 0, 75, 0, 0, 0, 0, 1.5, 1.5, 0, 0, False, False, "吃种子和蔬菜，过一天成长为渡渡鸟。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_doydoy_click(sender As Object, e As RoutedEventArgs) Handles button_A_doydoy.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "1", "F_drumstick", "2", "G_doydoy_feather", "2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃看到的任何食物除了自己的蛋(包括农场和锅里)")
        A_Show("渡渡鸟", "Doydoy", "A_doydoy", "SW", 0, 1, 0, 100, 0, 0, 0, 0, 2, 2, 0, 0, False, False, "一个地图会有两只渡渡鸟生成在不同的岛上(通常是丛林岛)。渡渡鸟夜晚会睡觉，当它睡觉时可以捡起(在身上时会吃身上的东西)。杀人蜂和蜘蛛会忽视渡渡鸟，而蛇、龙骑士和猎犬都会主动攻击渡渡鸟，需要保护它们免受攻击。每过两到三天在一起的两只渡渡鸟会配对成情侣，除非已经有20只渡渡鸟，配对后会产生一个渡渡鸟巢，若不采集就会生出幼小的渡渡鸟。渡渡鸟被攻击时不会逃跑，存在的数量越少击杀渡渡鸟增加的顽皮值越高。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_snake_click(sender As Object, e As RoutedEventArgs) Handles button_A_snake.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1(33%)", "G_snakeskin", "1(16%)", "G_snake_oil", "1(0.33%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("蛇", "Snake", "A_snake", "SW", 0, 1, 0, 100, 0, 10, 3, 0, 3, 3, 1, -40, True, True, "蛇有可能出生在藤蔓丛或浆果灌木丛，砍倒丛林树也有可能会出现。蛇通常出现在黄昏和夜晚，除非玩家砍伐丛林树或砍伐藤蔓从。蛇有时候会吃地上的肉和蛋。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_poison_snake_click(sender As Object, e As RoutedEventArgs) Handles button_A_poison_snake.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1(20%)", "G_venom_gland", "1(20%)", "G_snakeskin", "1(10%)", "G_snake_oil", "1(0.2%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "攻击带毒")
        A_Show("毒蛇", "Poison Snake", "A_poison_snake", "SW", 0, 1, 0, 100, 0, 10, 3, 2.5, 3, 3, 2, -40, True, True, "毒蛇类似于蛇，不过攻击附带毒性，如果毒蛇咬到将会每秒失去1.5的生命，除非用抗蛇毒血清解毒(击杀毒蛇有几率掉落抗蛇毒血清的制作材料毒腺体)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_clockwork_knight_click(sender As Object, e As RoutedEventArgs) Handles button_A_clockwork_knight.Click
        Dim LootHeight As Integer = A_Loot("G_gears", "2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("发条骑士", "Clockwork Knight", "A_clockwork_knight", "NoDLC", 1, 0, 1, 300, 900, 40, 2, 0, 5, 5, 4, 0, True, True, "保护传送机零件底座的生物之一，通常传送机零件底座周围有两只。一般处于睡觉状态，当有生物靠近会主动攻击，可以风筝击杀。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_clockwork_bishop_click(sender As Object, e As RoutedEventArgs) Handles button_A_clockwork_bishop.Click
        Dim LootHeight As Integer = A_Loot("G_gears", "2", "G_purple_gem", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "远程攻击")
        A_Show("发条主教", "Clockwork Bishop", "A_clockwork_bishop", "NoDLC", 1, 0, 1, 300, 900, 40, 4, 6, 5, 5, 5, 0, True, True, "保护传送机零件底座的生物之一，通常传送机零件底座周围有两只。一般处于睡觉状态，当有生物靠近会主动攻击，远程且攻击力高，一般先击杀其他机械生物最后击杀它，或者吸引机械战车冲撞击杀它。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_clckwork_rook_click(sender As Object, e As RoutedEventArgs) Handles button_A_clckwork_rook.Click
        Dim LootHeight As Integer = A_Loot("G_gears", "2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "摧毁冲撞路径上的树木、建筑")
        A_Show("机械战车", "Clockwork Rook", "A_clckwork_rook", "NoDLC", 1, 0, 1, 300, 900, 45, 2, 0, 5, 16, 4, 0, True, True, "保护传送机零件底座的生物之一，通常传送机零件底座周围有一只。平时很温和，但是一旦发现敌人就会开始冲撞起来，会摧毁沿途的树木、建筑，撞到玩家或生物会中途停止。如果撞到的是生物伤害为200。如果不是特别缺齿轮可以留着砍树什么的。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_grass_gekko_click(sender As Object, e As RoutedEventArgs) Handles button_A_grass_gekko.Click
        Dim LootHeight As Integer = A_Loot("G_cut_grass", "1", "F_leafy_meat", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "受到惊吓掉下尾巴(草)")
        A_Show("草壁虎", "Grass Gekko", "A_grass_gekko", "DST", 0, 0, 1, 0, 150, 0, 0, 0, 1, 10, 1, 0, False, False, "联机版中新型的草的来源，生存于岩石区域。睡觉的时候靠近它会惊醒后逃跑，并丢下尾巴(一个草)，过几天尾巴又会重生。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_grass_gekko_disease_click(sender As Object, e As RoutedEventArgs) Handles button_A_grass_gekko_disease.Click
        Dim LootHeight As Integer = A_Loot("F_rot", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("患病的草壁虎", "Diseased Grass Gekko", "A_grass_gekko_disease", "DST", 0, 0, 1, 0, 150, 0, 0, 0, 1, 10, 1, 0, False, False, "击杀患病的草壁虎没有草，而且患病的草壁虎两天后就会死亡。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_dragoon_click(sender As Object, e As RoutedEventArgs) Handles button_A_dragoon.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "F_dragoon_heart", "1(10%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "火焰冲刺")
        A_Show("龙骑士", "Dragoon", "A_dragoon", "SW", 0, 1, 0, 300, 0, 25, 1, 0, 3, 15, 3, 0, True, False, "龙骑士出生于龙人巢或孵化于完整的龙人蛋，它们只在白天外出，如果有龙人巢的话黄昏会回龙人巢，空闲的时候会在地上吐熔岩。进入战斗后它们会冲向攻击目标并且在经过的路上留下火焰。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_elephant_cactus_click(sender As Object, e As RoutedEventArgs) Handles button_A_elephant_cactus.Click
        Dim LootHeight As Integer = A_Loot("G_cactus_spike", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "范围攻击", "反击")
        A_Show("刺人的象仙人掌", "Prickly Elephant Cactus", "A_elephant_cactus", "SW", 0, 1, 0, 400, 0, 20, 2, 5, 0, 0, 2, 0, True, False, "天然刺人的象仙人掌只存在于火山，它们会范围攻击，并且攻击它的时候会反弹10点伤害，缺点是不会动，并且只在干旱季节才活跃，其他季节会枯萎。击杀后获得的仙人掌可以移植，不过只能移植在岩浆草皮、灰色的草皮和火山草皮上，用灰烬灌溉。", LootHeight, SpecialAbilityHeight)
    End Sub

    REM ------------------生物(海洋生物)-------------------
    Private Sub button_A_dogfish_click(sender As Object, e As RoutedEventArgs) Handles button_A_dogfish.Click
        Dim LootHeight As Integer = A_Loot("F_dead_dogfish", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("狗鱼", "Dogfish", "A_dogfish", "SW", 0, 1, 0, 100, 0, 0, 0, 0, 5, 8, 0, 0, False, False, "通常发现在海洋中和珊瑚礁旁，当船靠近会飞快游走。可以用矛枪或者逼到岸边角落击杀。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_swordfish_click(sender As Object, e As RoutedEventArgs) Handles button_A_swordfish.Click
        Dim LootHeight As Integer = A_Loot("F_dead_swordfish", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("旗鱼", "Swordfish", "A_swordfish", "SW", 0, 1, 0, 200, 0, 30, 2, 0, 5, 8, 3, 0, True, False, "旗鱼通常被发现于深海中，一旦发现玩家会追杀玩家。在其出现时可以放一个浮标之类的东西标记一下。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_wobster_click(sender As Object, e As RoutedEventArgs) Handles button_A_wobster.Click
        Dim LootHeight As Integer = A_Loot("F_dead_wobster", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("龙虾", "Wobster", "A_wobster", "SW", 0, 1, 0, 25, 0, 0, 0, 0, 1.5, 4, 0, 0, False, False, "龙虾出现在龙虾的巢穴附近，一个巢穴一只龙虾，黄昏和夜晚才会出来，玩家靠近它就会跑回巢穴里。可以用拖网或海洋陷阱捕捉。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_bioluminescence_click(sender As Object, e As RoutedEventArgs) Handles button_A_bioluminescence.Click
        Dim LootHeight As Integer = A_Loot("G_bioluminescence", "捕虫网", "G_bioluminescence", "拖网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "发光")
        A_Show("海洋生物", "Bioluminescence", "A_bioluminescence", "SW", 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "通常会在深海发现，12个一组可以认为是海洋中的萤火虫，但是玩家靠近它不会消失，可以用拖网或捕虫网捕捉。注意放在陆地上会被摧毁，放在海里才可以生存。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_A_jellyfish.Click
        Dim LootHeight As Integer = A_Loot("F_dead_jellyfish", "1", "F_jellyfish", "拖网", "F_jellyfish", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "使攻击它的玩家触电")
        A_Show("水母", "Jellyfish", "A_jellyfish", "SW", 0, 1, 0, 50, 0, 5, 0, 0, 2, 2, 0, 0, False, False, "可以在浅海区域找到它们，有时候会成群出现。攻击它会触电，可以用捕虫网或拖网直接捕获，也可以用蛇鳞帽或蛇鳞上衣防电。放在地上会直接死亡。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_bottenosed_ballphin_click(sender As Object, e As RoutedEventArgs) Handles button_A_bottenosed_ballphin.Click
        Dim LootHeight As Integer = A_Loot("F_fish_morsel", "2", "S_empty_bottle", "1(50%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("宽吻海豚", "Bottlenosed Ballphin", "A_bottenosed_ballphin", "SW", 0, 1, 0, 100, 0, 10, 6, 0, 5, 8, 1, 0, False, True, "可以在海洋的任何地方发现宽吻海豚，一般12个一组，在第15~16天才会出现。如果一组中的一部分被杀害，会在温和季节重生，如果一组中只剩一只，那么它会跟着玩家到一片新的海域，不过路上很容易分心。它们会攻击任何敌对生物(如恶臭魔鬼鱼或海狗)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_blue_whale_click(sender As Object, e As RoutedEventArgs) Handles button_A_blue_whale.Click
        Dim LootHeight As Integer = A_Loot("A_blue_whale_carcass", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "蓝鲸尸体战利品：", "生鱼片×4", "鲸油×4", "3~9样其他物品")
        A_Show("蓝鲸", "Blue Whale", "A_blue_whale", "SW", 0, 1, 0, 650, 0, 50, 3.5, 0, 2, 4, 4, 0, False, True, "有三分之二的几率可以通过可疑的泡泡(类似可疑的土堆)发现它，在被攻击之前它会尽量躲避玩家。被杀死后留下蓝鲸尸体，只有玩家在附近3~5天才能用砍刀切开获得战利品(火药可以加速腐烂)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_white_whale_click(sender As Object, e As RoutedEventArgs) Handles button_A_white_whale.Click
        Dim LootHeight As Integer = A_Loot("A_white_whale_carcass", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "白鲸尸体战利品：", "生鱼片×4", "鲸油×4", "鱼叉×1", "3~9样其他物品")
        A_Show("白鲸", "White Whale", "A_white_whale", "SW", 0, 1, 0, 800, 0, 75, 3, 0, 2.5, 5, 5, 0, True, False, "有三分之一的几率可以通过可疑的泡泡(类似可疑的土堆)发现它，玩家靠近就会一直追杀玩家。被杀死后留下白鲸尸体，只有玩家在附近3~5天才能用砍刀切开获得战利品。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_floaty_boaty_knight_click(sender As Object, e As RoutedEventArgs) Handles button_A_floaty_boaty_knight.Click
        Dim LootHeight As Integer = A_Loot("G_gears", "1~3")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "远程攻击*3")
        A_Show("浮船骑士", "Floaty Boaty Knight", "A_floaty_boaty_knight", "SW", 0, 1, 0, 500, 0, 50, 6, 0, 3, 6, 4, 0, True, False, "守护木制平台的生物，4只同时出现。也会在击杀任何海洋生物时有几率出现1~3只。一旦离玩家过近会拉开安全距离然后开始攻击(放炮)，一次性放出三个炮弹落在靠近玩家某一点附近的随机位置。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_stink_ray_click(sender As Object, e As RoutedEventArgs) Handles button_A_stink_ray.Click
        Dim LootHeight As Integer = A_Loot("G_venom_gland", "1(33%)", "F_monster_meat", "1(66%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "排出有毒气体")
        A_Show("恶臭魔鬼鱼", "Stink Ray", "A_stink_ray", "SW", 0, 1, 0, 50, 0, 0, 1, 1.5, 8, 8, 1, 0, True, True, "恶臭魔鬼鱼通常6个一组出现在海上，它们没有攻击力，只会释放毒气使玩家中毒。戴着粉尘净化器或者使用WX-78可以免疫毒气。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_water_beefalo_click(sender As Object, e As RoutedEventArgs) Handles button_A_water_beefalo.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "4", "G_horn", "1", "G_manure", "1(周期产生)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "春天生水牛，变成敌对")
        A_Show("水牛", "Water Beefalo", "A_water_beefalo", "SW", 0, 1, 0, 500, 0, 34, 4, 3, 1.5, 7.5, 3, 0, False, True, "和皮弗娄牛类似，不过无法对其使用剃刀，即没有牛毛，而且掉落的是角而不是牛角。它们通常生活在红树林，虽然一般都在水里，但也可以上岸。", LootHeight, SpecialAbilityHeight)
    End Sub

    REM ------------------生物(飞行生物)-------------------
    Private Sub button_A_bee_click(sender As Object, e As RoutedEventArgs) Handles button_A_bee.Click
        Dim LootHeight As Integer = A_Loot("F_honey", "1(16.7%)", "G_stinger", "1(83.3%)", "A_bee", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "给花传粉")
        A_Show("蜜蜂", "Bee", "A_bee", "NoDLC", 1, 1, 1, 100, 100, 10, 2, 3, 4, 6, 1, 0, False, True, "蜜蜂是中性生物，出现在蜂巢、杀人蜂巢和蜂箱。白天出来采蜜，黄昏回到蜂巢。若蜂箱附近没有花产蜜效率会急剧降低。当蜜蜂被攻击后，同一个蜂巢的蜜蜂会变成杀人蜂追杀攻击者，可以逃跑。到了春天所有蜜蜂从外观和行为上都会变成杀人蜂的样子(捕捉后依然是蜜蜂)，到夏天就还原。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_killer_bee_click(sender As Object, e As RoutedEventArgs) Handles button_A_killer_bee.Click
        Dim LootHeight As Integer = A_Loot("F_honey", "1(16.7%)", "G_stinger", "1(83.3%)", "A_killer_bee", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("杀人蜂", "Killer Bee", "A_killer_bee", "NoDLC", 1, 1, 1, 100, 100, 10, 3, 3, 4, 6, 3, 0, True, True, "如果一只蜜蜂被攻击，同一个蜂巢中的蜜蜂会变成杀人蜂。若靠近杀人蜂巢也会引出杀人蜂。春天只能找到杀人蜂，包括蜂箱里的蜜蜂也会全部变成杀人蜂。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_butterfly_click(sender As Object, e As RoutedEventArgs) Handles button_A_butterfly.Click
        Dim LootHeight As Integer = A_Loot("F_butterfly_wing", "1(98%)", "F_butter", "1(2%)", "A_butterfly", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("蝴蝶", "Butterfly", "A_butterfly", "NoDLC", 1, 0, 1, 1, 1, 0, 0, 0, 5, 5, 0, 0, False, False, "最弱小的生物，但是飞得很快，白天出现，黄昏消失在花里，冬季蝴蝶不会出现。击杀蝴蝶有极小几率掉落稀有的蝴蝶黄油，可以烹饪出冰淇淋或华夫饼，蝴蝶翅膀也可以烹饪出奶油松饼。想要击杀蝴蝶，需紧跟它并预判走位按下'CTRL+F'强制攻击即可。用捕虫网的话则可以比较方便捕捉到活蝴蝶，活蝴蝶右键种植会变成花。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_butterfly_sw_click(sender As Object, e As RoutedEventArgs) Handles button_A_butterfly_sw.Click
        Dim LootHeight As Integer = A_Loot("F_butterfly_wing_sw", "1(98%)", "F_butter", "1(2%)", "A_butterfly_sw", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("船难蝴蝶", "Butterfly", "A_butterfly_sw", "SW", 0, 1, 0, 1, 0, 0, 0, 0, 5, 5, 0, 0, False, False, "除了外貌不同，和普通蝴蝶没什么区别。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_mosquito_click(sender As Object, e As RoutedEventArgs) Handles button_A_mosquito.Click
        Dim LootHeight As Integer = A_Loot("G_mosquito_sack", "1(50%)", "A_mosquito", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吸完血后爆炸(伤害34)")
        A_Show("蚊子", "Mosquito", "A_mosquito", "NoDLC", 1, 0, 1, 100, 100, 3, 2, 4, 8, 12, 1, 0, True, False, "蚊子出现在黄昏和夜晚的沼泽池塘，成功攻击四次后自爆，对周围造成34点伤害。冬天池塘结冰不会出现。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_mosquito_sw_click(sender As Object, e As RoutedEventArgs) Handles button_A_mosquito_sw.Click
        Dim LootHeight As Integer = A_Loot("G_yellow_mosquito_sack", "1(50%)", "G_venom_gland", "1", "A_mosquito_sw", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吸完血后爆炸(伤害34)", "攻击带毒")
        A_Show("毒蚊子", "Poison Mosquito", "A_mosquito_sw", "SW", 0, 1, 0, 100, 0, 3, 2, 4, 8, 12, 1, 0, True, False, "毒蚊子出现于风季的水灾的潮水中或者坏结局的老虎机，成功攻击四次后自爆，对周围造成34点伤害。攻击附带毒性，要小心。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_redbird_click(sender As Object, e As RoutedEventArgs) Handles button_A_redbird.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1(50%)", "G_crimson_feather", "1(50%)", "A_redbird", "捕鸟器")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "掉落种子", "吃种子")
        A_Show("红雀", "Redbird", "A_redbird", "NoDLC", 1, 0, 1, 25, 25, 0, 0, 0, 0, 0, 0, 0, False, False, "只会出现在夏季的鸟类。玩家靠近会飞走，要用远程武器或者捕鸟器击杀和捕捉。囚禁在鸟笼里时，喂食熟怪兽肉会得到蛋，喂食作物可以得到1-2个作物种子和0-1个种子。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_snowbird_click(sender As Object, e As RoutedEventArgs) Handles button_A_snowbird.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1(50%)", "G_azure_feather", "1(50%)", "A_snowbird", "捕鸟器")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "掉落种子", "吃种子")
        A_Show("雪雀", "Snowbird", "A_snowbird", "NoDLC", 1, 0, 1, 25, 25, 0, 0, 0, 0, 0, 0, 0, False, False, "只会出现在冬季的鸟类。玩家靠近会飞走，要用远程武器或者捕鸟器击杀和捕捉。囚禁在鸟笼里时，喂食熟怪兽肉会得到蛋，喂食作物可以得到1-2个作物种子和0-1个种子。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_crow_click(sender As Object, e As RoutedEventArgs) Handles button_A_crow.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1(50%)", "G_jet_feather", "1(50%)", "A_crow", "捕鸟器")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "掉落种子", "吃种子")
        A_Show("乌鸦", "Crow", "A_crow", "NoDLC", 1, 0, 1, 25, 25, 0, 0, 0, 0, 0, 0, 0, False, False, "四季都能看到的鸟类。玩家靠近会飞走，要用远程武器或者捕鸟器击杀和捕捉。囚禁在鸟笼里时，喂食熟怪兽肉会得到蛋，喂食作物可以得到1-2个作物种子和0-1个种子。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_buzzards_click(sender As Object, e As RoutedEventArgs) Handles button_A_buzzards.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1-2", "F_drumstick", "1", "G_jet_feather", "1(33%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("秃鹰", "Buzzards", "A_buzzards", "NoDLC", 1, 0, 1, 125, 250, 15, 2, 0, 4, 8, 1, 0, False, True, "秃鹰通常盘旋在上空寻找肉类，可以在沙漠和岩石区找到它们。可以用肉类作诱饵引诱它们下来。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_parrot_click(sender As Object, e As RoutedEventArgs) Handles button_A_parrot.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1(50%)", "G_crimson_feather", "1(50%)", "G_parrot", "捕鸟器")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "掉落种子", "吃种子")
        A_Show("鹦鹉", "Parrot", "A_parrot", "SW", 0, 1, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "船难版中替代红雀的鸟类。囚禁在鸟笼里时，喂食熟怪兽肉会得到蛋，喂食作物可以得到1-2个作物种子和0-1个种子。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_parrot_pirate_click(sender As Object, e As RoutedEventArgs) Handles button_A_parrot_pirate.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1(50%)", "G_crimson_feather", "1(50%)", "A_parrot_pirate", "捕鸟器")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "掉落金币")
        A_Show("海盗鹦鹉", "Parrot Pirate", "A_parrot_pirate", "SW", 0, 1, 0, 25, 0, 0, 0, 0, 0, 0, 0, 25, False, False, "一种特殊的鹦鹉，不会掉落种子，而是掉落金币。捕捉后关在笼子里有恢复精神光环。囚禁在鸟笼里时，喂食熟怪兽肉会得到蛋，喂食作物可以得到1-2个作物种子和0-1个种子。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_toucan_click(sender As Object, e As RoutedEventArgs) Handles button_A_toucan.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1(50%)", "G_jet_feather", "1(50%)", "A_toucan", "捕鸟器")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "掉落种子", "吃种子")
        A_Show("大嘴鸟", "Toucan", "A_toucan", "SW", 0, 1, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "船难版中替代乌鸦的鸟类。囚禁在鸟笼里时，喂食熟怪兽肉会得到蛋，喂食作物可以得到1-2个作物种子和0-1个种子。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_seagull_click(sender As Object, e As RoutedEventArgs) Handles button_A_seagull.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1(50%)", "G_azure_feather", "1(50%)", "A_seagull", "捕鸟器")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "掉落种子", "吃种子")
        A_Show("海鸥", "Seagull", "A_seagull", "SW", 0, 1, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "海鸥是杂食鸟类，会吃地上的肉和帽贝岩的帽贝。囚禁在鸟笼里时，喂食熟怪兽肉会得到蛋，喂食作物可以得到1-2个作物种子和0-1个种子。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_fireflies_click(sender As Object, e As RoutedEventArgs) Handles button_A_fireflies.Click
        Dim LootHeight As Integer = A_Loot("G_fireflies", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "发光")
        A_Show("萤火虫", "Fireflies", "A_fireflies", "NoDLC", 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "萤火虫可以在夜晚的森林或草原上找到，也可以在洞穴(主要是蘑菇树附近)找到。萤火虫会发光，当玩家靠近后会消失，离开后1-2s又重新聚集。可以在3-4组邻近的萤火虫之间来回奔跑保证不被黑夜(查理)侵袭，不过精神仍然会明显下降。可以用捕虫网捕捉，制作矿工帽和南瓜灯，也可以给矿工帽添加燃料。注意萤火虫是不可再生资源。", LootHeight, SpecialAbilityHeight)
    End Sub

    REM ------------------生物(洞穴生物)-------------------
    Private Sub button_A_bunnyman_click(sender As Object, e As RoutedEventArgs) Handles button_A_bunnyman.Click
        Dim LootHeight As Integer = A_Loot("F_carrot", "2", "F_meat", "1(75%)", "G_bunny_puff", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃地上的水果和蔬菜", "变成黑兔人", "玩家身上携带肉类食物会主动攻击")
        A_Show("兔人", "Bunnyman", "A_bunnyman", "NoDLC", 1, 0, 1, 200, 200, 40, 2, 3, 3, 6, 2, 25, False, True, "兔人通常生活在蘑菇树森林附近，离洞穴一层入口也比较近。当携带肉类(火腿球棒和高脚鸟蛋除外)靠近兔人的时候会惹怒兔人，它们将会变成主动攻击。当兔人生命值低于70时会开始逃跑，只能逼到死角或者用远程武器击杀，击杀后得到的兔毛可以做兔子窝，兔人死亡一天后在兔子窝重生。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_beardlord_click(sender As Object, e As RoutedEventArgs) Handles button_A_beardlord.Click
        Dim LootHeight As Integer = A_Loot("G_beard_hair", "2", "F_monster_meat", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃地上的水果和蔬菜", "变成兔人", "玩家身上携带肉类食物会主动攻击")
        A_Show("黑兔人", "Beardlord", "A_beardlord", "NoDLC", 1, 0, 1, 200, 200, 60, 1, 3, 3, 6, 3, -40, False, True, "玩家精神低于40%时兔人会变成黑兔人，掉落物会有所变化，其他行为没有变化。当玩家精神回复到40%以上又会变回兔人。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_blue_spore_click(sender As Object, e As RoutedEventArgs) Handles button_A_blue_spore.Click
        Dim LootHeight As Integer = A_Loot("A_blue_spore", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "照明")
        A_Show("蓝色孢子", "Blue Spore", "A_blue_spore", "DST", 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "蘑菇树周围游荡的生物，可以用捕虫网捕捉，放在地上可以发出微弱的光芒。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_green_spore_click(sender As Object, e As RoutedEventArgs) Handles button_A_green_spore.Click
        Dim LootHeight As Integer = A_Loot("A_green_spore", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "照明")
        A_Show("绿色孢子", "Green Spore", "A_green_spore", "DST", 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "蘑菇树周围游荡的生物，可以用捕虫网捕捉，放在地上可以发出微弱的光芒。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_red_spore_click(sender As Object, e As RoutedEventArgs) Handles button_A_red_spore.Click
        Dim LootHeight As Integer = A_Loot("A_red_spore", "捕虫网")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "照明")
        A_Show("红色孢子", "Red Spore", "A_red_spore", "DST", 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "蘑菇树周围游荡的生物，可以用捕虫网捕捉，放在地上可以发出微弱的光芒。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_splumonkey_click(sender As Object, e As RoutedEventArgs) Handles button_A_splumonkey.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1", "F_banana", "1", "G_nightmare_fuel", "1(50%)", "", "偷取的任何东西")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "偷东西", "抛便便", "噩梦阶段变成暗影暴躁猴")
        A_Show("暴躁猴", "Splumonkey", "A_splumonkey", "NoDLC", 1, 0, 1, 125, 125, 20, 2, 3, 7, 7, 3, 0, False, True, "暴躁猴生活在废墟的暴躁猴群，它们会采摘洞穴香蕉树的香蕉，浆果灌木丛的浆果以及眼前的任何蘑菇，它们会偷农场、箱子、切斯特以及烹饪锅里的东西，如果偷到的是帽子，它们将会带上它并且产生相对应的效果。通常情况下，它们是中立的，但是它们也会远程扔便便保护自己，被便便扔中会受到少量伤害并降低10点精神。一旦开始地震，它们会回到暴躁猴群并把偷的东西扔下(帽子类除外)。它们偶尔会跟随玩家，特别是当玩家携带了香蕉的时候。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_shadow_splumonkey_click(sender As Object, e As RoutedEventArgs) Handles button_A_shadow_splumonkey.Click
        Dim LootHeight As Integer = A_Loot("F_morsel", "1", "F_banana", "1", "G_beard_hair", "1", "G_nightmare_fuel", "1(50%)", "", "偷取的任何东西")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "平静阶段变成暴躁猴")
        A_Show("暗影暴躁猴", "Shadow Splumonkey", "A_shadow_splumonkey", "NoDLC", 1, 0, 1, 125, 125, 20, 2, 3, 7, 7, 3, 0, True, True, "当噩梦循环到达噩梦阶段的时候，暴躁猴就会变成暗影暴躁猴，它们会变成主动攻击形态，并且在地震的时候不会回到暴躁猴群。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_cave_spider_click(sender As Object, e As RoutedEventArgs) Handles button_A_cave_spider.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1(50%)", "G_silk", "1(25%)", "G_spider_gland", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "可以躲在壳里抵挡75%伤害")
        A_Show("洞穴蜘蛛", "Cave Spider", "A_cave_spider", "NoDLC", 1, 0, 1, 150, 150, 20, 3, 0, 3, 5, 3, -15, True, True, "洞穴蜘蛛生活在蛛网岩，它拥有一个贝壳状的甲壳，缩起来可以抵挡75%的伤害，而普通状态下是极其脆弱的。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_spitter_click(sender As Object, e As RoutedEventArgs) Handles button_A_spitter.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1(50%)", "G_silk", "1(25%)", "G_spider_gland", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("喷吐蜘蛛", "Spitter", "A_spitter", "NoDLC", 1, 0, 1, 175, 175, 20, 6, 8, 4, 5, 4, -15, True, True, "喷吐蜘蛛同样生活在蛛网岩，它会喷出远程的蛛网攻击，尽量不要靠近它，除非只有一只喷吐蜘蛛。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_dangling_depth_dweller_click(sender As Object, e As RoutedEventArgs) Handles button_A_dangling_depth_dweller.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1(50%)", "G_silk", "1(25%)", "G_spider_gland", "1(25%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跳跃攻击")
        A_Show("白蜘蛛", "Dangling Depth Dweller", "A_dangling_depth_dweller", "NoDLC", 1, 0, 1, 200, 200, 20, 4, 3, 3, 5, 4, -25, True, True, "可以在废墟的蜘蛛网(迷宫区比较多)找到它们，当玩家站在蜘蛛网上时，它们会从天而降，和蜘蛛战士一样会跳跃攻击。它们的巢穴在顶部，单机版可以把可燃物放在网中心点燃来烧毁网。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_batilisk_click(sender As Object, e As RoutedEventArgs) Handles button_A_batilisk.Click
        Dim LootHeight As Integer = A_Loot("F_batilisk_wing", "1(15%)", "G_guano", "1(15%)", "F_monster_meat", "1(10%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("黑蝙蝠", "Batilisk", "A_batilisk", "NoDLC", 1, 0, 1, 50, 50, 20, 1, 1.5, 8, 8, 2, 0, True, True, "黑蝙蝠生活在石笋附近的蝙蝠洞里，除了冬天，挖开落水洞也会飞出几只黑蝙蝠。黑蝙蝠白天回蝙蝠洞(若无法回洞则站立睡觉)，黄昏和晚上出来活动，会定期掉落鸟粪，具有不俗的施肥能力。击杀后获得的黑蝙蝠翅膀更是能制作武器——蝙蝠蝙蝠。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_slurtles_click(sender As Object, e As RoutedEventArgs) Handles button_A_slurtles.Click
        Dim LootHeight As Integer = A_Loot("G_slurtle_slime", "2", "G_broken_shell", "1(90%)", "G_shelmet", "1(10%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃地上和容器里的矿物", "藏在壳里吸收95%伤害")
        A_Show("黏糊虫", "Slurtles", "A_slurtles", "NoDLC", 1, 0, 1, 600, 1200, 25, 4, 2.5, 3, 3, 4, 0, True, True, "黏糊虫是生活在含糊虫土堆的其中一种生物，每过一段时间会排出含糊虫粘液(类似皮弗娄牛排出便便)。它们是中性的，然而它们会吃矿石，若玩家身上有矿石它们也会攻击玩家，一次成功的攻击会掉落矿石(类似青蛙的攻击)。它们走得很缓慢，但是被数次攻击后会缩回壳里，吸收95%的伤害，所以最好打2-3下就停一下。杀死它有极小的几率掉落黏糊虫头盔。用火炬攻击一次就会在几秒钟后爆炸，形成小范围的300点伤害，但是不掉落战利品。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_snurtles_click(sender As Object, e As RoutedEventArgs) Handles button_A_snurtles.Click
        Dim LootHeight As Integer = A_Loot("G_slurtle_slime", "2", "G_broken_shell", "1(25%)", "G_snurtle_shell_armor", "1(75%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "吃地上和容器里的矿物", "藏在壳里吸收80%伤害")
        A_Show("含糊虫", "Snurtles", "A_snurtles", "NoDLC", 1, 0, 1, 200, 200, 0, 0, 0, 4, 4, 0, 0, False, False, "含糊虫是生活在含糊虫土堆的另一种生物，它们会主动逃离玩家，被攻击就会缩入壳中，吸收80%的伤害。杀死它有极小的几率掉落含糊虫壳甲。用火炬攻击一次就会在几秒钟后爆炸，形成小范围的300点伤害，但是不掉落战利品。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_rock_lobster_click(sender As Object, e As RoutedEventArgs) Handles button_A_rock_lobster.Click
        Dim LootHeight As Integer = A_Loot("G_rocks", "2", "F_meat", "1", "G_flint", "2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "伪装成石头吸收95%伤害并回复生命", "吃地上的矿物")
        A_Show("岩石大龙虾", "Rock Lobster", "A_rock_lobster", "NoDLC", 1, 0, 1, 1800, 3600, 90, 0, 0, 2.66, 2.66, 5, 0, False, True, "岩石大龙虾生活在洞穴里的岩石平原，吃除了大理石外的所有矿石，并且免疫冰冻。它们行动缓慢并且群居，每隔四天就会生长一次，最开始时，生命为1125(联机版为2250)，攻击力为56.25，速度为2.66，长到最大时，生命为1800(联机版为3600)，攻击力为90，速度为1.66(越长大速度越慢)。平时它们会四处寻找岩石和燧石吃，受到攻击会伪装成大石头，吸收95%的伤害，并且每秒恢复10点生命。像猪一样给它们吃任何矿物它们会跟着玩家并且为玩家战斗，一次半天，最多两天半。它们会清醒两天并睡一天半，睡着的时候可以用火药炸(单机版最大形态需要9个火药，联机版需要18个)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_slurper_click(sender As Object, e As RoutedEventArgs) Handles button_A_slurper.Click
        Dim LootHeight As Integer = A_Loot("F_light_bulb", "2", "G_slurper_pelt", "1(50%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "清醒时提供光", "替换玩家帽子并发光")
        A_Show("缀食者", "Slurper", "A_slurper", "NoDLC", 1, 0, 1, 200, 200, 30, 5, 8, 9, 9, 3, -25, True, True, "缀食者生活在废墟，刚生成时会睡觉，到达50%饥饿时会醒来并且四处游荡。它们会自动寻找玩家、兔人、猪人(总是优先玩家)并通过快速滚动攻击，并且试图寄生在猎物的脑袋上吸食饥饿，当成功寄生时，会发出和矿工帽相同的光芒，吸食到饥饿值到达90%时就会自动脱落并进入睡眠。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_damaged_knight_click(sender As Object, e As RoutedEventArgs) Handles button_A_damaged_knight.Click
        Dim LootHeight As Integer = A_Loot("G_gears", "1(50%)", "G_thulecite_fragments", "1(50%)", "G_nightmare_fuel", "1(60%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("损坏的发条骑士", "Damaged Knight", "A_damaged_knight", "NoDLC", 1, 0, 1, 300, 900, 40, 2, 0, 5, 5, 4, 0, True, True, "洞穴里的发条骑士，可以通过使用3个齿轮修复一个破碎的时钟作为永久友军(被闪电劈了会变成敌对)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_damaged_bishop_click(sender As Object, e As RoutedEventArgs) Handles button_A_damaged_bishop.Click
        Dim LootHeight As Integer = A_Loot("G_purple_gem", "1(60%)", "G_thulecite_fragments", "1(50%)", "G_nightmare_fuel", "1(60%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "远程攻击", "如果从破碎的时钟创建则可以跟随")
        A_Show("损坏的发条主教", "Damaged Bishop", "A_damaged_bishop", "NoDLC", 1, 0, 1, 300, 900, 40, 4, 6, 5, 5, 5, 0, True, True, "洞穴里的发条主教，可以通过使用3个齿轮修复一个破碎的时钟作为永久友军(被闪电劈了会变成敌对)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_damage_rook_click(sender As Object, e As RoutedEventArgs) Handles button_A_damage_rook.Click
        Dim LootHeight As Integer = A_Loot("G_gears", "1(50%)", "G_thulecite_fragments", "1(50%)", "G_nightmare_fuel", "1(60%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "摧毁冲撞路径上的树木、建筑")
        A_Show("损坏的机械战车", "Damaged Rook", "A_damage_rook", "NoDLC", 1, 0, 1, 300, 900, 45, 2, 0, 5, 16, 4, 0, True, True, "洞穴里的机械战车，可以通过使用3个齿轮修复一个破碎的时钟作为永久友军(被闪电劈了会变成敌对)。", LootHeight, SpecialAbilityHeight)
    End Sub

    REM ------------------生物(邪恶生物)-------------------
    Private Sub button_A_tentacle_click(sender As Object, e As RoutedEventArgs) Handles button_A_tentacle.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "2", "G_tentacle_spike", "1(50%)", "G_tentacle_spots", "1(20%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "躲藏在地下")
        A_Show("触手", "Tentacle", "A_tentacle", "NoDLC", 1, 0, 1, 500, 500, 34, 2, 4, 0, 0, 4, -40, True, False, "触手潜伏在沼泽区域，维克波顿的《在触手上》也可以召唤3只触手。当有生物经过的时候，会现在地上冒几个气泡然后冲出地面攻击。由于它一下可以攻击两次，所以伤害高达68点，要十分小心，最好勾引鱼人和触手打架，可以收获食物和触手棒。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_shadow_tentacle_click(sender As Object, e As RoutedEventArgs) Handles button_A_shadow_tentacle.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("暗影触手", "Shadow Tentacle", "A_shadow_tentacle", "NoDLC", 1, 0, 1, 500, 500, 34, 2, 2, 0, 0, 0, -40, True, False, "用铥矿棒命中敌人的时候，有20%的几率产生暗影触手，它们存在时间极短，攻击范围有限(攻击敌人)，并且同样拥有触手的负面精神光环。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_hound_click(sender As Object, e As RoutedEventArgs) Handles button_A_hound.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "G_hound's_tooth", "1(12.5%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("猎犬", "Hound", "A_hound", "NoDLC", 1, 1, 1, 150, 150, 20, 2, 3, 10, 10, 3, -40, True, True, "猎犬通常出生在猎犬丘，它们速度极快，成群结队，是新手的噩梦。除了猎犬丘，还会有周期性的野生猎犬全世界范围内搜寻并攻击玩家(在进攻前会有咆哮声警告)，必须在猎犬到来前做好应敌准备，每一波猎犬都会比前一波更猛烈，并且提前咆哮警告时间越来越短，直到猎犬数量达到最大值。夏季和秋季的猎犬进攻会伴随着红色猎犬，冬季和春季的猎犬进攻会伴随着蓝色猎犬。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_red_hound_click(sender As Object, e As RoutedEventArgs) Handles button_A_red_hound.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "G_hound's_tooth", "1", "G_ash", "3", "G_red_gem", "1(20%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "被杀死后燃烧周围可燃物")
        A_Show("红色猎犬", "Red Hound", "A_red_hound", "NoDLC", 1, 1, 1, 100, 100, 30, 2, 3, 10, 10, 5, -40, True, True, "红色猎犬死亡时会燃烧周围的可燃物，如果这些可燃物在雪球发射器的范围内就不会被烧毁，并且不会掉落灰烬。有时候会看到4、5只睡着的红色猎犬围着一个火魔杖睡觉，若直接捡起火魔杖，这些猎犬会醒来并攻击玩家，建议单独消灭每一只红色猎犬，因为攻击其中一只其他的红色猎犬并不会醒来。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_blue_hound_click(sender As Object, e As RoutedEventArgs) Handles button_A_blue_hound.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "G_hound's_tooth", "2", "G_blue_gem", "1(20%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "被杀死后冰冻周围可冻物")
        A_Show("蓝色猎犬", "Blue Hound", "A_blue_hound", "NoDLC", 1, 1, 1, 100, 100, 30, 2, 3, 10, 10, 4, -40, True, True, "蓝色猎犬死亡时会冰冻周围生物，持续约5秒。海象总是会带着两只蓝色猎犬。和火魔杖陷阱一样有几率可以找到冰魔杖陷阱。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_depths_worm_click(sender As Object, e As RoutedEventArgs) Handles button_A_depths_worm.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "4", "F_glow_berry", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "藏在地下并有发光的诱饵")
        A_Show("深渊蠕虫", "Depths Worm", "A_depths_worm", "NoDLC", 1, 0, 1, 900, 900, 75, 4, 3, 0, 0, 5, -25, True, True, "深渊蠕虫通常可以在废墟找到，也有可能有一只落单的深渊蠕虫在蘑菇树森林。平常它们会伪装成神秘的植物，一旦发现猎物就会从地下钻出来攻击，它们攻击一次就会回到地下行动并且再次钻出攻击。深渊蠕虫作为洞穴里的猎犬替代生物，每过一段时间就会主动找到玩家并攻击，一开始是12天1只，随着时间的增加频率和个数会越来越多，到了150天开始就是每5天出现3只。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_sea_hound_click(sender As Object, e As RoutedEventArgs) Handles button_A_sea_hound.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "G_hound's_tooth", "1(12.5%)", "F_shark_fin", "1(12.5%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("海狗", "Sea Hound", "A_sea_hound", "NoDLC", 1, 0, 1, 150, 0, 20, 1.5, 3, 10, 10, 3, -40, True, True, "在船难版里，如果猎犬进攻时间到了而玩家在海上的话，就会有海狗代替猎犬来发动进攻。其掉落的鱼翅可以做鱼翅汤和星芒头盔。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_krampus_click(sender As Object, e As RoutedEventArgs) Handles button_A_krampus.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "G_charcoal", "2", "G_krampus_sack", "1(1%)", "", "偷取的任何物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "偷东西(包括箱子和背包里的东西)")
        A_Show("坎普斯", "Krampus", "A_krampus", "NoDLC", 1, 0, 1, 200, 300, 50, 1.2, 3, 7, 7, 4, 0, False, False, "坎普斯会在顽皮值到达一定值(31-50之间的随机值)的时候出现，出现后顽皮值置0。它的动作类似火鸡，避免靠近玩家，但是会偷取箱子里或者地上的所有东西，包括背包(除了眼骨、鱼骨、星-空、露西斧、薇洛的打火机、Abigail之花、一堆气球、魔杖、传送机零件底座等特殊物品，除非它们在背包里)。在晚上它会睡觉，可以很容易击杀它。每分钟(一天8分钟)会减少一点顽皮值，而用火、狗牙陷阱、蜜蜂地雷、火药击杀生物将不会增加顽皮值，因为这被认为是环境破坏导致生物死亡。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_krampus_sw_click(sender As Object, e As RoutedEventArgs) Handles button_A_krampus_sw.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "G_charcoal", "2", "G_krampus_sack", "1(1%)", "", "偷取的任何物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "偷东西(包括箱子和背包里的东西)")
        A_Show("船难坎普斯", "Krampus", "A_krampus_sw", "SW", 0, 1, 0, 200, 0, 50, 1.2, 3, 7, 7, 4, 0, False, False, "坎普斯会在顽皮值到达一定值(31-50之间的随机值)的时候出现，出现后顽皮值置0。它的动作类似火鸡，避免靠近玩家，但是会偷取箱子里或者地上的所有东西，包括背包(除了眼骨、鱼骨、星-空、露西斧、薇洛的打火机、Abigail之花、一堆气球、魔杖、传送机零件底座等特殊物品，除非它们在背包里)。在晚上它会睡觉，可以很容易击杀它。每分钟(一天8分钟)会减少一点顽皮值，而用火、狗牙陷阱、蜜蜂地雷、火药击杀生物将不会增加顽皮值，因为这被认为是环境破坏导致生物死亡。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_ghost_click(sender As Object, e As RoutedEventArgs) Handles button_A_ghost.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("鬼魂", "Ghost", "A_ghost", "NoDLC", 1, 0, 1, 200, 200, 15, 1.2, 1.5, 2, 2, 2, -40, True, False, "用铲子挖坟有10%的几率挖出鬼魂，而挖开麦斯威尔的墓地里的所有坟墓一定会出现鬼魂。鬼魂移动速度非常缓慢并且过一段时间会消失，最好的办法就是置之不理。满月的时候每个墓地都会产生一只鬼魂。在血量过低的时候，鬼魂也会有伤心的样子。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_pirate_ghost_click(sender As Object, e As RoutedEventArgs) Handles button_A_pirate_ghost.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("海盗的鬼魂", "Pirate Ghost", "A_pirate_ghost", "SW", 0, 1, 0, 200, 0, 15, 1.2, 1.5, 2, 2, 2, -40, True, False, "在船难版里海盗的鬼魂替代了鬼魂，用锤子砸沉船或者在潮湿的坟墓里钓鱼都有可能会出现海盗的鬼魂。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_night_hand_click(sender As Object, e As RoutedEventArgs) Handles button_A_night_hand.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "抓灭各类火堆")
        A_Show("黑夜之手", "Night Hand", "A_night_hand", "NoDLC", 1, 1, 1, 0, 0, 0, 0, 0, 3, 3, 0, -40, False, False, "黑夜之手会在晚上点燃火堆时出现，它们会逐渐靠近并试图抓灭火堆，最多同时存在三只。玩家可以去踩黑夜之手使其后退，但是如果不踩掉其根源，就会一直试图靠近火堆。萤火虫可以有效防止黑夜之手。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_shadow_watcher_click(sender As Object, e As RoutedEventArgs) Handles button_A_shadow_watcher.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("暗影观察者", "Shadow Watcher", "A_shadow_watcher", "NoDLC", 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, -40, False, False, "它会出现在玩家精神低于65%的晚上，不能攻击或被攻击，踩在它的眼睛上或白天到来时就会消失。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_mr_skits_click(sender As Object, e As RoutedEventArgs) Handles button_A_mr_skits.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("Skits先生", "Mr.Skits", "A_mr_skits", "NoDLC", 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "它会出现在玩家精神低于82.5%的时候，不能攻击或被攻击，靠近时会转身爬走。它们还有可能出现在方尖碑升降的时候。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_mr_skittish_click(sender As Object, e As RoutedEventArgs) Handles button_A_mr_skittish.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("Skittish先生", "Mr.Skittish", "A_mr_skittish", "SW", 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "船难版的Skits先生。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_crawling_nightmare_click(sender As Object, e As RoutedEventArgs) Handles button_A_crawling_nightmare.Click
        Dim LootHeight As Integer = A_Loot("G_nightmare_fuel", "1", "G_nightmare_fuel", "1(50%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "被攻击时消失并在周围随机位置出现")
        A_Show("爬行的梦魇", "Crawling Nightmare", "A_crawling_nightmare", "NoDLC", 1, 1, 1, 300, 300, 20, 2.5, 3, 3, 3, 4, -100, True, True, "当玩家精神高于15%时爬行的梦魇会徘徊在玩家周围，并且会阻止玩家进入帐篷，当玩家精神低于15%时就会变成实体(可以攻击和被攻击)攻击玩家，并且被攻击时会消失并随机出现在附近某个位置，击杀爬行的梦魇会回复15点精神。诞生于影灯和远古雕像的爬行的梦魇会无视玩家的精神百分比攻击玩家(击杀后不回复精神)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_crawling_horror_click(sender As Object, e As RoutedEventArgs) Handles button_A_crawling_horror.Click
        Dim LootHeight As Integer = A_Loot("G_nightmare_fuel", "1", "G_nightmare_fuel", "1(50%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "被攻击时消失并在周围随机位置出现")
        A_Show("爬行的恐惧化身", "Crawling Horror", "A_crawling_horror", "NoDLC", 1, 1, 1, 400, 400, 50, 1.5, 3, 7, 7, 5, -100, True, True, "爬行的恐惧化身只在玩家精神低于15%的时候出现并攻击玩家，并且被攻击时会消失并随机出现在附近某个位置，击杀爬行的恐惧化身会回复33点精神。诞生于影灯、远古雕像和大豪华箱子的爬行的恐惧化身会无视玩家的精神百分比攻击玩家(击杀后不回复精神)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_swimming_horror_click(sender As Object, e As RoutedEventArgs) Handles button_A_swimming_horror.Click
        Dim LootHeight As Integer = A_Loot("G_nightmare_fuel", "1", "G_nightmare_fuel", "1(50%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "被攻击时消失并在周围随机位置出现")
        A_Show("海洋影怪", "Swimming Horror", "A_swimming_horror", "SW", 0, 1, 0, 300, 0, 20, 2.5, 3, 3, 3, 4, -100, True, True, "海洋影怪可以在浅滩发现，它们会主动不停地追逐玩家，除非玩家离开浅滩或回到岸上。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_charlie_click(sender As Object, e As RoutedEventArgs) Handles button_A_charlie.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "无所不在")
        A_Show("查理", "Charlie", "A_charlie", "NoDLC", 1, 1, 1, 0, 0, 100, 0, 0, 0, 0, 10, 0, True, False, "在玩家进入完全黑暗之后十秒左右，每隔几秒就会受到看不见的查理的攻击，攻击力高达100点，同时被攻击还会减少20点精神。在没有防御的情况下，大多数人物可以承受1次攻击。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_varg_click(sender As Object, e As RoutedEventArgs) Handles button_A_varg.Click
        Dim LootHeight As Integer = A_Loot("G_hound's_tooth", "1-3", "F_monster_meat", "4-6")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "召唤三种猎犬")
        A_Show("座狼", "Varg", "A_varg", "NoDLC", 1, 0, 1, 600, 1800, 50, 3, 5, 5.5, 5.5, 6, 0, True, True, "沿着动物足迹寻找有可能找到座狼(另外两种是考拉象或冬考拉象和钢羊)，找到的几率从第一天的5%到第一百天的33%。它会召唤猎犬、红色猎犬、蓝色猎犬中随机的两只，其中红色猎犬和蓝色猎犬根据季节而定，如果时间过久还会召唤更多的猎犬。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_ewecus_click(sender As Object, e As RoutedEventArgs) Handles button_A_ewecus.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "4", "G_steel_wool", "2-3", "F_phlegm", "1-2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "极黏黏液")
        A_Show("钢羊", "Ewecus", "A_ewecus", "DST", 0, 0, 1, 0, 800, 60, 0, 0, 0, 0, 6, 0, True, False, "沿着动物足迹寻找有可能找到钢羊(另外两种是考拉象或冬考拉象和座狼)，找到的几率从第一天的5%到第一百天的33%。钢羊的主要资源是钢丝绒和痰，钢丝绒可以制作浴血战鞍和刷子，至于痰，食用可以恢复12.5的饥饿但是要消耗15点精神。钢羊具有远程和近程两种攻击方式，远程是喷鼻涕黏住敌人，并且附带5点伤害，近程攻击伤害高达60点，单独挑战钢羊可不是什么好选择。", LootHeight, SpecialAbilityHeight)
    End Sub

    REM ------------------生物(其他)-------------------
    Private Sub button_A_glommer_click(sender As Object, e As RoutedEventArgs) Handles button_A_glommer.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "3", "G_glommer's_wings", "1", "F_glommer's_goop", "2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("格洛姆", "Glommer", "A_glommer", "NoDLC", 1, 0, 1, 100, 100, 0, 0, 0, 0, 0, 0, 6.25, False, False, "在月圆之夜(天数十位数为奇数个位数为一的晚上)在格洛姆雕像可以找到格洛姆之花，捡起它格洛姆就会出现，格洛姆会跟着格洛姆之花，可以带进洞穴。击杀格洛姆会直接增加50点顽皮值，召唤出坎普斯，格洛姆死后会在下个月圆之夜复活。格洛姆有6.25的精神光环，可以抵消晚上-5的精神光环影响。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_chester_click(sender As Object, e As RoutedEventArgs) Handles button_A_chester.Click
        Dim LootHeight As Integer = A_Loot("", "储存的物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跟随眼骨", "9格储物", "生命恢复", "升级为寒冰切斯特/暗影切斯特")
        A_Show("切斯特", "Chester", "A_chester", "NoDLC", 1, 0, 1, 450, 450, 0, 0, 0, 3, 7, 0, 0, False, False, "切斯特是跟随眼骨的储物生物，拥有9格储物功能。眼骨很有可能可以在一条通往青蛙池塘的路的尽头附近找到。切斯特每3秒可以恢复22.5点生命(寒冰切斯特、暗影切斯特、哈奇、河豚哈奇、音乐盒哈奇、鹈鹕、胖鹈鹕、火焰鹈鹕均有该属性)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_snow_chester_click(sender As Object, e As RoutedEventArgs) Handles button_A_snow_chester.Click
        Dim LootHeight As Integer = A_Loot("", "储存的物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跟随眼骨", "9格储物", "生命恢复", "食物保鲜")
        A_Show("寒冰切斯特", "Snow Chester", "A_snow_chester", "NoDLC", 1, 0, 1, 450, 450, 0, 0, 0, 3, 7, 0, 0, False, False, "月圆之夜在切斯特的每个格子里放入至少一个蓝宝石就会变成寒冰切斯特，具有冰箱的保鲜效果，死后变成正常的切斯特。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_shadow_chester_click(sender As Object, e As RoutedEventArgs) Handles button_A_shadow_chester.Click
        Dim LootHeight As Integer = A_Loot("", "储存的物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跟随眼骨", "12格储物", "生命恢复")
        A_Show("暗影切斯特", "Shadow Chester", "A_shadow_chester", "NoDLC", 1, 0, 1, 450, 450, 0, 0, 0, 3, 7, 0, 0, False, False, "月圆之夜在切斯特的每个格子里放入至少一个噩梦燃料就会变成暗影切斯特，拥有12个格子，死后变成正常的切斯特。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_hutch_click(sender As Object, e As RoutedEventArgs) Handles button_A_hutch.Click
        Dim LootHeight As Integer = A_Loot("", "储存的物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跟随星-空", "9格储物", "生命恢复", "放入荧光果发光", "升级为河豚哈奇/音乐盒哈奇")
        A_Show("哈奇", "Hutch", "A_hutch", "DST", 0, 0, 1, 0, 450, 0, 0, 0, 3, 7, 0, 0, False, False, "哈奇是洞穴版的切斯特，会跟随星-空，如果放入荧光果将会发光(不会消耗荧光果)。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_fugu_hutch_click(sender As Object, e As RoutedEventArgs) Handles button_A_fugu_hutch.Click
        Dim LootHeight As Integer = A_Loot("", "储存的物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跟随星-空", "9格储物", "生命恢复", "反击")
        A_Show("河豚哈奇", "Fugu Hutch", "A_fugu_hutch", "DST", 0, 0, 1, 0, 450, 0, 0, 0, 3, 7, 0, 0, False, False, "在有荧光果的情况下放入长矛或战斗长矛哈奇将会变成河豚哈奇，任何攻击河豚哈奇的生物将会受到30点反弹伤害，直到长矛或战斗长矛耗尽或取出。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_music_box_hutch_click(sender As Object, e As RoutedEventArgs) Handles button_A_music_box_hutch.Click
        Dim LootHeight As Integer = A_Loot("", "储存的物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跟随星-空", "9格储物", "生命恢复", "播放舒缓的音乐")
        A_Show("音乐盒哈奇", "Music Box Hutch", "A_music_box_hutch", "DST", 0, 0, 1, 0, 450, 0, 0, 0, 3, 7, 0, 0, False, False, "在有荧光果的情况下放入独奏乐器哈奇将会变成音乐盒哈奇，音乐盒哈奇会播放舒缓的音乐，提供精神回复光环，直到独奏乐器耗尽或取出。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_packim_baggims_click(sender As Object, e As RoutedEventArgs) Handles button_A_packim_baggims.Click
        Dim LootHeight As Integer = A_Loot("", "储存的物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跟随鱼骨", "9格储物", "生命恢复", "食用其体内鱼类食物", "升级为胖鹈鹕/火焰鹈鹕")
        A_Show("鹈鹕", "Packim Baggims", "A_packim_baggims", "SW", 0, 1, 0, 450, 450, 0, 0, 0, 10, 10, 0, 0, False, False, "船难版的切斯特，会跟随鱼骨(可能在除了出生岛屿之外的其他任何岛屿)，它可以吃最基本的鱼类食品，拥有150的最大饥饿值，饥饿值每天减少48点，在饥饿的时候不会掉血。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_fat_packim_baggims_click(sender As Object, e As RoutedEventArgs) Handles button_A_fat_packim_baggims.Click
        Dim LootHeight As Integer = A_Loot("", "储存的物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跟随鱼骨", "12格储物", "生命恢复", "食用其体内鱼类食物")
        A_Show("胖鹈鹕", "Fat Packim Baggims", "A_fat_packim_baggims", "SW", 0, 1, 0, 450, 450, 0, 0, 0, 10, 10, 0, 0, False, False, "当鹈鹕的饥饿值到达120以上时就会变成胖鹈鹕，拥有12个格子，此时当饥饿掉到0时又会变回普通鹈鹕，额外的三个格子里的物品会掉落下来。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_fire_packim_baggims_click(sender As Object, e As RoutedEventArgs) Handles button_A_fire_packim_baggims.Click
        Dim LootHeight As Integer = A_Loot("", "储存的物品")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "跟随鱼骨", "9格储物", "生命恢复", "火焰攻击附近怪物", "烹饪食物")
        A_Show("火焰鹈鹕", "Fire Packim Baggims", "A_fire_packim_baggims", "SW", 0, 1, 0, 450, 450, 0, 0, 0, 10, 10, 0, 0, False, False, "在鹈鹕的每个格子里放入至少一个黑曜石就会变成火焰鹈鹕，它免疫火并且会自动用远程火焰攻击怪物和敌对生物(包括韦伯)。它会自动烹煮食物，并且如果有易燃品那么就会变成灰烬。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_pig_king_click(sender As Object, e As RoutedEventArgs) Handles button_A_pig_king.Click
        ScrollViewer_AnimalLeft_PigKing.Visibility = Visibility.Visible
        ScrollViewer_AnimalLeft.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Krampus.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Apackim_baggims.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Yaarctopus.Visibility = Visibility.Collapsed
    End Sub

    Private Sub button_A_yaarctopus_click(sender As Object, e As RoutedEventArgs) Handles button_A_yaarctopus.Click
        ScrollViewer_AnimalLeft_Yaarctopus.Visibility = Visibility.Visible
        ScrollViewer_AnimalLeft.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Krampus.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_Apackim_baggims.Visibility = Visibility.Collapsed
        ScrollViewer_AnimalLeft_PigKing.Visibility = Visibility.Collapsed
    End Sub

    REM ------------------生物(巨型生物)-------------------
    Private Sub button_A_treeguard_1_click(sender As Object, e As RoutedEventArgs) Handles button_A_treeguard_1.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "G_living_log", "6")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("树精守卫(小)", "Treeguard", "A_treeguard", "NoDLC", 1, 0, 1, 1400, 2100, 35, 3, 2.1, 1.05, 1.05, 6, -100, True, True, "生成自小型常青树。世界天数3天后，砍倒常青树时，附近的常青树就有1.33%的可能性变成树精守卫，根据常青树的大小分为小、中、大三种。树精守卫刚生成时会对砍倒树的玩家产生永久性敌对关系，但是可以通过种植松果使其平静下来，在16单位以内种植有15%的可能性，在5单位以内种植有33%的可能性。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_treeguard_2_click(sender As Object, e As RoutedEventArgs) Handles button_A_treeguard_2.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "G_living_log", "6")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("树精守卫(中)", "Treeguard", "A_treeguard", "NoDLC", 1, 0, 1, 2000, 3000, 50, 3, 3, 1.5, 1.5, 6, -100, True, True, "生成自中型常青树。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_treeguard_3_click(sender As Object, e As RoutedEventArgs) Handles button_A_treeguard_3.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "1", "G_living_log", "6")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("树精守卫(大)", "Treeguard", "A_treeguard", "NoDLC", 1, 0, 1, 2500, 3750, 62, 3, 3.75, 1.875, 1.875, 6, -100, True, True, "生成自大型常青树。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_poison_birchnut_trees_click(sender As Object, e As RoutedEventArgs) Handles button_A_poison_birchnut_trees.Click
        Dim LootHeight As Integer = A_Loot("G_birchnut", "3", "G_living_log", "1-2", "G_living_log", "铲子", "G_nightmare_fuel", "1(20%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "生成坚果", "远程攻击的树根")
        A_Show("桦树精", "Poison Birchnut Trees", "A_poison_birchnut_trees", "NoDLC", 1, 0, 1, 0, 0, 40, 1.5, 2, 0, 0, 5, 0, True, True, "生成自桦树。世界天数3天后，除了冬天，砍倒桦树时，附近的桦树就有一定的可能性变成桦树精。桦树精会用它的树根(远程)攻击靠近它的所有生物，并且每过一小段时间就会生成一个坚果辅助攻击。经过足够长的时间桦树精会变回桦树。可以用斧头砍倒桦树。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_birchnutter_click(sender As Object, e As RoutedEventArgs) Handles button_A_birchnutter.Click
        Dim LootHeight As Integer = A_Loot("G_twigs", "1(60%)", "G_birchnut", "1(40%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("坚果", "Birchnutter", "A_birchnutter", "NoDLC", 1, 0, 1, 50, 50, 5, 2, 2.5, 3.5, 3.5, 2, 0, True, True, "坚果是桦树精生成的保护它的小生物，血量和攻击力不高，但胜在数量多，如果不理会它们直接去砍桦树精的话会吃不少苦头，如果有强力护甲就可以无视掉了。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_palm_treeguard_click(sender As Object, e As RoutedEventArgs) Handles button_A_palm_treeguard.Click
        Dim LootHeight As Integer = A_Loot("G_living_log", "6", "F_coconut", "2", "F_monster_meat", "1(一定几率)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "抛椰子远程攻击")
        A_Show("椰树守卫", "Palm Treeguard", "A_palm_treeguard", "SW", 0, 1, 0, 750, 0, 150, 3, 3, 1.5, 1.5, 6, -100, True, True, "和树精守卫一样，砍伐椰子树就有几率出现椰树守卫，它们同样有3种大小，但是属性并没有变化。一旦产生，它们会保持永久敌对状态，离开当前岛屿再回来仇恨还是保持，除非种植丛林树种或椰子。椰树守卫有近程攻击和远程抛椰子攻击，抛椰子可以通过地上的影子躲避。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_spider_queen_click(sender As Object, e As RoutedEventArgs) Handles button_A_spider_queen.Click
        Dim LootHeight As Integer = A_Loot("F_monster_meat", "4", "G_silk", "4", "S_spider_eggs", "1", "G_spiderhat", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "生成蜘蛛和战斗蜘蛛")
        A_Show("蜘蛛女王", "Spider Queen", "A_spider_queen", "NoDLC", 1, 0, 1, 1250, 2500, 80, 3, 5, 1.75, 1.75, 7, -400, True, True, "三级蜘蛛巢经过足够长的时间会变成蜘蛛女王。蜘蛛女王是一个小BOSS，每20秒会生成一只蜘蛛，如果攻击目标是玩家那么每到第三个蜘蛛就会是战斗蜘蛛，最多有十六只蜘蛛跟随蜘蛛女王，若玩家离得足够远，蜘蛛会回到蜘蛛女王身上。若经过足够长的时间不去管它(在视野外)，蜘蛛女王又会变成一级蜘蛛巢。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_big_tentacle_click(sender As Object, e As RoutedEventArgs) Handles button_A_big_tentacle.Click
        Dim LootHeight As Integer = A_Loot("F_light_bulb", "1(80%)", "G_tentacle_spike", "1(50%)", "G_tentacle_spots", "1(40%)", "G_marsh_turf", "1(25%)", "G_slurtle_slime", "1(10%)", "N_skeleton_3", "1(10%)", "G_rocks", "1(10%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "被攻击时生成小触手")
        A_Show("大触手", "Big Tentacle", "A_big_tentacle", "NoDLC", 1, 0, 1, 500, 750, 0, 0, 0, 0, 0, 4, -40, False, True, "洞穴中可以发现大触手，它们血量很少而且不会攻击，但是被攻击时会生成许多小触手保护。击杀后可以得到些许材料，而小触手会全部消失，所在位置会有一个巨大的洞，跳进去会传送到另一个大触手的位置。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_baby_tentacle_click(sender As Object, e As RoutedEventArgs) Handles button_A_baby_tentacle.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("小触手", "Baby Tentacle", "A_baby_tentacle", "NoDLC", 1, 0, 1, 20, 20, 5, 3, 3, 0, 0, 2, -40, True, True, "由大触手生成，攻击力极低但胜在数量多，有防御的话不足为惧。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_ancient_guardian_click(sender As Object, e As RoutedEventArgs) Handles button_A_ancient_guardian.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "8", "F_guardian's_horn", "1", "N_ornate_chest", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "摧毁冲撞路径上的障碍物")
        A_Show("远古守护者", "Ancient Guardian", "A_ancient_guardian", "NoDLC", 1, 0, 1, 2500, 10000, 100, 2, 25, 5, 17, 8, 0, True, False, "远古守护者是在废墟迷宫区域的小BOSS，在联机版中更是有10000点生命。它的行动方式和机械战车类似，但是多加了一个100点伤害的近程攻击。击杀后掉落的守卫者的角可以配合巨鹿眼球制作恒迪尤斯·舒提尤斯。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_moose_click(sender As Object, e As RoutedEventArgs) Handles button_A_moose.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "6", "F_drumstick", "2", "G_down_feather", "3-5")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "尖叫使得武器掉落")
        A_Show("鹿角鹅", "Moose", "A_moose", "NoDLC", 1, 0, 1, 3000, 6000, 75, 3, 5.5, 8, 12, 7, 0, True, True, "鹿角鹅是春季BOSS，在进入春天的2-4天出现(前提是至少有3个建筑在靠近的位置，比如科学机器、避雷针、烹煮锅等)，在夏季到达时会离开。到达约一天后会生成鹿角鹅蛋，孵化出5只莫斯林，莫斯林不会成长为鹿角鹅。注意如果青蛙雨的青蛙攻击了莫斯林可能会产生另一只鹿角鹅。鹿角鹅不会破坏玩家的建筑，除了墙。鹿角鹅只会近程攻击(攻击生物时攻击力为150)，每到第三次攻击就会让玩家的武器掉落在附近地上。可以把鹿角鹅吸引到皮弗娄牛处或者青蛙雨处击杀。如果自己动手打的话带上武器防具注意躲避攻击也是可以击杀的。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_mosling_click(sender As Object, e As RoutedEventArgs) Handles button_A_mosling.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "1", "F_drumstick", "1", "G_down_feather", "2-3")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "旋风冲撞")
        A_Show("莫斯林", "Mosling", "A_mosling", "NoDLC", 1, 0, 1, 350, 525, 50, 3, 2, 5, 5, 4, 0, True, True, "莫斯林出现在鹿角鹅和鹿角鹅巢附近。正常情况下，它们会像火鸡一样尽量避免冲突，但是如果鹿角鹅被杀死它们会变成愤怒的红色，并且让自身变为旋风攻击。旋风停下会有一小段眩晕时间，趁这个时候击杀它。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_dragonfly_click(sender As Object, e As RoutedEventArgs) Handles button_A_dragonfly.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "8(ROG)", "F_meat", "6(DST)", "G_scales", "1", "G_lavae_egg", "1(33%)(DST)", "S_gold_nugget", "4-8(DST)", "G_red_gem", "2(DST)", "G_blue_gem", "2(DST)", "G_purple_gem", "1-2(DST)", "G_green_gem", "1-2(DST)", "G_orange_gem", "1-2(DST)", "G_yellow_gem", "1-2(DST)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "生成熔岩幼虫", "被攻击时用火焰包围自己(ROG)", "让靠近它的任何东西着火(ROG)", "对附近任何东西造成火焰伤害(ROG)", "所有熔岩幼虫被击杀时用火焰包围自己(DST)")
        A_Show("龙蝇", "Dragonfly", "A_dragonfly", "NoDLC", 1, 0, 1, 2750, 27500, 75, 2.5, 4, 4, 4, 10, -400, True, True, "龙蝇是夏季BOSS，通常可以在荒漠的岩浆附近找到。拍打攻击伤害为75(对生物150)，并且会放出三次37.5(对生物75)伤害的火环，有燃烧效果。联机版的龙蝇是生命值最高的生物，还会生成能燃烧物品的熔岩幼虫，要十分小心。如果龙蝇离开它的巢穴过远就会飞回去并恢复满状态。可以建造围墙挡住熔岩幼虫然后穿上足够的装备击杀。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_lavae_click(sender As Object, e As RoutedEventArgs) Handles button_A_lavae.Click
        Dim LootHeight As Integer = A_Loot("G_rocks", "1-5(如果被冰冻)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "让靠近它的任何东西着火", "对附近任何东西造成火焰伤害")
        A_Show("熔岩幼虫", "Lavae", "A_lavae", "DST", 0, 0, 1, 0, 500, 50, 0, 3, 5.5, 5.5, 4, 0, True, True, "熔岩幼虫是龙蝇生成的生物，会把碰到的东西燃烧，但是可以用墙挡住它。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_Extra_Adorable_Lavae_click(sender As Object, e As RoutedEventArgs) Handles button_A_Extra_Adorable_Lavae.Click
        Dim LootHeight As Integer = A_Loot()
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "站在任何东西附近一会儿会使其着火", "提供光、热、烹饪")
        A_Show("超可爱的熔岩虫", "Extra-Adorable Lavae", "A_lavae", "DST", 0, 0, 1, 0, 250, 50, 0, 3, 5.5, 5.5, 0, 0, True, False, "在联机版里，击杀龙蝇将会有一定几率掉落龙岩虫卵，可以孵化出超可爱的熔岩虫，并且掉落熔岩虫牙。超可爱的熔岩虫会跟随着熔岩虫牙，能提供光和热，可以用它烹煮食物，就像星星呼唤者权杖召唤的矮人明星一样。超可爱的熔岩虫吃灰烬和木炭，如果实在是饿了，会放火，不顾玩家自己吃东西。它的饥饿度能让它活两天，一个灰烬能回复18.75%的饥饿。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_bearger_click(sender As Object, e As RoutedEventArgs) Handles button_A_bearger.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "8", "S_thick_fur", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "范围攻击", "吃地上和容器里的食物", "每45秒掉落一个毛簇(DST)")
        A_Show("熊獾", "Bearger", "A_bearger", "NoDLC", 1, 0, 1, 3000, 6000, 100, 3, 6, 3, 10, 8, -100, False, False, "熊獾是秋季BOSS，出现后会到处寻找食物吃，包括箱子里的食物，会被蜂箱吸引，吃了10个蜂蜜就会睡觉。它路过的建筑和树木会被摧毁。在联机版里，熊獾会一边走一边掉落毛簇，90个毛簇即可制作一个厚皮毛(击杀熊獾也会掉落一个厚皮毛)。熊獾被激怒后移动速度会提升到6，精神影响也会变成-400/min，它会攻击玩家和生物，攻击生物时攻击力为200点。它的攻击方式是打三下然后用前爪猛击地面，产生的冲击波会摧毁树木和建筑，所以击杀熊獾时要远离家里。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_deerclops_click(sender As Object, e As RoutedEventArgs) Handles button_A_deerclops.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "8", "F_deerclops_eyeball", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "范围攻击", "攻击摧毁树木、建筑", "冰冻攻击")
        A_Show("独眼巨鹿", "Deerclops", "A_deerclops", "NoDLC", 1, 0, 1, 2000, 4000, 75, 3, 6, 3, 3, 8, -100, True, False, "独眼巨鹿是冬季BOSS，会在入冬第十天左右的晚上出现，会有脚步声警示。它出现后的第一次攻击会寻找离玩家最近的建筑，它的攻击能破坏建筑，并且经过的地方树木也会被摧毁。它的目标是摧毁所有建筑，但是如果玩家去骚扰它的话它会先收拾掉玩家再去拆建筑。它的攻击方式是用爪子向一个方向拍出一大片冰的范围攻击，可以通过走位来躲避攻击。可以把它引到树精守卫多的地方或者皮弗娄牛群让它们打起来，当然对装备和操作有信心也可以自己击杀。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_quacken_click(sender As Object, e As RoutedEventArgs) Handles button_A_quacken.Click
        Dim LootHeight As Integer = A_Loot("N_chest_of_the_depths", "1", "N_booty_bag_1", "1", "G_iron_key", "1(仅第一次)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "墨水炸弹", "生成呱肯乌贼的触手")
        A_Show("呱肯乌贼", "Quacken", "A_quacken", "SW", 0, 1, 0, 1000, 0, 50, 7.5, 20, 0, 0, 6, 0, True, True, "呱肯乌贼是船难版的BOSS之一，用钓竿在潮湿的坟墓或者在深海用拖网就有机会召唤出呱肯乌贼，每一个物品陷入拖网中就有5%的几率产生呱肯乌贼。击杀呱肯乌贼的20天内不会再产生第二个呱肯乌贼。它本身没有近程攻击，但是会召唤呱肯乌贼的触手帮助它攻击(从小地图看就像是食人花和眼球草)，它的远程攻击是墨水弹，可以通过阴影躲避，被击中受到50点伤害，并且墨水弹掉落位置会形成持续几秒的墨水斑点，在墨水斑点区域内会减速70%。当本体每损失250点生命时，呱肯乌贼会移动到附近的另一个地方，总共会移动三次。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_quacken_tentacle_click(sender As Object, e As RoutedEventArgs) Handles button_A_quacken_tentacle.Click
        Dim LootHeight As Integer = A_Loot("G_tentacle_spike", "1(5%)", "G_tentacle_spots", "1(10%)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "制造巨浪")
        A_Show("呱肯乌贼的触手", "Quacken Tentacle", "A_quacken_tentacle", "SW", 0, 1, 0, 90, 0, 50, 6, 4, 0, 0, 3, 0, True, True, "呱肯乌贼的触手本身会近程攻击，另外还会掀起浪花攻击，生命值和呱肯乌贼本体并不共享，所以无视就好了。击杀后获得的铁制钥匙是和金制钥匙、骨制钥匙一起解锁火山笼子里的伍德莱格的。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_sealnado_click(sender As Object, e As RoutedEventArgs) Handles button_A_sealnado.Click
        Dim LootHeight As Integer = A_Loot("G_turbine_blades", "1")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "范围攻击", "可以在岛屿和海洋活动")
        A_Show("豹卷风", "Sealnado", "A_sealnado", "SW", 0, 1, 0, 3000, 0, 75, 3, 6, 5, 13, 9, 0, False, False, "豹卷风是船难版的BOSS之一，会在风季出现。平时不去惹它时不会主动攻击，但是当玩家靠的过近或者攻击它时它就会拍打一下然后吸入周围所有物品再喷出来，被吸入会减少250点生命并下降33点精神(目前版本有一个被吸入就无法出来的BUG，只能重启游戏了)。要击杀它用打二走一的方法或者用远程武器击杀，死后生成海豹。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_seal_click(sender As Object, e As RoutedEventArgs) Handles button_A_seal.Click
        Dim LootHeight As Integer = A_Loot("F_meat", "4", "G_magic_seal", "1(无论是否被击杀)")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight)
        A_Show("海豹", "Seal", "A_seal", "SW", 0, 1, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "豹卷风死后生成无害的海豹，但是击杀它会立即引来坎普斯。不论是否击杀都能得到豹印。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_tiger_shark_click(sender As Object, e As RoutedEventArgs) Handles button_A_tiger_shark.Click
        Dim LootHeight As Integer = A_Loot("F_raw_fish", "8", "F_eye_of_the_tiger_shark", "1-2", "G_shark_gills", "2-4")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "范围攻击", "可以在陆地或海洋", "生成小虎鲨")
        A_Show("虎鲨", "Tiger Shark", "A_tiger_shark", "SW", 0, 1, 0, 2500, 0, 100, 3, 4, 8, 12, 7, 0, True, False, "虎鲨是船难版的BOSS之一，会在任何季节玩家做某些事情时在海岸边生成，风季的可能性比较大(用海洋陷阱和拖网的可能性也比较大)。它的攻击是范围性的。白天时，当它的血量低于250就会逃入海中，玩家无法追上，只能用远程武器击杀，黄昏和夜晚则不会逃跑，并且在陆地，可以轻松击杀。", LootHeight, SpecialAbilityHeight)
    End Sub

    Private Sub button_A_sharkitten_click(sender As Object, e As RoutedEventArgs) Handles button_A_sharkitten.Click
        Dim LootHeight As Integer = A_Loot("F_raw_fish", "2-3", "G_shark_gills", "0-2")
        Dim SpecialAbilityHeight As Integer = A_SpecialAbility(LootHeight, "被虎鲨保护")
        A_Show("小虎鲨", "Sharkitten", "A_sharkitten", "SW", 0, 1, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, False, False, "虎鲨生成的生物，不会攻击，只会跟随着虎鲨。", LootHeight, SpecialAbilityHeight)
    End Sub

    REM 生物DLC检测初始化
    Private Sub A_DLC_Check_initialization()
        REM 陆地生物
        button_A_rabbit.Visibility = Visibility.Collapsed
        button_A_rabbit_winter.Visibility = Visibility.Collapsed
        button_A_beardling.Visibility = Visibility.Collapsed
        button_A_gobbler.Visibility = Visibility.Collapsed
        button_A_frog.Visibility = Visibility.Collapsed
        button_A_catcoon.Visibility = Visibility.Collapsed
        button_A_moleworm.Visibility = Visibility.Collapsed
        button_A_spider_warrior.Visibility = Visibility.Collapsed
        button_A_pig_man.Visibility = Visibility.Collapsed
        button_A_guard_pig.Visibility = Visibility.Collapsed
        button_A_baby_beefalo.Visibility = Visibility.Collapsed
        button_A_toddler_beefalo.Visibility = Visibility.Collapsed
        button_A_teen_beefalo.Visibility = Visibility.Collapsed
        button_A_beefalo.Visibility = Visibility.Collapsed
        button_A_pengull.Visibility = Visibility.Collapsed
        button_A_wee_mactusk.Visibility = Visibility.Collapsed
        button_A_mactusk.Visibility = Visibility.Collapsed
        button_A_volt_goat.Visibility = Visibility.Collapsed
        button_A_volt_goat_withelectric.Visibility = Visibility.Collapsed
        button_A_koalefant.Visibility = Visibility.Collapsed
        button_A_winter_koalefant.Visibility = Visibility.Collapsed
        button_A_clockwork_knight.Visibility = Visibility.Collapsed
        button_A_clockwork_bishop.Visibility = Visibility.Collapsed
        button_A_clckwork_rook.Visibility = Visibility.Collapsed
        button_A_grass_gekko.Visibility = Visibility.Collapsed
        button_A_grass_gekko_disease.Visibility = Visibility.Collapsed
        button_A_crabbit.Visibility = Visibility.Collapsed
        button_A_beardling_sw.Visibility = Visibility.Collapsed
        button_A_prime_ape.Visibility = Visibility.Collapsed
        button_A_spider_warrior_sw.Visibility = Visibility.Collapsed
        button_A_fishermerm.Visibility = Visibility.Collapsed
        button_A_flup.Visibility = Visibility.Collapsed
        button_A_wildbore.Visibility = Visibility.Collapsed
        button_A_snake.Visibility = Visibility.Collapsed
        button_A_poison_snake.Visibility = Visibility.Collapsed
        button_A_baby_doydoy.Visibility = Visibility.Collapsed
        button_A_doydoy_child.Visibility = Visibility.Collapsed
        button_A_doydoy.Visibility = Visibility.Collapsed
        button_A_dragoon.Visibility = Visibility.Collapsed
        button_A_elephant_cactus.Visibility = Visibility.Collapsed
        REM 海洋生物
        TextBlock_A_Sea.Visibility = Visibility.Collapsed
        WrapPanel_A_sea.Visibility = Visibility.Collapsed
        REM 飞行生物
        button_A_butterfly.Visibility = Visibility.Collapsed
        button_A_butterfly_sw.Visibility = Visibility.Collapsed
        button_A_mosquito.Visibility = Visibility.Collapsed
        button_A_mosquito_sw.Visibility = Visibility.Collapsed
        button_A_redbird.Visibility = Visibility.Collapsed
        button_A_snowbird.Visibility = Visibility.Collapsed
        button_A_crow.Visibility = Visibility.Collapsed
        button_A_buzzards.Visibility = Visibility.Collapsed
        button_A_parrot.Visibility = Visibility.Collapsed
        button_A_parrot_pirate.Visibility = Visibility.Collapsed
        button_A_toucan.Visibility = Visibility.Collapsed
        button_A_seagull.Visibility = Visibility.Collapsed
        button_A_fireflies.Visibility = Visibility.Collapsed
        REM 洞穴生物
        TextBlock_A_Cave.Visibility = Visibility.Collapsed
        WrapPanel_A_cave.Visibility = Visibility.Collapsed
        button_A_bunnyman.Visibility = Visibility.Collapsed
        button_A_beardlord.Visibility = Visibility.Collapsed
        button_A_blue_spore.Visibility = Visibility.Collapsed
        button_A_green_spore.Visibility = Visibility.Collapsed
        button_A_red_spore.Visibility = Visibility.Collapsed
        button_A_splumonkey.Visibility = Visibility.Collapsed
        button_A_shadow_splumonkey.Visibility = Visibility.Collapsed
        button_A_cave_spider.Visibility = Visibility.Collapsed
        button_A_spitter.Visibility = Visibility.Collapsed
        button_A_dangling_depth_dweller.Visibility = Visibility.Collapsed
        button_A_batilisk.Visibility = Visibility.Collapsed
        button_A_slurtles.Visibility = Visibility.Collapsed
        button_A_snurtles.Visibility = Visibility.Collapsed
        button_A_rock_lobster.Visibility = Visibility.Collapsed
        button_A_slurper.Visibility = Visibility.Collapsed
        button_A_big_tentacle.Visibility = Visibility.Collapsed
        button_A_baby_tentacle.Visibility = Visibility.Collapsed
        button_A_damaged_knight.Visibility = Visibility.Collapsed
        button_A_damaged_bishop.Visibility = Visibility.Collapsed
        button_A_damage_rook.Visibility = Visibility.Collapsed
        REM 邪恶生物
        button_A_tentacle.Visibility = Visibility.Collapsed
        button_A_shadow_tentacle.Visibility = Visibility.Collapsed
        button_A_depths_worm.Visibility = Visibility.Collapsed
        button_A_sea_hound.Visibility = Visibility.Collapsed
        button_A_krampus.Visibility = Visibility.Collapsed
        button_A_krampus_sw.Visibility = Visibility.Collapsed
        button_A_ghost.Visibility = Visibility.Collapsed
        button_A_pirate_ghost.Visibility = Visibility.Collapsed
        button_A_mr_skits.Visibility = Visibility.Collapsed
        button_A_mr_skittish.Visibility = Visibility.Collapsed
        button_A_swimming_horror.Visibility = Visibility.Collapsed
        button_A_varg.Visibility = Visibility.Collapsed
        button_A_ewecus.Visibility = Visibility.Collapsed
        REM 其他生物
        button_A_glommer.Visibility = Visibility.Collapsed
        button_A_chester.Visibility = Visibility.Collapsed
        button_A_snow_chester.Visibility = Visibility.Collapsed
        button_A_shadow_chester.Visibility = Visibility.Collapsed
        button_A_hutch.Visibility = Visibility.Collapsed
        button_A_fugu_hutch.Visibility = Visibility.Collapsed
        button_A_music_box_hutch.Visibility = Visibility.Collapsed
        button_A_packim_baggims.Visibility = Visibility.Collapsed
        button_A_fat_packim_baggims.Visibility = Visibility.Collapsed
        button_A_fire_packim_baggims.Visibility = Visibility.Collapsed
        button_A_pig_king.Visibility = Visibility.Collapsed
        button_A_yaarctopus.Visibility = Visibility.Collapsed
        REM 巨型生物
        button_A_treeguard_1.Visibility = Visibility.Collapsed
        button_A_treeguard_2.Visibility = Visibility.Collapsed
        button_A_treeguard_3.Visibility = Visibility.Collapsed
        button_A_poison_birchnut_trees.Visibility = Visibility.Collapsed
        button_A_birchnutter.Visibility = Visibility.Collapsed
        button_A_palm_treeguard.Visibility = Visibility.Collapsed
        button_A_spider_queen.Visibility = Visibility.Collapsed
        button_A_big_tentacle.Visibility = Visibility.Collapsed
        button_A_baby_tentacle.Visibility = Visibility.Collapsed
        button_A_ancient_guardian.Visibility = Visibility.Collapsed
        button_A_moose.Visibility = Visibility.Collapsed
        button_A_mosling.Visibility = Visibility.Collapsed
        button_A_dragonfly.Visibility = Visibility.Collapsed
        button_A_lavae.Visibility = Visibility.Collapsed
        button_A_Extra_Adorable_Lavae.Visibility = Visibility.Collapsed
        button_A_bearger.Visibility = Visibility.Collapsed
        button_A_deerclops.Visibility = Visibility.Collapsed
        button_A_quacken.Visibility = Visibility.Collapsed
        button_A_quacken_tentacle.Visibility = Visibility.Collapsed
        button_A_sealnado.Visibility = Visibility.Collapsed
        button_A_seal.Visibility = Visibility.Collapsed
        button_A_tiger_shark.Visibility = Visibility.Collapsed
        button_A_sharkitten.Visibility = Visibility.Collapsed
    End Sub

    Private Sub A_DLC_ROG_SHOW()
        REM 陆地生物
        button_A_rabbit.Visibility = Visibility.Visible
        button_A_rabbit_winter.Visibility = Visibility.Visible
        button_A_beardling.Visibility = Visibility.Visible
        button_A_gobbler.Visibility = Visibility.Visible
        button_A_frog.Visibility = Visibility.Visible
        button_A_catcoon.Visibility = Visibility.Visible
        button_A_moleworm.Visibility = Visibility.Visible
        button_A_spider_warrior.Visibility = Visibility.Visible
        button_A_pig_man.Visibility = Visibility.Visible
        button_A_guard_pig.Visibility = Visibility.Visible
        button_A_baby_beefalo.Visibility = Visibility.Visible
        button_A_toddler_beefalo.Visibility = Visibility.Visible
        button_A_teen_beefalo.Visibility = Visibility.Visible
        button_A_beefalo.Visibility = Visibility.Visible
        button_A_pengull.Visibility = Visibility.Visible
        button_A_wee_mactusk.Visibility = Visibility.Visible
        button_A_mactusk.Visibility = Visibility.Visible
        button_A_volt_goat.Visibility = Visibility.Visible
        button_A_volt_goat_withelectric.Visibility = Visibility.Visible
        button_A_koalefant.Visibility = Visibility.Visible
        button_A_winter_koalefant.Visibility = Visibility.Visible
        button_A_clockwork_knight.Visibility = Visibility.Visible
        button_A_clockwork_bishop.Visibility = Visibility.Visible
        button_A_clckwork_rook.Visibility = Visibility.Visible
        REM 海洋生物
        REM 飞行生物
        button_A_butterfly.Visibility = Visibility.Visible
        button_A_mosquito.Visibility = Visibility.Visible
        button_A_redbird.Visibility = Visibility.Visible
        button_A_snowbird.Visibility = Visibility.Visible
        button_A_crow.Visibility = Visibility.Visible
        button_A_buzzards.Visibility = Visibility.Visible
        button_A_fireflies.Visibility = Visibility.Visible
        REM 洞穴生物
        TextBlock_A_Cave.Visibility = Visibility.Visible
        WrapPanel_A_cave.Visibility = Visibility.Visible
        button_A_bunnyman.Visibility = Visibility.Visible
        button_A_beardlord.Visibility = Visibility.Visible
        button_A_splumonkey.Visibility = Visibility.Visible
        button_A_shadow_splumonkey.Visibility = Visibility.Visible
        button_A_cave_spider.Visibility = Visibility.Visible
        button_A_spitter.Visibility = Visibility.Visible
        button_A_dangling_depth_dweller.Visibility = Visibility.Visible
        button_A_batilisk.Visibility = Visibility.Visible
        button_A_slurtles.Visibility = Visibility.Visible
        button_A_snurtles.Visibility = Visibility.Visible
        button_A_rock_lobster.Visibility = Visibility.Visible
        button_A_slurper.Visibility = Visibility.Visible
        button_A_big_tentacle.Visibility = Visibility.Visible
        button_A_baby_tentacle.Visibility = Visibility.Visible
        button_A_damaged_knight.Visibility = Visibility.Visible
        button_A_damaged_bishop.Visibility = Visibility.Visible
        button_A_damage_rook.Visibility = Visibility.Visible
        REM 邪恶生物
        button_A_tentacle.Visibility = Visibility.Visible
        button_A_shadow_tentacle.Visibility = Visibility.Visible
        button_A_depths_worm.Visibility = Visibility.Visible
        button_A_krampus.Visibility = Visibility.Visible
        button_A_ghost.Visibility = Visibility.Visible
        button_A_mr_skits.Visibility = Visibility.Visible
        button_A_varg.Visibility = Visibility.Visible
        REM 其他生物
        button_A_glommer.Visibility = Visibility.Visible
        button_A_chester.Visibility = Visibility.Visible
        button_A_snow_chester.Visibility = Visibility.Visible
        button_A_shadow_chester.Visibility = Visibility.Visible
        button_A_pig_king.Visibility = Visibility.Visible
        REM 巨型生物
        button_A_treeguard_1.Visibility = Visibility.Visible
        button_A_treeguard_2.Visibility = Visibility.Visible
        button_A_treeguard_3.Visibility = Visibility.Visible
        button_A_poison_birchnut_trees.Visibility = Visibility.Visible
        button_A_birchnutter.Visibility = Visibility.Visible
        button_A_spider_queen.Visibility = Visibility.Visible
        button_A_big_tentacle.Visibility = Visibility.Visible
        button_A_baby_tentacle.Visibility = Visibility.Visible
        button_A_ancient_guardian.Visibility = Visibility.Visible
        button_A_moose.Visibility = Visibility.Visible
        button_A_mosling.Visibility = Visibility.Visible
        button_A_dragonfly.Visibility = Visibility.Visible
        button_A_lavae.Visibility = Visibility.Visible
        button_A_Extra_Adorable_Lavae.Visibility = Visibility.Visible
        button_A_bearger.Visibility = Visibility.Visible
        button_A_deerclops.Visibility = Visibility.Visible
    End Sub

    Private Sub A_DLC_SW_SHOW()
        REM 陆地生物
        button_A_crabbit.Visibility = Visibility.Visible
        button_A_beardling_sw.Visibility = Visibility.Visible
        button_A_prime_ape.Visibility = Visibility.Visible
        button_A_spider_warrior_sw.Visibility = Visibility.Visible
        button_A_fishermerm.Visibility = Visibility.Visible
        button_A_flup.Visibility = Visibility.Visible
        button_A_wildbore.Visibility = Visibility.Visible
        button_A_snake.Visibility = Visibility.Visible
        button_A_poison_snake.Visibility = Visibility.Visible
        button_A_baby_doydoy.Visibility = Visibility.Visible
        button_A_doydoy_child.Visibility = Visibility.Visible
        button_A_doydoy.Visibility = Visibility.Visible
        button_A_dragoon.Visibility = Visibility.Visible
        button_A_elephant_cactus.Visibility = Visibility.Visible
        REM 海洋生物
        TextBlock_A_Sea.Visibility = Visibility.Visible
        WrapPanel_A_sea.Visibility = Visibility.Visible
        REM 飞行生物
        button_A_butterfly_sw.Visibility = Visibility.Visible
        button_A_mosquito_sw.Visibility = Visibility.Visible
        button_A_parrot.Visibility = Visibility.Visible
        button_A_parrot_pirate.Visibility = Visibility.Visible
        button_A_toucan.Visibility = Visibility.Visible
        button_A_seagull.Visibility = Visibility.Visible
        REM 洞穴生物
        REM 邪恶生物
        button_A_sea_hound.Visibility = Visibility.Visible
        button_A_krampus_sw.Visibility = Visibility.Visible
        button_A_pirate_ghost.Visibility = Visibility.Visible
        button_A_mr_skittish.Visibility = Visibility.Visible
        button_A_swimming_horror.Visibility = Visibility.Visible
        REM 其他生物
        button_A_packim_baggims.Visibility = Visibility.Visible
        button_A_fat_packim_baggims.Visibility = Visibility.Visible
        button_A_fire_packim_baggims.Visibility = Visibility.Visible
        button_A_yaarctopus.Visibility = Visibility.Visible
        REM 巨型生物
        button_A_palm_treeguard.Visibility = Visibility.Visible
        button_A_quacken.Visibility = Visibility.Visible
        button_A_quacken_tentacle.Visibility = Visibility.Visible
        button_A_sealnado.Visibility = Visibility.Visible
        button_A_seal.Visibility = Visibility.Visible
        button_A_tiger_shark.Visibility = Visibility.Visible
        button_A_sharkitten.Visibility = Visibility.Visible
    End Sub

    Private Sub A_DLC_DST_SHOW()
        REM 陆地生物
        button_A_rabbit.Visibility = Visibility.Visible
        button_A_rabbit_winter.Visibility = Visibility.Visible
        button_A_beardling.Visibility = Visibility.Visible
        button_A_gobbler.Visibility = Visibility.Visible
        button_A_frog.Visibility = Visibility.Visible
        button_A_catcoon.Visibility = Visibility.Visible
        button_A_moleworm.Visibility = Visibility.Visible
        button_A_spider_warrior.Visibility = Visibility.Visible
        button_A_pig_man.Visibility = Visibility.Visible
        button_A_guard_pig.Visibility = Visibility.Visible
        button_A_baby_beefalo.Visibility = Visibility.Visible
        button_A_toddler_beefalo.Visibility = Visibility.Visible
        button_A_teen_beefalo.Visibility = Visibility.Visible
        button_A_beefalo.Visibility = Visibility.Visible
        button_A_pengull.Visibility = Visibility.Visible
        button_A_wee_mactusk.Visibility = Visibility.Visible
        button_A_mactusk.Visibility = Visibility.Visible
        button_A_volt_goat.Visibility = Visibility.Visible
        button_A_volt_goat_withelectric.Visibility = Visibility.Visible
        button_A_koalefant.Visibility = Visibility.Visible
        button_A_winter_koalefant.Visibility = Visibility.Visible
        button_A_clockwork_knight.Visibility = Visibility.Visible
        button_A_clockwork_bishop.Visibility = Visibility.Visible
        button_A_clckwork_rook.Visibility = Visibility.Visible
        button_A_grass_gekko.Visibility = Visibility.Visible
        button_A_grass_gekko_disease.Visibility = Visibility.Visible
        REM 海洋生物
        REM 飞行生物
        button_A_butterfly.Visibility = Visibility.Visible
        button_A_mosquito.Visibility = Visibility.Visible
        button_A_redbird.Visibility = Visibility.Visible
        button_A_snowbird.Visibility = Visibility.Visible
        button_A_crow.Visibility = Visibility.Visible
        button_A_buzzards.Visibility = Visibility.Visible
        button_A_fireflies.Visibility = Visibility.Visible
        REM 洞穴生物
        TextBlock_A_Cave.Visibility = Visibility.Visible
        WrapPanel_A_cave.Visibility = Visibility.Visible
        button_A_bunnyman.Visibility = Visibility.Visible
        button_A_beardlord.Visibility = Visibility.Visible
        button_A_blue_spore.Visibility = Visibility.Visible
        button_A_green_spore.Visibility = Visibility.Visible
        button_A_red_spore.Visibility = Visibility.Visible
        button_A_splumonkey.Visibility = Visibility.Visible
        button_A_shadow_splumonkey.Visibility = Visibility.Visible
        button_A_cave_spider.Visibility = Visibility.Visible
        button_A_spitter.Visibility = Visibility.Visible
        button_A_dangling_depth_dweller.Visibility = Visibility.Visible
        button_A_batilisk.Visibility = Visibility.Visible
        button_A_slurtles.Visibility = Visibility.Visible
        button_A_snurtles.Visibility = Visibility.Visible
        button_A_rock_lobster.Visibility = Visibility.Visible
        button_A_slurper.Visibility = Visibility.Visible
        button_A_big_tentacle.Visibility = Visibility.Visible
        button_A_baby_tentacle.Visibility = Visibility.Visible
        button_A_damaged_knight.Visibility = Visibility.Visible
        button_A_damaged_bishop.Visibility = Visibility.Visible
        button_A_damage_rook.Visibility = Visibility.Visible
        REM 邪恶生物
        button_A_tentacle.Visibility = Visibility.Visible
        button_A_shadow_tentacle.Visibility = Visibility.Visible
        button_A_depths_worm.Visibility = Visibility.Visible
        button_A_krampus.Visibility = Visibility.Visible
        button_A_ghost.Visibility = Visibility.Visible
        button_A_mr_skits.Visibility = Visibility.Visible
        button_A_varg.Visibility = Visibility.Visible
        button_A_ewecus.Visibility = Visibility.Visible
        REM 其他生物
        button_A_glommer.Visibility = Visibility.Visible
        button_A_chester.Visibility = Visibility.Visible
        button_A_snow_chester.Visibility = Visibility.Visible
        button_A_shadow_chester.Visibility = Visibility.Visible
        button_A_hutch.Visibility = Visibility.Visible
        button_A_fugu_hutch.Visibility = Visibility.Visible
        button_A_music_box_hutch.Visibility = Visibility.Visible
        button_A_pig_king.Visibility = Visibility.Visible
        REM 巨型生物
        button_A_treeguard_1.Visibility = Visibility.Visible
        button_A_treeguard_2.Visibility = Visibility.Visible
        button_A_treeguard_3.Visibility = Visibility.Visible
        button_A_poison_birchnut_trees.Visibility = Visibility.Visible
        button_A_birchnutter.Visibility = Visibility.Visible
        button_A_spider_queen.Visibility = Visibility.Visible
        button_A_big_tentacle.Visibility = Visibility.Visible
        button_A_baby_tentacle.Visibility = Visibility.Visible
        button_A_ancient_guardian.Visibility = Visibility.Visible
        button_A_moose.Visibility = Visibility.Visible
        button_A_mosling.Visibility = Visibility.Visible
        button_A_dragonfly.Visibility = Visibility.Visible
        button_A_lavae.Visibility = Visibility.Visible
        button_A_Extra_Adorable_Lavae.Visibility = Visibility.Visible
        button_A_bearger.Visibility = Visibility.Visible
        button_A_deerclops.Visibility = Visibility.Visible
    End Sub

    REM 生物DLC检测
    Private Sub A_DLC_Check()

        Dim A_ROG_SW_DST As SByte
        Dim A_ROG__ As SByte
        Dim A_SW__ As SByte
        Dim A_DST__ As SByte
        If checkBox_A_DLC_ROG.IsChecked = True Then
            A_ROG__ = 1
        Else
            A_ROG__ = 0
        End If
        If checkBox_A_DLC_SW.IsChecked = True Then
            A_SW__ = 2
        Else
            A_SW__ = 0
        End If
        If checkBox_A_DLC_DST.IsChecked = True Then
            A_DST__ = 4
        Else
            A_DST__ = 0
        End If
        A_ROG_SW_DST = A_ROG__ + A_SW__ + A_DST__
        If A_ROG_SW_DST = 0 Then
            MsgBox("至少选择一项！")
            checkBox_A_DLC_ROG.IsChecked = True
            A_DLC_Check()
        Else
            A_DLC_Check_initialization()
            Select Case A_ROG_SW_DST
                Case 1
                    A_DLC_ROG_SHOW()
                    WrapPanel_A_terrestrial.Height = 550
                    WrapPanel_A_fly.Height = 220
                    WrapPanel_A_cave.Height = 330
                    WrapPanel_A_evil.Height = 330
                    WrapPanel_A_other.Height = 110
                    WrapPanel_A_megafauna.Height = 330
                    WrapPanel_Animal.Height = 2870 - 7 * 110 - 25.4
                    Reg_Write("Animal", 1)
                Case 2
                    A_DLC_SW_SHOW()
                    WrapPanel_A_terrestrial.Height = 330
                    WrapPanel_A_sea.Height = 220
                    WrapPanel_A_fly.Height = 220
                    WrapPanel_A_evil.Height = 220
                    WrapPanel_A_other.Height = 110
                    WrapPanel_A_megafauna.Height = 110
                    WrapPanel_Animal.Height = 2870 - 12 * 110 - 25.4
                    Reg_Write("Animal", 2)
                Case 3
                    A_DLC_ROG_SHOW()
                    A_DLC_SW_SHOW()
                    WrapPanel_A_terrestrial.Height = 770
                    WrapPanel_A_sea.Height = 220
                    WrapPanel_A_fly.Height = 330
                    WrapPanel_A_cave.Height = 330
                    WrapPanel_A_evil.Height = 330
                    WrapPanel_A_other.Height = 220
                    WrapPanel_A_megafauna.Height = 330
                    WrapPanel_Animal.Height = 2870
                    Reg_Write("Animal", 3)
                Case 4
                    A_DLC_DST_SHOW()
                    WrapPanel_A_terrestrial.Height = 550
                    WrapPanel_A_fly.Height = 220
                    WrapPanel_A_cave.Height = 330
                    WrapPanel_A_evil.Height = 330
                    WrapPanel_A_other.Height = 220
                    WrapPanel_A_megafauna.Height = 330
                    WrapPanel_Animal.Height = 2870 - 6 * 110 - 25.4
                    Reg_Write("Animal", 4)
                Case 5
                    A_DLC_ROG_SHOW()
                    A_DLC_DST_SHOW()
                    WrapPanel_A_terrestrial.Height = 550
                    WrapPanel_A_fly.Height = 220
                    WrapPanel_A_cave.Height = 330
                    WrapPanel_A_evil.Height = 330
                    WrapPanel_A_other.Height = 220
                    WrapPanel_A_megafauna.Height = 330
                    WrapPanel_Animal.Height = 2870 - 6 * 110 - 25.4
                    Reg_Write("Animal", 5)
                Case 6
                    A_DLC_SW_SHOW()
                    A_DLC_DST_SHOW()
                    WrapPanel_A_terrestrial.Height = 770
                    WrapPanel_A_sea.Height = 220
                    WrapPanel_A_fly.Height = 330
                    WrapPanel_A_cave.Height = 330
                    WrapPanel_A_evil.Height = 330
                    WrapPanel_A_other.Height = 220
                    WrapPanel_A_megafauna.Height = 330
                    WrapPanel_Animal.Height = 2870
                    Reg_Write("Animal", 6)
                Case 7
                    A_DLC_ROG_SHOW()
                    A_DLC_SW_SHOW()
                    A_DLC_DST_SHOW()
                    WrapPanel_A_terrestrial.Height = 770
                    WrapPanel_A_sea.Height = 220
                    WrapPanel_A_fly.Height = 330
                    WrapPanel_A_cave.Height = 330
                    WrapPanel_A_evil.Height = 330
                    WrapPanel_A_other.Height = 220
                    WrapPanel_A_megafauna.Height = 330
                    WrapPanel_Animal.Height = 2870
                    Reg_Write("Animal", 7)
            End Select
        End If
    End Sub

    Private Sub checkBox_A_DLC_ROG_click(sender As Object, e As RoutedEventArgs) Handles checkBox_A_DLC_ROG.Click
        A_DLC_Check()
    End Sub

    Private Sub SL_button_A_DLC_ROG_click(sender As Object, e As RoutedEventArgs) Handles SL_button_A_DLC_ROG.Click
        If checkBox_A_DLC_ROG.IsChecked = True Then
            checkBox_A_DLC_ROG.IsChecked = False
        Else
            checkBox_A_DLC_ROG.IsChecked = True
        End If
        checkBox_A_DLC_ROG_click(Nothing, Nothing)
    End Sub

    Private Sub checkBox_A_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles checkBox_A_DLC_SW.Click
        A_DLC_Check()
    End Sub

    Private Sub SL_button_A_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles SL_button_A_DLC_SW.Click
        If checkBox_A_DLC_SW.IsChecked = True Then
            checkBox_A_DLC_SW.IsChecked = False
        Else
            checkBox_A_DLC_SW.IsChecked = True
        End If
        checkBox_A_DLC_SW_click(Nothing, Nothing)
    End Sub

    Private Sub checkBox_A_DLC_DST_click(sender As Object, e As RoutedEventArgs) Handles checkBox_A_DLC_DST.Click
        A_DLC_Check()
    End Sub

    Private Sub SL_button_A_DLC_DST_click(sender As Object, e As RoutedEventArgs) Handles SL_button_A_DLC_DST.Click
        If checkBox_A_DLC_DST.IsChecked = True Then
            checkBox_A_DLC_DST.IsChecked = False
        Else
            checkBox_A_DLC_DST.IsChecked = True
        End If
        checkBox_A_DLC_DST_click(Nothing, Nothing)
    End Sub

    REM ------------------左侧面板隐藏(自然)------------------
    Private Sub N_LeftPanel_Initialization()
        ScrollViewer_NaturalLeft_biome.Visibility = Visibility.Collapsed
        ScrollViewer_NaturalLeft_Plant.Visibility = Visibility.Collapsed

    End Sub

    REM ------------------左侧面板(自然_生物群落)------------------
    Private Sub N_Show_B(N_Name As String, N_EnName As String, N_picture As String, N_DLC As String, N_DLC_ROG As SByte, N_DLC_SW As SByte, N_DLC_DST As SByte, N_Introduce As String)
        REM ------------------初始化------------------
        N_LeftPanel_Initialization()
        ScrollViewer_NaturalLeft_biome.Visibility = Visibility.Visible
        REM ------------------物品名字------------------
        NL_textBlock_NaturalName_B.Text = N_Name
        NL_textBlock_NaturalName_B.UpdateLayout()
        Dim N_N_MarginLeft As Integer
        N_N_MarginLeft = (Canvas_NaturalLeft_B.ActualWidth - NL_textBlock_NaturalName_B.ActualWidth) / 2
        Dim N_N_T As New Thickness()
        N_N_T.Top = 120
        N_N_T.Left = N_N_MarginLeft
        NL_textBlock_NaturalName_B.Margin = N_N_T

        NL_textBlock_NaturalEnName_B.Text = N_EnName
        NL_textBlock_NaturalEnName_B.UpdateLayout()
        Dim N_EnN_MarginLeft As Integer
        N_EnN_MarginLeft = (Canvas_NaturalLeft_B.ActualWidth - NL_textBlock_NaturalEnName_B.ActualWidth) / 2
        Dim N_EnN_T As New Thickness()
        N_EnN_T.Top = 140
        N_EnN_T.Left = N_EnN_MarginLeft
        NL_textBlock_NaturalEnName_B.Margin = N_EnN_T
        REM ------------------物品图片------------------
        NL_image_NaturalPicture_B.Source = Picture_Short_Name(Res_Short_Name(N_picture))
        REM ------------------物品DLC-------------------
        If N_DLC = "ROG" Then
            NL_image_NB_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf N_DLC = "SW" Then
            NL_image_NB_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf N_DLC = "DST" Then
            NL_image_NB_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            NL_image_NB_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------存在版本-------------------
        NL_textBlock_NB_DLC_1.Foreground = Brushes.Black
        NL_textBlock_NB_DLC_2.Foreground = Brushes.Black
        NL_textBlock_NB_DLC_3.Foreground = Brushes.Black
        If N_DLC_ROG = 0 Then
            NL_textBlock_NB_DLC_1.Foreground = Brushes.Silver
        End If
        If N_DLC_SW = 0 Then
            NL_textBlock_NB_DLC_2.Foreground = Brushes.Silver
        End If
        If N_DLC_DST = 0 Then
            NL_textBlock_NB_DLC_3.Foreground = Brushes.Silver
        End If
        REM ------------------物品简介-------------------
        TextBlock_NB_Introduce.Text = N_Introduce
        TextBlock_NB_Introduce.Height = SetTextBlockHeight(N_Introduce, 10)
        N_WrapPanel_Introduce_B.Height = SetTextBlockHeight(N_Introduce, 10)
        REM ------------------高度设置------------------- 
        Dim WrapPanel_Frame_B_Height As Integer = 0
        If N_WrapPanel_Abundant_B.Visibility = Visibility.Visible Then
            WrapPanel_Frame_B_Height += N_WrapPanel_Abundant_B.Height + N_WrapPanel_AbundantButton_B.Height
        End If
        If N_WrapPanel_Occasional_B.Visibility = Visibility.Visible Then
            WrapPanel_Frame_B_Height += N_WrapPanel_Occasional_B.Height + N_WrapPanel_OccasionalButton_B.Height
        End If
        If N_WrapPanel_Rare_B.Visibility = Visibility.Visible Then
            WrapPanel_Frame_B_Height += N_WrapPanel_Rare_B.Height + N_WrapPanel_RareButton_B.Height
        End If
        WrapPanel_Frame_B_Height += N_WrapPanel_DLC_B.Height + N_WrapPanel_Introduce_B.Height
        N_WrapPanel_Frame_B.Height = WrapPanel_Frame_B_Height
        If N_WrapPanel_Frame_B.Height + 175 > 604 Then
            Canvas_GoodsLeft_B.Height = N_WrapPanel_Frame_B.Height + 175
        Else
            Canvas_GoodsLeft_B.Height = 604
        End If
    End Sub

    Public Sub NB_Abundant(ParamArray Abundant() As String)
        Dim AbundantNum As Byte = Abundant.Length
        If AbundantNum = 0 Then
            N_WrapPanel_Abundant_B.Visibility = Visibility.Collapsed
            N_WrapPanel_AbundantButton_B.Visibility = Visibility.Collapsed
            Exit Sub
        Else
            N_WrapPanel_Abundant_B.Visibility = Visibility.Visible
            N_WrapPanel_AbundantButton_B.Visibility = Visibility.Visible
        End If
        Dim ArrayAbundant() As Button = {button_NB_Abundant_1, button_NB_Abundant_2, button_NB_Abundant_3, button_NB_Abundant_4, button_NB_Abundant_5, button_NB_Abundant_6, button_NB_Abundant_7}
        Dim ArrayAbundantImage() As Image = {image_NB_Abundant_1, image_NB_Abundant_2, image_NB_Abundant_3, image_NB_Abundant_4, image_NB_Abundant_5, image_NB_Abundant_6, image_NB_Abundant_7}
        For i = AbundantNum To ArrayAbundant.Length - 1
            ArrayAbundant(i).Visibility = Visibility.Collapsed
        Next
        For i = 0 To AbundantNum - 1
            NB_AbundantArray(i) = Abundant(i)
            ArrayAbundant(i).Visibility = Visibility.Visible
            ArrayAbundantImage(i).Source = Picture_Short_Name(Res_Short_Name(Abundant(i)))
        Next
        N_WrapPanel_AbundantButton_B.Height = ((AbundantNum - 1) \ 5 + 1) * 34 + 10
    End Sub

    Private Sub button_NB_Abundant_1_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Abundant_1.Click
        ButtonJump(NB_AbundantArray(0))
    End Sub

    Private Sub button_NB_Abundant_2_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Abundant_2.Click
        ButtonJump(NB_AbundantArray(1))
    End Sub

    Private Sub button_NB_Abundant_3_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Abundant_3.Click
        ButtonJump(NB_AbundantArray(2))
    End Sub

    Private Sub button_NB_Abundant_4_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Abundant_4.Click
        ButtonJump(NB_AbundantArray(3))
    End Sub

    Private Sub button_NB_Abundant_5_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Abundant_5.Click
        ButtonJump(NB_AbundantArray(4))
    End Sub

    Private Sub button_NB_Abundant_6_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Abundant_6.Click
        ButtonJump(NB_AbundantArray(5))
    End Sub

    Private Sub button_NB_Abundant_7_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Abundant_7.Click
        ButtonJump(NB_AbundantArray(6))
    End Sub

    Public Sub NB_Occasional(ParamArray Occasional() As String)
        Dim OccasionalNum As Byte = Occasional.Length
        If OccasionalNum = 0 Then
            N_WrapPanel_Occasional_B.Visibility = Visibility.Collapsed
            N_WrapPanel_OccasionalButton_B.Visibility = Visibility.Collapsed
            Exit Sub
        Else
            N_WrapPanel_Occasional_B.Visibility = Visibility.Visible
            N_WrapPanel_OccasionalButton_B.Visibility = Visibility.Visible
        End If
        Dim ArrayOccasional() As Button = {button_NB_Occasional_1, button_NB_Occasional_2, button_NB_Occasional_3, button_NB_Occasional_4, button_NB_Occasional_5, button_NB_Occasional_6, button_NB_Occasional_7, button_NB_Occasional_8, button_NB_Occasional_9, button_NB_Occasional_10, button_NB_Occasional_11, button_NB_Occasional_12, button_NB_Occasional_13}
        Dim ArrayOccasionalImage() As Image = {image_NB_Occasional_1, image_NB_Occasional_2, image_NB_Occasional_3, image_NB_Occasional_4, image_NB_Occasional_5, image_NB_Occasional_6, image_NB_Occasional_7, image_NB_Occasional_8, image_NB_Occasional_9, image_NB_Occasional_10, image_NB_Occasional_11, image_NB_Occasional_12, image_NB_Occasional_13}
        For i = OccasionalNum To ArrayOccasional.Length - 1
            ArrayOccasional(i).Visibility = Visibility.Collapsed
        Next
        For i = 0 To OccasionalNum - 1
            NB_OccasionalArray(i) = Occasional(i)
            ArrayOccasional(i).Visibility = Visibility.Visible
            ArrayOccasionalImage(i).Source = Picture_Short_Name(Res_Short_Name(Occasional(i)))
        Next
        N_WrapPanel_OccasionalButton_B.Height = ((OccasionalNum - 1) \ 5 + 1) * 34 + 10
    End Sub

    Private Sub button_NB_Occasional_1_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_1.Click
        ButtonJump(NB_OccasionalArray(0))
    End Sub

    Private Sub button_NB_Occasional_2_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_2.Click
        ButtonJump(NB_OccasionalArray(1))
    End Sub

    Private Sub button_NB_Occasional_3_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_3.Click
        ButtonJump(NB_OccasionalArray(2))
    End Sub

    Private Sub button_NB_Occasional_4_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_4.Click
        ButtonJump(NB_OccasionalArray(3))
    End Sub

    Private Sub button_NB_Occasional_5_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_5.Click
        ButtonJump(NB_OccasionalArray(4))
    End Sub

    Private Sub button_NB_Occasional_6_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_6.Click
        ButtonJump(NB_OccasionalArray(5))
    End Sub

    Private Sub button_NB_Occasional_7_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_7.Click
        ButtonJump(NB_OccasionalArray(6))
    End Sub

    Private Sub button_NB_Occasional_8_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_8.Click
        ButtonJump(NB_OccasionalArray(7))
    End Sub

    Private Sub button_NB_Occasional_9_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_9.Click
        ButtonJump(NB_OccasionalArray(8))
    End Sub

    Private Sub button_NB_Occasional_10_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_10.Click
        ButtonJump(NB_OccasionalArray(9))
    End Sub

    Private Sub button_NB_Occasional_11_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_11.Click
        ButtonJump(NB_OccasionalArray(10))
    End Sub

    Private Sub button_NB_Occasional_12_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_12.Click
        ButtonJump(NB_OccasionalArray(11))
    End Sub

    Private Sub button_NB_Occasional_13_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Occasional_13.Click
        ButtonJump(NB_OccasionalArray(12))
    End Sub

    Public Sub NB_Rare(ParamArray Rare() As String)
        Dim RareNum As Byte = Rare.Length
        If RareNum = 0 Then
            N_WrapPanel_Rare_B.Visibility = Visibility.Collapsed
            N_WrapPanel_RareButton_B.Visibility = Visibility.Collapsed
            Exit Sub
        Else
            N_WrapPanel_Rare_B.Visibility = Visibility.Visible
            N_WrapPanel_RareButton_B.Visibility = Visibility.Visible
        End If
        Dim ArrayRare() As Button = {button_NB_Rare_1, button_NB_Rare_2, button_NB_Rare_3, button_NB_Rare_4, button_NB_Rare_5, button_NB_Rare_6}
        Dim ArrayRareImage() As Image = {image_NB_Rare_1, image_NB_Rare_2, image_NB_Rare_3, image_NB_Rare_4, image_NB_Rare_5, image_NB_Rare_6}
        For i = RareNum To ArrayRare.Length - 1
            ArrayRare(i).Visibility = Visibility.Collapsed
        Next
        For i = 0 To RareNum - 1
            NB_RareArray(i) = Rare(i)
            ArrayRare(i).Visibility = Visibility.Visible
            ArrayRareImage(i).Source = Picture_Short_Name(Res_Short_Name(Rare(i)))
        Next
        N_WrapPanel_RareButton_B.Height = ((RareNum - 1) \ 5 + 1) * 34 + 10
    End Sub

    Private Sub button_NB_Rare_1_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Rare_1.Click
        ButtonJump(NB_RareArray(0))
    End Sub

    Private Sub button_NB_Rare_2_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Rare_2.Click
        ButtonJump(NB_RareArray(1))
    End Sub

    Private Sub button_NB_Rare_3_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Rare_3.Click
        ButtonJump(NB_RareArray(2))
    End Sub

    Private Sub button_NB_Rare_4_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Rare_4.Click
        ButtonJump(NB_RareArray(3))
    End Sub

    Private Sub button_NB_Rare_5_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Rare_5.Click
        ButtonJump(NB_RareArray(4))
    End Sub

    Private Sub button_NB_Rare_6_click(sender As Object, e As RoutedEventArgs) Handles button_NB_Rare_6.Click
        ButtonJump(NB_RareArray(5))
    End Sub


    Private Sub button_N_grasslands_click(sender As Object, e As RoutedEventArgs) Handles button_N_grasslands.Click
        NB_Abundant({"A_redbird", "N_flower_1"})
        NB_Occasional({"G_flint", "F_carrot", "N_berry_bush_1", "N_evergreen_1", "N_grass", "N_sapling", "A_bee", "N_beehive", "A_butterfly", "A_fireflies", "N_pond", "A_rabbit", "N_rabbit_hole"})
        NB_Rare({"A_spider", "N_spider_den", "N_red_mushroom", "A_killer_bee", "N_killer_bee_hive"})
        N_Show_B("草原", "Grasslands", "N_grasslands", "NoDLC", 1, 0, 1, "草原是常见的生物群落之一，它们一般有多种类型的资源。玩家进入世界总会出生在草原(除了船难版)，无论是小的或是大片的草原。草原主要的危险是蜘蛛巢和杀人蜂巢，有时候也会出现海象巢穴。")
    End Sub

    Private Sub button_N_savanna_click(sender As Object, e As RoutedEventArgs) Handles button_N_savanna.Click
        NB_Abundant({"N_grass", "A_rabbit", "N_rabbit_hole", "A_redbird", "A_crow"})
        NB_Occasional({"A_beefalo", "G_manure", "N_boulder_1"})
        NB_Rare({"N_flower_1", "A_butterfly", "N_spider_den", "G_crank_thing_1"})
        N_Show_B("稀树大草原", "Savanna", "N_savanna", "NoDLC", 1, 0, 1, "稀树大草原是唯一已知产生皮弗娄牛的生物群落(需要稀树大草原至少有25块草皮)，曲柄状传送机零件总是在稀树大草原。稀树大草原是获得草的好地方，不过春天要小心皮弗娄牛。")
    End Sub

    Private Sub button_N_deciduous_forest_click(sender As Object, e As RoutedEventArgs) Handles button_N_deciduous_forest.Click
        NB_Abundant({"N_birchnut_tree_1", "N_blue_mushroom", "N_green_mushroom", "N_red_mushroom", "G_fireflies"})
        NB_Occasional({"N_hollow_stump", "N_sapling", "N_berry_bush_1"})
        NB_Rare({"N_grass", "N_boulder_1", "N_glommer's_statue"})
        N_Show_B("落叶阔叶林", "Deciduous Forest", "N_deciduous_forest", "NoDLC", 1, 0, 1, "落叶阔叶林是桦树的盛产地，中空树桩和萤火虫通常在这里出现。这里也是唯一一个可以产生三种蘑菇的生物群落，猪王会产生在落叶阔叶林中的一小片森林生物群落里，格洛姆雕像就在猪王附近。")
    End Sub

    Private Sub button_N_forest_click(sender As Object, e As RoutedEventArgs) Handles button_N_forest.Click
        NB_Abundant({"N_evergreen_1", "N_sapling", "A_redbird", "A_crow"})
        NB_Occasional({"N_pig_house", "A_spider", "N_spider_den", "N_headstone_1", "N_blue_mushroom", "N_red_mushroom", "N_flower_1"})
        NB_Rare({"N_berry_bush_1", "N_grass", "N_boulder_1", "N_boulder_2", "N_totally_normal_tree", "N_sinkhole"})
        N_Show_B("森林", "Forest", "N_forest", "NoDLC", 1, 0, 1, "森林里有时会发现大范围的蜘蛛巢和分散的坟墓，这里是最容易着火的区域。森林里有时也会找到猪舍和卵石。")
    End Sub

    Private Sub button_N_desert_click(sender As Object, e As RoutedEventArgs) Handles button_N_desert.Click
        NB_Abundant({"A_buzzards", "N_cactus", "N_boulder_3", "N_tumbleweed"})
        NB_Occasional({"N_hound_mound", "N_bones_1", "N_spiky_bush", "N_spiky_tree", "N_grass", "N_boulder_1"})
        NB_Rare({"A_volt_goat"})
        N_Show_B("沙漠", "Desert", "N_desert", "NoDLC", 1, 0, 1, "沙漠里的主要生物是秃鹰和猎犬，有时候会有一两群电羊。沙漠也是唯一产生猎犬丘(通常是成片的)和风滚草(风滚草可能会滚到其他生物群落)的地方。如果在沙漠里遇到了冰箱陷阱，那么触发陷阱会使季节变成夏天。最强BOSS龙蝇也会诞生在这里。")
    End Sub

    Private Sub button_N_graveyard_click(sender As Object, e As RoutedEventArgs) Handles button_N_graveyard.Click
        NB_Abundant({"N_headstone_1", "S_gold_nugget"})
        NB_Occasional({"N_evergreen_1", "G_flint", "G_rocks"})
        NB_Rare({})
        N_Show_B("墓地", "Graveyard", "N_graveyard", "NoDLC", 1, 0, 1, "墓地通常会在森林附近，有几颗稀疏的树，地上常常能看到金块。如果喜欢挖坟找玩具就来这里吧！")
    End Sub

    Private Sub button_N_marsh_click(sender As Object, e As RoutedEventArgs) Handles button_N_marsh.Click
        NB_Abundant({"A_tentacle", "N_spiky_bush", "N_spiky_tree", "A_crow", "N_blue_mushroom", "N_green_mushroom"})
        NB_Occasional({"A_spider", "N_spider_den", "N_pond_mos", "N_reeds", "N_merm_hut", "N_rundown_house"})
        NB_Rare({"N_grass", "N_evergreen_1"})
        N_Show_B("沼泽", "Marsh", "N_marsh", "NoDLC", 1, 0, 1, "沼泽是十分危险的地方，这里有触手、鱼人和蜘蛛，它们经常会打起来，可以坐收渔翁之利。沿着道路走一般不会遇到触手。")
    End Sub

    Private Sub button_N_chess_click(sender As Object, e As RoutedEventArgs) Handles button_N_chess.Click
        NB_Abundant({"N_evil_flower_1", "N_marble_tree_1", "N_marble_pillar"})
        NB_Occasional({"A_butterfly", "A_clockwork_knight", "A_clockwork_bishop"})
        NB_Rare({"N_maxwell_statue", "N_harp_statue"})
        N_Show_B("棋盘", "Chess", "N_chess", "NoDLC", 1, 0, 1, "棋盘生物群落有少量的大理石和发条生物，是保护传送机零件底座的地方，同时，花也被恶魔之花取代。")
    End Sub

    Private Sub button_N_mosaic_click(sender As Object, e As RoutedEventArgs) Handles button_N_mosaic.Click
        NB_Abundant({"G_rocks", "G_flint", "A_crow"})
        NB_Occasional({"S_gold_nugget", "N_evergreen_1", "N_boulder_1"})
        NB_Rare({"G_birchnut", "N_grass", "N_berry_bush_1"})
        N_Show_B("马赛克", "Mosaic", "N_mosaic", "NoDLC", 1, 0, 1, "马赛克区域通常在海边，由各种地皮组成，也有各种资源。")
    End Sub

    Private Sub button_N_rockyland_click(sender As Object, e As RoutedEventArgs) Handles button_N_rockyland.Click
        NB_Abundant({"G_flint", "G_rocks", "A_crow", "N_boulder_1", "N_boulder_2"})
        NB_Occasional({"S_gold_nugget", "A_spider", "N_spider_den"})
        NB_Rare({"A_tallbird", "N_tallbird_nest"})
        N_Show_B("矿区", "Rockyland", "N_rockyland", "NoDLC", 1, 0, 1, "顾名思义，有很多的卵石，冬天也会有不少迷你冰川，一般会有2-3只高脚鸟生活在这里。有的时候会发现成片的蜘蛛巢，并且伴随着不少金块。")
    End Sub

    Private Sub button_N_ocean_click(sender As Object, e As RoutedEventArgs) Handles button_N_ocean.Click
        NB_Abundant({})
        NB_Occasional({})
        NB_Rare({})
        N_Show_B("海洋", "Ocean", "N_ocean", "NoDLC", 1, 0, 1, "海洋是不可穿越的，没有任何的生物、植物或建筑，不过如果用了至少4倍速度的话很有可能冲到海洋哦！")
    End Sub

    Private Sub button_N_spider_infested_click(sender As Object, e As RoutedEventArgs) Handles button_N_spider_infested.Click
        NB_Abundant({"A_spider", "N_spider_den", "A_tentacle", "N_reeds"})
        NB_Occasional({})
        NB_Rare({"N_spiky_bush"})
        N_Show_B("蜘蛛出没", "Spider Infested", "N_spider_infested", "NoDLC", 1, 0, 1, "在这里，芦苇资源丰富，然而要小心极多的蜘蛛，还有触手。")
    End Sub

    Private Sub button_N_light_flower_swamp_click(sender As Object, e As RoutedEventArgs) Handles button_N_light_flower_swamp.Click
        NB_Abundant({"N_light_flower_1", "A_big_tentacle"})
        NB_Occasional({})
        NB_Rare({"A_tentacle"})
        N_Show_B("荧光草沼泽", "Light Flower Swamp", "N_light_flower_swamp", "NoDLC", 1, 0, 1, "这里有丰富的荧光草资源，然而有时会遇到大触手。")
    End Sub

    Private Sub button_N_mushtree_forest_click(sender As Object, e As RoutedEventArgs) Handles button_N_mushtree_forest.Click
        NB_Abundant({"N_blue_mushtree", "N_green_mushtree", "N_red_mushtree", "N_green_mushroom"})
        NB_Occasional({})
        NB_Rare({})
        N_Show_B("蘑菇树森林", "Mushtree Forest", "N_mushtree_forest", "NoDLC", 1, 0, 1, "富含三种蘑菇树和绿蘑菇。")
    End Sub

    Private Sub button_N_blue_mushtree_forest_click(sender As Object, e As RoutedEventArgs) Handles button_N_blue_mushtree_forest.Click
        NB_Abundant({"N_blue_mushtree", "N_blue_mushroom", "N_light_flower_1", "N_fern_1"})
        NB_Occasional({"N_rabbit_hutch"})
        NB_Rare({"A_depths_worm", "N_slurtle_mound"})
        N_Show_B("蓝色蘑菇树森林", "Blue Mushtree Forest", "N_blue_mushtree_forest", "NoDLC", 1, 0, 1, "富含蓝色蘑菇树、蓝蘑菇和荧光草，有时会找到兔子窝，偶尔会有深渊蠕虫在附近活动，也可能看到含糊虫土堆。")
    End Sub

    Private Sub button_N_green_mushtree_forest_click(sender As Object, e As RoutedEventArgs) Handles button_N_green_mushtree_forest.Click
        NB_Abundant({"N_green_mushtree", "N_green_mushroom", "N_light_flower_1", "N_fern_1"})
        NB_Occasional({})
        NB_Rare({"A_depths_worm", "N_slurtle_mound"})
        N_Show_B("绿色蘑菇树森林", "Green Mushtree Forest", "N_green_mushtree_forest", "NoDLC", 1, 0, 1, "富含绿色蘑菇树、绿蘑菇和荧光草，偶尔会有深渊蠕虫在附近活动，也可能看到含糊虫土堆。")
    End Sub

    Private Sub button_N_red_mushtree_forest_click(sender As Object, e As RoutedEventArgs) Handles button_N_red_mushtree_forest.Click
        NB_Abundant({"N_red_mushtree", "N_red_mushroom", "N_light_flower_1", "N_fern_1"})
        NB_Occasional({})
        NB_Rare({"A_depths_worm", "N_slurtle_mound"})
        N_Show_B("红色蘑菇树森林", "Red Mushtree Forest", "N_red_mushtree_forest", "NoDLC", 1, 0, 1, "富含红色蘑菇树、红蘑菇和荧光草，偶尔会有深渊蠕虫在附近活动，也可能看到含糊虫土堆。")
    End Sub

    Private Sub button_N_rocky_plains_click(sender As Object, e As RoutedEventArgs) Handles button_N_rocky_plains.Click
        NB_Abundant({"A_rock_lobster", "N_boulder_3"})
        NB_Occasional({})
        NB_Rare({"N_nightmare_fissure"})
        N_Show_B("岩石平原", "Rocky Plains", "N_rocky_plains", "NoDLC", 1, 0, 1, "这里几乎只有岩石大龙虾和卵石，偶尔会看到噩梦裂缝。")
    End Sub

    Private Sub button_N_stalagmite_biomes_click(sender As Object, e As RoutedEventArgs) Handles button_N_stalagmite_biomes.Click
        NB_Abundant({"N_stalagmite", "A_spitter"})
        NB_Occasional({})
        NB_Rare({"N_nightmare_fissure"})
        N_Show_B("石笋生物群落", "Stalagmite Biomes", "N_stalagmite_biomes", "NoDLC", 1, 0, 1, "这里生活的生物是喷吐蜘蛛，偶尔会看到噩梦裂缝。被塞上的洞可能会在这里。")
    End Sub

    Private Sub button_N_tall_stalagmite_biomes_click(sender As Object, e As RoutedEventArgs) Handles button_N_tall_stalagmite_biomes.Click
        NB_Abundant({"N_stalagmite_tall"})
        NB_Occasional({"A_batilisk", "G_guano"})
        NB_Rare({"N_nightmare_fissure"})
        N_Show_B("高石笋生物群落", "Tall Stalagmite Biomes", "N_tall_stalagmite_biomes", "NoDLC", 1, 0, 1, "这里生活的生物是黑蝙蝠，偶尔会看到噩梦裂缝。被塞上的洞可能会在这里。")
    End Sub

    Private Sub button_N_sunken_forest_click(sender As Object, e As RoutedEventArgs) Handles button_N_sunken_forest.Click
        NB_Abundant({"N_evergreen_1"})
        NB_Occasional({"N_grass", "N_sapling", "N_berry_bush_1", "N_cave_banana_tree"})
        NB_Rare({})
        N_Show_B("沉没森林", "Sunke Forest", "N_sunken_forest", "NoDLC", 1, 0, 1, "这里通常有一大片的树和少量的树苗和浆果灌木丛，不会有生物在这里生成(除了火鸡和树精守卫)。")
    End Sub

    Private Sub button_N_labyrinth_click(sender As Object, e As RoutedEventArgs) Handles button_N_labyrinth.Click
        NB_Abundant({"N_ornate_chest", "A_dangling_depth_dweller"})
        NB_Occasional({"N_nightmare_light", "N_cave_lichen", "N_light_flower_1", "N_thulecite_wall_1"})
        NB_Rare({"A_ancient_guardian"})
        N_Show_B("迷宫", "Labyrinth", "N_labyrinth", "NoDLC", 1, 0, 1, "迷宫区域十分曲折，通常路的尽头会有豪华箱子，然而几乎不可避免地会遇上白蜘蛛。会有一大片空地并且能在那里找到远古守护者。")
    End Sub

    Private Sub button_N_military_click(sender As Object, e As RoutedEventArgs) Handles button_N_military.Click
        NB_Abundant({"N_ancient_statue_head", "N_ancient_statue_mage_nogem", "N_nightmare_light"})
        NB_Occasional({"A_dangling_depth_dweller", "N_boulder_3", "N_broken_clockworks_1", "N_thulecite_wall_1"})
        NB_Rare({"N_ancient_pseudoscience_station"})
        N_Show_B("军事", "Military", "N_military", "NoDLC", 1, 0, 1, "在这里会发现不少影灯和远古雕像，也会找到白蜘蛛和破碎的时钟，两个能找到破碎的远古遗迹的生物群落之一。")
    End Sub

    Private Sub button_N_sacred_click(sender As Object, e As RoutedEventArgs) Handles button_N_sacred.Click
        NB_Abundant({"N_broken_clockworks_1", "N_ancient_statue_head", "N_ancient_statue_mage_nogem", "N_nightmare_light"})
        NB_Occasional({"N_thulecite_wall_1"})
        NB_Rare({"N_ancient_pseudoscience_station"})
        N_Show_B("神圣", "Sacred", "N_sacred", "NoDLC", 1, 0, 1, "尽管这里很像军事生物群落，但是通过紫色的草皮可以很容易分辨。在这里会发现不少影灯和远古雕像，破碎的时钟大多聚集在一起，两个能找到破碎的远古遗迹的生物群落之一(只有在这里可能发现完整的远古遗迹)。")
    End Sub

    Private Sub button_N_village_click(sender As Object, e As RoutedEventArgs) Handles button_N_village.Click
        NB_Abundant({"A_splumonkey", "N_cave_banana_tree", "N_relic_bowl"})
        NB_Occasional({"N_pond_cave", "N_light_flower_1", "N_cave_lichen", "N_thulecite_wall_1"})
        NB_Rare({})
        N_Show_B("村庄", "Village", "N_village", "NoDLC", 1, 0, 1, "这里是古代文明的住宅区，可以找到洞穴香蕉树和暴躁猴，偶尔会有少量池塘和荧光草等。")
    End Sub

    Private Sub button_N_wilds_click(sender As Object, e As RoutedEventArgs) Handles button_N_wilds.Click
        NB_Abundant({"N_pond_cave", "N_light_flower_1", "N_cave_lichen", "A_slurper", "N_blue_mushroom"})
        NB_Occasional({})
        NB_Rare({"A_depths_worm"})
        N_Show_B("野外", "Wilds", "N_wilds", "NoDLC", 1, 0, 1, "古代文明也没有涉足的区域，可以找到洞穴池塘和各种植物，缀食者和深渊蠕虫也会在这里出现。")
    End Sub

    Private Sub button_N_beach_click(sender As Object, e As RoutedEventArgs) Handles button_N_beach.Click
        NB_Abundant({"N_palm_tree_1", "N_sapling", "N_grass_sw", "G_flint", "N_crabbit_den", "N_sandy_pile", "G_seashell"})
        NB_Occasional({"G_rocky_turf", "N_boulder_1", "N_boulder_3", "N_limpet_rock", "N_wildbore_house"})
        NB_Rare({"N_crate_1"})
        N_Show_B("海滩", "Beach", "N_beach", "NoDLC", 0, 1, 0, "海滩是船难版常见的生物群落之一，它们一般有多种类型的资源。玩家进入世界(船难版)总会出生在海滩。晚上临近海洋的区域会被淹没，月圆之夜是潮汐最强的时候。")
    End Sub

    Private Sub button_N_jungle_click(sender As Object, e As RoutedEventArgs) Handles button_N_jungle.Click
        NB_Abundant({"N_jungle_tree_1", "N_bamboo_patch", "N_viney_bush", "N_flower_1", "A_fireflies", "N_prime_ape_hut", "A_spider"})
        NB_Occasional({"N_berry_bush_2", "N_boulder_2", "N_wildbore_house", "N_spider_den", "N_blue_mushroom", "N_green_mushroom", "N_red_mushroom", "A_snake", "A_poison_snake", "A_butterfly"})
        NB_Rare({"N_boulder_1", "G_rocks", "G_flint"})
        N_Show_B("热带雨林", "Jungle", "N_jungle", "NoDLC", 0, 1, 0, "热带雨林生物群落的生物、植物和资源都十分丰富，在这里要小心蛇和猿猴，它们十分令人讨厌。")
    End Sub

    Private Sub button_N_mangrove_bio_click(sender As Object, e As RoutedEventArgs) Handles button_N_mangrove_bio.Click
        NB_Abundant({"N_mangrove_bio", "N_grass_sw"})
        NB_Occasional({"N_shoal"})
        NB_Rare({"A_water_beefalo"})
        N_Show_B("红树林", "Mangrove", "N_mangrove_bio", "NoDLC", 0, 1, 0, "这里虽然只能乘船通行，然而一般还是认为是一个岛屿，这里生活着水牛，草不会枯萎。")
    End Sub

    Private Sub button_N_magma_field_click(sender As Object, e As RoutedEventArgs) Handles button_N_magma_field.Click
        NB_Abundant({"N_magma_pile", "N_magma_pile_gold", "N_krissure"})
        NB_Occasional({"N_sapling", "N_boulder_1", "N_boulder_2"})
        NB_Rare({"A_tallbird", "N_tallbird_nest"})
        N_Show_B("岩浆领域", "Magma Field", "N_magma_field", "NoDLC", 0, 1, 0, "黑暗而崎岖的地形，有丰富的熔岩矿和熔岩金矿，偶尔会遇到高脚鸟。要小心压力火泉。")
    End Sub

    Private Sub button_N_meadow_click(sender As Object, e As RoutedEventArgs) Handles button_N_meadow.Click
        NB_Abundant({"N_grass_sw", "N_flower_1", "A_butterfly", "F_sweet_potato"})
        NB_Occasional({"G_rocks", "G_flint", "N_beehive", "A_mandrake"})
        NB_Rare({"N_red_mushroom"})
        N_Show_B("草甸", "Meadow", "N_meadow", "NoDLC", 0, 1, 0, "富含草的生物群落，这里也是唯一能找到甘薯的地方，通常还有少量的蜂巢和曼德拉草。")
    End Sub

    Private Sub button_N_tidal_marsh_click(sender As Object, e As RoutedEventArgs) Handles button_N_tidal_marsh.Click
        NB_Abundant({"N_reeds", "A_flup", "N_tidal_pool", "N_poisonous_hole"})
        NB_Occasional({"A_merm", "N_merm_hut"})
        NB_Rare({"A_fishermerm", "N_fishermerm's_hut"})
        N_Show_B("潮汐沼泽", "Tidal Marsh", "N_tidal_marsh", "NoDLC", 0, 1, 0, "潮汐沼泽通常存在于单独的小岛，覆盖满了潮滩地皮。这里是鱼人/渔人和追踪性弹涂鱼的天堂，要小心毒洞哦！")
    End Sub

    Private Sub button_N_coral_reef_bio_click(sender As Object, e As RoutedEventArgs) Handles button_N_coral_reef_bio.Click
        NB_Abundant({"N_coral_reef", "N_shoal", "A_dogfish"})
        NB_Occasional({"N_seaweed", "A_jellyfish"})
        NB_Rare({"N_brainy_sprout"})
        N_Show_B("珊瑚礁", "Coral Reef", "N_coral_reef_bio", "NoDLC", 0, 1, 0, "珊瑚礁生物群落富含珊瑚礁，能找到浅滩和狗鱼，偶尔会有海藻和水母。运气好可以找到聪明芽。")
    End Sub

    Private Sub button_N_ocean_shallow_click(sender As Object, e As RoutedEventArgs) Handles button_N_ocean_shallow.Click
        NB_Abundant({"N_seaweed"})
        NB_Occasional({"A_jellyfish", "N_wobster_den", "F_mussel", "A_dogfish"})
        NB_Rare({"G_message_in_a_bottle"})
        N_Show_B("海洋(浅)", "Ocean(shallow)", "N_ocean_shallow", "NoDLC", 0, 1, 0, "淡蓝色的地形，没有海浪，围绕着所有岛屿，但是有可能会延伸。在这里可以发现贻贝、龙虾的巢穴、狗鱼和海藻。")
    End Sub

    Private Sub button_N_ocean_medium_click(sender As Object, e As RoutedEventArgs) Handles button_N_ocean_medium.Click
        NB_Abundant({"N_waves"})
        NB_Occasional({"A_stink_ray", "A_seagull"})
        NB_Rare({"A_dogfish", "N_shoal"})
        N_Show_B("海洋(中)", "Ocean(medium)", "N_ocean_medium", "NoDLC", 0, 1, 0, "这里有许多的海浪，在这里可以发现恶臭魔鬼鱼、可疑的气泡、浅滩、旗鱼，温和季节会有海鸥出现。不会与岛屿接壤。")
    End Sub

    Private Sub button_N_ocean_deep_click(sender As Object, e As RoutedEventArgs) Handles button_N_ocean_deep.Click
        NB_Abundant({"N_waves"})
        NB_Occasional({"A_bottenosed_ballphin", "A_seagull"})
        NB_Rare({"A_swordfish", "N_shoal", "G_rawling", "N_volcano"})
        N_Show_B("海洋(深)", "Ocean(deep)", "N_ocean_deep", "NoDLC", 0, 1, 0, "海洋深处，深色区域，只有这里有残骸和潮湿的坟墓，通常可以找到浅滩、海藻和狗鱼。不会与海洋(浅)和岛屿接壤。")
    End Sub

    Private Sub button_N_ship_graveyard_click(sender As Object, e As RoutedEventArgs) Handles button_N_ship_graveyard.Click
        NB_Abundant({})
        NB_Occasional({"N_wreck_1", "N_steamer_trunk", "N_shoal"})
        NB_Rare({"N_seaweed", "N_watery_grave_1", "A_swordfish"})
        N_Show_B("船舶墓地", "Ship Graveyard", "N_ship_graveyard", "NoDLC", 0, 1, 0, "残骸和潮湿的坟墓区域。")
    End Sub

    Private Sub button_N_volcano_bio_click(sender As Object, e As RoutedEventArgs) Handles button_N_volcano_bio.Click
        NB_Abundant({"N_coffee_plant", "N_magma_pile", "N_magma_pile_gold", "N_charcoal_boulder", "N_obsidian_boulder"})
        NB_Occasional({"N_skeleton_1", "N_elephant_cactus", "N_krissure", "N_dragoon_den"})
        NB_Rare({"N_obsidian_workbench", "N_woodlegs'_cage"})
        N_Show_B("火山", "Volcano", "N_volcano_bio", "NoDLC", 0, 1, 0, "围绕火山口的生物群落，中间是含有大量熔岩的区域，靠近会过热。在这里可以找到大量咖啡树、熔岩矿熔岩金矿、黑曜岩和煤矿，也有不少象仙人掌和龙人巢，必定有一个火山祭坛和黑曜石工作台。如果没有解锁伍德莱格那么在这里会发现伍德莱格的笼子。")
    End Sub

    REM ------------------左侧面板(自然_小型植物/树)------------------
    Private Sub N_Show_P(N_Name As String, N_EnName As String, N_DLC As String, N_DLC_ROG As SByte, N_DLC_SW As SByte, N_DLC_DST As SByte, N_Tools As String, N_ResourcesBurnt_1 As String, N_ResourcesBurntT_1 As String, N_ResourcesBurnt_2 As String, N_ResourcesBurntT_2 As String, N_ResourcesBurnt_3 As String, N_ResourcesBurntT_3 As String, N_ResourcesBurnt_4 As String, N_ResourcesBurntT_4 As String, N_Biome_1 As String, N_Biome_2 As String, N_SpecialAbility_1 As String, N_SpecialAbilityButton_1 As String, N_SpecialAbility_2 As String, N_SpecialAbilityButton_2 As String, N_Introduce As String, Optional N_ReGenerate As Boolean = True, Optional N_Combustible As Boolean = True)
        REM ------------------初始化------------------
        N_LeftPanel_Initialization()
        ScrollViewer_NaturalLeft_Plant.Visibility = Visibility.Visible
        N_WrapPanel_Switch_P.Visibility = Visibility.Visible
        button_NP_Switch_Left.IsEnabled = False
        button_NP_Switch_Right.IsEnabled = True
        NP_ArrayIndex = 0
        REM ------------------物品名字------------------
        NL_textBlock_NaturalName_P.Text = N_Name
        NL_textBlock_NaturalName_P.UpdateLayout()
        Dim N_N_MarginLeft As Integer
        N_N_MarginLeft = (Canvas_NaturalLeft_P.ActualWidth - NL_textBlock_NaturalName_P.ActualWidth) / 2
        Dim N_N_T As New Thickness()
        N_N_T.Top = 80
        N_N_T.Left = N_N_MarginLeft
        NL_textBlock_NaturalName_P.Margin = N_N_T

        NL_textBlock_NaturalEnName_P.Text = N_EnName
        NL_textBlock_NaturalEnName_P.UpdateLayout()
        Dim N_EnN_MarginLeft As Integer
        N_EnN_MarginLeft = (Canvas_NaturalLeft_P.ActualWidth - NL_textBlock_NaturalEnName_P.ActualWidth) / 2
        Dim N_EnN_T As New Thickness()
        N_EnN_T.Top = 120
        N_EnN_T.Left = N_EnN_MarginLeft
        NL_textBlock_NaturalEnName_P.Margin = N_EnN_T
        REM ------------------物品DLC-------------------
        If N_DLC = "ROG" Then
            NL_image_NP_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf N_DLC = "SW" Then
            NL_image_NP_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf N_DLC = "DST" Then
            NL_image_NP_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            NL_image_NP_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------存在版本-------------------
        NL_textBlock_NP_DLC_1.Foreground = Brushes.Black
        NL_textBlock_NP_DLC_2.Foreground = Brushes.Black
        NL_textBlock_NP_DLC_3.Foreground = Brushes.Black
        If N_DLC_ROG = 0 Then
            NL_textBlock_NP_DLC_1.Foreground = Brushes.Silver
        End If
        If N_DLC_SW = 0 Then
            NL_textBlock_NP_DLC_2.Foreground = Brushes.Silver
        End If
        If N_DLC_DST = 0 Then
            NL_textBlock_NP_DLC_3.Foreground = Brushes.Silver
        End If
        REM ------------------所需工具-------------------
        Select Case N_Tools
            Case "NoTool"
                N_WrapPanel_Tools_P.Visibility = Visibility.Collapsed
                N_WrapPanel_ToolsButton_P.Visibility = Visibility.Collapsed
            Case "Normal"
                N_WrapPanel_Tools_P.Visibility = Visibility.Visible
                N_WrapPanel_ToolsButton_P.Visibility = Visibility.Visible
                button_NP_Tools_1.Visibility = Visibility.Visible
                button_NP_Tools_2.Visibility = Visibility.Visible
                button_NP_Tools_3.Visibility = Visibility.Collapsed
                button_NP_Tools_4.Visibility = Visibility.Collapsed
                button_NP_Tools_6.Visibility = Visibility.Visible
            Case "PetrifiedTree"
                N_WrapPanel_Tools_P.Visibility = Visibility.Visible
                N_WrapPanel_ToolsButton_P.Visibility = Visibility.Visible
                button_NP_Tools_1.Visibility = Visibility.Collapsed
                button_NP_Tools_2.Visibility = Visibility.Collapsed
                button_NP_Tools_3.Visibility = Visibility.Visible
                button_NP_Tools_4.Visibility = Visibility.Visible
                button_NP_Tools_6.Visibility = Visibility.Collapsed
        End Select
        REM -----------------燃烧后资源------------------
        If N_ResourcesBurnt_1 = "" Then
            N_WrapPanel_ResourcesBurnt_P.Visibility = Visibility.Collapsed
            N_WrapPanel_ResourcesBurntButton_P.Visibility = Visibility.Collapsed
        Else
            N_WrapPanel_ResourcesBurnt_P.Visibility = Visibility.Visible
            N_WrapPanel_ResourcesBurntButton_P.Visibility = Visibility.Visible
            image_NP_ResourcesBurnt_1.Source = Picture_Short_Name(Res_Short_Name(N_ResourcesBurnt_1))
            NL_textBlock_ResourcesBurnt_T_1.Text = N_ResourcesBurntT_1
            NP_ResourcesBurnt_1 = N_ResourcesBurnt_1
            If N_ResourcesBurnt_2 <> "" Then
                button_NP_ResourcesBurnt_2.Visibility = Visibility.Visible
                NL_textBlock_ResourcesBurnt_T_2.Visibility = Visibility.Visible
                image_NP_ResourcesBurnt_2.Source = Picture_Short_Name(Res_Short_Name(N_ResourcesBurnt_2))
                NL_textBlock_ResourcesBurnt_T_2.Text = N_ResourcesBurntT_2
                NP_ResourcesBurnt_2 = N_ResourcesBurnt_2
            End If
            If N_ResourcesBurnt_3 <> "" Then
                button_NP_ResourcesBurnt_3.Visibility = Visibility.Visible
                NL_textBlock_ResourcesBurnt_T_3.Visibility = Visibility.Visible
                image_NP_ResourcesBurnt_3.Source = Picture_Short_Name(Res_Short_Name(N_ResourcesBurnt_3))
                NL_textBlock_ResourcesBurnt_T_3.Text = N_ResourcesBurntT_3
                NP_ResourcesBurnt_3 = N_ResourcesBurnt_3
            End If
            If N_ResourcesBurnt_4 <> "" Then
                button_NP_ResourcesBurnt_4.Visibility = Visibility.Visible
                NL_textBlock_ResourcesBurnt_T_4.Visibility = Visibility.Visible
                image_NP_ResourcesBurnt_4.Source = Picture_Short_Name(Res_Short_Name(N_ResourcesBurnt_4))
                NL_textBlock_ResourcesBurnt_T_4.Text = N_ResourcesBurntT_4
                NP_ResourcesBurnt_4 = N_ResourcesBurnt_4
            End If
        End If
        REM ---------------可再生/可燃性-----------------
        If N_ReGenerate = True Then
            Image_N_PB_Regenerate.Visibility = Visibility.Visible
        Else
            Image_N_PB_Regenerate.Visibility = Visibility.Collapsed
        End If
        If N_Combustible = True Then
            Image_N_PB_Combustible.Visibility = Visibility.Visible
        Else
            Image_N_PB_Combustible.Visibility = Visibility.Collapsed
        End If
        REM ------------------特殊能力-------------------
        button_NP_SpecialAbility_2.Visibility = Visibility.Collapsed
        If N_SpecialAbility_1 = "" Then
            N_WrapPanel_SpecialAbility_P.Visibility = Visibility.Collapsed
            N_Canvas_SpecialAbilityDetals_P.Visibility = Visibility.Collapsed
        Else
            N_Canvas_SpecialAbilityDetals_P.Height = 49
            N_WrapPanel_SpecialAbility_P.Visibility = Visibility.Visible
            N_Canvas_SpecialAbilityDetals_P.Visibility = Visibility.Visible
            NL_textBlock_SpecialAbilityText_1_P.Text = N_SpecialAbility_1
            If N_SpecialAbilityButton_1 = "" Then
                button_NP_SpecialAbility_1.Visibility = Visibility.Collapsed
            Else
                button_NP_SpecialAbility_1.Visibility = Visibility.Visible
                image_NP_SpecialAbility_1.Source = Picture_Short_Name(Res_Short_Name(N_SpecialAbilityButton_1))
                NP_SpecialAbility_1 = N_SpecialAbilityButton_1
            End If
            If N_SpecialAbility_2 = "" Then
                NL_textBlock_SpecialAbilityText_2_P.Visibility = Visibility.Collapsed
            Else
                N_Canvas_SpecialAbilityDetals_P.Height = 83
                NL_textBlock_SpecialAbilityText_2_P.Text = N_SpecialAbility_2
                NL_textBlock_SpecialAbilityText_2_P.Visibility = Visibility.Visible
                If N_SpecialAbilityButton_2 = "" Then
                    button_NP_SpecialAbility_2.Visibility = Visibility.Collapsed
                Else
                    button_NP_SpecialAbility_2.Visibility = Visibility.Visible
                    image_NP_SpecialAbility_2.Source = Picture_Short_Name(Res_Short_Name(N_SpecialAbilityButton_2))
                    NP_SpecialAbility_2 = N_SpecialAbilityButton_2
                End If
            End If
        End If
        REM ------------------生物群落-------------------
        NP_Biome_1 = N_Biome_1
        image_NP_Biome_1.Source = Picture_Short_Name(Res_Short_Name(N_Biome_1))
        image_NP_Biome_2.Visibility = Visibility.Collapsed
        Dim N_Plant_T As New Thickness()
        N_Plant_T.Top = 0
        N_Plant_T.Left = 45
        button_NP_Biome_1.Margin = N_Plant_T
        If N_Biome_2 <> "" Then
            NP_Biome_2 = N_Biome_2
            image_NP_Biome_2.Source = Picture_Short_Name(Res_Short_Name(N_Biome_2))
            image_NP_Biome_2.Visibility = Visibility.Visible
            N_Plant_T.Top = 0
            N_Plant_T.Left = 0
            button_NP_Biome_1.Margin = N_Plant_T
        End If
        REM ------------------物品简介-------------------
        TextBlock_NP_Introduce.Text = N_Introduce
        TextBlock_NP_Introduce.Height = SetTextBlockHeight(N_Introduce, 10)
        N_WrapPanel_Introduce_P.Height = SetTextBlockHeight(N_Introduce, 10)
        REM ------------------图片及资源显示-------------------
        N_Show_P_Change(0)
        REM ------------------高度设置------------------- 
        Dim WrapPanel_Frame_P_Height As Integer = 0
        If N_WrapPanel_ToolsButton_P.Visibility = Visibility.Visible Then
            WrapPanel_Frame_P_Height += N_WrapPanel_Tools_P.Height + N_WrapPanel_ToolsButton_P.Height
        End If
        If N_Canvas_ResourcesDetals_P.Visibility = Visibility.Visible Then
            WrapPanel_Frame_P_Height += N_WrapPanel_Resources_P.Height + N_Canvas_ResourcesDetals_P.Height
        End If
        If N_WrapPanel_ResourcesBurntButton_P.Visibility = Visibility.Visible Then
            WrapPanel_Frame_P_Height += N_WrapPanel_ResourcesBurnt_P.Height + N_WrapPanel_ResourcesBurntButton_P.Height
        End If
        If N_Canvas_SpecialAbilityDetals_P.Visibility = Visibility.Visible Then
            WrapPanel_Frame_P_Height += N_WrapPanel_SpecialAbility_P.Height + N_Canvas_SpecialAbilityDetals_P.Height
        End If
        If N_WrapPanel_BiomeButton_P.Visibility = Visibility.Visible Then
            WrapPanel_Frame_P_Height += N_WrapPanel_Biome_P.Height + N_WrapPanel_BiomeButton_P.Height
        End If
        If N_WrapPanel_Switch_P.Visibility = Visibility.Visible Then
            WrapPanel_Frame_P_Height += 100
        End If
        WrapPanel_Frame_P_Height += N_WrapPanel_DLC_P.Height + N_Canvas_RegenerateAndCombustibleAttribite_P.Height + N_WrapPanel_Introduce_P.Height 
        N_WrapPanel_Frame_P.Height = WrapPanel_Frame_P_Height
        If N_WrapPanel_Frame_P.Height + 135 > 604 Then
            Canvas_NaturalLeft_P.Height = N_WrapPanel_Frame_P.Height + 135
        Else
            Canvas_NaturalLeft_P.Height = 604
        End If
    End Sub

    Private Sub N_Show_P_Change(Index As Byte)
        REM ------------------物品图片------------------
        NL_image_NaturalPicture_P.Source = Picture_Short_Name(Res_Short_Name(NP_PictureArray(Index)))
        REM --------------------资源--------------------
        If NP_ResourcesArray.Length = 12 Then
            N_WrapPanel_Switch_P.Visibility = Visibility.Collapsed
        End If
        Dim NP_RAExtract(11) As String
        For i = 0 To 11
            NP_RAExtract(i) = NP_ResourcesArray(Index, i)
        Next
        N_WrapPanel_Resources_P.Visibility = Visibility.Visible
        N_Canvas_ResourcesDetals_P.Visibility = Visibility.Visible
        button_NP_Resources_2.Visibility = Visibility.Collapsed
        NL_textBlock_Resources_P_2.Visibility = Visibility.Collapsed
        button_NP_Resources_3.Visibility = Visibility.Collapsed
        NL_textBlock_Resources_P_3.Visibility = Visibility.Collapsed
        button_NP_Resources_4.Visibility = Visibility.Collapsed
        NL_textBlock_Resources_P_4.Visibility = Visibility.Collapsed
        button_NP_ResourcesTools_1.Visibility = Visibility.Collapsed
        button_NP_ResourcesTools_2.Visibility = Visibility.Collapsed
        button_NP_ResourcesTools_3.Visibility = Visibility.Collapsed
        button_NP_ResourcesTools_4.Visibility = Visibility.Collapsed
        If NP_RAExtract(0) = "" Then
            N_WrapPanel_Resources_P.Visibility = Visibility.Collapsed
            N_Canvas_ResourcesDetals_P.Visibility = Visibility.Collapsed
        Else
            N_Canvas_ResourcesDetals_P.Height = 49
            image_NP_Resources_1.Source = Picture_Short_Name(Res_Short_Name(NP_RAExtract(0)))
            NL_textBlock_Resources_P_1.Text = NP_RAExtract(1)
            If NP_RAExtract(2) <> "" Then
                N_Canvas_ResourcesDetals_P.Height = 83
                button_NP_Resources_2.Visibility = Visibility.Visible
                NL_textBlock_Resources_P_2.Visibility = Visibility.Visible
                image_NP_Resources_2.Source = Picture_Short_Name(Res_Short_Name(NP_RAExtract(2)))
                NL_textBlock_Resources_P_2.Text = NP_RAExtract(3)
            End If
            If NP_RAExtract(4) <> "" Then
                N_Canvas_ResourcesDetals_P.Height = 117
                button_NP_Resources_3.Visibility = Visibility.Visible
                NL_textBlock_Resources_P_3.Visibility = Visibility.Visible
                image_NP_Resources_3.Source = Picture_Short_Name(Res_Short_Name(NP_RAExtract(4)))
                NL_textBlock_Resources_P_3.Text = NP_RAExtract(5)
            End If
            If NP_RAExtract(6) <> "" Then
                N_Canvas_ResourcesDetals_P.Height = 151
                button_NP_Resources_4.Visibility = Visibility.Visible
                NL_textBlock_Resources_P_4.Visibility = Visibility.Visible
                image_NP_Resources_4.Source = Picture_Short_Name(Res_Short_Name(NP_RAExtract(6)))
                NL_textBlock_Resources_P_4.Text = NP_RAExtract(7)
            End If
            If NP_RAExtract(8) <> "" Then
                button_NP_ResourcesTools_1.Visibility = Visibility.Visible
                image_NP_ResourcesTools_1.Source = Picture_Short_Name(Res_Short_Name(NP_RAExtract(8)))
            End If
            If NP_RAExtract(9) <> "" Then
                button_NP_ResourcesTools_2.Visibility = Visibility.Visible
                image_NP_ResourcesTools_2.Source = Picture_Short_Name(Res_Short_Name(NP_RAExtract(9)))
            End If
            If NP_RAExtract(10) <> "" Then
                button_NP_ResourcesTools_3.Visibility = Visibility.Visible
                image_NP_ResourcesTools_3.Source = Picture_Short_Name(Res_Short_Name(NP_RAExtract(10)))
            End If
            If NP_RAExtract(11) <> "" Then
                button_NP_ResourcesTools_4.Visibility = Visibility.Visible
                image_NP_ResourcesTools_4.Source = Picture_Short_Name(Res_Short_Name(NP_RAExtract(11)))
            End If
        End If
    End Sub

    Public Overloads Sub NP_Picture(ParamArray Picture() As String)
        Dim PictureNum As Byte = Picture.Length
        ReDim NP_PictureArray(PictureNum - 1)
        For i = 0 To PictureNum - 1
            NP_PictureArray(i) = Picture(i)
        Next
    End Sub

    Public Overloads Sub NP_Picture(Name As String, Num As Integer)
        Dim PictureNum As Byte = Num
        ReDim NP_PictureArray(PictureNum - 1)
        For i = 0 To PictureNum - 1
            NP_PictureArray(i) = Name & i + 1
        Next
    End Sub

    Public Overloads Sub NP_Picture(Name As String, Num As Integer, Name_2 As String, Num_2 As Integer)
        Dim PictureNum As Byte = Num + Num_2
        ReDim NP_PictureArray(PictureNum - 1)
        For i = 0 To Num - 1
            NP_PictureArray(i) = Name & i + 1
        Next
        For i = Num To Num_2 - 1
            For j = 1 To Num_2
                NP_PictureArray(i) = Name_2 & j
            Next
        Next
    End Sub

    Public Overloads Sub NP_Picture(Name As String, Num As Integer, Name_2 As String, Num_2 As Integer, Name_3 As String, Num_3 As Integer)
        Dim PictureNum As Byte = Num + Num_2 + Num_3
        ReDim NP_PictureArray(PictureNum - 1)
        For i = 0 To Num - 1
            NP_PictureArray(i) = Name & i + 1
        Next
        For i = Num To Num_2 - 1
            For j = 1 To Num_2
                NP_PictureArray(i) = Name_2 & j
            Next
        Next
        For i = Num_2 To Num_3 - 1
            For j = 1 To Num_3
                NP_PictureArray(i) = Name_3 & j
            Next
        Next
    End Sub

    Public Sub NP_Resources(Num As Integer, ParamArray Resources() As String)
        ReDim NP_ResourcesArray(Num - 1, 11)
        For i = 0 To Num - 1
            For j = 0 To 11
                NP_ResourcesArray(i, j) = Resources(j)
            Next
        Next
    End Sub

    Public Sub NP_Resources(IndexFirst As Byte, IndexLast As Byte, ParamArray Resources() As String)
        If IndexFirst = IndexLast Then
            For j = 0 To 11
                NP_ResourcesArray(IndexFirst, j) = Resources(j)
            Next
        Else
            For i = IndexFirst To IndexLast
                For j = 0 To 11
                    NP_ResourcesArray(i, j) = Resources(j)
                Next
            Next
        End If
    End Sub

    Private Sub button_NP_Switch_Left_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_NP_Switch_Left.Click
        button_NP_Switch_Right.IsEnabled = True
        If NP_ArrayIndex <> 0 Then
            NP_ArrayIndex -= 1
            If NP_ArrayIndex = 0 Then
                button_NP_Switch_Left.IsEnabled = False
            End If
            N_Show_P_Change(NP_ArrayIndex)
        End If
    End Sub

    Private Sub button_NP_Switch_Right_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_NP_Switch_Right.Click
        button_NP_Switch_Left.IsEnabled = True
        If NP_ArrayIndex <> NP_PictureArray.Length - 1 Then
            NP_ArrayIndex += 1
            If NP_ArrayIndex = NP_PictureArray.Length - 1 Then
                button_NP_Switch_Right.IsEnabled = False
            End If
            N_Show_P_Change(NP_ArrayIndex)
        End If
    End Sub

    Private Sub button_NP_Tools_1_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Tools_1.Click
        ButtonJump("S_axe")
    End Sub

    Private Sub button_NP_Tools_2_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Tools_2.Click
        ButtonJump("S_goldenaxe")
    End Sub

    Private Sub button_NP_Tools_3_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Tools_3.Click
        ButtonJump("S_pickaxe")
    End Sub

    Private Sub button_NP_Tools_4_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Tools_4.Click
        ButtonJump("S_goldenpickaxe")
    End Sub

    Private Sub button_NP_Tools_5_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Tools_5.Click
        ButtonJump("S_pickaxe_1")
    End Sub

    Private Sub button_NP_Tools_6_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Tools_6.Click
        ButtonJump("G_lucy_the_axe")
    End Sub

    Private Sub button_NP_Resources_1_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Resources_1.Click
        ButtonJump(NP_ResourcesArray(NP_ArrayIndex, 0))
    End Sub

    Private Sub button_NP_Resources_2_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Resources_2.Click
        ButtonJump(NP_ResourcesArray(NP_ArrayIndex, 2))
    End Sub

    Private Sub button_NP_Resources_3_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Resources_3.Click
        ButtonJump(NP_ResourcesArray(NP_ArrayIndex, 4))
    End Sub

    Private Sub button_NP_Resources_4_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Resources_4.Click
        ButtonJump(NP_ResourcesArray(NP_ArrayIndex, 6))
    End Sub

    Private Sub button_NP_ResourcesTools_1_click(sender As Object, e As RoutedEventArgs) Handles button_NP_ResourcesTools_1.Click
        ButtonJump(NP_ResourcesArray(NP_ArrayIndex, 8))
    End Sub

    Private Sub button_NP_ResourcesTools_2_click(sender As Object, e As RoutedEventArgs) Handles button_NP_ResourcesTools_2.Click
        ButtonJump(NP_ResourcesArray(NP_ArrayIndex, 9))
    End Sub

    Private Sub button_NP_ResourcesTools_3_click(sender As Object, e As RoutedEventArgs) Handles button_NP_ResourcesTools_3.Click
        ButtonJump(NP_ResourcesArray(NP_ArrayIndex, 10))
    End Sub

    Private Sub button_NP_ResourcesTools_4_click(sender As Object, e As RoutedEventArgs) Handles button_NP_ResourcesTools_4.Click
        ButtonJump(NP_ResourcesArray(NP_ArrayIndex, 11))
    End Sub

    Private Sub button_NP_ResourcesBurnt_1_click(sender As Object, e As RoutedEventArgs) Handles button_NP_ResourcesBurnt_1.Click
        ButtonJump(NP_ResourcesBurnt_1)
    End Sub

    Private Sub button_NP_ResourcesBurnt_2_click(sender As Object, e As RoutedEventArgs) Handles button_NP_ResourcesBurnt_2.Click
        ButtonJump(NP_ResourcesBurnt_2)
    End Sub

    Private Sub button_NP_ResourcesBurnt_3_click(sender As Object, e As RoutedEventArgs) Handles button_NP_ResourcesBurnt_3.Click
        ButtonJump(NP_ResourcesBurnt_3)
    End Sub

    Private Sub button_NP_ResourcesBurnt_4_click(sender As Object, e As RoutedEventArgs) Handles button_NP_ResourcesBurnt_4.Click
        ButtonJump(NP_ResourcesBurnt_4)
    End Sub

    Private Sub button_NP_SpecialAbility_1_click(sender As Object, e As RoutedEventArgs) Handles button_NP_SpecialAbility_1.Click
        ButtonJump(NP_SpecialAbility_1)
    End Sub

    Private Sub button_NP_SpecialAbility_2_click(sender As Object, e As RoutedEventArgs) Handles button_NP_SpecialAbility_1.Click
        ButtonJump(NP_SpecialAbility_2)
    End Sub

    Private Sub button_NP_Biome_1_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Biome_1.Click
        ButtonJump(NP_Biome_1)
    End Sub

    Private Sub button_NP_Biome_2_click(sender As Object, e As RoutedEventArgs) Handles button_NP_Biome_2.Click
        ButtonJump(NP_Biome_2)
    End Sub

    Private Sub button_N_flower_click(sender As Object, e As RoutedEventArgs) Handles button_N_flower.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("花", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "拾取时精神+5", "", "", True, True)
    End Sub

    Private Sub button_N_evil_flower_click(sender As Object, e As RoutedEventArgs) Handles button_N_evil_flower.Click
        NP_Picture("N_evil_flower_", 8)
        NP_Resources(11, {"G_dark_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("恶魔之花", "Evil Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_chess", "N_forest", "生成", "A_butterfly", "拾取时精神-5", "", "", True, True)
    End Sub

    Private Sub button_N_sapling_click(sender As Object, e As RoutedEventArgs) Handles button_N_sapling.Click
        'NP_Picture("N_flower_", 11)
        'NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        'N_Show_P("树苗", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_grass_click(sender As Object, e As RoutedEventArgs) Handles button_N_grass.Click
        NP_Picture({"N_grass", "N_grass_empty", "N_grass_dead"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"G_cut_grass", "×1", "G_grass_tuft", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(1, 2, {"G_grass_tuft", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("草", "Grass", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_savanna", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_berry_bush_click(sender As Object, e As RoutedEventArgs) Handles button_N_berry_bush.Click
        NP_Picture({"N_berry_bush_1", "N_berry_bush_empty_1", "N_berry_bush_dead_1"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"F_berries", "×1", "G_berry_bush", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(1, 1, {"G_berry_bush", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        NP_Resources(2, 2, {"G_twigs", "×2(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("浆果灌木丛", "Berry Bush", "NoDLC", 1, 0, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_gobbler", "", "", "", False, True)
    End Sub

    Private Sub button_N_berry_bush_2_click(sender As Object, e As RoutedEventArgs) Handles button_N_berry_bush_2.Click
        NP_Picture({"N_berry_bush_2", "N_berry_bush_empty_2", "N_berry_bush_dead_2"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"F_berries", "×1", "G_berry_bush_2", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(1, 1, {"G_berry_bush_2", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        NP_Resources(2, 2, {"G_twigs", "×2(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("浆果灌木丛", "Berry Bush", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "", "生成", "A_gobbler", "生成              (SW)", "A_snake", "", False, True)
    End Sub

    Private Sub button_N_juicy_berry_bush_click(sender As Object, e As RoutedEventArgs) Handles button_N_juicy_berry_bush.Click
        'NP_Picture("N_flower_", 11)
        'NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        'N_Show_P("蜜汁浆果丛", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_reeds_click(sender As Object, e As RoutedEventArgs) Handles button_N_reeds.Click
        NP_Picture({"N_reeds", "N_reeds_empty"})
        ReDim NP_ResourcesArray(1, 11)
        NP_Resources(0, 0, {"G_cut_reeds", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("芦苇", "Reeds", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_marsh", "", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_spiky_bush_click(sender As Object, e As RoutedEventArgs) Handles button_N_spiky_bush.Click
        NP_Picture({"N_spiky_bush", "N_spiky_bush_empty"})
        ReDim NP_ResourcesArray(1, 11)
        NP_Resources(0, 0, {"G_twigs", "×1", "", "", "", "", "", "", "", "", "", ""})
        NP_Resources(1, 1, {"G_spiky_bushes", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("尖刺灌木", "Spiky Bush", "NoDLC", 1, 0, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_marsh", "", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_cactus_click(sender As Object, e As RoutedEventArgs) Handles button_N_cactus.Click
        NP_Picture({"N_cactus", "N_cactus_flower", "N_cactus_empty"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"F_cactus_flesh", "×1", "", "", "", "", "", "", "", "", "", ""})
        NP_Resources(1, 1, {"F_cactus_flower", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("仙人掌", "Cactus", "NoDLC", 1, 0, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_desert", "", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_plant_click(sender As Object, e As RoutedEventArgs) Handles button_N_plant.Click
        NP_Picture({"N_plant"})
        ReDim NP_ResourcesArray(0, 11)
        N_Show_P("种植", "Plant", "NoDLC", 1, 0, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "", "生成", "A_butterfly", "", "", "", False, True)
    End Sub

    Private Sub button_N_algae_click(sender As Object, e As RoutedEventArgs) Handles button_N_algae.Click
        NP_Picture({"N_algae"})
        ReDim NP_ResourcesArray(0, 11)
        N_Show_P("水藻", "Algae", "NoDLC", 1, 0, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_village", "N_wilds", "生成", "A_butterfly", "", "", "", False, True)
    End Sub

    Private Sub button_N_blue_mushroom_click(sender As Object, e As RoutedEventArgs) Handles button_N_blue_mushroom.Click
        NP_Picture({"N_blue_mushroom", "N_mushroom_inground", "N_mushroom_picked"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"F_blue_cap", "×1", "F_blue_cap", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(1, 1, {"F_blue_cap", "×1", "F_blue_cap", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(2, 2, {"F_blue_cap", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("蓝蘑菇", "Blue Mushroom", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_forest", "N_blue_mushtree_forest", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_green_mushroom_click(sender As Object, e As RoutedEventArgs) Handles button_N_green_mushroom.Click
        NP_Picture({"N_green_mushroom", "N_mushroom_inground", "N_mushroom_picked"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"F_green_cap", "×1", "F_green_cap", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(1, 1, {"F_green_cap", "×1", "F_green_cap", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(2, 2, {"F_green_cap", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("绿蘑菇", "Green Mushroom", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_marsh", "N_green_mushtree_forest", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_red_mushroom_click(sender As Object, e As RoutedEventArgs) Handles button_N_red_mushroom.Click
        NP_Picture({"N_red_mushroom", "N_mushroom_inground", "N_mushroom_picked"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"F_red_cap", "×1", "F_red_cap", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(1, 1, {"F_red_cap", "×1", "F_red_cap", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(2, 2, {"F_red_cap", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("红蘑菇", "Red Mushroom", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_red_mushtree_forest", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_light_flower_1_click(sender As Object, e As RoutedEventArgs) Handles button_N_light_flower_1.Click
        NP_Picture({"N_light_flower_springy", "N_light_flower_springy_empty", "N_light_flower_1", "N_light_flower_empty_1", "N_light_flower_2", "N_light_flower_empty_2", "N_light_flower_3", "N_light_flower_empty_3"})
        ReDim NP_ResourcesArray(7, 11)
        NP_Resources(0, 0, {"G_light_bulb", "×1", "", "", "", "", "", "", "", "", "", ""})
        NP_Resources(2, 2, {"G_light_bulb", "×1", "", "", "", "", "", "", "", "", "", ""})
        NP_Resources(4, 4, {"G_light_bulb", "×2", "", "", "", "", "", "", "", "", "", ""})
        NP_Resources(6, 6, {"G_light_bulb", "×3", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("荧光草", "Light Flower", "NoDLC", 1, 0, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_wilds", "N_light_flower_swamp", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_fern_1_click(sender As Object, e As RoutedEventArgs) Handles button_N_fern_1.Click
        NP_Picture("N_fern_", 10)
        NP_Resources(10, {"G_foliage", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("蕨类植物", "Fern", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_wilds", "", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_cave_lichen_click(sender As Object, e As RoutedEventArgs) Handles button_N_cave_lichen.Click
        NP_Picture({"N_cave_lichen", "N_cave_lichen_empty"})
        ReDim NP_ResourcesArray(1, 11)
        NP_Resources(0, 0, {"F_lichen", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("洞穴苔藓", "Cave Lichen", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_wilds", "", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_grass_sw_click(sender As Object, e As RoutedEventArgs) Handles button_N_grass_sw.Click
        NP_Picture({"N_grass_sw", "N_grass_empty_sw", "N_grass_dead"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"G_cut_grass_SW", "×1", "G_grass_tuft_SW", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(1, 2, {"G_grass_tuft_SW", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("船难草", "Grass(SW)", "SW", 0, 1, 0, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_beach", "N_meadow", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_bamboo_patch_click(sender As Object, e As RoutedEventArgs) Handles button_N_bamboo_patch.Click
        NP_Picture({"N_bamboo_patch", "N_bamboo_patch_empty", "N_bamboo_patch_dead"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"G_bamboo_patch", "×1(           )", "G_bamboo_root", "×1(                      )", "", "", "", "", "S_machete", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(1, 2, {"G_bamboo_root", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("竹子", "Bamboo Patch", "SW", 0, 1, 0, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_jungle", "", "", "", "", "", "", False, True)
    End Sub

    Private Sub button_N_viney_bush_click(sender As Object, e As RoutedEventArgs) Handles button_N_viney_bush.Click
        NP_Picture({"N_viney_bush", "N_viney_bush_empty", "N_viney_bush_dead"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"G_vine", "×1(           )", "G_viney_bush_root", "×1(                      )", "", "", "", "", "S_machete", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(1, 2, {"G_viney_bush_root", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("藤蔓丛", "Viney Bush", "SW", 0, 1, 0, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_jungle", "", "生成", "A_snake", "生成", "A_poison_snake", "", False, True)
    End Sub

    Private Sub button_N_seaweed_click(sender As Object, e As RoutedEventArgs) Handles button_N_seaweed.Click
        NP_Picture({"N_seaweed", "N_seaweed_empty"})
        ReDim NP_ResourcesArray(1, 11)
        NP_Resources(0, 0, {"F_seaweed", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("海藻", "Seaweed", "SW", 0, 1, 0, "NoTool", "", "", "", "", "", "", "", "", "N_ocean_shallow", "", "", "", "", "", "", True, False)
    End Sub

    Private Sub button_N_coffee_plant_click(sender As Object, e As RoutedEventArgs) Handles button_N_coffee_plant.Click
        NP_Picture({"N_coffee_plant", "N_coffee_plant_empty", "N_coffee_plant_dead"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"F_coffee_beans", "×1", "G_coffee_plant", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel"})
        NP_Resources(1, 2, {"G_coffee_plant", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("咖啡树", "Coffee Plant", "SW", 0, 1, 0, "NoTool", "", "", "", "", "", "", "", "", "N_volcano_bio", "", "", "", "", "", "", False, False)
    End Sub

    Private Sub button_N_elephant_cactus_click(sender As Object, e As RoutedEventArgs) Handles button_N_elephant_cactus.Click
        NP_Picture({"N_elephant_cactus", "N_elephant_cactus_dead", "N_elephant_cactus_stump"})
        ReDim NP_ResourcesArray(2, 11)
        NP_Resources(0, 0, {"G_cactus_spike", "×1", "", "", "", "", "", "", "", "", "", ""})
        NP_Resources(1, 1, {"G_twigs", "×2(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        NP_Resources(2, 2, {"G_elephant_cactus_stump", "×1(                      )", "", "", "", "", "", "", "S_shovel", "S_goldenshovel", "", ""})
        N_Show_P("象仙人掌", "Elephant Cactus", "SW", 0, 1, 0, "NoTool", "", "", "", "", "", "", "", "", "N_volcano_bio", "", "", "", "", "", "", False, False)
    End Sub

    Private Sub button_N_sapling_diseased_click(sender As Object, e As RoutedEventArgs) Handles button_N_sapling_diseased.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("患病的树苗", "Flower", "DST", 0, 0, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_grass_diseased_click(sender As Object, e As RoutedEventArgs) Handles button_N_grass_diseased.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("患病的草", "Flower", "DST", 0, 0, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_berry_bush_diseased_click(sender As Object, e As RoutedEventArgs) Handles button_N_berry_bush_diseased.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("患病的浆果灌木丛", "Flower", "DST", 0, 0, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_juicy_berry_bush_diseased_click(sender As Object, e As RoutedEventArgs) Handles button_N_juicy_berry_bush_diseased.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("患病的蜜汁浆果丛", "Flower", "DST", 0, 0, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_evergreen_click(sender As Object, e As RoutedEventArgs) Handles button_N_evergreen.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("常青树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_lumpy_evergreen_click(sender As Object, e As RoutedEventArgs) Handles button_N_lumpy_evergreen.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("粗壮常青树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_birchnut_tree_click(sender As Object, e As RoutedEventArgs) Handles button_N_birchnut_tree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("桦树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_totally_normal_tree_click(sender As Object, e As RoutedEventArgs) Handles button_N_totally_normal_tree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("完全正常的树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_spiky_tree_click(sender As Object, e As RoutedEventArgs) Handles button_N_spiky_tree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("针叶树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_blue_mushtree_click(sender As Object, e As RoutedEventArgs) Handles button_N_blue_mushtree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("蓝色蘑菇树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_green_mushtree_click(sender As Object, e As RoutedEventArgs) Handles button_N_green_mushtree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("绿色蘑菇树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_red_mushtree_click(sender As Object, e As RoutedEventArgs) Handles button_N_red_mushtree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("红色蘑菇树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_webbed_blue_mushtree_click(sender As Object, e As RoutedEventArgs) Handles button_N_webbed_blue_mushtree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("长满网的蓝色蘑菇树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_cave_banana_tree_click(sender As Object, e As RoutedEventArgs) Handles button_N_cave_banana_tree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("洞穴香蕉树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_palm_tree_click(sender As Object, e As RoutedEventArgs) Handles button_N_palm_tree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("椰子树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_jungle_tree_click(sender As Object, e As RoutedEventArgs) Handles button_N_jungle_tree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("丛林树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_mangrove_click(sender As Object, e As RoutedEventArgs) Handles button_N_mangrove.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("红树林", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_regular_jungle_tree_click(sender As Object, e As RoutedEventArgs) Handles button_N_regular_jungle_tree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("正常的丛林树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_burnt_ash_tree_click(sender As Object, e As RoutedEventArgs) Handles button_N_burnt_ash_tree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("烧焦的灰树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_brainy_sprout_click(sender As Object, e As RoutedEventArgs) Handles button_N_brainy_sprout.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("聪明芽", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_twiggy_tree_click(sender As Object, e As RoutedEventArgs) Handles button_N_twiggy_tree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("多枝的树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_twiggy_tree_diseased_click(sender As Object, e As RoutedEventArgs) Handles button_N_twiggy_tree_diseased.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("患病的多枝的树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    Private Sub button_N_petrified_tree_click(sender As Object, e As RoutedEventArgs) Handles button_N_petrified_tree.Click
        NP_Picture("N_flower_", 11)
        NP_Resources(11, {"G_petals", "×1", "", "", "", "", "", "", "", "", "", ""})
        N_Show_P("石化树", "Flower", "NoDLC", 1, 1, 1, "NoTool", "G_ash", "×1", "", "", "", "", "", "", "N_grasslands", "N_forest", "生成", "A_butterfly", "", "", "", True, True)
    End Sub

    REM ------------------左侧面板隐藏(物品)------------------
    Private Sub G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_Material.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_Equipment.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_Sapling.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_Animal.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_Turf.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_Pet.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_PetLavaeEgg.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_Unlock.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_Component.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_Blueprint.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_BallphinFreeTuna.Visibility = Visibility.Collapsed
        ScrollViewer_GoodsLeft_MessageInABottle.Visibility = Visibility.Collapsed
    End Sub

    REM ------------------左侧面板(物品_材料)------------------
    Private Sub G_Show_M(G_Name As String, G_EnName As String, G_picture As String, G_DLC As String, G_DLC_ROG As SByte, G_DLC_SW As SByte, G_DLC_DST As SByte, G_Introduce As String, Optional PigKing As Boolean = False, Optional Yaarctopus As Boolean = False)
        REM ------------------初始化------------------
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_Material.Visibility = Visibility.Visible
        G_WrapPanel_Gift_M.Visibility = Visibility.Collapsed
        G_WrapPanel_GiftButton_M.Visibility = Visibility.Collapsed
        REM ------------------物品名字------------------
        GL_textBlock_GoodsName_M.Text = G_Name
        GL_textBlock_GoodsName_M.UpdateLayout()
        Dim G_N_MarginLeft As Integer
        G_N_MarginLeft = (Canvas_GoodsLeft_M.ActualWidth - GL_textBlock_GoodsName_M.ActualWidth) / 2
        Dim G_N_T As New Thickness()
        G_N_T.Top = 80
        G_N_T.Left = G_N_MarginLeft
        GL_textBlock_GoodsName_M.Margin = G_N_T

        GL_textBlock_GoodsEnName_M.Text = G_EnName
        GL_textBlock_GoodsEnName_M.UpdateLayout()
        Dim G_EnN_MarginLeft As Integer
        G_EnN_MarginLeft = (Canvas_GoodsLeft_M.ActualWidth - GL_textBlock_GoodsEnName_M.ActualWidth) / 2
        Dim G_EnN_T As New Thickness()
        G_EnN_T.Top = 100
        G_EnN_T.Left = G_EnN_MarginLeft
        GL_textBlock_GoodsEnName_M.Margin = G_EnN_T
        REM ------------------物品图片------------------
        GL_image_GoodsPicture_M.Source = Picture_Short_Name(Res_Short_Name(G_picture))
        REM ------------------物品DLC-------------------
        If G_DLC = "ROG" Then
            GL_image_GM_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf G_DLC = "SW" Then
            GL_image_GM_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf G_DLC = "DST" Then
            GL_image_GM_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            GL_image_GM_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------存在版本-------------------
        GL_textBlock_GM_DLC_1.Foreground = Brushes.Black
        GL_textBlock_GM_DLC_2.Foreground = Brushes.Black
        GL_textBlock_GM_DLC_3.Foreground = Brushes.Black
        If G_DLC_ROG = 0 Then
            GL_textBlock_GM_DLC_1.Foreground = Brushes.Silver
        End If
        If G_DLC_SW = 0 Then
            GL_textBlock_GM_DLC_2.Foreground = Brushes.Silver
        End If
        If G_DLC_DST = 0 Then
            GL_textBlock_GM_DLC_3.Foreground = Brushes.Silver
        End If
        REM ------------------礼物-------------------
        G_Gift(PigKing, Yaarctopus)
        REM ------------------物品简介-------------------
        TextBlock_GM_Introduce.Text = G_Introduce
        TextBlock_GM_Introduce.Height = SetTextBlockHeight(G_Introduce, 13)
        G_WrapPanel_Introduce_M.Height = SetTextBlockHeight(G_Introduce, 13)
        REM ------------------高度设置------------------- 
        Dim WrapPanel_Frame_M_Height As Integer = 0
        If G_WrapPanel_Science_M.Visibility = Visibility.Visible Then
            WrapPanel_Frame_M_Height += G_WrapPanel_Science_M.Height + G_WrapPanel_ScienceButton_M.Height
        End If
        If G_WrapPanel_Animal_M.Visibility = Visibility.Visible Then
            WrapPanel_Frame_M_Height += G_WrapPanel_Animal_M.Height + G_WrapPanel_AnimalButton_M.Height
        End If
        If G_WrapPanel_Gift_M.Visibility = Visibility.Visible Then
            WrapPanel_Frame_M_Height += G_WrapPanel_Gift_M.Height + G_WrapPanel_GiftButton_M.Height
        End If
        WrapPanel_Frame_M_Height += G_WrapPanel_DLC_M.Height + G_WrapPanel_Introduce_M.Height
        G_WrapPanel_Frame_M.Height = WrapPanel_Frame_M_Height
        If G_WrapPanel_Frame_M.Height + 135 > 604 Then
            Canvas_GoodsLeft_M.Height = G_WrapPanel_Frame_M.Height + 135
        Else
            Canvas_GoodsLeft_M.Height = 604
        End If
    End Sub

    Public Sub G_Science(ScienceNum As Byte, ParamArray Science() As String)
        If ScienceNum = 0 Then
            G_WrapPanel_Science_M.Visibility = Visibility.Collapsed
            G_WrapPanel_ScienceButton_M.Visibility = Visibility.Collapsed
            Exit Sub
        Else
            G_WrapPanel_Science_M.Visibility = Visibility.Visible
            G_WrapPanel_ScienceButton_M.Visibility = Visibility.Visible
        End If
        Dim ArrayScience() As Button = {button_GM_Science_1, button_GM_Science_2, button_GM_Science_3, button_GM_Science_4, button_GM_Science_5, button_GM_Science_6, button_GM_Science_7, button_GM_Science_8, button_GM_Science_9, button_GM_Science_10, button_GM_Science_11, button_GM_Science_12, button_GM_Science_13, button_GM_Science_14, button_GM_Science_15, button_GM_Science_16, button_GM_Science_17, button_GM_Science_18, button_GM_Science_19, button_GM_Science_20, button_GM_Science_21, button_GM_Science_22, button_GM_Science_23, button_GM_Science_24, button_GM_Science_25, button_GM_Science_26, button_GM_Science_27, button_GM_Science_28, button_GM_Science_29, button_GM_Science_30, button_GM_Science_31, button_GM_Science_32, button_GM_Science_33, button_GM_Science_34, button_GM_Science_35, button_GM_Science_36, button_GM_Science_37, button_GM_Science_38, button_GM_Science_39, button_GM_Science_40, button_GM_Science_41}
        Dim ArrayScienceImage() As Image = {image_GM_Science_1, image_GM_Science_2, image_GM_Science_3, image_GM_Science_4, image_GM_Science_5, image_GM_Science_6, image_GM_Science_7, image_GM_Science_8, image_GM_Science_9, image_GM_Science_10, image_GM_Science_11, image_GM_Science_12, image_GM_Science_13, image_GM_Science_14, image_GM_Science_15, image_GM_Science_16, image_GM_Science_17, image_GM_Science_18, image_GM_Science_19, image_GM_Science_20, image_GM_Science_21, image_GM_Science_22, image_GM_Science_23, image_GM_Science_24, image_GM_Science_25, image_GM_Science_26, image_GM_Science_27, image_GM_Science_28, image_GM_Science_29, image_GM_Science_30, image_GM_Science_31, image_GM_Science_32, image_GM_Science_33, image_GM_Science_34, image_GM_Science_35, image_GM_Science_36, image_GM_Science_37, image_GM_Science_38, image_GM_Science_39, image_GM_Science_40, image_GM_Science_41}
        For i = ScienceNum To 40
            ArrayScience(i).Visibility = Visibility.Collapsed
        Next
        For i = 0 To ScienceNum - 1
            GM_ScienceArray(i) = Science(i)
            ArrayScience(i).Visibility = Visibility.Visible
            ArrayScienceImage(i).Source = Picture_Short_Name(Res_Short_Name(Science(i)))
        Next
        G_WrapPanel_ScienceButton_M.Height = (ScienceNum / 5 + 1) * 34 + 3 '5是空隙
    End Sub

    Private Sub button_GM_Science_1_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_1.Click
        ButtonJump(GM_ScienceArray(0))
    End Sub

    Private Sub button_GM_Science_2_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_2.Click
        ButtonJump(GM_ScienceArray(1))
    End Sub

    Private Sub button_GM_Science_3_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_3.Click
        ButtonJump(GM_ScienceArray(2))
    End Sub

    Private Sub button_GM_Science_4_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_4.Click
        ButtonJump(GM_ScienceArray(3))
    End Sub

    Private Sub button_GM_Science_5_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_5.Click
        ButtonJump(GM_ScienceArray(4))
    End Sub

    Private Sub button_GM_Science_6_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_6.Click
        ButtonJump(GM_ScienceArray(5))
    End Sub

    Private Sub button_GM_Science_7_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_7.Click
        ButtonJump(GM_ScienceArray(6))
    End Sub

    Private Sub button_GM_Science_8_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_8.Click
        ButtonJump(GM_ScienceArray(7))
    End Sub

    Private Sub button_GM_Science_9_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_9.Click
        ButtonJump(GM_ScienceArray(8))
    End Sub

    Private Sub button_GM_Science_10_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_10.Click
        ButtonJump(GM_ScienceArray(9))
    End Sub

    Private Sub button_GM_Science_11_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_11.Click
        ButtonJump(GM_ScienceArray(10))
    End Sub

    Private Sub button_GM_Science_12_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_12.Click
        ButtonJump(GM_ScienceArray(11))
    End Sub

    Private Sub button_GM_Science_13_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_13.Click
        ButtonJump(GM_ScienceArray(12))
    End Sub

    Private Sub button_GM_Science_14_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_14.Click
        ButtonJump(GM_ScienceArray(13))
    End Sub

    Private Sub button_GM_Science_15_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_15.Click
        ButtonJump(GM_ScienceArray(14))
    End Sub

    Private Sub button_GM_Science_16_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_16.Click
        ButtonJump(GM_ScienceArray(15))
    End Sub

    Private Sub button_GM_Science_17_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_17.Click
        ButtonJump(GM_ScienceArray(16))
    End Sub

    Private Sub button_GM_Science_18_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_18.Click
        ButtonJump(GM_ScienceArray(17))
    End Sub

    Private Sub button_GM_Science_19_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_19.Click
        ButtonJump(GM_ScienceArray(18))
    End Sub

    Private Sub button_GM_Science_20_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_20.Click
        ButtonJump(GM_ScienceArray(19))
    End Sub

    Private Sub button_GM_Science_21_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_21.Click
        ButtonJump(GM_ScienceArray(20))
    End Sub

    Private Sub button_GM_Science_22_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_22.Click
        ButtonJump(GM_ScienceArray(21))
    End Sub

    Private Sub button_GM_Science_23_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_23.Click
        ButtonJump(GM_ScienceArray(22))
    End Sub

    Private Sub button_GM_Science_24_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_24.Click
        ButtonJump(GM_ScienceArray(23))
    End Sub

    Private Sub button_GM_Science_25_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_25.Click
        ButtonJump(GM_ScienceArray(24))
    End Sub

    Private Sub button_GM_Science_26_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_26.Click
        ButtonJump(GM_ScienceArray(25))
    End Sub

    Private Sub button_GM_Science_27_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_27.Click
        ButtonJump(GM_ScienceArray(26))
    End Sub

    Private Sub button_GM_Science_28_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_28.Click
        ButtonJump(GM_ScienceArray(27))
    End Sub

    Private Sub button_GM_Science_29_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_29.Click
        ButtonJump(GM_ScienceArray(28))
    End Sub

    Private Sub button_GM_Science_30_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_30.Click
        ButtonJump(GM_ScienceArray(29))
    End Sub

    Private Sub button_GM_Science_31_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_31.Click
        ButtonJump(GM_ScienceArray(30))
    End Sub

    Private Sub button_GM_Science_32_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_32.Click
        ButtonJump(GM_ScienceArray(31))
    End Sub

    Private Sub button_GM_Science_33_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_33.Click
        ButtonJump(GM_ScienceArray(32))
    End Sub

    Private Sub button_GM_Science_34_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_34.Click
        ButtonJump(GM_ScienceArray(33))
    End Sub

    Private Sub button_GM_Science_35_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_35.Click
        ButtonJump(GM_ScienceArray(34))
    End Sub

    Private Sub button_GM_Science_36_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_36.Click
        ButtonJump(GM_ScienceArray(35))
    End Sub

    Private Sub button_GM_Science_37_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_37.Click
        ButtonJump(GM_ScienceArray(36))
    End Sub

    Private Sub button_GM_Science_38_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_38.Click
        ButtonJump(GM_ScienceArray(37))
    End Sub

    Private Sub button_GM_Science_39_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_39.Click
        ButtonJump(GM_ScienceArray(38))
    End Sub

    Private Sub button_GM_Science_40_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_40.Click
        ButtonJump(GM_ScienceArray(39))
    End Sub

    Private Sub button_GM_Science_41_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Science_41.Click
        ButtonJump(GM_ScienceArray(40))
    End Sub

    Public Sub G_Animal(AnimalNum As Byte, ParamArray Animal() As String)
        If AnimalNum = 0 Then
            G_WrapPanel_Animal_M.Visibility = Visibility.Collapsed
            G_WrapPanel_AnimalButton_M.Visibility = Visibility.Collapsed
            Exit Sub
        Else
            G_WrapPanel_Animal_M.Visibility = Visibility.Visible
            G_WrapPanel_AnimalButton_M.Visibility = Visibility.Visible
        End If
        Dim ArrayAnimal() As Button = {button_GM_Animal_1, button_GM_Animal_2, button_GM_Animal_3, button_GM_Animal_4, button_GM_Animal_5, button_GM_Animal_6, button_GM_Animal_7, button_GM_Animal_8, button_GM_Animal_9, button_GM_Animal_10, button_GM_Animal_11, button_GM_Animal_12, button_GM_Animal_13, button_GM_Animal_14, button_GM_Animal_15, button_GM_Animal_16, button_GM_Animal_17, button_GM_Animal_18, button_GM_Animal_19, button_GM_Animal_20, button_GM_Animal_21, button_GM_Animal_22, button_GM_Animal_23, button_GM_Animal_24, button_GM_Animal_25, button_GM_Animal_26, button_GM_Animal_27, button_GM_Animal_28, button_GM_Animal_29}
        Dim ArrayAnimalImage() As Image = {image_GM_Animal_1, image_GM_Animal_2, image_GM_Animal_3, image_GM_Animal_4, image_GM_Animal_5, image_GM_Animal_6, image_GM_Animal_7, image_GM_Animal_8, image_GM_Animal_9, image_GM_Animal_10, image_GM_Animal_11, image_GM_Animal_12, image_GM_Animal_13, image_GM_Animal_14, image_GM_Animal_15, image_GM_Animal_16, image_GM_Animal_17, image_GM_Animal_18, image_GM_Animal_19, image_GM_Animal_20, image_GM_Animal_21, image_GM_Animal_22, image_GM_Animal_23, image_GM_Animal_24, image_GM_Animal_25, image_GM_Animal_26, image_GM_Animal_27, image_GM_Animal_28, image_GM_Animal_29}
        For i = AnimalNum To 28
            ArrayAnimal(i).Visibility = Visibility.Collapsed
        Next
        For i = 0 To AnimalNum - 1
            GM_AnimalArray(i) = Animal(i)
            If Animal(i) = "A_treeguard_1" Or Animal(i) = "A_treeguard_2" Or Animal(i) = "A_treeguard_3" Then
                Animal(i) = "A_treeguard"
            End If
            ArrayAnimal(i).Visibility = Visibility.Visible
            ArrayAnimalImage(i).Source = Picture_Short_Name(Res_Short_Name(Animal(i)))
        Next
        G_WrapPanel_AnimalButton_M.Height = (AnimalNum / 5 + 1) * 34 + 9
    End Sub

    Private Sub button_GM_Animal_1_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_1.Click
        ButtonJump(GM_AnimalArray(0))
    End Sub

    Private Sub button_GM_Animal_2_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_2.Click
        ButtonJump(GM_AnimalArray(1))
    End Sub

    Private Sub button_GM_Animal_3_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_3.Click
        ButtonJump(GM_AnimalArray(2))
    End Sub

    Private Sub button_GM_Animal_4_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_4.Click
        ButtonJump(GM_AnimalArray(3))
    End Sub

    Private Sub button_GM_Animal_5_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_5.Click
        ButtonJump(GM_AnimalArray(4))
    End Sub

    Private Sub button_GM_Animal_6_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_6.Click
        ButtonJump(GM_AnimalArray(5))
    End Sub

    Private Sub button_GM_Animal_7_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_7.Click
        ButtonJump(GM_AnimalArray(6))
    End Sub

    Private Sub button_GM_Animal_8_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_8.Click
        ButtonJump(GM_AnimalArray(7))
    End Sub

    Private Sub button_GM_Animal_9_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_9.Click
        ButtonJump(GM_AnimalArray(8))
    End Sub

    Private Sub button_GM_Animal_10_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_10.Click
        ButtonJump(GM_AnimalArray(9))
    End Sub

    Private Sub button_GM_Animal_11_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_11.Click
        ButtonJump(GM_AnimalArray(10))
    End Sub

    Private Sub button_GM_Animal_12_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_12.Click
        ButtonJump(GM_AnimalArray(11))
    End Sub

    Private Sub button_GM_Animal_13_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_13.Click
        ButtonJump(GM_AnimalArray(12))
    End Sub

    Private Sub button_GM_Animal_14_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_14.Click
        ButtonJump(GM_AnimalArray(13))
    End Sub

    Private Sub button_GM_Animal_15_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_15.Click
        ButtonJump(GM_AnimalArray(14))
    End Sub

    Private Sub button_GM_Animal_16_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_16.Click
        ButtonJump(GM_AnimalArray(15))
    End Sub

    Private Sub button_GM_Animal_17_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_17.Click
        ButtonJump(GM_AnimalArray(16))
    End Sub

    Private Sub button_GM_Animal_18_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_18.Click
        ButtonJump(GM_AnimalArray(17))
    End Sub

    Private Sub button_GM_Animal_19_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_19.Click
        ButtonJump(GM_AnimalArray(18))
    End Sub

    Private Sub button_GM_Animal_20_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_20.Click
        ButtonJump(GM_AnimalArray(19))
    End Sub

    Private Sub button_GM_Animal_21_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_21.Click
        ButtonJump(GM_AnimalArray(20))
    End Sub

    Private Sub button_GM_Animal_22_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_22.Click
        ButtonJump(GM_AnimalArray(21))
    End Sub

    Private Sub button_GM_Animal_23_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_23.Click
        ButtonJump(GM_AnimalArray(22))
    End Sub

    Private Sub button_GM_Animal_24_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_24.Click
        ButtonJump(GM_AnimalArray(23))
    End Sub

    Private Sub button_GM_Animal_25_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_25.Click
        ButtonJump(GM_AnimalArray(24))
    End Sub

    Private Sub button_GM_Animal_26_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_26.Click
        ButtonJump(GM_AnimalArray(25))
    End Sub

    Private Sub button_GM_Animal_27_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_27.Click
        ButtonJump(GM_AnimalArray(26))
    End Sub

    Private Sub button_GM_Animal_28_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_28.Click
        ButtonJump(GM_AnimalArray(27))
    End Sub

    Private Sub button_GM_Animal_29_click(sender As Object, e As RoutedEventArgs) Handles button_GM_Animal_29.Click
        ButtonJump(GM_AnimalArray(28))
    End Sub

    Public Sub G_Gift(PigKing As Boolean, Yaarctopus As Boolean)
        If PigKing <> False Or Yaarctopus <> False Then
            G_WrapPanel_Gift_M.Visibility = Visibility.Visible
            G_WrapPanel_GiftButton_M.Visibility = Visibility.Visible
            button_GM_GiftPigKing.Visibility = Visibility.Collapsed
            button_GM_GiftYaarctopus.Visibility = Visibility.Collapsed
            If PigKing = True Then
                button_GM_GiftPigKing.Visibility = Visibility.Visible
            End If
            If Yaarctopus = True Then
                button_GM_GiftYaarctopus.Visibility = Visibility.Visible
            End If
        End If
    End Sub

    Private Sub button_GM_GiftPigKing_click(sender As Object, e As RoutedEventArgs) Handles button_GM_GiftPigKing.Click
        ButtonJump("A_pig_king")
    End Sub

    Private Sub button_GM_GiftYaarctopus_click(sender As Object, e As RoutedEventArgs) Handles button_GM_GiftYaarctopus.Click
        ButtonJump("A_yaarctopus")
    End Sub

    Private Sub button_G_ash_click(sender As Object, e As RoutedEventArgs) Handles button_G_ash.Click
        G_Science(1, {"S_healing_salve"})
        G_Animal(1, {"A_red_hound"})
        G_Show_M("灰烬", "Ash", "G_ash", "NoDLC", 1, 1, 1, "主要还是来源于烧毁的植物。")
    End Sub

    Private Sub button_G_flint_click(sender As Object, e As RoutedEventArgs) Handles button_G_flint.Click
        G_Science(11, {"S_axe", "S_machete", "S_pickaxe", "S_shovel", "S_pitchfork", "S_razor", "S_compass", "S_thermal_stone", "S_battle_spear", "S_spear", "S_bee_mine"})
        G_Animal(1, {"A_rock_lobster"})
        G_Show_M("燧石", "Flint", "G_flint", "NoDLC", 1, 1, 1, "通常草原和马赛克区域的地上能找到不少，通过挖卵石也可以获得。")
    End Sub

    Private Sub button_G_nitre_click(sender As Object, e As RoutedEventArgs) Handles button_G_nitre.Click
        G_Science(6, {"S_salt_lick", "S_endothermic_fire", "S_endothermic_fire_pit", "S_booster_shot", "S_gunpowder", "S_morning_star"})
        G_Animal(0, {})
        G_Show_M("硝石", "Nitre", "G_nitre", "NoDLC", 1, 1, 1, "主要靠挖卵石获得。")
    End Sub

    Private Sub button_G_rocks_click(sender As Object, e As RoutedEventArgs) Handles button_G_rocks.Click
        G_Science(9, {"S_hammer", "S_fire_pit", "S_healing_salve", "S_thermal_stone", "S_improved_farm", "S_science_machine", "S_battle_helm", "S_dragoon_den", "S_cut_stone"})
        G_Animal(3, {"A_rock_lobster", "A_big_tentacle", "A_lavae"})
        G_Show_M("岩石", "Rocks", "G_rocks", "NoDLC", 1, 1, 1, "通常草原和马赛克区域的地上能找到不少，通过挖卵石也可以获得。")
    End Sub

    Private Sub button_G_gold_nugget_click(sender As Object, e As RoutedEventArgs) Handles button_G_gold_nugget.Click
        G_Science(32, {"S_goldenaxe", "S_luxury_machete", "S_goldenpickaxe", "S_goldenshovel", "S_saddle", "S_brush", "S_willow's_lighter", "S_miner_hat", "S_iron_wind", "S_spyglass", "S_super_spyglass", "S_compass", "S_ice_box", "S_science_machine", "S_electrical_doodad", "S_thermal_measurer", "S_rainometer", "S_lightning_rod", "S_accomploshrine", "S_battle_spear", "S_battle_helm", "S_cutlass_supreme", "S_birdcage", "S_scaled_chest", "S_one-man_band", "S_night_light", "S_life_giving_amulet", "S_chilled_amulet", "S_nightmare_amulet", "S_telelocator_focus", "S_shark_tooth_crown", "S_walking_cane"})
        G_Animal(1, {"A_dragonfly"})
        G_Show_M("金块", "Gold Nugget", "S_gold_nugget", "NoDLC", 1, 1, 1, "挖金卵石可以获得，有时候矿区地上也会有很多散落的金块。")
    End Sub

    Private Sub button_G_marble_click(sender As Object, e As RoutedEventArgs) Handles button_G_marble.Click
        G_Science(2, {"S_marble_suit", "S_checkered_flooring"})
        G_Animal(0, {})
        G_Show_M("大理石", "Marble", "G_marble", "NoDLC", 1, 0, 1, "挖大理石树和大理石柱子获得。")
    End Sub

    Private Sub button_G_moon_rock_click(sender As Object, e As RoutedEventArgs) Handles button_G_moon_rock.Click
        G_Science(1, {"S_moon_rock_wall"})
        G_Animal(0, {})
        G_Show_M("月之石", "Moon Rock", "G_moon_rock", "DST", 0, 0, 1, "仅联机版才有的月之石，通常可以在陨石雨区域发现。")
    End Sub

    Private Sub button_G_twigs_click(sender As Object, e As RoutedEventArgs) Handles button_G_twigs.Click
        G_Science(41, {"S_axe", "S_goldenaxe", "S_machete", "S_luxury_machete", "S_pickaxe", "S_goldenpickaxe", "S_shovel", "S_goldenshovel", "S_hammer", "S_pitchfork", "S_razor", "S_saddlehorn", "S_torch", "S_lantern_1", "S_boat_torch", "S_boat_lantern", "S_trap", "S_bird_trap", "S_bug_net", "S_fishing_rod", "S_tropical_parasol", "S_pretty_parasol", "S_umbrella", "S_backpack", "S_tent", "S_whirly_fan", "S_doydoy_nest", "S_drying_rack", "S_crock_pot", "S_divining_rod", "S_battle_spear", "S_spear", "S_ham_bat", "S_grass_suit", "S_cutlass_supreme", "S_hay_wall", "S_prime_ape_hut", "S_rabbit_earmuffs", "S_fashion_melon", "S_walking_cane", "S_eyebrella"})
        G_Animal(1, {"A_birchnutter"})
        G_Show_M("树枝", "Twigs", "G_twigs", "NoDLC", 1, 1, 1, "采集树苗、尖刺灌木或砍多枝的树、针叶树、红树林、洞穴香蕉树获得，用铲子挖没有灌溉的浆果灌木丛也会得到两个树枝，树枝是可以制作科技种类最多的材料。")
    End Sub

    Private Sub button_G_cut_grass_click(sender As Object, e As RoutedEventArgs) Handles button_G_cut_grass.Click
        G_Science(17, {"S_hammer", "S_campfire", "S_torch", "S_endothermic_fire", "S_lantern_1", "S_log_raft", "S_telltale_heart", "S_trap", "S_pretty_parasol", "S_backpack", "S_straw_roll", "S_basic_farm", "S_improved_farm", "S_grass_suit", "S_hay_wall", "S_straw_hat", "S_rope"})
        G_Animal(1, {"A_grass_gekko"})
        G_Show_M("割下的草", "Cut Grass", "G_cut_grass", "NoDLC", 1, 1, 1, "采集草或者靠近草壁虎获得。")
    End Sub

    Private Sub button_G_log_click(sender As Object, e As RoutedEventArgs) Handles button_G_log.Click
        G_Science(15, {"S_war_saddle", "S_campfire", "S_fire_pit", "S_chiminea", "S_obsidian_fire_pit", "S_log_raft", "S_snakeskin_sail", "S_boat_cannon", "S_basic_farm", "S_bucket-o-poop", "S_science_machine", "S_log_suit", "S_tooth_trap", "S_sewing_kit", "S_boards"})
        G_Animal(0, {})
        G_Show_M("木材", "Log", "G_log", "NoDLC", 1, 1, 1, "砍树砍树！")
    End Sub

    Private Sub button_G_charcoal_click(sender As Object, e As RoutedEventArgs) Handles button_G_charcoal.Click
        G_Science(5, {"S_drying_rack", "S_crock_pot", "S_gunpowder", "S_fire_dart", "S_boomerang"})
        G_Animal(1, {"A_krampus"})
        G_Show_M("木炭", "Charcoal", "G_charcoal", "NoDLC", 1, 1, 1, "烧树吧！")
    End Sub

    Private Sub button_G_cut_reeds_click(sender As Object, e As RoutedEventArgs) Handles button_G_cut_reeds.Click
        G_Science(9, {"S_luxury_fan", "S_tropical_fan", "S_sea_sack", "S_blow_dart", "S_sleep_dart", "S_fire_dart", "S_poison_dart", "S_pan_flute", "S_papyrus"})
        G_Animal(0, {})
        G_Show_M("割好的芦苇", "Cut Reeds", "G_cut_reeds", "NoDLC", 1, 1, 1, "只能去沼泽地里割芦苇了。")
    End Sub

    Private Sub button_G_petals_click(sender As Object, e As RoutedEventArgs) Handles button_G_petals.Click
        G_Science(6, {"S_willow's_lighter", "S_tropical_parasol", "S_pretty_parasol", "S_whirly_fan", "S_abigail's_flower", "S_garland"})
        G_Animal(0, {})
        G_Show_M("花瓣", "Petals", "G_petals", "NoDLC", 1, 1, 1, "一般草原和森林里比较多。")
    End Sub

    Private Sub button_G_dark_petals_click(sender As Object, e As RoutedEventArgs) Handles button_G_dark_petals.Click
        G_Science(1, {"S_nightmare_fuel"})
        G_Animal(0, {})
        G_Show_M("恶魔花瓣", "Dark Petals", "G_dark_petals", "NoDLC", 1, 1, 1, "完全正常的树、正常的丛林树周围会有不少，有的麦克斯韦雕像旁边也会有一些。")
    End Sub

    Private Sub button_G_boneshard_click(sender As Object, e As RoutedEventArgs) Handles button_G_boneshard.Click
        G_Science(10, {"S_saddlehorn", "S_lucky_hat", "S_captain_hat", "S_pirate_hat", "S_bucket-o-poop", "S_rain_hat", "S_snakeskin_hat", "S_snakeskin_jacket", "S_rain_coat", "S_eyebrella"})
        G_Animal(0, {})
        G_Show_M("骨片", "Boneshard", "G_boneshard", "NoDLC", 1, 1, 1, "用锤子敲骨架、猎犬尸骨获得，摧毁猎犬丘也会掉落。")
    End Sub

    Private Sub button_G_stinger_click(sender As Object, e As RoutedEventArgs) Handles button_G_stinger.Click
        G_Science(3, {"S_boat_repair_kit", "S_booster_shot", "S_sleep_dart"})
        G_Animal(2, {"A_bee", "A_killer_bee"})
        G_Show_M("蜂刺", "Stinger", "G_stinger", "NoDLC", 1, 1, 1, "击杀蜜蜂和杀人蜂都可以掉落。")
    End Sub

    Private Sub button_G_hounds_tooth_click(sender As Object, e As RoutedEventArgs) Handles button_G_hounds_tooth.Click
        G_Science(5, {"S_blow_dart", "S_blow_dart", "S_sewing_kit", "S_shark_tooth_crown", "S_dapper_vest"})
        G_Animal(5, {"A_hound", "A_red_hound", "A_blue_hound", "A_sea_hound", "A_varg"})
        G_Show_M("狗牙", "Hound's Tooth", "G_hound's_tooth", "NoDLC", 1, 1, 1, "击杀三种猎犬、海狗还有座狼可以获得。")
    End Sub

    Private Sub button_G_azure_feather_click(sender As Object, e As RoutedEventArgs) Handles button_G_azure_feather.Click
        G_Science(2, {"S_saddlehorn", "S_blow_dart"})
        G_Animal(2, {"A_snowbird", "A_seagull"})
        G_Show_M("蓝色羽毛", "Azure Feather", "G_azure_feather", "NoDLC", 1, 1, 1, "击杀雪鸟、海鸥获得。")
    End Sub

    Private Sub button_G_crimson_feather_click(sender As Object, e As RoutedEventArgs) Handles button_G_crimson_feather.Click
        G_Science(4, {"S_fire_dart", "S_feather_hat", "S_bush_hat", "S_summer_frest"})
        G_Animal(3, {"A_redbird", "A_parrot", "A_parrot_pirate"})
        G_Show_M("红色羽毛", "Crimson Feather", "G_crimson_feather", "NoDLC", 1, 1, 1, "击杀红雀、鹦鹉、海盗鹦鹉获得。")
    End Sub

    Private Sub button_G_jet_feather_click(sender As Object, e As RoutedEventArgs) Handles button_G_jet_feather.Click
        G_Science(4, {"S_saddlehorn", "S_sleep_dart", "S_poison_dart", "S_feather_hat"})
        G_Animal(4, {"A_pengull", "A_crow", "A_buzzards", "A_toucan"})
        G_Show_M("黑色羽毛", "Jet Feather", "G_jet_feather", "NoDLC", 1, 1, 1, "击杀企鹅、乌鸦、秃鹰、大嘴鸟获得。")
    End Sub

    Private Sub button_G_pig_skin_click(sender As Object, e As RoutedEventArgs) Handles button_G_pig_skin.Click
        G_Science(11, {"S_saddle", "S_spyglass", "S_umbrella", "S_piggyback", "S_ham_bat", "S_football_helmet", "S_scalemail", "S_pig_house", "S_wildbore_house", "S_one-man_band", "S_summer_frest"})
        G_Animal(4, {"A_pig_man", "A_guard_pig", "A_werepig", "A_wildbore"})
        G_Show_M("猪皮", "Pig Skin", "G_pig_skin", "NoDLC", 1, 1, 1, "击杀各种猪或摧毁猪舍和猪人头获得。", True, False)
    End Sub

    Private Sub button_G_beefalo_wool_click(sender As Object, e As RoutedEventArgs) Handles button_G_beefalo_wool.Click
        G_Science(6, {"S_saddle", "S_bernie_1", "S_carpeted_flooring", "S_beefalo_hat", "S_winter_hat", "S_puffy_vest"})
        G_Animal(4, {"A_baby_beefalo", "A_baby_beefalo_2", "A_baby_beefalo_3", "A_beefalo"})
        G_Show_M("牛毛", "Beefalo Wool", "G_beefalo_wool", "NoDLC", 1, 1, 1, "击杀皮弗娄牛获得。")
    End Sub

    Private Sub button_G_beefalo_horn_click(sender As Object, e As RoutedEventArgs) Handles button_G_beefalo_horn.Click
        G_Science(1, {"S_beefalo_hat"})
        G_Animal(1, {"A_beefalo"})
        G_Show_M("牛角", "Beefalo Horn", "G_beefalo_horn", "NoDLC", 1, 0, 1, "击杀皮弗娄牛获得，做牛帽的必备材料。使用牛角会使至多5头皮弗娄牛跟随玩家，小皮弗娄牛会跟随父母，晚上皮弗娄牛会睡觉，皮弗娄牛最后会停留在最终位置附近活动。使用牛角还可以使发怒的皮弗娄牛冷静下来。")
    End Sub

    Private Sub button_G_manure_click(sender As Object, e As RoutedEventArgs) Handles button_G_manure.Click
        G_Science(6, {"S_doydoy_nest", "S_basic_farm", "S_improved_farm", "S_bucket-o-poop", "S_prime_ape_hut", "S_applied_horticulture"})
        G_Animal(5, {"A_beefalo", "A_koalefant", "A_winter_koalefant", "A_prime_ape", "A_water_beefalo"})
        G_Show_M("便便", "Manure", "G_manure", "NoDLC", 1, 1, 1, "皮弗娄牛、考拉象、冬考拉象、水牛都会周期性地产生便便，猿猴会抛出便便。便便可以用来浇灌植物，不过最好还是做成便便篮提高利用效率。")
    End Sub

    Private Sub button_G_guano_click(sender As Object, e As RoutedEventArgs) Handles button_G_guano.Click
        G_Science(0, {})
        G_Animal(1, {"A_batilisk"})
        G_Show_M("鸟屎", "Guano", "G_guano", "NoDLC", 1, 0, 1, "击杀黑蝙蝠获得(黑蝙蝠活动范围内也会产生)，联机版里喂食被囚禁的鸟种子也会得到，可以用来浇灌植物。")
    End Sub

    Private Sub button_G_coontail_click(sender As Object, e As RoutedEventArgs) Handles button_G_coontail.Click
        G_Science(2, {"S_tail_o'_three_cats", "S_cat_cap"})
        G_Animal(1, {"A_catcoon"})
        G_Show_M("猫尾", "Coontail", "G_coontail", "NoDLC", 1, 1, 1, "击杀猫熊获得。")
    End Sub

    Private Sub button_G_fleshy_bulb_click(sender As Object, e As RoutedEventArgs) Handles button_G_fleshy_bulb.Click
        G_Science(0, {})
        G_Animal(1, {"A_lureplants"})
        G_Show_M("多肉的球茎", "Fleshy Bulb", "G_fleshy_bulb", "NoDLC", 1, 1, 1, "击杀食人花获得，可以种植在需要的地方。")
    End Sub

    Private Sub button_G_silk_click(sender As Object, e As RoutedEventArgs) Handles button_G_silk.Click
        G_Science(21, {"S_glossamer_saddle", "S_pirate_hat", "S_bernie_1", "S_bird_trap", "S_bug_net", "S_fishing_rod", "S_umbrella", "S_piggyback", "S_tent", "S_siesta_lean-to", "S_boomerang", "S_spider_eggs", "S_sewing_kit", "S_top_hat", "S_winter_hat", "S_cat_cap", "S_beekeeper_hat", "S_dapper_vest", "S_breezy_vest", "S_puffy_vest", "S_floral_shirt"})
        G_Animal(7, {"A_spider", "A_spider_warrior", "A_spider_warrior_sw", "A_cave_spider", "A_spitter", "A_dangling_depth_dweller", "A_spider_queen"})
        G_Show_M("蜘蛛网", "Silk", "G_silk", "NoDLC", 1, 1, 1, "击杀所有蜘蛛都可以获得，摧毁蜘蛛巢也会掉落。")
    End Sub

    Private Sub button_G_spider_gland_click(sender As Object, e As RoutedEventArgs) Handles button_G_spider_gland.Click
        G_Science(3, {"S_telltale_heart", "S_healing_salve", "S_spider_eggs"})
        G_Animal(5, {"A_spider", "A_spider_warrior", "A_cave_spider", "A_spitter", "A_dangling_depth_dweller"})
        G_Show_M("蜘蛛腺体", "Spider Gland", "G_spider_gland", "NoDLC", 1, 1, 1, "击杀部分蜘蛛可以得到。")
    End Sub

    Private Sub button_G_gears_click(sender As Object, e As RoutedEventArgs) Handles button_G_gears.Click
        G_Science(6, {"S_insulated_pack", "S_ice_box", "S_divining_rod", "S_ice_flingomatic", "S_accomploshrine", "S_weather_pain"})
        G_Animal(6, {"A_clockwork_knight", "A_clockwork_bishop", "A_clckwork_rook", "A_floaty_boaty_knight", "A_damaged_knight", "A_damage_rook"})
        G_Show_M("齿轮", "Gears", "G_gears", "NoDLC", 1, 1, 1, "击杀发条生物得到，在棋盘区域有时候也会有几个齿轮，实在找不到还可以开风滚草。")
    End Sub

    Private Sub button_G_glommers_flower_click(sender As Object, e As RoutedEventArgs) Handles button_G_glommers_flower.Click
        G_Science(1, {"S_old_bell"})
        G_Animal(0, {})
        G_Show_M("格洛姆的花", "Glommer's Flower", "G_glommer's_flower", "NoDLC", 1, 0, 1, "月圆之夜在猪王附近的格洛姆的雕像下可以找到格洛姆之花，带着它格洛姆就会跟随玩家，同时也是制作旧钟的材料之一。")
    End Sub

    Private Sub button_G_glommers_wings_click(sender As Object, e As RoutedEventArgs) Handles button_G_glommers_wings.Click
        G_Science(1, {"S_old_bell"})
        G_Animal(1, {"A_glommer"})
        G_Show_M("格洛姆的翅膀", "Glommer's Wings", "G_glommer's_wings", "NoDLC", 1, 0, 1, "击杀格洛姆获得，制作旧钟的材料之一。")
    End Sub

    Private Sub button_G_tentacle_spots_click(sender As Object, e As RoutedEventArgs) Handles button_G_tentacle_spots.Click
        G_Science(4, {"S_tail_o'_three_cats", "S_feather_hat", "S_rain_coat", "S_on_tentacles"})
        G_Animal(3, {"A_tentacle", "A_big_tentacle", "A_quacken_tentacle"})
        G_Show_M("斑点触手皮", "Tentacle Spots", "G_tentacle_spots", "NoDLC", 1, 1, 1, "击杀触手、大触手、呱肯乌贼的触手获得。")
    End Sub

    Private Sub button_G_nightmare_fuel_click(sender As Object, e As RoutedEventArgs) Handles button_G_nightmare_fuel.Click
        G_Science(32, {"S_divining_rod", "S_abigail's_flower", "S_shadow_manipulator", "S_dripple_pipes", "S_one-man_band", "S_night_light", "S_night_armour", "S_dark_sword", "S_belt_of_hunger", "S_life_giving_amulet", "S_nightmare_amulet", "S_fire_staff", "S_telelocator_staff", "S_telelocator_focus", "S_Seaworthy", "S_thulecite_medallion", "S_the_lazy_forager", "S_magiluminescence", "S_construction_amulet", "S_the_lazy_explorer", "S_star_caller's_staff", "S_deconstruction_staff", "S_thulecite_crown", "S_thulecite_suit", "S_thulecite_club", "S_sleepytime_stories", "S_codex_umbra", "S_shadow_logger", "S_shadow_miner", "S_shadow_digger", "S_shadow_duelist", "S_sail_stick"})
        G_Animal(11, {"A_beardling", "A_beardling_sw", "A_splumonkey", "A_shadow_splumonkey", "A_damaged_knight", "A_damaged_bishop", "A_damage_rook", "A_crawling_nightmare", "A_crawling_horror", "A_swimming_horror", "A_poison_birchnut_trees"})
        G_Show_M("噩梦燃料", "Nightmare Fuel", "G_nightmare_fuel", "NoDLC", 1, 1, 1, "击杀黑化生物、各种影怪、损坏的发条生物和暴躁猴获得，也可以用恶魔花瓣合成，魔法科技的重要材料。")
    End Sub

    Private Sub button_G_living_log_click(sender As Object, e As RoutedEventArgs) Handles button_G_living_log.Click
        G_Science(10, {"S_glossamer_saddle", "S_shadow_manipulator", "S_dark_sword", "S_bat_bat", "S_telelocator_staff", "S_telelocator_focus", "S_Seaworthy", "S_star_caller's_staff", "S_deconstruction_staff", "S_thulecite_club"})
        G_Animal(5, {"A_treeguard_1", "A_treeguard_2", "A_treeguard_3", "A_poison_birchnut_trees", "A_palm_treeguard"})
        G_Show_M("活木", "Living Log", "G_living_log", "NoDLC", 1, 1, 1, "击杀树精守卫、桦树精、椰树守卫获得，砍倒完全正常的树、正常的丛林树也可以得到。")
    End Sub

    Private Sub button_G_mosquito_sack_click(sender As Object, e As RoutedEventArgs) Handles button_G_mosquito_sack.Click
        G_Science(1, {"S_waterballoon"})
        G_Animal(1, {"A_mosquito"})
        G_Show_M("蚊子血袋", "Mosquito Sack", "G_mosquito_sack", "NoDLC", 1, 0, 1, "击杀蚊子获得，可以做水球。")
    End Sub

    Private Sub button_G_volt_goat_horn_click(sender As Object, e As RoutedEventArgs) Handles button_G_volt_goat_horn.Click
        G_Science(2, {"S_morning_star", "S_weather_pain"})
        G_Animal(2, {"A_volt_goat", "A_volt_goat_withelectric"})
        G_Show_M("电羊角", "Volt Goat Horn", "G_volt_goat_horn", "NoDLC", 1, 0, 1, "击杀电羊获得。")
    End Sub

    Private Sub button_G_walrus_tusk_click(sender As Object, e As RoutedEventArgs) Handles button_G_walrus_tusk.Click
        G_Science(2, {"S_brush", "S_walking_cane"})
        G_Animal(1, {"A_mactusk"})
        G_Show_M("海象牙", "Walrus Tusk", "G_walrus_tusk", "NoDLC", 1, 0, 1, "击杀海象获得。")
    End Sub

    Private Sub button_G_steel_wool_click(sender As Object, e As RoutedEventArgs) Handles button_G_steel_wool.Click
        G_Science(2, {"S_war_saddle", "S_brush"})
        G_Animal(1, {"A_ewecus"})
        G_Show_M("钢丝绒", "Steel Wool", "G_steel_wool", "DST", 0, 0, 1, "击杀钢羊获得。")
    End Sub

    Private Sub button_G_phlegm_click(sender As Object, e As RoutedEventArgs) Handles button_G_phlegm.Click
        G_Science(0, {})
        G_Animal(1, {"A_ewecus"})
        G_Show_M("痰", "Phlegm", "F_phlegm", "DST", 0, 0, 1, "击杀钢羊获得，食用后回复少量饥饿而损失少量精神。")
    End Sub

    Private Sub button_G_red_gem_click(sender As Object, e As RoutedEventArgs) Handles button_G_red_gem.Click
        G_Science(5, {"S_night_light", "S_life_giving_amulet", "S_fire_staff", "S_the_end_is_nigh!", "S_purple_gem"})
        G_Animal(2, {"A_red_hound", "A_dragonfly"})
        G_Show_M("红宝石", "Red Gem", "G_red_gem", "NoDLC", 1, 1, 1, "击杀红色猎犬、龙蝇，挖坟墓、熔岩金矿、沙堆，敲古代雕像、石笋、破碎的时钟、远古遗迹、沉船获得，大豪华箱子里也有可能出现。")
    End Sub

    Private Sub button_G_blue_gem_click(sender As Object, e As RoutedEventArgs) Handles button_G_blue_gem.Click
        G_Science(3, {"S_chilled_amulet", "S_ice_staff", "S_purple_gem"})
        G_Animal(2, {"A_blue_hound", "A_dragonfly"})
        G_Show_M("蓝宝石", "Blue Gem", "G_blue_gem", "NoDLC", 1, 1, 1, "击杀蓝色猎犬、龙蝇，挖坟墓、熔岩金矿，敲古代雕像、石笋、破碎的时钟、远古遗迹、沉船获得，大豪华箱子里也有可能出现。")
    End Sub

    Private Sub button_G_purple_gem_click(sender As Object, e As RoutedEventArgs) Handles button_G_purple_gem.Click
        G_Science(5, {"S_bat_bat", "S_shadow_manipulator", "S_nightmare_amulet", "S_telelocator_staff", "S_howling_conch"})
        G_Animal(3, {"A_clockwork_bishop", "A_damaged_bishop", "A_dragonfly"})
        G_Show_M("紫宝石", "Purple Gem", "G_purple_gem", "NoDLC", 1, 1, 1, "击杀发条骑士、发条主教、龙蝇，挖沙堆，敲金色古代雕像、破碎的时钟、远古遗迹获得，大豪华箱子里也有可能出现，还可以用红宝石和蓝宝石合成。")
    End Sub

    Private Sub button_G_orange_gem_click(sender As Object, e As RoutedEventArgs) Handles button_G_orange_gem.Click
        G_Science(2, {"S_the_lazy_forager", "S_the_lazy_explorer"})
        G_Animal(1, {"A_dragonfly"})
        G_Show_M("橙宝石", "Orange Gem", "G_orange_gem", "NoDLC", 1, 1, 1, "击杀龙蝇，敲金色古代雕像、破碎的时钟、远古遗迹、废墟获得，大豪华箱子里也有可能出现。")
    End Sub

    Private Sub button_G_green_gem_click(sender As Object, e As RoutedEventArgs) Handles button_G_green_gem.Click
        G_Science(2, {"S_construction_amulet", "S_deconstruction_staff"})
        G_Animal(1, {"A_dragonfly"})
        G_Show_M("绿宝石", "Green Gem", "G_green_gem", "NoDLC", 1, 1, 1, "击杀龙蝇，敲金色古代雕像、破碎的时钟、远古遗迹、废墟获得，大豪华箱子里也有可能出现。")
    End Sub

    Private Sub button_G_yellow_gem_click(sender As Object, e As RoutedEventArgs) Handles button_G_yellow_gem.Click
        G_Science(2, {"S_magiluminescence", "S_star_caller's_staff"})
        G_Animal(1, {"A_dragonfly"})
        G_Show_M("黄宝石", "Yellow Gem", "G_yellow_gem", "NoDLC", 1, 1, 1, "击杀龙蝇，敲金色古代雕像、破碎的时钟、远古遗迹、废墟获得，大豪华箱子里也有可能出现。")
    End Sub

    Private Sub button_G_light_bulb_click(sender As Object, e As RoutedEventArgs) Handles button_G_light_bulb.Click
        G_Science(1, {"S_lantern_1"})
        G_Animal(2, {"A_slurper", "A_big_tentacle"})
        G_Show_M("荧光果", "Light Bulb", "G_light_bulb", "NoDLC", 1, 0, 1, "采集荧光草获得。")
    End Sub

    Private Sub button_G_bunny_puff_click(sender As Object, e As RoutedEventArgs) Handles button_G_bunny_puff.Click
        G_Science(2, {"S_fur_roll", "S_rabbit_hutch"})
        G_Animal(1, {"A_bunnyman"})
        G_Show_M("兔毛", "Bunny Puff", "G_bunny_puff", "NoDLC", 1, 0, 1, "击杀兔人或拆兔子窝获得。", True, False)
    End Sub

    Private Sub button_G_foliage_click(sender As Object, e As RoutedEventArgs) Handles button_G_foliage.Click
        G_Science(1, {"S_potted_fern"})
        G_Animal(0, {})
        G_Show_M("蕨叶", "Foliage", "G_foliage", "NoDLC", 1, 0, 1, "采集蕨类植物获得。")
    End Sub

    Private Sub button_G_broken_shell_click(sender As Object, e As RoutedEventArgs) Handles button_G_broken_shell.Click
        G_Science(1, {"S_potted_fern"})
        G_Animal(2, {"A_slurtles", "A_snurtles"})
        G_Show_M("破碎的背壳", "Broken Shell", "G_broken_shell", "NoDLC", 1, 0, 1, "击杀黏糊虫和含糊虫获得。")
    End Sub

    Private Sub button_G_slurtle_slime_click(sender As Object, e As RoutedEventArgs) Handles button_G_slurtle_slime.Click
        G_Science(0, {})
        G_Animal(3, {"A_slurtles", "A_snurtles", "A_big_tentacle"})
        G_Show_M("粘滑含糊虫", "Slurtle Slime", "G_slurtle_slime", "NoDLC", 1, 0, 1, "击杀黏糊虫、含糊虫和大触手获得，点燃后几秒爆炸。")
    End Sub

    Private Sub button_G_beard_hair_click(sender As Object, e As RoutedEventArgs) Handles button_G_beard_hair.Click
        G_Science(2, {"S_bernie_1", "S_meat_effigy"})
        G_Animal(4, {"A_beardling", "A_beardling_sw", "A_beardlord", "A_shadow_splumonkey"})
        G_Show_M("胡须", "Beard Hair", "G_beard_hair", "NoDLC", 1, 0, 1, "击杀黑化生物获得，威尔逊用剃刀刮胡子也可以得到，制作肉块雕像的材料之一。")
    End Sub

    Private Sub button_G_slurper_pelt_click(sender As Object, e As RoutedEventArgs) Handles button_G_slurper_pelt.Click
        G_Science(1, {"S_belt_of_hunger"})
        G_Animal(1, {"A_slurper"})
        G_Show_M("缀食者之皮", "Slurper Pelt", "G_slurper_pelt", "NoDLC", 1, 0, 1, "击杀缀食者获得。", True, False)
    End Sub

    Private Sub button_G_thulecite_fragments_click(sender As Object, e As RoutedEventArgs) Handles button_G_thulecite_fragments.Click
        G_Science(1, {"S_thulecite"})
        G_Animal(3, {"A_damaged_knight", "A_damaged_bishop", "A_damage_rook"})
        G_Show_M("铥矿石碎片", "Thulecite Fragments", "G_thulecite_fragments", "NoDLC", 1, 0, 1, "击杀损坏的发条生物，敲二层洞穴入口和铥矿墙壁获得，大豪华箱子里也有可能出现。")
    End Sub

    Private Sub button_G_down_feather_click(sender As Object, e As RoutedEventArgs) Handles button_G_down_feather.Click
        G_Science(2, {"S_luxury_fan", "S_weather_pain"})
        G_Animal(2, {"A_moose", "A_mosling"})
        G_Show_M("掉落的羽毛", "Down Feather", "G_down_feather", "NoDLC", 1, 0, 1, "击杀鹿角鹅和莫斯林获得。")
    End Sub

    Private Sub button_G_scales_click(sender As Object, e As RoutedEventArgs) Handles button_G_scales.Click
        G_Science(3, {"S_scalemail", "S_scaled_flooring", "S_scaled_chest"})
        G_Animal(1, {"A_dragonfly"})
        G_Show_M("鳞片", "Scales", "G_scales", "NoDLC", 1, 0, 1, "击杀龙蝇获得。")
    End Sub

    Private Sub button_G_fur_tuft_click(sender As Object, e As RoutedEventArgs) Handles button_G_fur_tuft.Click
        G_Science(1, {"S_thick_fur"})
        G_Animal(0, {})
        G_Show_M("毛簇", "Fur Tuft", "G_fur_tuft", "DST", 0, 0, 1, "仅联机版在熊獾走路的时候会掉落。")
    End Sub

    Private Sub button_G_thick_fur_click(sender As Object, e As RoutedEventArgs) Handles button_G_thick_fur.Click
        G_Science(2, {"S_insulated_pack", "S_hibearnation_vest"})
        G_Animal(1, {"A_bearger"})
        G_Show_M("厚皮毛", "Thick Fur", "S_thick_fur", "NoDLC", 1, 0, 1, "击杀熊獾获得。")
    End Sub

    Private Sub button_G_bamboo_patch_click(sender As Object, e As RoutedEventArgs) Handles button_G_bamboo_patch.Click
        G_Science(12, {"S_raft", "S_thatch_sail", "S_cloth_sail", "S_feather_lite_sail", "S_trawl_net", "S_buoy", "S_palm_leaf_hut", "S_mussel_stick", "S_ice_maker_3000", "S_spear_gun", "S_wildbore_house", "S_cloth"})
        G_Animal(0, {})
        G_Show_M("竹子", "Bamboo Patch", "G_bamboo_patch", "SW", 0, 1, 0, "砍竹子获得。")
    End Sub

    Private Sub button_G_cut_grass_SW_click(sender As Object, e As RoutedEventArgs) Handles button_G_cut_grass_SW.Click
        G_Science(17, {"S_hammer", "S_campfire", "S_torch", "S_endothermic_fire", "S_lantern_1", "S_log_raft", "S_telltale_heart", "S_trap", "S_pretty_parasol", "S_backpack", "S_straw_roll", "S_basic_farm", "S_improved_farm", "S_grass_suit", "S_hay_wall", "S_straw_hat", "S_rope"})
        G_Animal(0, {})
        G_Show_M("干草", "Cut Grass(SW)", "G_cut_grass_SW", "SW", 0, 1, 0, "采集草获得。")
    End Sub

    Private Sub button_G_vine_click(sender As Object, e As RoutedEventArgs) Handles button_G_vine.Click
        G_Science(8, {"S_raft", "S_row_boat", "S_thatch_sail", "S_life_jacket", "S_sea_sack", "S_mussel_stick", "S_snakeskin_jacket", "S_sleek_hat"})
        G_Animal(0, {})
        G_Show_M("藤蔓", "Vine", "G_vine", "SW", 0, 1, 0, "砍藤蔓从获得。")
    End Sub

    Private Sub button_G_sand_click(sender As Object, e As RoutedEventArgs) Handles button_G_sand.Click
        G_Science(4, {"S_chiminea", "S_sandbag", "S_sand_castle", "S_empty_bottle"})
        G_Animal(0, {})
        G_Show_M("沙子", "Sand", "G_sand", "SW", 0, 1, 0, "挖沙堆获得。")
    End Sub

    Private Sub button_G_snakeskin_click(sender As Object, e As RoutedEventArgs) Handles button_G_snakeskin.Click
        G_Science(5, {"S_snakeskin_sail", "S_silly_monkey_ball", "S_snakeskin_rug", "S_snakeskin_hat", "S_snakeskin_jacket"})
        G_Animal(2, {"A_snake", "A_poison_snake"})
        G_Show_M("蛇皮", "Snakeskin", "G_snakeskin", "SW", 0, 1, 0, "击杀蛇和毒蛇获得。")
    End Sub

    Private Sub button_G_snake_oil_click(sender As Object, e As RoutedEventArgs) Handles button_G_snake_oil.Click
        G_Science(0, {})
        G_Animal(2, {"A_snake", "A_poison_snake"})
        G_Show_M("蛇油", "Snake Oil", "G_snake_oil", "SW", 0, 1, 0, "击杀蛇和毒蛇获得。")
    End Sub

    Private Sub button_G_palm_leaf_click(sender As Object, e As RoutedEventArgs) Handles button_G_palm_leaf.Click
        G_Science(8, {"S_thatch_sail", "S_sea_trap", "S_tropical_parasol", "S_thatch_pack", "S_palm_leaf_hut", "S_wildbore_house", "S_sand_castle", "S_blubber_suit"})
        G_Animal(0, {})
        G_Show_M("椰树叶", "Palm Leaf", "G_palm_leaf", "SW", 0, 1, 0, "砍倒椰子树获得。")
    End Sub

    Private Sub button_G_venom_gland_click(sender As Object, e As RoutedEventArgs) Handles button_G_venom_gland.Click
        G_Science(3, {"S_anti_venom", "S_poison_spear", "S_poison_dart"})
        G_Animal(4, {"A_spider_warrior_sw", "A_poison_snake", "A_stink_ray", "A_mosquito_sw"})
        G_Show_M("毒蛇腺体", "Venom Gland", "G_venom_gland", "SW", 0, 1, 0, "击杀毒蜘蛛、毒蛇、恶臭魔鬼鱼和毒蚊子获得。")
    End Sub

    Private Sub button_G_yellow_mosquito_sack_click(sender As Object, e As RoutedEventArgs) Handles button_G_yellow_mosquito_sack.Click
        G_Science(0, {})
        G_Animal(1, {"A_mosquito_sw"})
        G_Show_M("黄色蚊子血袋", "Yellow Mosquito Sack", "G_yellow_mosquito_sack", "SW", 0, 1, 0, "击杀毒蚊子获得。")
    End Sub

    Private Sub button_G_seashell_click(sender As Object, e As RoutedEventArgs) Handles button_G_seashell.Click
        G_Science(5, {"S_surfboard", "S_armoured_boat", "S_seashell_suit", "S_horned_helmet", "S_sand_castle"})
        G_Animal(0, {})
        G_Show_M("贝壳", "Seashell", "G_seashell", "SW", 0, 1, 0, "挖沙堆有几率获得，有时候海滩也可以捡到。", False, True)
    End Sub

    Private Sub button_G_doydoy_feather_click(sender As Object, e As RoutedEventArgs) Handles button_G_doydoy_feather.Click
        G_Science(3, {"S_feather_lite_sail", "S_tropical_fan", "S_doydoy_nest"})
        G_Animal(3, {"A_baby_doydoy", "A_doydoy_child", "A_doydoy"})
        G_Show_M("渡渡鸟的羽毛", "Doydoy Feather", "G_doydoy_feather", "SW", 0, 1, 0, "击杀渡渡鸟获得。")
    End Sub

    Private Sub button_G_dubloons_click(sender As Object, e As RoutedEventArgs) Handles button_G_dubloons.Click
        G_Science(3, {"S_lucky_hat", "S_the_'sea_legs'", "S_gold_nugget"})
        G_Animal(0, {})
        G_Show_M("金币", "Dubloons", "G_dubloons", "SW", 0, 1, 0, "海盗鹦鹉会掉落金币，挖沙堆也可能会挖出金币。金币是船难版里另一种获得金块的方式。除了制作科技物品，还可以玩老虎机。", False, True)
    End Sub

    Private Sub button_G_hail_click(sender As Object, e As RoutedEventArgs) Handles button_G_hail.Click
        G_Science(1, {"S_ice"})
        G_Animal(0, {})
        G_Show_M("冰雹", "Hail", "G_hail", "SW", 0, 1, 0, "船难版里合成冰块的材料，在飓风季节伴随着风暴出现。有一天的保质期，放在冰箱里不会融化，靠近火融化得更快。可以用来熄灭火。")
    End Sub

    Private Sub button_G_horn_click(sender As Object, e As RoutedEventArgs) Handles button_G_horn.Click
        G_Science(2, {"S_horned_helmet", "S_dripple_pipes"})
        G_Animal(1, {"A_water_beefalo"})
        G_Show_M("角", "Horn", "G_horn", "SW", 0, 1, 0, "击杀水牛获得。")
    End Sub

    Private Sub button_G_coral_click(sender As Object, e As RoutedEventArgs) Handles button_G_coral.Click
        G_Science(3, {"S_anti_venom", "S_particulate_purifier", "S_limestone"})
        G_Animal(0, {})
        G_Show_M("珊瑚", "Coral", "G_coral", "SW", 0, 1, 0, "挖珊瑚礁或在海洋用拖网获得，也可以用钓竿或拖网在潮湿的坟墓处获得。是制作石灰石的重要材料。", False, True)
    End Sub

    Private Sub button_G_obsidian_click(sender As Object, e As RoutedEventArgs) Handles button_G_obsidian.Click
        G_Science(11, {"S_obsidian_fire_pit", "S_dragoon_den", "S_joy_of_volcanology", "S_obsidian_machete", "S_obsidian_axe", "S_obsidian_spear", "S_volcano_staff", "S_obsidian_armour", "S_obsidian_coconade", "S_howling_conch", "S_sail_stick"})
        G_Animal(0, {})
        G_Show_M("黑曜石", "Obsidian", "G_obsidian", "SW", 0, 1, 0, "熄灭熔岩池或炸毁黑曜岩获得。制作火山科技的重要材料。")
    End Sub

    Private Sub button_G_cactus_spike_click(sender As Object, e As RoutedEventArgs) Handles button_G_cactus_spike.Click
        G_Science(1, {"S_cactus_armour"})
        G_Animal(1, {"A_elephant_cactus"})
        G_Show_M("象仙人掌刺", "Cactus Spike", "G_cactus_spike", "SW", 0, 1, 0, "击杀刺人的象仙人掌获得。")
    End Sub

    Private Sub button_G_dragoon_heart_click(sender As Object, e As RoutedEventArgs) Handles button_G_dragoon_heart.Click
        G_Science(7, {"S_dragoon_den", "S_obsidian_machete", "S_obsidian_axe", "S_obsidian_spear", "S_volcano_staff", "S_obsidian_armour", "S_obsidian_coconade"})
        G_Animal(1, {"A_dragoon"})
        G_Show_M("龙人心", "Dragoon Heart", "G_dragoon_heart", "SW", 0, 1, 0, "击杀龙骑士获得。制作火山科技的重要材料。")
    End Sub

    Private Sub button_G_turbine_blades_click(sender As Object, e As RoutedEventArgs) Handles button_G_turbine_blades.Click
        G_Science(1, {"S_iron_wind"})
        G_Animal(1, {"A_sealnado"})
        G_Show_M("涡轮叶片", "Turbine Blades", "G_turbine_blades", "SW", 0, 1, 0, "击杀豹卷风获得，可以制作铁风牌发动机。")
    End Sub

    Private Sub button_G_magic_seal_click(sender As Object, e As RoutedEventArgs) Handles button_G_magic_seal.Click
        G_Science(2, {"S_howling_conch", "S_sail_stick"})
        G_Animal(1, {"A_seal"})
        G_Show_M("豹印", "Magic Seal", "G_magic_seal", "SW", 0, 1, 0, "击杀豹卷风后产生的海豹处获得，可以用来制作呼啸的海螺和帆棍。")
    End Sub

    Private Sub button_G_shark_gills_click(sender As Object, e As RoutedEventArgs) Handles button_G_shark_gills.Click
        G_Science(2, {"S_sea_sack", "S_dumbrella"})
        G_Animal(2, {"A_tiger_shark", "A_sharkitten"})
        G_Show_M("鲨鱼腮", "Shark Gills", "G_shark_gills", "SW", 0, 1, 0, "击杀虎鲨和小虎鲨获得，用来制作海上麻袋和双层伞帽。")
    End Sub

    Private Sub button_G_air_unfreshener_click(sender As Object, e As RoutedEventArgs) Handles button_G_air_unfreshener.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("空气""清新""剂", "Air Unfreshener", "T_air_unfreshener", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_back_scratcher_click(sender As Object, e As RoutedEventArgs) Handles button_G_back_scratcher.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("挠痒器", "Back Scratcher", "T_back_scratcher", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_ball_and_cup_click(sender As Object, e As RoutedEventArgs) Handles button_G_ball_and_cup.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("剑玉", "Ball and Cup", "T_ball_and_cup", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_beaten_beater_click(sender As Object, e As RoutedEventArgs) Handles button_G_beaten_beater.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("敲打锤", "Beaten beater", "T_beaten_beater", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_bent_spork_click(sender As Object, e As RoutedEventArgs) Handles button_G_bent_spork.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("弯曲的叉勺", "Bent Spork", "T_bent_spork", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_black_bishop_click(sender As Object, e As RoutedEventArgs) Handles button_G_black_bishop.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("黑主教", "Black Bishop", "T_black_bishop", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_dessicated_tentacle_click(sender As Object, e As RoutedEventArgs) Handles button_G_dessicated_tentacle.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("干瘪触手", "Dessicated Tentacle", "T_dessicated_tentacle", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_fake_kazoo_click(sender As Object, e As RoutedEventArgs) Handles button_G_fake_kazoo.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("假卡祖笛", "Fake Kazoo", "T_fake_kazoo", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_frayed_yarn_click(sender As Object, e As RoutedEventArgs) Handles button_G_frayed_yarn.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("不耐磨的毛线", "Frayed Yarn", "T_frayed_yarn", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_frazzled_wires_click(sender As Object, e As RoutedEventArgs) Handles button_G_frazzled_wires.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("破烂电线", "Frazzled Wires", "T_frazzled_wires", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_gnome_1_click(sender As Object, e As RoutedEventArgs) Handles button_G_gnome_1.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("地精玩偶", "Gnome", "T_gnome_1", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_gnome_2_click(sender As Object, e As RoutedEventArgs) Handles button_G_gnome_2.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("地精玩偶", "Gnome", "T_gnome_2", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_gords_knot_click(sender As Object, e As RoutedEventArgs) Handles button_G_gords_knot.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("歌德结", "Gord's Knot", "T_gord's_knot", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_hardened_rubber_bung_click(sender As Object, e As RoutedEventArgs) Handles button_G_hardened_rubber_bung.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("硬化橡胶塞", "Hardened Rubber Bung", "T_hardened_rubber_bung", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_leaky_teacup_click(sender As Object, e As RoutedEventArgs) Handles button_G_leaky_teacup.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("漏水的茶杯", "Leaky Teacup", "T_leaky_teacup", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_lucky_cat_jar_click(sender As Object, e As RoutedEventArgs) Handles button_G_lucky_cat_jar.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("幸运猫罐", "Lucky Cat Jar", "T_lucky_cat_jar", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_lying_robot_click(sender As Object, e As RoutedEventArgs) Handles button_G_lying_robot.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("半躺机器人", "Lying Robot", "T_lying_robot", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_melty_marbles_click(sender As Object, e As RoutedEventArgs) Handles button_G_melty_marbles.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("融化的大理石", "Melty Marbles", "T_melty_marbles", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_mismatched_buttons_click(sender As Object, e As RoutedEventArgs) Handles button_G_mismatched_buttons.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("不匹配的纽扣", "Mismatched Buttons", "T_mismatched_buttons", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_potato_cup_click(sender As Object, e As RoutedEventArgs) Handles button_G_potato_cup.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("土豆杯", "Potato Cup", "T_potato_cup", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_second_hand_dentures_click(sender As Object, e As RoutedEventArgs) Handles button_G_second_hand_dentures.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("二手假牙", "Second Hand Dentures", "T_second_hand_dentures", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_shoe_horn_click(sender As Object, e As RoutedEventArgs) Handles button_G_shoe_horn.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("鞋拔", "Shoe Horn", "T_shoe_horn", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_tiny_rocketship_click(sender As Object, e As RoutedEventArgs) Handles button_G_tiny_rocketship.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("小型火箭飞船", "Tiny Rocketship", "T_tiny_rocketship", "NoDLC", 1, 0, 1, "", True, True)
    End Sub

    Private Sub button_G_toy_trojan_horse_click(sender As Object, e As RoutedEventArgs) Handles button_G_toy_trojan_horse.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("玩具木马", "Toy Trojan Horse", "T_toy_trojan_horse", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_unbalanced_top_click(sender As Object, e As RoutedEventArgs) Handles button_G_unbalanced_top.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("陀螺", "Unbalanced Top", "T_unbalanced_top", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_white_bishop_click(sender As Object, e As RoutedEventArgs) Handles button_G_white_bishop.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("白主教", "White Bishop", "T_white_bishop", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_wire_hanger_click(sender As Object, e As RoutedEventArgs) Handles button_G_wire_hanger.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("铁丝衣架", "Wire Hanger", "T_wire_hanger", "DST", 0, 0, 1, "", True, False)
    End Sub

    Private Sub button_G_ancient_vase_click(sender As Object, e As RoutedEventArgs) Handles button_G_ancient_vase.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("古老的花瓶", "Ancient Vase", "T_ancient_vase", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_brain_cloud_pill_click(sender As Object, e As RoutedEventArgs) Handles button_G_brain_cloud_pill.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("脑云丸", "Brain Cloud Pill", "T_brain_cloud_pill", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_broken_AAC_device_click(sender As Object, e As RoutedEventArgs) Handles button_G_broken_AAC_device.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("破碎的音频编码设备", "Broken AAC Device", "T_broken_AAC_device", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_license_plate_click(sender As Object, e As RoutedEventArgs) Handles button_G_license_plate.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("号码牌", "License Plate", "T_license_plate", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_old_boot_click(sender As Object, e As RoutedEventArgs) Handles button_G_old_boot.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("旧靴子", "Old Boot", "T_old_boot", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_one_true_earring_click(sender As Object, e As RoutedEventArgs) Handles button_G_one_true_earring.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("至尊耳环", "One True Earring", "T_one_true_earring", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_orange_soda_click(sender As Object, e As RoutedEventArgs) Handles button_G_orange_soda.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("橘子汽水", "Orange Soda", "T_orange_soda", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_sea_worther_click(sender As Object, e As RoutedEventArgs) Handles button_G_sea_worther.Click
        G_Science(1, {"S_Seaworthy"})
        G_Animal(0, {})
        G_Show_M("沃尔特海", "Sea Worther", "T_sea_worther", "SW", 0, 1, 0, "", True, False)
    End Sub

    Private Sub button_G_sextant_click(sender As Object, e As RoutedEventArgs) Handles button_G_sextant.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("六分仪", "Sextant", "T_sextant", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_soaked_candle_click(sender As Object, e As RoutedEventArgs) Handles button_G_soaked_candle.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("浸泡过的蜡烛", "Soaked Candle", "T_soaked_candle", "SW", 0, 1, 0, "", True, False)
    End Sub

    Private Sub button_G_toy_boat_click(sender As Object, e As RoutedEventArgs) Handles button_G_toy_boat.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("玩具船", "Toy Boat", "T_toy_boat", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_ukulele_click(sender As Object, e As RoutedEventArgs) Handles button_G_ukulele.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("四弦琴", "Ukulele", "T_ukulele", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_voodoo_doll_click(sender As Object, e As RoutedEventArgs) Handles button_G_voodoo_doll.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("巫毒娃娃", "Voodoo Doll", "T_voodoo_doll", "SW", 0, 1, 0, "", True, True)
    End Sub

    Private Sub button_G_wine_bottle_candle_click(sender As Object, e As RoutedEventArgs) Handles button_G_wine_bottle_candle.Click
        G_Science(0, {})
        G_Animal(0, {})
        G_Show_M("酒瓶蜡烛", "Wine Bottle Candle", "T_wine_bottle_candle", "SW", 0, 1, 0, "", True, True)
    End Sub

    REM ------------------左侧面板(物品_装备)------------------
    Private Sub G_Show_E(G_Name As String, G_EnName As String, G_picture As String, G_DLC As String, G_DLC_ROG As SByte, G_DLC_SW As SByte, G_DLC_DST As SByte, G_SpecialAbility_1 As String, G_SpecialAbility_2 As String, G_FromAnimal_1 As String, G_FromAnimal_2 As String, G_Introduce As String, ParamArray EquipmentAttribute() As Double)
        REM ------------------初始化------------------
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_Equipment.Visibility = Visibility.Visible
        Dim EquipmentAttributeNumSum As Byte = 0
        G_Canvas_Attack_E.Visibility = Visibility.Collapsed
        G_Canvas_AttackMin_E.Visibility = Visibility.Collapsed
        G_Canvas_AttackMax_E.Visibility = Visibility.Collapsed
        If G_Name = "晨星" Then
            EquipmentAttributeNumSum += 2
            G_Canvas_AttackDry_E.Visibility = Visibility.Visible
            G_Canvas_AttackWet_E.Visibility = Visibility.Visible
        Else
            G_Canvas_AttackDry_E.Visibility = Visibility.Collapsed
            G_Canvas_AttackWet_E.Visibility = Visibility.Collapsed
        End If
        If G_Name = "矛枪" Then
            EquipmentAttributeNumSum += 1
            G_Canvas_AttackSpearGun_E.Visibility = Visibility.Visible
        Else
            G_Canvas_AttackSpearGun_E.Visibility = Visibility.Collapsed
        End If
        If G_Name = "三叉戟" Then
            EquipmentAttributeNumSum += 1
            G_Canvas_AttackTrident_E.Visibility = Visibility.Visible
        Else
            G_Canvas_AttackTrident_E.Visibility = Visibility.Collapsed
        End If
        G_Canvas_Defense_E.Visibility = Visibility.Collapsed
        G_Canvas_DurabilityTime_E.Visibility = Visibility.Collapsed
        If G_Name = "热能石" Then
            EquipmentAttributeNumSum += 1
            G_Canvas_DurabilityTimeDST_E.Visibility = Visibility.Visible
        Else
            G_Canvas_DurabilityTimeDST_E.Visibility = Visibility.Collapsed
        End If
        G_Canvas_DurabilitySecond_E.Visibility = Visibility.Collapsed
        G_Canvas_DurabilityDay_E.Visibility = Visibility.Collapsed
        G_Canvas_DurabilityHP_E.Visibility = Visibility.Collapsed
        G_Canvas_Wetness_E.Visibility = Visibility.Collapsed
        G_Canvas_Overheating_E.Visibility = Visibility.Collapsed
        G_Canvas_Sanity_E.Visibility = Visibility.Collapsed
        If G_Name = "火魔杖" Or G_Name = "冰魔杖" Then
            EquipmentAttributeNumSum += 1
            G_Canvas_SanityTime_E.Visibility = Visibility.Visible
        Else
            G_Canvas_SanityTime_E.Visibility = Visibility.Collapsed
        End If
        G_WrapPanel_SpecialAbility_E.Visibility = Visibility.Collapsed
        G_WrapPanel_SpecialAbilityText_E.Visibility = Visibility.Collapsed
        If G_Name = "缀食者" Then
            EquipmentAttributeNumSum += 1
            G_Canvas_Hunger_E.Visibility = Visibility.Visible
            Image_GA_PB_Hunger.Width = 80
            TextBlock_GA_HungerValue.Text = "-100/分"
            TextBlock_GA_HungerValue.Foreground = Brushes.Red
        ElseIf G_Name = "饥饿腰带" Then
            EquipmentAttributeNumSum += 1
            G_Canvas_Hunger_E.Visibility = Visibility.Visible
            Image_GA_PB_Hunger.Width = 48
            TextBlock_GA_HungerValue.Text = "60%"
            TextBlock_GA_HungerValue.Foreground = Brushes.Black
        ElseIf G_Name = "熊皮背心" Then
            EquipmentAttributeNumSum += 1
            G_Canvas_Hunger_E.Visibility = Visibility.Visible
            Image_GA_PB_Hunger.Width = 60
            TextBlock_GA_HungerValue.Text = "75%"
            TextBlock_GA_HungerValue.Foreground = Brushes.Black
        Else
            G_Canvas_Hunger_E.Visibility = Visibility.Collapsed
        End If
        GL_textBlock_SpecialAbilityText_2_E.Visibility = Visibility.Collapsed
        G_WrapPanel_FromAnimal_E.Visibility = Visibility.Collapsed
        button_GE_FromAnimalButton_2.Visibility = Visibility.Collapsed
        If G_Name = "便携式烹饪锅" Then
            G_Canvas_PortableCrockPot_E.Visibility = Visibility.Visible
            G_Canvas_PortableCrockPotButton_E.Visibility = Visibility.Visible
        Else
            G_Canvas_PortableCrockPot_E.Visibility = Visibility.Collapsed
            G_Canvas_PortableCrockPotButton_E.Visibility = Visibility.Collapsed
        End If
        G_Canvas_FromAnimalButton_E.Visibility = Visibility.Collapsed
        REM ------------------物品名字------------------
        GL_textBlock_GoodsName_E.Text = G_Name
        GL_textBlock_GoodsName_E.UpdateLayout()
        Dim G_N_MarginLeft As Integer
        G_N_MarginLeft = (Canvas_GoodsLeft_E.ActualWidth - GL_textBlock_GoodsName_E.ActualWidth) / 2
        Dim G_N_T As New Thickness()
        G_N_T.Top = 80
        G_N_T.Left = G_N_MarginLeft
        GL_textBlock_GoodsName_E.Margin = G_N_T

        GL_textBlock_GoodsEnName_E.Text = G_EnName
        GL_textBlock_GoodsEnName_E.UpdateLayout()
        Dim G_EnN_MarginLeft As Integer
        G_EnN_MarginLeft = (Canvas_GoodsLeft_E.ActualWidth - GL_textBlock_GoodsEnName_E.ActualWidth) / 2
        Dim G_EnN_T As New Thickness()
        G_EnN_T.Top = 100
        G_EnN_T.Left = G_EnN_MarginLeft
        GL_textBlock_GoodsEnName_E.Margin = G_EnN_T
        REM ------------------物品图片------------------
        GL_image_GoodsPicture_E.Source = Picture_Short_Name(Res_Short_Name(G_picture))
        REM ------------------物品DLC-------------------
        If G_DLC = "ROG" Then
            GL_image_GE_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf G_DLC = "SW" Then
            GL_image_GE_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf G_DLC = "DST" Then
            GL_image_GE_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            GL_image_GE_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------装备属性-------------------

        If EquipmentAttribute(0) <> 0 Then
            EquipmentAttributeNumSum += 1
            G_Canvas_Attack_E.Visibility = Visibility.Visible
            Image_GA_PB_Attack.Width = EquipmentAttribute(0) / 4.25
            TextBlock_GA_AttackValue.Text = EquipmentAttribute(0)
        End If

        If EquipmentAttribute(1) <> 0 Then
            EquipmentAttributeNumSum += 1
            G_Canvas_AttackMin_E.Visibility = Visibility.Visible
            Image_GA_PB_AttackMin.Width = EquipmentAttribute(1) / 1.25
            TextBlock_GA_AttackMinValue.Text = EquipmentAttribute(1)
        End If

        If EquipmentAttribute(2) <> 0 Then
            EquipmentAttributeNumSum += 1
            G_Canvas_AttackMax_E.Visibility = Visibility.Visible
            Image_GA_PB_AttackMax.Width = EquipmentAttribute(2) / 1.875
            TextBlock_GA_AttackMaxValue.Text = EquipmentAttribute(2)
        End If

        If EquipmentAttribute(3) <> 0 Then
            EquipmentAttributeNumSum += 1
            G_Canvas_Defense_E.Visibility = Visibility.Visible
            Image_GA_PB_Defense.Width = EquipmentAttribute(3) / 1.1875
            TextBlock_GA_DefenseValue.Text = EquipmentAttribute(3) & "%"
        End If

        If EquipmentAttribute(4) <> 0 Then
            EquipmentAttributeNumSum += 1
            G_Canvas_DurabilityTime_E.Visibility = Visibility.Visible
            Image_GA_PB_DurabilityTime.Width = EquipmentAttribute(4) / 5
            TextBlock_GA_DurabilityTimeValue.Text = EquipmentAttribute(4) & "次"
        End If

        If EquipmentAttribute(5) <> 0 Then
            EquipmentAttributeNumSum += 1
            G_Canvas_DurabilitySecond_E.Visibility = Visibility.Visible
            If EquipmentAttribute(5) = 1000 Then
                Image_GA_PB_DurabilitySecond.Width = 80
                TextBlock_GA_DurabilitySecondValue.Text = "∞"
            Else
                Image_GA_PB_DurabilitySecond.Width = EquipmentAttribute(5) / 5.85
                TextBlock_GA_DurabilitySecondValue.Text = EquipmentAttribute(5) & "秒"
            End If
        End If

        If EquipmentAttribute(6) <> 0 Then
            EquipmentAttributeNumSum += 1
            G_Canvas_DurabilityDay_E.Visibility = Visibility.Visible
            Image_GA_PB_DurabilityDay.Width = EquipmentAttribute(6) / 0.3125
            TextBlock_GA_DurabilityDayValue.Text = EquipmentAttribute(6) & "天"
        End If

        If EquipmentAttribute(7) <> 0 Then
            EquipmentAttributeNumSum += 1
            G_Canvas_DurabilityHP_E.Visibility = Visibility.Visible
            Image_GA_PB_DurabilityHP.Width = EquipmentAttribute(7) / 22.5
            TextBlock_GA_DurabilityHPValue.Text = EquipmentAttribute(7) & "HP"
        End If

        If EquipmentAttribute(8) <> 0 Then
            EquipmentAttributeNumSum += 1
            G_Canvas_Wetness_E.Visibility = Visibility.Visible
            Image_GA_PB_Wetness.Width = EquipmentAttribute(8) / 1.25
            TextBlock_GA_WetnessValue.Text = EquipmentAttribute(8) & "%"
        End If

        If EquipmentAttribute(9) <> 0 Then
            EquipmentAttributeNumSum += 1
            G_Canvas_Overheating_E.Visibility = Visibility.Visible
            Image_GA_PB_Overheating.Width = EquipmentAttribute(9) / 3
            TextBlock_GA_OverheatingValue.Text = EquipmentAttribute(9)
        End If

        If EquipmentAttribute(10) <> 0 Then
            Dim G_Sanity_T As New Thickness()
            G_Sanity_T.Top = 0
            EquipmentAttributeNumSum += 1
            G_Canvas_Sanity_E.Visibility = Visibility.Visible
            TextBlock_GA_SanityValue.Text = EquipmentAttribute(10) & "/分"
            If EquipmentAttribute(10) < 0 Then
                Image_GA_PB_Sanity.Width = -EquipmentAttribute(10) / 0.25
                G_Sanity_T.Left = 148 - Image_GA_PB_Sanity.Width
                TextBlock_GA_SanityValue.Foreground = Brushes.Red
            Else
                Image_GA_PB_Sanity.Width = EquipmentAttribute(10) / 0.25
                G_Sanity_T.Left = 68
                TextBlock_GA_SanityValue.Text = "+" & TextBlock_GA_SanityValue.Text
                TextBlock_GA_SanityValue.Foreground = Brushes.Black
            End If
            Image_GA_PB_Sanity.Margin = G_Sanity_T
        End If

        G_WrapPanel_Frame_E_Attribute.Height = EquipmentAttributeNumSum * 22 + 15
        REM ------------------存在版本-------------------
        GL_textBlock_GE_DLC_1.Foreground = Brushes.Black
        GL_textBlock_GE_DLC_2.Foreground = Brushes.Black
        GL_textBlock_GE_DLC_3.Foreground = Brushes.Black
        If G_DLC_ROG = 0 Then
            GL_textBlock_GE_DLC_1.Foreground = Brushes.Silver
        End If
        If G_DLC_SW = 0 Then
            GL_textBlock_GE_DLC_2.Foreground = Brushes.Silver
        End If
        If G_DLC_DST = 0 Then
            GL_textBlock_GE_DLC_3.Foreground = Brushes.Silver
        End If
        REM ------------------特殊能力-------------------
        If G_SpecialAbility_1 <> "" Then
            G_WrapPanel_SpecialAbility_E.Visibility = Visibility.Visible
            G_WrapPanel_SpecialAbilityText_E.Visibility = Visibility.Visible
            GL_textBlock_SpecialAbilityText_1_E.Text = G_SpecialAbility_1
            G_WrapPanel_SpecialAbilityText_E.Height = 30
            If G_SpecialAbility_2 <> "" Then
                GL_textBlock_SpecialAbilityText_2_E.Visibility = Visibility.Visible
                GL_textBlock_SpecialAbilityText_2_E.Text = G_SpecialAbility_2
                G_WrapPanel_SpecialAbilityText_E.Height = 45
                If G_Name = "铥矿石皇冠" Then
                    G_WrapPanel_SpecialAbilityText_E.Height = 60
                End If
            End If
        End If
        REM -----------------来源于生物------------------
        If G_FromAnimal_1 <> "" Then
            GS_Equipment_FromAnimal_Select_1 = G_FromAnimal_1
            G_WrapPanel_FromAnimal_E.Visibility = Visibility.Visible
            G_Canvas_FromAnimalButton_E.Visibility = Visibility.Visible
            image_GE_FromAnimal_1.Source = Picture_Short_Name(Res_Short_Name(G_FromAnimal_1))
            If G_FromAnimal_2 <> "" Then
                GS_Equipment_FromAnimal_Select_2 = G_FromAnimal_2
                button_GE_FromAnimalButton_2.Visibility = Visibility.Visible
                image_GE_FromAnimal_2.Source = Picture_Short_Name(Res_Short_Name(G_FromAnimal_2))
            End If
        End If
        REM ------------------物品简介-------------------
        TextBlock_GE_Introduce.Text = G_Introduce
        TextBlock_GE_Introduce.Height = SetTextBlockHeight(G_Introduce, 13)
        G_WrapPanel_Introduce_E.Height = SetTextBlockHeight(G_Introduce, 13)
        REM ------------------面板高度-------------------
        Canvas_GoodsLeft_E.Height = 135 + G_WrapPanel_Frame_E_Attribute.Height
        If G_WrapPanel_SpecialAbilityText_E.Visibility = Visibility.Visible Then
            Canvas_GoodsLeft_E.Height += 15.24 + G_WrapPanel_SpecialAbilityText_E.Height
        End If
        If G_Canvas_FromAnimalButton_E.Visibility = Visibility.Visible Then
            Canvas_GoodsLeft_E.Height += 65.24
        End If
        If G_Canvas_PortableCrockPotButton_E.Visibility = Visibility.Visible Then
            Canvas_GoodsLeft_E.Height += 65.24
        End If
        Canvas_GoodsLeft_E.Height += 60 + G_WrapPanel_Introduce_E.Height
        If Canvas_GoodsLeft_E.Height < 604 Then
            Canvas_GoodsLeft_E.Height = 604
        End If
    End Sub

    Private Sub button_GE_FromAnimalButton_1_click(sender As Object, e As RoutedEventArgs) Handles button_GE_FromAnimalButton_1.Click
        ButtonJump(GS_Equipment_FromAnimal_Select_1)
    End Sub

    Private Sub button_GE_FromAnimalButton_2_click(sender As Object, e As RoutedEventArgs) Handles button_GE_FromAnimalButton_2.Click
        ButtonJump(GS_Equipment_FromAnimal_Select_2)
    End Sub

    Private Sub button_GE_PortableCrockPotButton_1_click(sender As Object, e As RoutedEventArgs) Handles button_GE_PortableCrockPotButton_1.Click
        LeftTabItem_Food.IsSelected = True
        button_F_fresh_fruit_crepes_click(Nothing, Nothing)
    End Sub

    Private Sub button_GE_PortableCrockPotButton_2_click(sender As Object, e As RoutedEventArgs) Handles button_GE_PortableCrockPotButton_2.Click
        LeftTabItem_Food.IsSelected = True
        button_F_monster_tartare_click(Nothing, Nothing)
    End Sub

    Private Sub button_GE_PortableCrockPotButton_3_click(sender As Object, e As RoutedEventArgs) Handles button_GE_PortableCrockPotButton_3.Click
        LeftTabItem_Food.IsSelected = True
        button_F_mussel_bouillabaise_click(Nothing, Nothing)
    End Sub

    Private Sub button_GE_PortableCrockPotButton_4_click(sender As Object, e As RoutedEventArgs) Handles button_GE_PortableCrockPotButton_4.Click
        LeftTabItem_Food.IsSelected = True
        button_F_sweet_potato_souffle_click(Nothing, Nothing)
    End Sub

    Private Sub button_G_axe_click(sender As Object, e As RoutedEventArgs) Handles button_G_axe.Click
        G_Show_E("斧头", "Axe", "S_axe", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {27.2, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_goldenaxe_click(sender As Object, e As RoutedEventArgs) Handles button_G_goldenaxe.Click
        G_Show_E("金斧头", "Luxury Axe", "S_goldenaxe", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {27.2, 0, 0, 0, 400, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_machete_click(sender As Object, e As RoutedEventArgs) Handles button_G_machete.Click
        G_Show_E("砍刀", "Machete", "S_machete", "SW", 0, 1, 0, "", "", "", "", "除了用作工具还能用来打怪。", {29.92, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_luxury_machete_click(sender As Object, e As RoutedEventArgs) Handles button_G_luxury_machete.Click
        G_Show_E("黄金砍刀", "Luxury Machete", "S_luxury_machete", "SW", 0, 1, 0, "", "", "", "", "除了用作工具还能用来打怪。", {29.92, 0, 0, 0, 400, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_pickaxe_click(sender As Object, e As RoutedEventArgs) Handles button_G_pickaxe.Click
        G_Show_E("鹤嘴锄", "Pickaxe", "S_pickaxe", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {27.2, 0, 0, 0, 33, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_goldenpickaxe_click(sender As Object, e As RoutedEventArgs) Handles button_G_goldenpickaxe.Click
        G_Show_E("黄金鹤嘴锄", "Opulent Pickaxe", "S_goldenpickaxe", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {27.2, 0, 0, 0, 132, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_shovel_click(sender As Object, e As RoutedEventArgs) Handles button_G_shovel.Click
        G_Show_E("铁铲", "Shovel", "S_shovel", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {17, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_goldenshovel_click(sender As Object, e As RoutedEventArgs) Handles button_G_goldenshovel.Click
        G_Show_E("黄金铁铲", "Regal Shovel", "S_goldenshovel", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {17, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_hammer_click(sender As Object, e As RoutedEventArgs) Handles button_G_hammer.Click
        G_Show_E("锤子", "Hammer", "S_hammer", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {17, 0, 0, 0, 75, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_pitchfork_click(sender As Object, e As RoutedEventArgs) Handles button_G_pitchfork.Click
        G_Show_E("草叉", "Pitchfork", "S_pitchfork", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {17, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_saddlehorn_click(sender As Object, e As RoutedEventArgs) Handles button_G_saddlehorn.Click
        G_Show_E("取鞍器", "Saddlehorn", "S_saddlehorn", "DST", 0, 0, 1, "", "", "", "", "除了用作工具还能用来打怪。", {17, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_brush_click(sender As Object, e As RoutedEventArgs) Handles button_G_brush.Click
        G_Show_E("刷子", "Brush", "S_brush", "DST", 0, 0, 1, "", "", "", "", "除了用作工具还能用来打怪。", {27.2, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_pickaxe_1_click(sender As Object, e As RoutedEventArgs) Handles button_G_pickaxe_1.Click
        G_Show_E("摘/斧头", "Pick/Axe", "S_pickaxe_1", "NoDLC", 1, 0, 1, "", "", "", "", "除了用作工具还能用来打怪。", {30.6, 0, 0, 0, 400, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_obsidian_machete_click(sender As Object, e As RoutedEventArgs) Handles button_G_obsidian_machete.Click
        G_Show_E("黑曜石砍刀", "Obsidian Machete", "S_obsidian_machete", "SW", 0, 1, 0, "", "", "", "", "除了用作工具还能用来打怪。", {0, 29.92, 59.84, 0, 250, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_obsidian_axe_click(sender As Object, e As RoutedEventArgs) Handles button_G_obsidian_axe.Click
        G_Show_E("黑曜石斧", "Obsidian Axe", "S_obsidian_axe", "SW", 0, 1, 0, "", "", "", "", "除了用作工具还能用来打怪。", {0, 27.2, 54.4, 0, 250, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_lucy_the_axe_click(sender As Object, e As RoutedEventArgs) Handles button_G_lucy_the_axe.Click
        G_Show_E("露西斧", "Lucy the Axe", "G_lucy_the_axe", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {27.2, 0, 0, 0, 0, 1000, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_bug_net_click(sender As Object, e As RoutedEventArgs) Handles button_G_bug_net.Click
        G_Show_E("捕虫网", "Bug Net", "S_bug_net", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {4.25, 0, 0, 0, 3.3, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_fishing_rod_click(sender As Object, e As RoutedEventArgs) Handles button_G_fishing_rod.Click
        G_Show_E("钓竿", "Fishing Rod", "S_fishing_rod", "NoDLC", 1, 1, 1, "", "", "", "", "除了用作工具还能用来打怪。", {4.25, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_walking_cane_click(sender As Object, e As RoutedEventArgs) Handles button_G_walking_cane.Click
        G_Show_E("步行手杖", "Walking Cane", "S_walking_cane", "NoDLC", 1, 0, 1, "", "", "", "", "用海象牙制作的加速工具。", {17, 0, 0, 0, 0, 1000, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_rawling_click(sender As Object, e As RoutedEventArgs) Handles button_G_rawling.Click
        G_Show_E("罗林", "Rawling", "G_rawling", "SW", 0, 1, 0, "", "", "", "", "可以在深海发现它，带在身上有精神光环，每抛一次也会加1点精神。", {0, 0, 0, 0, 0, 1000, 0, 0, 0, 0, 2})
    End Sub

    Private Sub button_G_torch_click(sender As Object, e As RoutedEventArgs) Handles button_G_torch.Click
        G_Show_E("火炬", "Torch", "S_torch", "NoDLC", 1, 1, 1, "", "", "", "", "除了攻击伤害还有后续的燃烧伤害，带着还有一定的防潮效果。", {17, 0, 0, 0, 0, 75, 0, 0, 20, 0, 0})
    End Sub

    Private Sub button_G_miner_hat_click(sender As Object, e As RoutedEventArgs) Handles button_G_miner_hat.Click
        G_Show_E("矿工帽", "Miner Hat", "S_miner_hat", "NoDLC", 1, 1, 1, "", "", "", "", "不仅可以照明还有少量防潮效果。", {0, 0, 0, 0, 0, 468, 0, 0, 20, 0, 0})
    End Sub

    Private Sub button_G_pirate_hat_click(sender As Object, e As RoutedEventArgs) Handles button_G_pirate_hat.Click
        G_Show_E("海盗帽", "Pirate Hat", "S_pirate_hat", "SW", 0, 1, 0, "", "", "", "", "拥有少量防潮效果。", {0, 0, 0, 0, 0, 0, 2, 0, 20, 0, 2})
    End Sub

    Private Sub button_G_tropical_parasol_click(sender As Object, e As RoutedEventArgs) Handles button_G_tropical_parasol.Click
        G_Show_E("热带遮阳伞", "tropical Parasol", "S_tropical_parasol", "SW", 0, 1, 0, "", "", "", "", "既防潮又隔热。", {0, 0, 0, 0, 0, 0, 2, 0, 20, 120, 2})
    End Sub

    Private Sub button_G_pretty_parasol_click(sender As Object, e As RoutedEventArgs) Handles button_G_pretty_parasol.Click
        G_Show_E("漂亮太阳伞", "Pretty Parasol", "S_pretty_parasol", "NoDLC", 1, 0, 1, "", "", "", "", "既防潮又隔热。", {0, 0, 0, 0, 0, 0, 2, 0, 50, 120, 2})
    End Sub

    Private Sub button_G_umbrella_click(sender As Object, e As RoutedEventArgs) Handles button_G_umbrella.Click
        G_Show_E("雨伞", "Umbrella", "S_umbrella", "NoDLC", 1, 1, 1, "", "", "", "", "不俗的防潮效果。", {17, 0, 0, 0, 20, 0, 0, 0, 50, 0, 2})
    End Sub

    Private Sub button_G_straw_hat_click(sender As Object, e As RoutedEventArgs) Handles button_G_straw_hat.Click
        G_Show_E("草帽", "Straw Hat", "S_straw_hat", "NoDLC", 1, 1, 1, "", "", "", "", "少量的防潮效果，还有少量隔热和效果和精神光环。", {0, 0, 0, 0, 0, 0, 6, 0, 20, 60, 2})
    End Sub

    Private Sub button_G_top_hat_click(sender As Object, e As RoutedEventArgs) Handles button_G_top_hat.Click
        G_Show_E("高礼帽", "Top Hat", "S_top_hat", "NoDLC", 1, 1, 1, "", "", "", "", "一般的防潮隔热装备。", {0, 0, 0, 0, 0, 0, 5, 0, 20, 60, 0})
    End Sub

    Private Sub button_G_rain_hat_click(sender As Object, e As RoutedEventArgs) Handles button_G_rain_hat.Click
        G_Show_E("雨帽", "Rain Hat", "S_rain_hat", "NoDLC", 1, 0, 1, "", "", "", "", "雨帽加雨衣，绝配！", {0, 0, 0, 0, 0, 0, 8, 0, 20, 0, 3.33})
    End Sub

    Private Sub button_G_rain_coat_click(sender As Object, e As RoutedEventArgs) Handles button_G_rain_coat.Click
        G_Show_E("雨衣", "Rain Coat", "S_rain_coat", "NoDLC", 1, 0, 1, "", "", "", "", "雨帽加雨衣，绝配！", {0, 0, 0, 0, 0, 0, 8, 0, 70, 60, 0})
    End Sub

    Private Sub button_G_snakeskin_hat_click(sender As Object, e As RoutedEventArgs) Handles button_G_snakeskin_hat.Click
        G_Show_E("蛇鳞帽", "Snakeskin Hat", "S_snakeskin_hat", "SW", 0, 1, 0, "", "", "", "", "除了防潮还能防电！", {0, 0, 0, 0, 0, 0, 10, 0, 70, 0, 0})
    End Sub

    Private Sub button_G_snakeskin_jacket_click(sender As Object, e As RoutedEventArgs) Handles button_G_snakeskin_jacket.Click
        G_Show_E("蛇鳞上衣", "Snakeskin Jacket", "S_snakeskin_jacket", "SW", 0, 1, 0, "", "", "", "", "除了防潮还能防电！", {0, 0, 0, 0, 0, 0, 8, 0, 70, 0, 0})
    End Sub

    Private Sub button_G_blubber_suit_click(sender As Object, e As RoutedEventArgs) Handles button_G_blubber_suit.Click
        G_Show_E("鲸脂套装", "Blubber Suit", "S_blubber_suit", "SW", 0, 1, 0, "", "", "", "", "无敌的防潮效果！", {0, 0, 0, 0, 0, 0, 10, 0, 100, 60, 0})
    End Sub

    Private Sub button_G_eyebrella_click(sender As Object, e As RoutedEventArgs) Handles button_G_eyebrella.Click
        G_Show_E("眼球伞", "Eyebrella", "S_eyebrella", "NoDLC", 1, 0, 1, "", "", "", "", "无敌的防潮效果！隔热效果也是绝佳！", {0, 0, 0, 0, 0, 0, 8, 0, 100, 240, 0})
    End Sub

    Private Sub button_G_dumbrella_click(sender As Object, e As RoutedEventArgs) Handles button_G_dumbrella.Click
        G_Show_E("双层伞帽", "Dumbrella", "S_dumbrella", "SW", 0, 1, 0, "", "", "", "", "船难版的眼球伞，无敌的防潮效果！隔热效果也是绝佳！", {0, 0, 0, 0, 0, 0, 9, 0, 100, 240, 0})
    End Sub

    Private Sub button_G_windbreaker_click(sender As Object, e As RoutedEventArgs) Handles button_G_windbreaker.Click
        G_Show_E("风衣", "Windbreaker", "S_windbreaker", "SW", 0, 1, 0, "", "", "", "", "有少量的防潮效果，并且防风。", {0, 0, 0, 0, 0, 0, 12, 0, 20, 240, 0})
    End Sub

    Private Sub button_G_thermal_stone_click(sender As Object, e As RoutedEventArgs) Handles button_G_thermal_stone.Click
        G_Show_E("热能石", "Thermal Stone", "S_thermal_stone", "NoDLC", 1, 1, 1, "", "", "", "", "每次变灰耐久度减少1点，可以用来升温也可以用来降温。", {0, 0, 0, 0, 0, 0, 0, 0, 0, 120, 0})
    End Sub

    Private Sub button_G_whirly_fan_click(sender As Object, e As RoutedEventArgs) Handles button_G_whirly_fan.Click
        G_Show_E("旋风扇", "Whirly Fan", "S_whirly_fan", "DST", 0, 0, 1, "", "", "", "", "DST版中玩具似的隔热道具，可以用来打人哦！", {17, 0, 0, 0, 0, 90, 0, 0, 0, 30, 0})
    End Sub

    Private Sub button_G_rabbit_earmuffs_click(sender As Object, e As RoutedEventArgs) Handles button_G_rabbit_earmuffs.Click
        G_Show_E("兔毛耳套", "Rabbit Earmuffs", "S_rabbit_earmuffs", "NoDLC", 1, 0, 1, "", "", "", "", "不怎么样的隔热装备，不过挺可爱的~", {0, 0, 0, 0, 0, 0, 5, 0, 0, 60, 0})
    End Sub

    Private Sub button_G_beefalo_hat_click(sender As Object, e As RoutedEventArgs) Handles button_G_beefalo_hat.Click
        G_Show_E("牛帽", "Beefalo Hat", "S_beefalo_hat", "NoDLC", 1, 0, 1, "", "", "", "", "有了牛帽冬天就不用愁了。", {0, 0, 0, 0, 0, 0, 10, 0, 20, 240, 0})
    End Sub

    Private Sub button_G_winter_hat_click(sender As Object, e As RoutedEventArgs) Handles button_G_winter_hat.Click
        G_Show_E("寒冬帽", "Winter Hat", "S_winter_hat", "NoDLC", 1, 0, 1, "", "", "", "", "一般过冬的装备。", {0, 0, 0, 0, 0, 0, 10, 0, 0, 120, 1.33})
    End Sub

    Private Sub button_G_cat_cap_click(sender As Object, e As RoutedEventArgs) Handles button_G_cat_cap.Click
        G_Show_E("猫帽", "Cat Cap", "S_cat_cap", "NoDLC", 1, 0, 1, "", "", "", "", "没啥用的隔热装备。", {0, 0, 0, 0, 0, 0, 10, 0, 0, 60, 3.33})
    End Sub

    Private Sub button_G_fashion_melon_click(sender As Object, e As RoutedEventArgs) Handles button_G_fashion_melon.Click
        G_Show_E("时尚西瓜帽", "Fassion Melon", "S_fashion_melon", "NoDLC", 1, 1, 1, "", "", "", "", "廉价的隔热装备。", {0, 0, 0, 0, 0, 0, 3, 0, 20, 120, -2})
    End Sub

    Private Sub button_G_ice_cube_click(sender As Object, e As RoutedEventArgs) Handles button_G_ice_cube.Click
        G_Show_E("冰块", "Ice Cube", "S_ice_cube", "NoDLC", 1, 1, 1, "", "", "", "", "隔热效果很好，不过会增加潮湿度。", {0, 0, 0, 0, 0, 0, 4, 0, 0, 240, 0})
    End Sub

    Private Sub button_G_dapper_vest_click(sender As Object, e As RoutedEventArgs) Handles button_G_dapper_vest.Click
        G_Show_E("小巧背心", "Dapper Vest", "S_dapper_vest", "NoDLC", 1, 0, 1, "", "", "", "", "没啥用的隔热装备。", {0, 0, 0, 0, 0, 0, 10, 0, 0, 60, 3.33})
    End Sub

    Private Sub button_G_breezy_vest_click(sender As Object, e As RoutedEventArgs) Handles button_G_breezy_vest.Click
        G_Show_E("夏日背心", "Breezy Vest", "S_breezy_vest", "NoDLC", 1, 0, 1, "", "", "", "", "纯粹是浪费红色象鼻。", {0, 0, 0, 0, 0, 0, 10, 0, 0, 60, 2})
    End Sub

    Private Sub button_G_puffy_vest_click(sender As Object, e As RoutedEventArgs) Handles button_G_puffy_vest.Click
        G_Show_E("寒冬背心", "Puffy Vest", "S_puffy_vest", "NoDLC", 1, 0, 1, "", "", "", "", "极好的隔热装备。", {0, 0, 0, 0, 0, 0, 15, 0, 0, 240, 2})
    End Sub

    Private Sub button_G_summer_frest_click(sender As Object, e As RoutedEventArgs) Handles button_G_summer_frest.Click
        G_Show_E("夏季背心", "Summer Frest", "S_summer_frest", "NoDLC", 1, 1, 1, "", "", "", "", "不错的隔热装备。", {0, 0, 0, 0, 0, 0, 8, 0, 20, 120, 2})
    End Sub

    Private Sub button_G_floral_shirt_click(sender As Object, e As RoutedEventArgs) Handles button_G_floral_shirt.Click
        G_Show_E("花纹衬衫", "Floral Shirt", "S_floral_shirt", "NoDLC", 1, 1, 1, "", "", "", "", "极好的隔热装备，而且在船难版里制作材料比较简单。", {0, 0, 0, 0, 0, 0, 15, 0, 0, 240, 3.33})
    End Sub

    Private Sub button_G_hibearnation_vest_click(sender As Object, e As RoutedEventArgs) Handles button_G_hibearnation_vest.Click
        G_Show_E("熊皮背心", "Hibearnation Vest", "S_hibearnation_vest", "NoDLC", 1, 0, 1, "", "", "", "", "隔热效果是最好，可惜材料太难得，而且耐久度不长。", {0, 0, 0, 0, 0, 0, 7, 0, 0, 240, 4.44})
    End Sub

    Private Sub button_G_boat_cannon_click(sender As Object, e As RoutedEventArgs) Handles button_G_boat_cannon.Click
        G_Show_E("船载加农炮", "Boat Cannon", "S_boat_cannon", "SW", 0, 1, 0, "", "", "", "", "装在船上的攻击性武器。", {100, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_gunpowder_click(sender As Object, e As RoutedEventArgs) Handles button_G_gunpowder.Click
        G_Show_E("火药", "Gunpowder", "S_gunpowder", "NoDLC", 1, 1, 1, "", "", "", "", "Boom！", {200, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_coconade_click(sender As Object, e As RoutedEventArgs) Handles button_G_coconade.Click
        G_Show_E("椰壳炸弹", "Coconade", "S_coconade", "SW", 0, 1, 0, "", "", "", "", "火药升级版，Boom！Boom！", {250, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_obsidian_coconade_click(sender As Object, e As RoutedEventArgs) Handles button_G_obsidian_coconade.Click
        G_Show_E("黑曜石炸弹", "Obsidian Coconade", "S_obsidian_coconade", "SW", 0, 1, 0, "", "", "", "", "椰壳炸弹升级版，Boom！Boom！Boom！", {340, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_battle_spear_click(sender As Object, e As RoutedEventArgs) Handles button_G_battle_spear.Click
        G_Show_E("战斗长矛", "Battle Spear", "S_battle_spear", "NoDLC", 1, 1, 1, "", "", "", "", "威戈芙瑞德专属武器，联机版可以给别人用。", {42.5, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_spear_click(sender As Object, e As RoutedEventArgs) Handles button_G_spear.Click
        G_Show_E("长矛", "Spear", "S_spear", "NoDLC", 1, 1, 1, "", "", "", "", "一般的武器。", {34, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_poison_spear_click(sender As Object, e As RoutedEventArgs) Handles button_G_poison_spear.Click
        G_Show_E("毒矛", "Poison Spear", "S_poison_spear", "SW", 0, 1, 0, "攻击带毒", "", "", "", "攻击伤害和长矛一致，附带毒。", {34, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_ham_bat_click(sender As Object, e As RoutedEventArgs) Handles button_G_ham_bat.Click
        G_Show_E("火腿球棒", "Ham Bat", "S_ham_bat", "NoDLC", 1, 1, 1, "", "", "", "", "过得越久攻击越低", {0, 29.75, 59.5, 0, 0, 0, 10, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_morning_star_click(sender As Object, e As RoutedEventArgs) Handles button_G_morning_star.Click
        G_Show_E("晨星", "Morning Star", "S_morning_star", "NoDLC", 1, 0, 1, "", "", "", "", "有照明效果，攻击潮湿的生物有1.66倍伤害加成。", {0, 0, 0, 0, 0, 360, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_tail_o_three_cats_click(sender As Object, e As RoutedEventArgs) Handles button_G_tail_o_three_cats.Click
        G_Show_E("三尾猫的教诲", "Tail o' Three Cats", "S_tail_o'_three_cats", "DST", 0, 0, 1, "", "", "", "", "驯牛神器。", {27.2, 0, 0, 0, 175, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_blow_dart_click(sender As Object, e As RoutedEventArgs) Handles button_G_blow_dart.Click
        G_Show_E("吹箭", "Blow Dart", "S_blow_dart", "NoDLC", 1, 1, 1, "", "", "A_mactusk", "", "一下100点的伤害也不错了。", {100, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_sleep_dart_click(sender As Object, e As RoutedEventArgs) Handles button_G_sleep_dart.Click
        G_Show_E("麻醉吹箭", "Sleep Dart", "S_sleep_dart", "NoDLC", 1, 1, 1, "", "", "", "", "让生物睡觉的吹箭。", {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_fire_dart_click(sender As Object, e As RoutedEventArgs) Handles button_G_fire_dart.Click
        G_Show_E("燃烧吹箭", "Fire Dart", "S_fire_dart", "NoDLC", 1, 1, 1, "", "", "", "", "虽然攻击只有5，但是有后续燃烧伤害。", {5, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_poison_dart_click(sender As Object, e As RoutedEventArgs) Handles button_G_poison_dart.Click
        G_Show_E("毒镖", "Poison Dart", "S_poison_dart", "SW", 0, 1, 0, "", "", "", "", "顾名思义，让对手中毒。", {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_boomerang_click(sender As Object, e As RoutedEventArgs) Handles button_G_boomerang.Click
        G_Show_E("回旋镖", "Boomerang", "S_boomerang", "NoDLC", 1, 1, 1, "", "", "", "", "不按空格接住还会打到自己。", {27.2, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_weather_pain_click(sender As Object, e As RoutedEventArgs) Handles button_G_weather_pain.Click
        G_Show_E("旋风", "Weather Pain", "S_weather_pain", "NoDLC", 1, 0, 1, "", "", "", "", "每秒7点伤害，不受人物影响，对于行动迟缓的生物伤害极高，小心拆家。", {7, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_spear_gun_click(sender As Object, e As RoutedEventArgs) Handles button_G_spear_gun.Click
        G_Show_E("矛枪", "Spear Gun", "S_spear_gun", "SW", 0, 1, 0, "", "", "", "", "可以装载战斗长矛、长矛、毒矛、黑曜石矛，射出去的矛要自己捡回来。", {0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_cutlass_supreme_click(sender As Object, e As RoutedEventArgs) Handles button_G_cutlass_supreme.Click
        G_Show_E("旗鱼短剑", "Cutlass Supreme", "S_cutlass_supreme", "SW", 0, 1, 0, "", "", "", "", "攻击力不错。", {68, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_dark_sword_click(sender As Object, e As RoutedEventArgs) Handles button_G_dark_sword.Click
        G_Show_E("暗夜剑", "Dark Sword", "S_dark_sword", "NoDLC", 1, 1, 1, "", "", "", "", "打着打着精神就没了。", {68, 0, 0, 0, 100, 0, 0, 0, 0, 0, -20})
    End Sub

    Private Sub button_G_bat_bat_click(sender As Object, e As RoutedEventArgs) Handles button_G_bat_bat.Click
        G_Show_E("蝙蝠斧棍", "Bat Bat", "S_bat_bat", "NoDLC", 1, 0, 1, "攻击吸血", "", "", "", "每次攻击吸血6.8。", {42.5, 0, 0, 0, 75, 0, 0, 0, 0, 0, -3.33})
    End Sub

    Private Sub button_G_belt_of_hunger_click(sender As Object, e As RoutedEventArgs) Handles button_G_belt_of_hunger.Click
        G_Show_E("饥饿腰带", "Belt of Hunger", "S_belt_of_hunger", "NoDLC", 1, 0, 1, "抵御饥饿", "", "", "", "饥饿速度变为默认的60%。", {0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 2})
    End Sub

    Private Sub button_G_fire_staff_click(sender As Object, e As RoutedEventArgs) Handles button_G_fire_staff.Click
        G_Show_E("火魔杖", "Fire Staff", "S_fire_staff", "NoDLC", 1, 1, 1, "", "", "", "", "烧吧烧吧！", {0, 100, 150, 0, 20, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_ice_staff_click(sender As Object, e As RoutedEventArgs) Handles button_G_ice_staff.Click
        G_Show_E("冰魔杖", "Ice Staff", "S_ice_staff", "NoDLC", 1, 1, 1, "", "", "", "", "冰冻敌人！", {0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_thulecite_club_click(sender As Object, e As RoutedEventArgs) Handles button_G_thulecite_club.Click
        G_Show_E("铥矿棒", "Thulecite Club", "S_thulecite_club", "NoDLC", 1, 0, 1, "", "", "", "", "攻击力不错的武器。", {59.5, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_obsidian_spear_click(sender As Object, e As RoutedEventArgs) Handles button_G_obsidian_spear.Click
        G_Show_E("黑曜石矛", "Obsidian Spear", "S_obsidian_spear", "SW", 0, 1, 0, "", "", "", "", "越打越来劲。", {0, 51, 102, 0, 375, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_tentacle_spike_click(sender As Object, e As RoutedEventArgs) Handles button_G_tentacle_spike.Click
        G_Show_E("触手尖刺", "Tentacle Spike", "G_tentacle_spike", "NoDLC", 1, 1, 1, "", "", "A_tentacle", "A_quacken_tentacle", "没事去沼泽就能捡到几个。", {51, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_eyeshot_click(sender As Object, e As RoutedEventArgs) Handles button_G_eyeshot.Click
        G_Show_E("眼睛吹箭", "Eyeshot", "G_eyeshot", "SW", 0, 1, 0, "", "", "A_flup", "", "只有追踪性弹涂鱼才会掉落，打中小鸟会让小鸟眩晕5秒。", {20, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_harpoon_click(sender As Object, e As RoutedEventArgs) Handles button_G_harpoon.Click
        G_Show_E("鱼叉", "Harpoon", "G_harpoon", "SW", 0, 1, 0, "", "", "A_white_whale", "", "超高攻击力。", {200, 0, 0, 0, 10, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_peg_leg_click(sender As Object, e As RoutedEventArgs) Handles button_G_peg_leg.Click
        G_Show_E("假腿", "Peg Leg", "G_peg_leg", "SW", 0, 1, 0, "", "", "", "", "可以在X标记点宝箱找到。", {34, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_trident_click(sender As Object, e As RoutedEventArgs) Handles button_G_trident.Click
        G_Show_E("三叉戟", "Trident", "G_trident", "SW", 0, 1, 0, "", "", "", "", "在深海区域用拖网有可能找到三叉戟，在陆地上只有34点攻击，在船上有三倍攻击加成。", {34, 0, 0, 0, 150, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_battle_helm_click(sender As Object, e As RoutedEventArgs) Handles button_G_battle_helm.Click
        G_Show_E("战斗头盔", "Battle Helm", "S_battle_helm", "NoDLC", 1, 1, 1, "", "", "", "", "威戈芙瑞德专属防具，联机版可以给别人用。", {0, 0, 0, 80, 0, 0, 0, 750, 20, 0, 0})
    End Sub

    Private Sub button_G_grass_suit_click(sender As Object, e As RoutedEventArgs) Handles button_G_grass_suit.Click
        G_Show_E("草地盔甲", "Grass Suit", "S_grass_suit", "NoDLC", 1, 0, 1, "", "", "", "", "最弱的防御装备。", {0, 0, 0, 60, 0, 0, 0, 225, 0, 0, 0})
    End Sub

    Private Sub button_G_log_suit_click(sender As Object, e As RoutedEventArgs) Handles button_G_log_suit.Click
        G_Show_E("木制盔甲", "Log Suit", "S_log_suit", "NoDLC", 1, 1, 1, "", "", "", "", "看得过去的防御装备。", {0, 0, 0, 80, 0, 0, 0, 450, 0, 0, 0})
    End Sub

    Private Sub button_G_marble_suit_click(sender As Object, e As RoutedEventArgs) Handles button_G_marble_suit.Click
        G_Show_E("大理石盔甲", "Marble Suit", "S_marble_suit", "NoDLC", 1, 0, 1, "减速30%", "", "", "", "接近无敌的防御装备，不过会减速30%，而且材料不好找。", {0, 0, 0, 95, 0, 0, 0, 1050, 0, 0, 0})
    End Sub

    Private Sub button_G_seashell_suit_click(sender As Object, e As RoutedEventArgs) Handles button_G_seashell_suit.Click
        G_Show_E("海贝盔甲", "Seashell Suit", "S_seashell_suit", "SW", 0, 1, 0, "免疫中毒", "", "", "", "防御力还行，特点是免疫毒。", {0, 0, 0, 75, 0, 0, 0, 750, 0, 0, 0})
    End Sub

    Private Sub button_G_limestone_suit_click(sender As Object, e As RoutedEventArgs) Handles button_G_limestone_suit.Click
        G_Show_E("石灰石盔甲", "Limestone Suit", "S_limestone_suit", "SW", 0, 1, 0, "减速10%", "", "", "", "防御不错，但是会减速10%。", {0, 0, 0, 70, 0, 0, 0, 825, 0, 0, 2})
    End Sub

    Private Sub button_G_cactus_armour_click(sender As Object, e As RoutedEventArgs) Handles button_G_cactus_armour.Click
        G_Show_E("象仙人掌盔甲", "Cactus Armour", "S_cactus_armour", "SW", 0, 1, 0, "对攻击者造成17点伤害", "", "", "", "反伤神器。", {0, 0, 0, 80, 0, 0, 0, 450, 0, 0, 3.33})
    End Sub

    Private Sub button_G_football_helmet_click(sender As Object, e As RoutedEventArgs) Handles button_G_football_helmet.Click
        G_Show_E("猪皮足球头盔", "Football Helmet", "S_football_helmet", "NoDLC", 1, 1, 1, "", "", "", "", "防御性能不错！", {0, 0, 0, 80, 0, 0, 0, 450, 20, 0, 0})
    End Sub

    Private Sub button_G_horned_helmet_click(sender As Object, e As RoutedEventArgs) Handles button_G_horned_helmet.Click
        G_Show_E("角状头盔", "Horned Helmet", "S_horned_helmet", "SW", 0, 1, 0, "免疫中毒", "", "", "", "防御不错而且免疫毒。", {0, 0, 0, 85, 0, 0, 0, 600, 35, 0, 0})
    End Sub

    Private Sub button_G_scalemail_click(sender As Object, e As RoutedEventArgs) Handles button_G_scalemail.Click
        G_Show_E("鳞甲", "Scalemail", "S_scalemail", "NoDLC", 1, 0, 1, "免疫火", "点燃攻击者", "", "", "耐久超高的防御装备，免疫火，并且攻击你的敌人会燃烧，就是防御力不怎么样。", {0, 0, 0, 70, 0, 0, 0, 1800, 0, 0, 3.33})
    End Sub

    Private Sub button_G_night_armour_click(sender As Object, e As RoutedEventArgs) Handles button_G_night_armour.Click
        G_Show_E("暗影之甲", "Night Armour", "S_night_armour", "NoDLC", 1, 1, 1, "降低所受伤害10%的精神", "", "", "", "防御力极高，打着打着就没精神了。", {0, 0, 0, 95, 0, 0, 0, 750, 0, 0, -10})
    End Sub

    Private Sub button_G_beekeeper_hat_click(sender As Object, e As RoutedEventArgs) Handles button_G_beekeeper_hat.Click
        G_Show_E("养蜂人的帽子", "Beekeeper Hat", "S_beekeeper_hat", "NoDLC", 1, 1, 1, "", "", "", "", "仅防御来自蜜蜂和杀人蜂的攻击。", {0, 0, 0, 80, 0, 0, 0, 750, 20, 0, 0})
    End Sub

    Private Sub button_G_thulecite_crown_click(sender As Object, e As RoutedEventArgs) Handles button_G_thulecite_crown.Click
        G_Show_E("铥矿石皇冠", "Thulecite Crown", "S_thulecite_crown", "NoDLC", 1, 0, 1, "被攻击有1/3几率创建免疫火、雷的力场", "降低力场吸收伤害5%的精神", "", "", "防御力很高而且耐久也不错，还有被动效果。", {0, 0, 0, 90, 0, 0, 0, 1200, 0, 0, 0})
    End Sub

    Private Sub button_G_thulecite_suit_click(sender As Object, e As RoutedEventArgs) Handles button_G_thulecite_suit.Click
        G_Show_E("铥矿甲胄", "Thulecite Suit", "S_thulecite_suit", "NoDLC", 1, 0, 1, "", "", "", "", "耐久最高，防御力也很高。", {0, 0, 0, 90, 0, 0, 0, 1800, 0, 0, 3.33})
    End Sub

    Private Sub button_G_obsidian_armour_click(sender As Object, e As RoutedEventArgs) Handles button_G_obsidian_armour.Click
        G_Show_E("黑曜石盔甲", "Obsidian Armour", "S_obsidian_armour", "SW", 0, 1, 0, "", "", "", "", "耐久只有鳞甲的四分之三，免疫火，并且攻击你的敌人会燃烧，防御力也一般。", {0, 0, 0, 70, 0, 0, 0, 1350, 0, 0, 0})
    End Sub

    Private Sub button_G_shelmet_click(sender As Object, e As RoutedEventArgs) Handles button_G_shelmet.Click
        G_Show_E("背壳头盔", "Shelmet", "G_shelmet", "NoDLC", 1, 0, 1, "", "", "A_slurtles", "", "防御力高，爆率低。", {0, 0, 0, 90, 0, 0, 0, 750, 20, 0, 0})
    End Sub

    Private Sub button_G_snurtle_shell_armor_click(sender As Object, e As RoutedEventArgs) Handles button_G_snurtle_shell_armor.Click
        G_Show_E("圆壳蛞蝓壳", "Snurtle Shell Armor", "G_snurtle_shell_armor", "NoDLC", 1, 0, 1, "", "", "A_snurtles", "", "防御力低，爆率高，使用后可以钻进壳里躲避怪物攻击仇恨。", {0, 0, 0, 60, 0, 0, 0, 1050, 0, 0, 0})
    End Sub

    Private Sub button_G_tam_o_shanter_click(sender As Object, e As RoutedEventArgs) Handles button_G_tam_o_shanter.Click
        G_Show_E("贝雷帽", "Tam o' Shanter", "G_tam_o'shanter", "NoDLC", 1, 0, 1, "", "", "A_mactusk", "", "只能打海象掉落。", {0, 0, 0, 0, 0, 0, 25, 0, 0, 120, 6.66})
    End Sub

    Private Sub button_G_spiderhat_click(sender As Object, e As RoutedEventArgs) Handles button_G_spiderhat.Click
        G_Show_E("蜘蛛帽", "Spiderhat", "G_spiderhat", "NoDLC", 1, 0, 1, "控制蜘蛛", "", "A_spider_queen", "", "只能打蜘蛛女王掉落，可以控制蜘蛛。", {0, 0, 0, 0, 0, 120, 0, 0, 0, 0, -2})
    End Sub

    Private Sub button_G_slurper_click(sender As Object, e As RoutedEventArgs) Handles button_G_slurper.Click
        G_Show_E("缀食者", "Slurper", "G_slurper", "NoDLC", 1, 0, 1, "提供和矿工帽相同的光照", "", "A_slurper", "", "戴在头上会发光，吸食饥饿。", {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_krampus_sack_click(sender As Object, e As RoutedEventArgs) Handles button_G_krampus_sack.Click
        G_Show_E("坎普斯背包", "Krampus Sack", "G_krampus_sack", "NoDLC", 1, 1, 1, "14格背包", "", "A_krampus", "A_krampus_sw", "击杀坎普斯极小几率获得。", {0, 0, 0, 0, 0, 1000, 0, 0, 0, 0, 0})
    End Sub

    Private Sub button_G_portable_crock_pot_click(sender As Object, e As RoutedEventArgs) Handles button_G_portable_crock_pot.Click
        G_Show_E("便携式烹饪锅", "Portable Crock Pot", "G_portable_crock_pot", "SW", 0, 1, 0, "沃利专属，可以额外制作四种食谱", "可移动", "", "", "在潮水中会失效。", {0, 0, 0, 0, 0, 1000, 0, 0, 0, 0, 0})
    End Sub

    REM ------------------左侧面板(物品_树苗)------------------
    Private Sub G_Show_S(G_Name As String, G_EnName As String, G_picture As String, G_DLC As String, G_DLC_ROG As SByte, G_DLC_SW As SByte, G_DLC_DST As SByte, G_Introduce As String, ParamArray FromPlant() As String)
        REM ------------------初始化------------------
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_Sapling.Visibility = Visibility.Visible
        REM ------------------物品名字------------------
        GL_textBlock_GoodsName_S.Text = G_Name
        GL_textBlock_GoodsName_S.UpdateLayout()
        Dim G_N_MarginLeft As Integer
        G_N_MarginLeft = (Canvas_GoodsLeft_S.ActualWidth - GL_textBlock_GoodsName_S.ActualWidth) / 2
        Dim G_N_T As New Thickness()
        G_N_T.Top = 80
        G_N_T.Left = G_N_MarginLeft
        GL_textBlock_GoodsName_S.Margin = G_N_T

        GL_textBlock_GoodsEnName_S.Text = G_EnName
        GL_textBlock_GoodsEnName_S.UpdateLayout()
        Dim G_EnN_MarginLeft As Integer
        G_EnN_MarginLeft = (Canvas_GoodsLeft_S.ActualWidth - GL_textBlock_GoodsEnName_S.ActualWidth) / 2
        Dim G_EnN_T As New Thickness()
        G_EnN_T.Top = 100
        G_EnN_T.Left = G_EnN_MarginLeft
        GL_textBlock_GoodsEnName_S.Margin = G_EnN_T
        REM ------------------物品图片------------------
        GL_image_GoodsPicture_S.Source = Picture_Short_Name(Res_Short_Name(G_picture))
        REM ------------------物品DLC-------------------
        If G_DLC = "ROG" Then
            GL_image_GS_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf G_DLC = "SW" Then
            GL_image_GS_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf G_DLC = "DST" Then
            GL_image_GS_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            GL_image_GS_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------存在版本-------------------
        GL_textBlock_GS_DLC_1.Foreground = Brushes.Black
        GL_textBlock_GS_DLC_2.Foreground = Brushes.Black
        GL_textBlock_GS_DLC_3.Foreground = Brushes.Black
        If G_DLC_ROG = 0 Then
            GL_textBlock_GS_DLC_1.Foreground = Brushes.Silver
        End If
        If G_DLC_SW = 0 Then
            GL_textBlock_GS_DLC_2.Foreground = Brushes.Silver
        End If
        If G_DLC_DST = 0 Then
            GL_textBlock_GS_DLC_3.Foreground = Brushes.Silver
        End If
        REM ------------------物品简介-------------------
        TextBlock_GS_Introduce.Text = G_Introduce
        TextBlock_GS_Introduce.Height = SetTextBlockHeight(G_Introduce, 13)
        G_WrapPanel_Introduce_S.Height = SetTextBlockHeight(G_Introduce, 13)
        REM -----------来源于植物按钮位置调整------------
        image_GS_Science_1.Source = Picture_Short_Name(Res_Short_Name(FromPlant(0)))
        GS_Sapling_Select_1 = FromPlant(0)
        If FromPlant.Length = 1 Then
            button_GS_Plant_2.Visibility = Visibility.Collapsed
            Dim G_Plant_T As New Thickness()
            G_Plant_T.Top = 0
            G_Plant_T.Left = 57
            button_GS_Plant_1.Margin = G_Plant_T
        Else
            button_GS_Plant_2.Visibility = Visibility.Visible
            image_GS_Science_2.Source = Picture_Short_Name(Res_Short_Name(FromPlant(1)))
            GS_Sapling_Select_2 = FromPlant(1)
            Dim G_Plant_T As New Thickness()
            G_Plant_T.Top = 0
            G_Plant_T.Left = 24
            button_GS_Plant_1.Margin = G_Plant_T
        End If
    End Sub

    Private Sub button_GS_Plant_1_click(sender As Object, e As RoutedEventArgs) Handles button_GS_Plant_1.Click
        ButtonJump(GS_Sapling_Select_1)
    End Sub

    Private Sub button_GS_Plant_2_click(sender As Object, e As RoutedEventArgs) Handles button_GS_Plant_2.Click
        ButtonJump(GS_Sapling_Select_2)
    End Sub

    Private Sub button_G_birchnut_click(sender As Object, e As RoutedEventArgs) Handles button_G_birchnut.Click
        G_Show_S("坚果", "Birchnut", "G_birchnut", "NoDLC", 1, 0, 1, "砍伐桦树获得。", {"N_birchnut_tree_1"})
    End Sub

    Private Sub button_G_pine_cone_click(sender As Object, e As RoutedEventArgs) Handles button_G_pine_cone.Click
        G_Show_S("松果", "Pine Cone", "G_pine_cone", "NoDLC", 1, 0, 1, "砍伐常青树获得。", {"N_evergreen_1"})
    End Sub

    Private Sub button_G_sapling_click(sender As Object, e As RoutedEventArgs) Handles button_G_sapling.Click
        G_Show_S("树苗", "Sapling", "G_sapling", "NoDLC", 1, 0, 1, "挖取树苗或患病的树苗获得。", {"N_sapling", "N_sapling_diseased"})
    End Sub

    Private Sub button_G_grass_tuft_click(sender As Object, e As RoutedEventArgs) Handles button_G_grass_tuft.Click
        G_Show_S("长草簇", "Grass Tuft", "G_grass_tuft", "NoDLC", 1, 0, 1, "挖取草或患病的草获得。", {"N_grass", "N_grass_diseased"})
    End Sub

    Private Sub button_G_berry_bush_click(sender As Object, e As RoutedEventArgs) Handles button_G_berry_bush.Click
        G_Show_S("浆果灌木丛", "Berry Bush", "G_berry_bush", "NoDLC", 1, 0, 1, "挖取浆果灌木丛或患病的浆果灌木丛获得。", {"N_berry_bush_1", "N_berry_bush_diseased"})
    End Sub

    Private Sub button_G_berry_bush_2_click(sender As Object, e As RoutedEventArgs) Handles button_G_berry_bush_2.Click
        G_Show_S("浆果灌木丛", "Berry Bush", "G_berry_bush_2", "NoDLC", 1, 0, 1, "挖取浆果灌木丛获得。", {"N_berry_bush_2"})
    End Sub

    Private Sub button_G_spiky_bushes_click(sender As Object, e As RoutedEventArgs) Handles button_G_spiky_bushes.Click
        G_Show_S("尖刺灌木", "Spiky Bushes", "G_spiky_bushes", "NoDLC", 1, 0, 1, "挖取尖刺灌木获得。", {"N_spiky_bush"})
    End Sub

    Private Sub button_G_bamboo_root_click(sender As Object, e As RoutedEventArgs) Handles button_G_bamboo_root.Click
        G_Show_S("竹根", "Bamboo Root", "G_bamboo_root", "SW", 0, 1, 0, "挖取竹子获得。", {"N_bamboo_patch"})
    End Sub

    Private Sub button_G_grass_tuft_SW_click(sender As Object, e As RoutedEventArgs) Handles button_G_grass_tuft_SW.Click
        G_Show_S("船难长草簇", "Grass Tuft", "G_grass_tuft_SW", "SW", 0, 1, 0, "挖取草获得。", {"N_grass_sw"})
    End Sub

    Private Sub button_G_coconut_click(sender As Object, e As RoutedEventArgs) Handles button_G_coconut.Click
        G_Show_S("椰子", "Coconut", "F_coconut", "SW", 0, 1, 0, "砍伐椰子树获得。", {"N_palm_tree_1"})
    End Sub

    Private Sub button_G_jungle_tree_seed_click(sender As Object, e As RoutedEventArgs) Handles button_G_jungle_tree_seed.Click
        G_Show_S("丛林树种", "Jungle Tree Seed", "G_jungle_tree_seed", "SW", 0, 1, 0, "砍伐丛林树获得。", {"N_jungle_tree_1"})
    End Sub

    Private Sub button_G_viney_bush_root_click(sender As Object, e As RoutedEventArgs) Handles button_G_viney_bush_root.Click
        G_Show_S("藤蔓根", "Viney Bush Root", "G_viney_bush_root", "SW", 0, 1, 0, "挖取藤蔓丛获得。", {"N_viney_bush"})
    End Sub

    Private Sub button_G_coffee_plant_click(sender As Object, e As RoutedEventArgs) Handles button_G_coffee_plant.Click
        G_Show_S("咖啡树", "Coffee Plant", "G_coffee_plant", "SW", 0, 1, 0, "挖取咖啡树获得。", {"N_coffee_plant"})
    End Sub

    Private Sub button_G_elephant_cactus_stump_click(sender As Object, e As RoutedEventArgs) Handles button_G_elephant_cactus_stump.Click
        G_Show_S("象仙人掌根", "Elephant Cactus Stump", "G_elephant_cactus_stump", "SW", 0, 1, 0, "挖取象仙人掌获得。", {"N_elephant_cactus"})
    End Sub

    Private Sub button_G_juicy_berry_bush_click(sender As Object, e As RoutedEventArgs) Handles button_G_juicy_berry_bush.Click
        G_Show_S("蜜汁浆果丛", "Juicy Berry Bush", "G_juicy_berry_bush", "DST", 0, 0, 1, "挖取蜜汁浆果丛或患病的蜜汁浆果丛获得。", {"N_juicy_berry_bush", "N_juicy_berry_bush_diseased"})
    End Sub

    Private Sub button_G_twiggy_tree_cone_click(sender As Object, e As RoutedEventArgs) Handles button_G_twiggy_tree_cone.Click
        G_Show_S("多枝的树的果实", "Twiggy Tree Cone", "G_twiggy_tree_cone", "DST", 0, 0, 1, "砍伐多枝的树或患病的多枝的树获得。", {"N_twiggy_tree_1", "N_twiggy_tree_diseased_1"})
    End Sub

    Private GS_Animal_Select_1 As String
    Private GS_Animal_Select_2 As String
    Private GS_Animal_Select_3 As String
    Private GS_Animal_Food_Select As String

    REM ------------------左侧面板(物品_生物)------------------
    Private Sub G_Show_A(G_Name As String, G_EnName As String, G_picture As String, G_DLC As String, G_DLC_ROG As SByte, G_DLC_SW As SByte, G_DLC_DST As SByte, G_Food As String, G_Introduce As String, ParamArray G_murder() As String)
        REM ------------------初始化------------------
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_Animal.Visibility = Visibility.Visible
        G_Canvas_Perish_A.Visibility = Visibility.Visible
        G_WrapPanel_Food_A.Visibility = Visibility.Collapsed
        G_Canvas_FoodButton_A.Visibility = Visibility.Collapsed
        REM ------------------物品名字------------------
        GL_textBlock_GoodsName_A.Text = G_Name
        GL_textBlock_GoodsName_A.UpdateLayout()
        Dim G_N_MarginLeft As Integer
        G_N_MarginLeft = (Canvas_GoodsLeft_A.ActualWidth - GL_textBlock_GoodsName_A.ActualWidth) / 2
        Dim G_N_T As New Thickness()
        G_N_T.Top = 80
        G_N_T.Left = G_N_MarginLeft
        GL_textBlock_GoodsName_A.Margin = G_N_T

        GL_textBlock_GoodsEnName_A.Text = G_EnName
        GL_textBlock_GoodsEnName_A.UpdateLayout()
        Dim G_EnN_MarginLeft As Integer
        G_EnN_MarginLeft = (Canvas_GoodsLeft_A.ActualWidth - GL_textBlock_GoodsEnName_A.ActualWidth) / 2
        Dim G_EnN_T As New Thickness()
        G_EnN_T.Top = 100
        G_EnN_T.Left = G_EnN_MarginLeft
        GL_textBlock_GoodsEnName_A.Margin = G_EnN_T
        REM ------------------物品图片------------------
        GL_image_GoodsPicture_A.Source = Picture_Short_Name(Res_Short_Name(G_picture))
        REM ------------------物品DLC-------------------
        If G_DLC = "ROG" Then
            GL_image_GA_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf G_DLC = "SW" Then
            GL_image_GA_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf G_DLC = "DST" Then
            GL_image_GA_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            GL_image_GA_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------存在版本-------------------
        GL_textBlock_GA_DLC_1.Foreground = Brushes.Black
        GL_textBlock_GA_DLC_2.Foreground = Brushes.Black
        GL_textBlock_GA_DLC_3.Foreground = Brushes.Black
        If G_DLC_ROG = 0 Then
            GL_textBlock_GA_DLC_1.Foreground = Brushes.Silver
        End If
        If G_DLC_SW = 0 Then
            GL_textBlock_GA_DLC_2.Foreground = Brushes.Silver
        End If
        If G_DLC_DST = 0 Then
            GL_textBlock_GA_DLC_3.Foreground = Brushes.Silver
        End If
        REM -----------------杀害后获得------------------        
        G_WrapPanel_Murder_A.Visibility = Visibility.Visible
        G_Canvas_MurderButton_A.Visibility = Visibility.Visible
        button_GA_MurderButton_2.Visibility = Visibility.Collapsed
        TextBlock_GA_Murder_2.Visibility = Visibility.Collapsed
        button_GA_MurderButton_3.Visibility = Visibility.Collapsed
        TextBlock_GA_Murder_3.Visibility = Visibility.Collapsed
        If G_murder.Length > 4 Then
            GS_Animal_Select_1 = G_murder(0)
            GS_Animal_Select_2 = G_murder(2)
            GS_Animal_Select_3 = G_murder(4)
            button_GA_MurderButton_2.Visibility = Visibility.Visible
            TextBlock_GA_Murder_2.Visibility = Visibility.Visible
            button_GA_MurderButton_3.Visibility = Visibility.Visible
            TextBlock_GA_Murder_3.Visibility = Visibility.Visible
            image_GA_Murder_1.Source = Picture_Short_Name(Res_Short_Name(G_murder(0)))
            TextBlock_GA_Murder_1.Text = "×" & G_murder(1)
            image_GA_Murder_2.Source = Picture_Short_Name(Res_Short_Name(G_murder(2)))
            TextBlock_GA_Murder_2.Text = "×" & G_murder(3)
            image_GA_Murder_3.Source = Picture_Short_Name(Res_Short_Name(G_murder(4)))
            TextBlock_GA_Murder_3.Text = "×" & G_murder(5)
            G_Canvas_MurderButton_A.Height = 118
        ElseIf G_murder.Length > 2 Then
            GS_Animal_Select_1 = G_murder(0)
            GS_Animal_Select_2 = G_murder(2)
            button_GA_MurderButton_2.Visibility = Visibility.Visible
            TextBlock_GA_Murder_2.Visibility = Visibility.Visible
            image_GA_Murder_1.Source = Picture_Short_Name(Res_Short_Name(G_murder(0)))
            TextBlock_GA_Murder_1.Text = "×" & G_murder(1)
            image_GA_Murder_2.Source = Picture_Short_Name(Res_Short_Name(G_murder(2)))
            TextBlock_GA_Murder_2.Text = "×" & G_murder(3)
            G_Canvas_MurderButton_A.Height = 84
        ElseIf G_murder.Length > 0 Then
            GS_Animal_Select_1 = G_murder(0)
            image_GA_Murder_1.Source = Picture_Short_Name(Res_Short_Name(G_murder(0)))
            TextBlock_GA_Murder_1.Text = "×" & G_murder(1)
            G_Canvas_MurderButton_A.Height = 50
        Else
            G_Canvas_Perish_A.Visibility = Visibility.Collapsed
            G_WrapPanel_Murder_A.Visibility = Visibility.Collapsed
            G_Canvas_MurderButton_A.Visibility = Visibility.Collapsed
        End If
        REM ------------------食物详情-------------------
        If G_Food <> "" Then
            GS_Animal_Food_Select = G_Food
            G_WrapPanel_Food_A.Visibility = Visibility.Visible
            G_Canvas_FoodButton_A.Visibility = Visibility.Visible
            image_GA_Food.Source = Picture_Short_Name(Res_Short_Name(G_Food))
        End If
        REM ------------------物品简介-------------------
        TextBlock_GA_Introduce.Text = G_Introduce
        TextBlock_GA_Introduce.Height = SetTextBlockHeight(G_Introduce, 13)
        G_WrapPanel_Introduce_A.Height = SetTextBlockHeight(G_Introduce, 13)
    End Sub

    Private Sub button_GA_MurderButton_1_click(sender As Object, e As RoutedEventArgs) Handles button_GA_MurderButton_1.Click
        ButtonJump(GS_Animal_Select_1)
    End Sub

    Private Sub button_GA_MurderButton_2_click(sender As Object, e As RoutedEventArgs) Handles button_GA_MurderButton_2.Click
        ButtonJump(GS_Animal_Select_2)
    End Sub

    Private Sub button_GA_MurderButton_3_click(sender As Object, e As RoutedEventArgs) Handles button_GA_MurderButton_3.Click
        ButtonJump(GS_Animal_Select_3)
    End Sub

    Private Sub button_GA_FoodButton_click(sender As Object, e As RoutedEventArgs) Handles button_GA_FoodButton.Click
        ButtonJump(GS_Animal_Food_Select)
    End Sub

    Private Sub button_G_rabbit_click(sender As Object, e As RoutedEventArgs) Handles button_G_rabbit.Click
        G_Show_A("兔子", "Rabbit", "A_rabbit", "NoDLC", 1, 0, 1, "", "", {"F_morsel", "1"})
    End Sub

    Private Sub button_G_winter_rabbit_click(sender As Object, e As RoutedEventArgs) Handles button_G_winter_rabbit.Click
        G_Show_A("雪兔", "Winter Rabbit", "A_rabbit_winter", "NoDLC", 1, 0, 1, "", "", {"F_morsel", "1"})
    End Sub

    Private Sub button_G_beardling_click(sender As Object, e As RoutedEventArgs) Handles button_G_beardling.Click
        G_Show_A("黑兔子", "Rabbit", "A_beardling", "NoDLC", 1, 0, 1, "", "", {"F_monster_meat", "1(40%)", "G_nightmare_fuel", "1(40%)", "G_beard_hair", "1(20%)"})
    End Sub

    Private Sub button_G_moleworm_click(sender As Object, e As RoutedEventArgs) Handles button_G_moleworm.Click
        G_Show_A("鼹鼠", "Moleworm", "F_moleworm", "NoDLC", 1, 0, 1, "", "", {"F_morsel", "1"})
    End Sub

    Private Sub button_G_bee_click(sender As Object, e As RoutedEventArgs) Handles button_G_bee.Click
        G_Show_A("蜜蜂", "Bee", "A_bee", "NoDLC", 1, 0, 1, "", "", {"G_stinger", "1(80%)", "F_honey", "1(20%)"})
    End Sub

    Private Sub button_G_killer_bee_click(sender As Object, e As RoutedEventArgs) Handles button_G_killer_bee.Click
        G_Show_A("杀人蜂", "Killer Bee", "A_killer_bee", "NoDLC", 1, 0, 1, "", "", {"G_stinger", "1(80%)", "F_honey", "1(20%)"})
    End Sub

    Private Sub button_G_butterfly_click(sender As Object, e As RoutedEventArgs) Handles button_G_butterfly.Click
        G_Show_A("蝴蝶", "Butterfly", "A_butterfly", "NoDLC", 1, 0, 1, "", "", {"F_butterfly_wing", "1(98%)", "F_butter", "1(2%)"})
    End Sub

    Private Sub button_G_mosquito_click(sender As Object, e As RoutedEventArgs) Handles button_G_mosquito.Click
        G_Show_A("蚊子", "Mosquito", "A_mosquito", "NoDLC", 1, 0, 1, "", "", {"G_mosquito_sack", "1(50%)"})
    End Sub

    Private Sub button_G_redbird_click(sender As Object, e As RoutedEventArgs) Handles button_G_redbird.Click
        G_Show_A("红雀", "Redbird", "A_redbird", "NoDLC", 1, 0, 1, "", "可以囚禁在鸟笼里。", {"F_morsel", "1(50%)", "G_crimson_feather", "1(50%)"})
    End Sub

    Private Sub button_G_snowbird_click(sender As Object, e As RoutedEventArgs) Handles button_G_snowbird.Click
        G_Show_A("雪雀", "Snowbird", "A_snowbird", "NoDLC", 1, 0, 1, "", "可以囚禁在鸟笼里。", {"F_morsel", "1(50%)", "G_azure_feather", "1(50%)"})
    End Sub

    Private Sub button_G_crow_click(sender As Object, e As RoutedEventArgs) Handles button_G_crow.Click
        G_Show_A("乌鸦", "Crow", "A_crow", "NoDLC", 1, 0, 1, "", "可以囚禁在鸟笼里。", {"F_morsel", "1(50%)", "G_jet_feather", "1(50%)"})
    End Sub

    Private Sub button_G_fireflies_click(sender As Object, e As RoutedEventArgs) Handles button_G_fireflies.Click
        G_Show_A("萤火虫", "Fireflies", "G_fireflies", "NoDLC", 1, 0, 1, "", "可以重新释放以获得微量的光，还可以为矿工帽、提灯、水瓶提灯和船灯添加燃料。", {})
    End Sub

    Private Sub button_G_crabbit_click(sender As Object, e As RoutedEventArgs) Handles button_G_crabbit.Click
        G_Show_A("兔蟹", "Crabbit", "A_crabbit", "SW", 0, 1, 0, "", "", {"F_fish_morsel", "1"})
    End Sub

    Private Sub button_G_beardling_sw_click(sender As Object, e As RoutedEventArgs) Handles button_G_beardling_sw.Click
        G_Show_A("黑兔蟹", "Crabbit", "A_beardling_sw", "SW", 0, 1, 0, "", "", {"F_monster_meat", "1(40%)", "G_nightmare_fuel", "1(40%)", "G_beard_hair", "1(20%)"})
    End Sub

    Private Sub button_G_dead_dogfish_click(sender As Object, e As RoutedEventArgs) Handles button_G_dead_dogfish.Click
        G_Show_A("死狗鱼", "Dead Dogfish", "F_dead_dogfish", "SW", 0, 1, 0, "F_dead_dogfish", "", {})
    End Sub

    Private Sub button_G_dead_swordfish_click(sender As Object, e As RoutedEventArgs) Handles button_G_dead_swordfish.Click
        G_Show_A("死旗鱼", "Dead Swordfish", "F_dead_swordfish", "SW", 0, 1, 0, "F_dead_swordfish", "", {})
    End Sub

    Private Sub button_G_dead_wobster_click(sender As Object, e As RoutedEventArgs) Handles button_G_dead_wobster.Click
        G_Show_A("死龙虾", "Dead Wobster", "F_dead_wobster", "SW", 0, 1, 0, "F_dead_wobster", "", {})
    End Sub

    Private Sub button_G_bioluminescence_click(sender As Object, e As RoutedEventArgs) Handles button_G_bioluminescence.Click
        G_Show_A("海洋生物", "Bioluminescence", "G_bioluminescence", "SW", 0, 1, 0, "", "可以重新释放以获得微量的光。", {})
    End Sub

    Private Sub button_G_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_G_jellyfish.Click
        G_Show_A("水母", "Jellyfish", "F_jellyfish", "SW", 0, 1, 0, "F_jellyfish", "", {})
    End Sub

    Private Sub button_G_dead_jellyfish_click(sender As Object, e As RoutedEventArgs) Handles button_G_dead_jellyfish.Click
        G_Show_A("死水母", "Dead jellyfish", "F_dead_jellyfish", "SW", 0, 1, 0, "F_dead_jellyfish", "", {})
    End Sub

    Private Sub button_G_butterfly_sw_click(sender As Object, e As RoutedEventArgs) Handles button_G_butterfly_sw.Click
        G_Show_A("船难蝴蝶", "Butterfly", "A_butterfly_sw", "SW", 0, 1, 0, "", "", {"F_butterfly_wing_sw", "1(98%)", "F_butter", "1(2%)"})
    End Sub

    Private Sub button_G_mosquito_sw_click(sender As Object, e As RoutedEventArgs) Handles button_G_mosquito_sw.Click
        G_Show_A("毒蚊子", "Poison Mosquito", "A_mosquito_sw", "SW", 0, 1, 0, "", "", {"G_yellow_mosquito_sack", "1(50%)"})
    End Sub

    Private Sub button_G_parrot_click(sender As Object, e As RoutedEventArgs) Handles button_G_parrot.Click
        G_Show_A("鹦鹉", "Parrot", "A_parrot", "SW", 0, 1, 0, "", "可以囚禁在鸟笼里。", {"F_morsel", "1(50%)", "G_crimson_feather", "1(50%)"})
    End Sub

    Private Sub button_G_parrot_pirate_click(sender As Object, e As RoutedEventArgs) Handles button_G_parrot_pirate.Click
        G_Show_A("海盗鹦鹉", "Parrot Pirate", "A_parrot_pirate", "SW", 0, 1, 0, "", "可以囚禁在鸟笼里。", {"F_morsel", "1(50%)", "G_crimson_feather", "1(50%)"})
    End Sub

    Private Sub button_G_toucan_click(sender As Object, e As RoutedEventArgs) Handles button_G_toucan.Click
        G_Show_A("大嘴鸟", "Toucan", "A_toucan", "SW", 0, 1, 0, "", "可以囚禁在鸟笼里。", {"F_morsel", "1(50%)", "G_jet_feather", "1(50%)"})
    End Sub

    Private Sub button_G_seagull_click(sender As Object, e As RoutedEventArgs) Handles button_G_seagull.Click
        G_Show_A("海鸥", "Seagull", "A_seagull", "SW", 0, 1, 0, "", "可以囚禁在鸟笼里。", {"F_morsel", "1(50%)", "G_azure_feather", "1(50%)"})
    End Sub

    Private Sub button_G_blue_spore_click(sender As Object, e As RoutedEventArgs) Handles button_G_blue_spore.Click
        G_Show_A("蓝色孢子", "Blue Spore", "A_blue_spore", "DST", 0, 0, 1, "", "可以重新释放以获得微量的光。", {})
    End Sub

    Private Sub button_G_green_spore_click(sender As Object, e As RoutedEventArgs) Handles button_G_green_spore.Click
        G_Show_A("绿色孢子", "Green Spore", "A_green_spore", "DST", 0, 0, 1, "", "可以重新释放以获得微量的光。", {})
    End Sub

    Private Sub button_G_red_spore_click(sender As Object, e As RoutedEventArgs) Handles button_G_red_spore.Click
        G_Show_A("红色孢子", "Red Spore", "A_red_spore", "DST", 0, 0, 1, "", "可以重新释放以获得微量的光。", {})
    End Sub

    REM ------------------左侧面板(物品_草皮)------------------
    Private Sub G_Show_T(G_Name As String, G_EnName As String, G_picture As String, G_DLC As String, G_DLC_ROG As SByte, G_DLC_SW As SByte, G_DLC_DST As SByte, G_Texture As String, G_Introduce As String, Optional G_Craft As String = "")
        REM ------------------初始化------------------
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_Turf.Visibility = Visibility.Visible
        G_WrapPanel_Craft_T.Visibility = Visibility.Collapsed
        G_Canvas_CraftButton_T.Visibility = Visibility.Collapsed
        REM ------------------物品名字------------------
        GL_textBlock_GoodsName_T.Text = G_Name
        GL_textBlock_GoodsName_T.UpdateLayout()
        Dim G_N_MarginLeft As Integer
        G_N_MarginLeft = (Canvas_GoodsLeft_T.ActualWidth - GL_textBlock_GoodsName_T.ActualWidth) / 2
        Dim G_N_T As New Thickness()
        G_N_T.Top = 80
        G_N_T.Left = G_N_MarginLeft
        GL_textBlock_GoodsName_T.Margin = G_N_T

        GL_textBlock_GoodsEnName_T.Text = G_EnName
        GL_textBlock_GoodsEnName_T.UpdateLayout()
        Dim G_EnN_MarginLeft As Integer
        G_EnN_MarginLeft = (Canvas_GoodsLeft_T.ActualWidth - GL_textBlock_GoodsEnName_T.ActualWidth) / 2
        Dim G_EnN_T As New Thickness()
        G_EnN_T.Top = 100
        G_EnN_T.Left = G_EnN_MarginLeft
        GL_textBlock_GoodsEnName_T.Margin = G_EnN_T
        REM ------------------物品图片------------------
        GL_image_GoodsPicture_T.Source = Picture_Short_Name(Res_Short_Name(G_picture))
        REM ------------------物品DLC-------------------
        If G_DLC = "ROG" Then
            GL_image_GT_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf G_DLC = "SW" Then
            GL_image_GT_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf G_DLC = "DST" Then
            GL_image_GT_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            GL_image_GT_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------存在版本-------------------
        GL_textBlock_GT_DLC_1.Foreground = Brushes.Black
        GL_textBlock_GT_DLC_2.Foreground = Brushes.Black
        GL_textBlock_GT_DLC_3.Foreground = Brushes.Black
        If G_DLC_ROG = 0 Then
            GL_textBlock_GT_DLC_1.Foreground = Brushes.Silver
        End If
        If G_DLC_SW = 0 Then
            GL_textBlock_GT_DLC_2.Foreground = Brushes.Silver
        End If
        If G_DLC_DST = 0 Then
            GL_textBlock_GT_DLC_3.Foreground = Brushes.Silver
        End If
        REM ------------------制作科技-------------------
        If G_Craft <> "" Then
            G_WrapPanel_Craft_T.Visibility = Visibility.Visible
            G_Canvas_CraftButton_T.Visibility = Visibility.Visible
            GS_Turf_Select = G_Craft
            image_GT_Craft.Source = Picture_Short_Name(Res_Short_Name(G_Craft))
        End If
        REM ------------------草皮纹理-------------------
        image_GT_TurfTexture.Source = Picture_Short_Name(Res_Short_Name(G_Texture))
        REM ------------------物品简介-------------------
        TextBlock_GT_Introduce.Text = G_Introduce
        TextBlock_GT_Introduce.Height = SetTextBlockHeight(G_Introduce, 13)
        G_WrapPanel_Introduce_T.Height = SetTextBlockHeight(G_Introduce, 13)
    End Sub

    Private Sub button_GT_CraftButton_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GT_CraftButton.Click
        ButtonJump(GS_Turf_Select)
    End Sub

    Private Sub button_G_cobblestones_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_cobblestones.Click
        G_Show_T("卵石路", "Cobblestones", "S_cobblestones", "NoDLC", 1, 1, 1, "Texture_cobblestone", "使玩家和所有生物加速30%，不能在上面种植植物，阻止食人花和眼球草的生长，当铺设的卵石路足够大时，附近只会出现乌鸦。", "S_cobblestones")
    End Sub

    Private Sub button_G_wooden_flooring_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_wooden_flooring.Click
        G_Show_T("木质地板", "Wooden Flooring", "S_wooden_flooring", "NoDLC", 1, 1, 1, "Texture_wooden_flooring", "猪王和试金石所在地的草皮，不能在上面种植植物，阻止食人花和眼球草的生长，当铺设的木质地板足够大时，附近只会出现乌鸦。", "S_wooden_flooring")
    End Sub

    Private Sub button_G_checkered_flooring_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_checkered_flooring.Click
        G_Show_T("方格地板", "Checkered Flooring", "S_checkered_flooring", "NoDLC", 1, 0, 1, "Texture_checkered_flooring", "出现在棋盘区域，不能在上面种植植物，阻止食人花和眼球草的生长，当铺设的方格地板足够大时，附近只会出现乌鸦。在船难版中，不会产生水坑，但是依然会被潮汐和洪水淹没。", "S_checkered_flooring")
    End Sub

    Private Sub button_G_carpeted_flooring_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_carpeted_flooring.Click
        G_Show_T("地毯地板", "Carpeted Flooring", "S_carpeted_flooring", "NoDLC", 1, 0, 1, "Texture_carpeted_flooring", "出现在棋盘区域，不能在上面种植植物，阻止食人花和眼球草的生长，当铺设的地毯地板足够大时，附近只会出现乌鸦。在船难版中，不会产生水坑，但是依然会被潮汐和洪水淹没。", "S_carpeted_flooring")
    End Sub

    Private Sub button_G_scaled_flooring_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_scaled_flooring.Click
        G_Show_T("龙鳞地板", "Scaled Flooring", "S_scaled_flooring", "DST", 0, 0, 1, "Texture_scaled_flooring", "仅联机版里有的草皮，能够减缓火势蔓延。", "S_scaled_flooring")
    End Sub

    Private Sub button_G_snakeskin_rug_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_snakeskin_rug.Click
        G_Show_T("蛇皮地毯", "Snakeskin Rug", "S_snakeskin_rug", "SW", 0, 1, 0, "Texture_snakeskin_rug", "仅船难版里有的草皮，阻止食人花和眼球草的生长，不会产生水坑，但是依然会被潮汐和洪水淹没。", "S_snakeskin_rug")
    End Sub

    Private Sub button_G_deciduous_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_deciduous_turf.Click
        G_Show_T("季节性草地", "Deciduous Turf", "G_deciduous_turf", "NoDLC", 1, 0, 1, "Texture_deciduous_turf", "出现在落叶阔叶林区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的季节性草地足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_forest_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_forest_turf.Click
        G_Show_T("森林草皮", "Forest Turf", "G_forest_turf", "NoDLC", 1, 0, 1, "Texture_forest_turf", "出现在森林区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的森林草皮足够大时，附近只会出现乌鸦、红雀和雪雀。")
    End Sub

    Private Sub button_G_grass_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_grass_turf.Click
        G_Show_T("长草草皮", "Grass Turf", "G_grass_turf", "NoDLC", 1, 0, 1, "Texture_grass_turf", "出现在草原区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的长草草皮足够大时，附近只会出现红雀和雪雀。")
    End Sub

    Private Sub button_G_jungle_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_jungle_turf.Click
        G_Show_T("丛林草皮", "Jungle Turf", "G_jungle_turf", "SW", 0, 1, 0, "Texture_jungle_turf", "出现在热带雨林区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的丛林草皮足够大时，附近只会出现鹦鹉、海盗鹦鹉和海鸥。")
    End Sub

    Private Sub button_G_magma_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_magma_turf.Click
        G_Show_T("岩浆地皮", "Magma Turf", "G_magma_turf", "SW", 0, 1, 0, "Texture_magma_turf", "出现在岩浆领域区域，不能在上面种植植物，但是可以种植食人花(眼球草不会生长)和咖啡树，当铺设的岩浆地皮足够大时，附近只会出现大嘴鸟。")
    End Sub

    Private Sub button_G_marsh_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_marsh_turf.Click
        G_Show_T("沼泽草皮", "Marsh Turf", "G_marsh_turf", "NoDLC", 1, 0, 1, "Texture_marsh_turf", "出现在沼泽区域，不能在上面种植植物，阻止食人花和眼球草的生长，当铺设的沼泽草皮足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_meadow_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_meadow_turf.Click
        G_Show_T("草甸草皮", "Meadow Turf", "G_meadow_turf", "NoDLC", 1, 0, 1, "Texture_meadow_turf", "出现在草甸区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的草甸草皮足够大时，附近只会出现大嘴鸟。")
    End Sub

    Private Sub button_G_rocky_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_rocky_turf.Click
        G_Show_T("岩石草皮", "Rocky Turf", "G_rocky_turf", "NoDLC", 1, 0, 1, "Texture_rocky_turf", "出现在矿区区域，不能在上面种植植物，但是可以种植食人花(眼球草不会生长)，当铺设的岩石草皮足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_sandy_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_sandy_turf.Click
        G_Show_T("沙地", "Sandy Turf", "G_sandy_turf", "NoDLC", 1, 0, 1, "Texture_sandy_turf", "出现在沙漠区域，不能在上面种植植物，但是可以种植食人花(眼球草不会生长)，当铺设的沙地足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_savanna_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_savanna_turf.Click
        G_Show_T("热带草原草皮", "Savanna Turf", "G_savanna_turf", "NoDLC", 1, 0, 1, "Texture_savanna_turf", "出现在稀树大草原区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的热带草原草皮足够大时，附近只会出现乌鸦、红雀和雪雀。")
    End Sub

    Private Sub button_G_tidal_marsh_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_tidal_marsh_turf.Click
        G_Show_T("潮滩地皮", "Tidal Marsh", "G_tidal_marsh_turf", "SW", 0, 1, 0, "Texture_tidal_marsh_turf", "出现在潮汐沼泽区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的潮滩地皮足够大时，附近只会出现大嘴鸟。")
    End Sub

    Private Sub button_G_cave_rock_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_cave_rock_turf.Click
        G_Show_T("洞穴石地", "Cave Rock Turf", "G_cave_rock_turf", "NoDLC", 1, 0, 1, "Texture_cave_rock_turf", "出现在岩石平原和石笋生物群落区域，不能在上面种植植物，阻止食人花和眼球草的生长，当铺设的洞穴石地足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_fungal_turf_blue_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_fungal_turf_blue.Click
        G_Show_T("菌类草地(蓝)", "Fungal Turf", "G_fungal_turf_blue", "NoDLC", 1, 0, 1, "Texture_fungal_turf_blue", "出现在蘑菇树森林区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的菌类草地足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_fungal_turf_green_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_fungal_turf_green.Click
        G_Show_T("菌类草地(绿)", "Fungal Turf", "G_fungal_turf_green", "NoDLC", 1, 0, 1, "Texture_fungal_turf_green", "出现在蘑菇树森林区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的菌类草地足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_fungal_turf_red_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_fungal_turf_red.Click
        G_Show_T("菌类草地(红)", "Fungal Turf", "G_fungal_turf_red", "NoDLC", 1, 0, 1, "Texture_fungal_turf_red", "出现在蘑菇树森林区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的菌类草地足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_guano_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_guano_turf.Click
        G_Show_T("鸟粪草地", "Guano Turf", "G_guano_turf", "NoDLC", 1, 0, 1, "Texture_guano_turf", "出现在石笋生物群落区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的鸟粪草地足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_mud_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_mud_turf.Click
        G_Show_T("泥泞草地", "Mud Turf", "G_mud_turf", "NoDLC", 1, 0, 1, "Texture_mud_turf", "出现在沉没森林区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的泥泞草地足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_slimey_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_slimey_turf.Click
        G_Show_T("粘滑草地", "Slimey Turf", "G_slimey_turf", "NoDLC", 1, 0, 1, "Texture_slimey_turf", "出现在沉没森林区域，可以在上面种植植物，也不阻止食人花和眼球草的生长，当铺设的粘滑草地足够大时，附近只会出现乌鸦。")
    End Sub

    Private Sub button_G_ashy_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_ashy_turf.Click
        G_Show_T("灰色地皮", "Ashy Turf", "G_ashy_turf", "SW", 0, 1, 0, "Texture_ashy_turf", "出现在火山区域，不能在上面种植植物，但是可以种植食人花(眼球草不会生长)，当铺设的灰色地皮足够大时，附近只会出现大嘴鸟。")
    End Sub

    Private Sub button_G_volcano_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_volcano_turf.Click
        G_Show_T("火山地皮", "Volcano Turf", "G_volcano_turf", "SW", 0, 1, 0, "Texture_volcano_turf", "出现在火山区域，不能在上面种植植物，但是可以种植食人花(眼球草不会生长)、咖啡树和象仙人掌，当铺设的火山地皮足够大时，附近只会出现大嘴鸟。")
    End Sub

    Private Sub button_G_sticky_webbing_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_sticky_webbing.Click
        G_Show_T("蛛网地形", "Sticky Webbing", "G_sticky_webbing", "NoDLC", 1, 1, 1, "Texture_sticky_webbing", "不可以挖出！出现在蜘蛛巢周围，如果踩在上面会导致蜘蛛出现和调查，如果看不到威胁就会回巢穴(白蜘蛛除外)。所有经过的生物都会被减速，除了蜘蛛和韦伯。摧毁蜘蛛巢后蛛网地形会消失。")
    End Sub

    Private Sub button_G_beach_turf_click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_beach_turf.Click
        G_Show_T("海滩地皮", "Beach Turf", "G_beach_turf", "SW", 0, 1, 0, "Texture_beach_turf", "不可以挖出(可以用控制台调出)！出现在海滩区域，可以在上面种植植物，也不阻止食人花和眼球草的生长。")
    End Sub

    REM ------------------左侧面板(物品_宠物)------------------
    Private Sub G_Show_P(G_Name As String, G_EnName As String, G_picture As String, G_DLC As String, G_DLC_ROG As SByte, G_DLC_SW As SByte, G_DLC_DST As SByte, G_Introduce As String, ParamArray Pet() As String)
        REM ------------------初始化------------------
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_Pet.Visibility = Visibility.Visible
        button_GP_PetButton_2.Visibility = Visibility.Collapsed
        button_GP_PetButton_3.Visibility = Visibility.Collapsed
        REM ------------------物品名字------------------
        GL_textBlock_GoodsName_P.Text = G_Name
        GL_textBlock_GoodsName_P.UpdateLayout()
        Dim G_N_MarginLeft As Integer
        G_N_MarginLeft = (Canvas_GoodsLeft_P.ActualWidth - GL_textBlock_GoodsName_P.ActualWidth) / 2
        Dim G_N_T As New Thickness()
        G_N_T.Top = 80
        G_N_T.Left = G_N_MarginLeft
        GL_textBlock_GoodsName_P.Margin = G_N_T

        GL_textBlock_GoodsEnName_P.Text = G_EnName
        GL_textBlock_GoodsEnName_P.UpdateLayout()
        Dim G_EnN_MarginLeft As Integer
        G_EnN_MarginLeft = (Canvas_GoodsLeft_P.ActualWidth - GL_textBlock_GoodsEnName_P.ActualWidth) / 2
        Dim G_EnN_T As New Thickness()
        G_EnN_T.Top = 100
        G_EnN_T.Left = G_EnN_MarginLeft
        GL_textBlock_GoodsEnName_P.Margin = G_EnN_T
        REM ------------------物品图片------------------
        GL_image_GoodsPicture_P.Source = Picture_Short_Name(Res_Short_Name(G_picture))
        REM ------------------物品DLC-------------------
        If G_DLC = "ROG" Then
            GL_image_GP_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf G_DLC = "SW" Then
            GL_image_GP_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf G_DLC = "DST" Then
            GL_image_GP_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            GL_image_GP_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------存在版本-------------------
        GL_textBlock_GP_DLC_1.Foreground = Brushes.Black
        GL_textBlock_GP_DLC_2.Foreground = Brushes.Black
        GL_textBlock_GP_DLC_3.Foreground = Brushes.Black
        If G_DLC_ROG = 0 Then
            GL_textBlock_GP_DLC_1.Foreground = Brushes.Silver
        End If
        If G_DLC_SW = 0 Then
            GL_textBlock_GP_DLC_2.Foreground = Brushes.Silver
        End If
        If G_DLC_DST = 0 Then
            GL_textBlock_GP_DLC_3.Foreground = Brushes.Silver
        End If
        REM ------------------跟随宠物-------------------
        Dim G_Pet_T As New Thickness()
        G_Pet_T.Top = 0
        If Pet.Length = 1 Then
            G_Pet_T.Left = 65
        Else
            G_Pet_T.Left = 15
            button_GP_PetButton_2.Visibility = Visibility.Visible
            button_GP_PetButton_3.Visibility = Visibility.Visible
            image_GM_Pet_2.Source = Picture_Short_Name(Res_Short_Name(Pet(1)))
            image_GM_Pet_3.Source = Picture_Short_Name(Res_Short_Name(Pet(2)))
            GP_Pet_Select_2 = Pet(1)
            GP_Pet_Select_3 = Pet(2)
        End If
        image_GM_Pet_1.Source = Picture_Short_Name(Res_Short_Name(Pet(0)))
        GP_Pet_Select_1 = Pet(0)
        button_GP_PetButton_1.Margin = G_Pet_T
        REM ------------------物品简介-------------------
        TextBlock_GP_Introduce.Text = G_Introduce
        TextBlock_GP_Introduce.Height = SetTextBlockHeight(G_Introduce, 13)
        G_WrapPanel_Introduce_P.Height = SetTextBlockHeight(G_Introduce, 13)
    End Sub

    Private Sub button_GP_PetButton_1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GP_PetButton_1.Click
        ButtonJump(GP_Pet_Select_1)
    End Sub

    Private Sub button_GP_PetButton_2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GP_PetButton_2.Click
        ButtonJump(GP_Pet_Select_2)
    End Sub

    Private Sub button_GP_PetButton_3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GP_PetButton_3.Click
        ButtonJump(GP_Pet_Select_3)
    End Sub

    Private Sub button_GP_Switch_Left_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GP_Switch_Left.Click
        button_GP_Switch_Right.IsEnabled = True
        If G_PetListIndex <> 0 Then
            G_PetListIndex -= 1
            If G_PetListIndex = 0 Then
                button_GP_Switch_Left.IsEnabled = False
            End If
            Select Case G_PetList(G_PetListIndex)
                Case "chester"
                    G_Show_P("切斯特眼骨", "Eye Bone", "G_eye_bone", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_chester"})
                Case "chester_dead"
                    G_Show_P("切斯特眼骨(死亡)", "Eye Bone", "G_eye_bone_died", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_chester"})
                Case "chester_snow"
                    G_Show_P("寒冰切斯特眼骨", "Eye Bone", "G_eye_bone_snow", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_snow_chester"})
                Case "chester_snow_dead"
                    G_Show_P("寒冰切斯特眼骨(死亡)", "Eye Bone", "G_eye_bone_died_snow", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_snow_chester"})
                Case "chester_shadow"
                    G_Show_P("暗影切斯特眼骨", "Eye Bone", "G_eye_bone_shadow", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_shadow_chester"})
                Case "chester_shadow_dead"
                    G_Show_P("暗影切斯特眼骨(死亡)", "Eye Bone", "G_eye_bone_died_shadow", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_shadow_chester"})
                Case "packim_baggims"
                    G_Show_P("鱼骨", "Fishbone", "G_fishbone", "SW", 0, 1, 0, "鱼骨有可能出现在除了起始岛的任意岛上，是船难版的眼骨，会召唤鹈鹕，不会沉入水中。", {"A_packim_baggims", "A_fat_packim_baggims", "A_fire_packim_baggims"})
                Case "packim_baggims_dead"
                    G_Show_P("鱼骨(死亡)", "Fishbone", "G_fishbone_died", "SW", 0, 1, 0, "鱼骨有可能出现在除了起始岛的任意岛上，是船难版的眼骨，会召唤鹈鹕，不会沉入水中。", {"A_packim_baggims", "A_fat_packim_baggims", "A_fire_packim_baggims"})
                Case "hutch"
                    G_Show_P("星-空", "Star-Sky", "G_star_sky", "DST", 0, 0, 1, "星-空是联机版的眼骨，只在洞穴里，会召唤哈奇。", {"A_hutch", "A_fugu_hutch", "A_music_box_hutch"})
                Case "hutch_dead"
                    G_Show_P("星-空(死亡)", "Star-Sky", "G_star_sky_died", "DST", 0, 0, 1, "星-空是联机版的眼骨，只在洞穴里，会召唤哈奇。", {"A_hutch", "A_fugu_hutch", "A_music_box_hutch"})
            End Select
        End If
    End Sub

    Private Sub button_GP_Switch_Right_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GP_Switch_Right.Click
        button_GP_Switch_Left.IsEnabled = True
        If G_PetListIndex <> G_PetListIndexMax Then
            G_PetListIndex += 1
            If G_PetListIndex = G_PetListIndexMax Then
                button_GP_Switch_Right.IsEnabled = False
            End If
            Select Case G_PetList(G_PetListIndex)
                Case "chester"
                    G_Show_P("切斯特眼骨", "Eye Bone", "G_eye_bone", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_chester"})
                Case "chester_dead"
                    G_Show_P("切斯特眼骨(死亡)", "Eye Bone", "G_eye_bone_died", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_chester"})
                Case "chester_snow"
                    G_Show_P("寒冰切斯特眼骨", "Eye Bone", "G_eye_bone_snow", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_snow_chester"})
                Case "chester_snow_dead"
                    G_Show_P("寒冰切斯特眼骨(死亡)", "Eye Bone", "G_eye_bone_died_snow", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_snow_chester"})
                Case "chester_shadow"
                    G_Show_P("暗影切斯特眼骨", "Eye Bone", "G_eye_bone_shadow", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_shadow_chester"})
                Case "chester_shadow_dead"
                    G_Show_P("暗影切斯特眼骨(死亡)", "Eye Bone", "G_eye_bone_died_shadow", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_shadow_chester"})
                Case "packim_baggims"
                    G_Show_P("鱼骨", "Fishbone", "G_fishbone", "SW", 0, 1, 0, "鱼骨有可能出现在除了起始岛的任意岛上，是船难版的眼骨，会召唤鹈鹕，不会沉入水中。", {"A_packim_baggims", "A_fat_packim_baggims", "A_fire_packim_baggims"})
                Case "packim_baggims_dead"
                    G_Show_P("鱼骨(死亡)", "Fishbone", "G_fishbone_died", "SW", 0, 1, 0, "鱼骨有可能出现在除了起始岛的任意岛上，是船难版的眼骨，会召唤鹈鹕，不会沉入水中。", {"A_packim_baggims", "A_fat_packim_baggims", "A_fire_packim_baggims"})
                Case "hutch"
                    G_Show_P("星-空", "Star-Sky", "G_star_sky", "DST", 0, 0, 1, "星-空是联机版的眼骨，只在洞穴里，会召唤哈奇。", {"A_hutch", "A_fugu_hutch", "A_music_box_hutch"})
                Case "hutch_dead"
                    G_Show_P("星-空(死亡)", "Star-Sky", "G_star_sky_died", "DST", 0, 0, 1, "星-空是联机版的眼骨，只在洞穴里，会召唤哈奇。", {"A_hutch", "A_fugu_hutch", "A_music_box_hutch"})
            End Select
        End If
    End Sub

    Private Sub button_GP_Switch_initialization()
        button_GP_Switch_Left.Visibility = Visibility.Visible
        button_GP_Switch_Right.Visibility = Visibility.Visible
        button_GP_Switch_Left.IsEnabled = False
        button_GP_Switch_Right.IsEnabled = True
        ReDim G_PetList(1)
        G_PetList(0) = ""
        G_PetListIndex = 0
        G_PetListIndexMax = -128
    End Sub

    Private Sub G_PetListAddPet(PetName As String)
        If G_PetListIndex >= G_PetListIndexMax Then
            G_PetListIndexMax = G_PetListIndex
            G_PetListIndex += 1
            ReDim Preserve G_PetList(G_PetListIndex)
            G_PetList(G_PetListIndex - 1) = PetName
        End If
    End Sub

    Private Sub button_G_eye_bone_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_eye_bone.Click
        button_GP_Switch_initialization()
        G_PetListAddPet("chester")
        G_PetListAddPet("chester_dead")
        G_PetListAddPet("chester_snow")
        G_PetListAddPet("chester_snow_dead")
        G_PetListAddPet("chester_shadow")
        G_PetListAddPet("chester_shadow_dead")
        G_PetListIndex = 0
        G_Show_P("切斯特眼骨", "Eye Bone", "G_eye_bone", "NoDLC", 1, 0, 1, "通常可以在卵石路的尽头找到，也有可能在沼泽附近。眼骨可以召唤切斯特并让切斯特跟随，放在地上或切斯特里会使切斯特停止跟随。切斯特死亡后眼骨将闭眼，一天后切斯特重生。", {"A_chester"})
    End Sub

    Private Sub button_G_fishbone_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_fishbone.Click
        button_GP_Switch_initialization()
        G_PetListAddPet("packim_baggims")
        G_PetListAddPet("packim_baggims_dead")
        G_PetListIndex = 0
        G_Show_P("鱼骨", "Fishbone", "G_fishbone", "SW", 0, 1, 0, "鱼骨有可能出现在除了起始岛的任意岛上，是船难版的眼骨，会召唤鹈鹕，不会沉入水中。", {"A_packim_baggims", "A_fat_packim_baggims", "A_fire_packim_baggims"})
    End Sub

    Private Sub button_G_lavae_egg_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_lavae_egg.Click
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_PetLavaeEgg.Visibility = Visibility.Visible
    End Sub

    Private Sub button_GP_SpawnButton_1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GP_SpawnButton_1.Click
        button_G_lavae_tooth_Click(Nothing, Nothing)
    End Sub

    Private Sub button_GP_SpawnButton_2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GP_SpawnButton_2.Click
        LeftTabItem_Animal.IsSelected = True
        button_A_Extra_Adorable_Lavae_click(Nothing, Nothing)
    End Sub

    Private Sub button_G_lavae_tooth_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_lavae_tooth.Click
        G_Show_P("熔岩虫牙", "Lavae Tooth", "G_lavae_tooth", "DST", 0, 0, 1, "熔岩虫卵孵化后会产生熔岩虫牙和一只超可爱的熔岩虫，超可爱的熔岩虫会跟随熔岩虫牙，当超可爱的熔岩虫死后熔岩虫牙化为灰烬。", {"A_lavae"})
        button_GP_Switch_Left.Visibility = Visibility.Collapsed
        button_GP_Switch_Right.Visibility = Visibility.Collapsed
    End Sub

    Private Sub button_G_star_sky_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_star_sky.Click
        button_GP_Switch_initialization()
        G_PetListAddPet("hutch")
        G_PetListAddPet("hutch_dead")
        G_PetListIndex = 0
        G_Show_P("星-空", "Star-Sky", "G_star_sky", "DST", 0, 0, 1, "星-空是联机版的眼骨，只在洞穴里，会召唤哈奇。", {"A_hutch", "A_fugu_hutch", "A_music_box_hutch"})
    End Sub

    REM ------------------左侧面板(物品_解锁)------------------
    Private Sub G_Show_U(G_Name As String, G_EnName As String, G_picture As String, G_DLC As String, G_DLC_ROG As SByte, G_DLC_SW As SByte, G_DLC_DST As SByte, G_UnlockCharacter As String, G_Introduce As String, ParamArray Drop() As String)
        REM ------------------初始化------------------
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_Unlock.Visibility = Visibility.Visible
        button_GU_Drop_2.Visibility = Visibility.Collapsed
        button_GU_Drop_3.Visibility = Visibility.Collapsed
        button_GU_Drop_4.Visibility = Visibility.Collapsed
        button_GU_Drop_5.Visibility = Visibility.Collapsed
        button_GU_Drop_6.Visibility = Visibility.Collapsed
        button_GU_Drop_7.Visibility = Visibility.Collapsed
        button_GU_Drop_8.Visibility = Visibility.Collapsed
        REM ------------------物品名字------------------
        GL_textBlock_GoodsName_U.Text = G_Name
        GL_textBlock_GoodsName_U.UpdateLayout()
        Dim G_N_MarginLeft As Integer
        G_N_MarginLeft = (Canvas_GoodsLeft_U.ActualWidth - GL_textBlock_GoodsName_U.ActualWidth) / 2
        Dim G_N_T As New Thickness()
        G_N_T.Top = 80
        G_N_T.Left = G_N_MarginLeft
        GL_textBlock_GoodsName_U.Margin = G_N_T

        GL_textBlock_GoodsEnName_U.Text = G_EnName
        GL_textBlock_GoodsEnName_U.UpdateLayout()
        Dim G_EnN_MarginLeft As Integer
        G_EnN_MarginLeft = (Canvas_GoodsLeft_U.ActualWidth - GL_textBlock_GoodsEnName_U.ActualWidth) / 2
        Dim G_EnN_T As New Thickness()
        G_EnN_T.Top = 100
        G_EnN_T.Left = G_EnN_MarginLeft
        GL_textBlock_GoodsEnName_U.Margin = G_EnN_T
        REM ------------------物品图片------------------
        GL_image_GoodsPicture_U.Source = Picture_Short_Name(Res_Short_Name(G_picture))
        REM ------------------物品DLC-------------------
        If G_DLC = "ROG" Then
            GL_image_GU_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf G_DLC = "SW" Then
            GL_image_GU_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf G_DLC = "DST" Then
            GL_image_GU_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            GL_image_GU_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------存在版本-------------------
        GL_textBlock_GU_DLC_1.Foreground = Brushes.Black
        GL_textBlock_GU_DLC_2.Foreground = Brushes.Black
        GL_textBlock_GU_DLC_3.Foreground = Brushes.Black
        If G_DLC_ROG = 0 Then
            GL_textBlock_GU_DLC_1.Foreground = Brushes.Silver
        End If
        If G_DLC_SW = 0 Then
            GL_textBlock_GU_DLC_2.Foreground = Brushes.Silver
        End If
        If G_DLC_DST = 0 Then
            GL_textBlock_GU_DLC_3.Foreground = Brushes.Silver
        End If
        REM --------------------掉落---------------------
        Dim DL As Byte
        DL = Drop.Length
        Select Case DL
            Case 1
                image_GU_Drop_1.Source = Picture_Short_Name(Res_Short_Name(Drop(0)))
                G_WrapPanel_DropButton_U.Height = 60
            Case 2
                GS_UnlockDrop_Select_2 = Drop(1)
                button_GU_Drop_2.Visibility = Visibility.Visible
                image_GU_Drop_1.Source = Picture_Short_Name(Res_Short_Name(Drop(0)))
                image_GU_Drop_2.Source = Picture_Short_Name(Res_Short_Name(Drop(1)))
                G_WrapPanel_DropButton_U.Height = 60
            Case 8
                GS_UnlockDrop_Select_2 = Drop(1)
                button_GU_Drop_2.Visibility = Visibility.Visible
                button_GU_Drop_3.Visibility = Visibility.Visible
                button_GU_Drop_4.Visibility = Visibility.Visible
                button_GU_Drop_5.Visibility = Visibility.Visible
                button_GU_Drop_6.Visibility = Visibility.Visible
                button_GU_Drop_7.Visibility = Visibility.Visible
                button_GU_Drop_8.Visibility = Visibility.Visible
                image_GU_Drop_1.Source = Picture_Short_Name(Res_Short_Name(Drop(0)))
                image_GU_Drop_2.Source = Picture_Short_Name(Res_Short_Name(Drop(1)))
                image_GU_Drop_3.Source = Picture_Short_Name(Res_Short_Name(Drop(2)))
                image_GU_Drop_4.Source = Picture_Short_Name(Res_Short_Name(Drop(3)))
                image_GU_Drop_5.Source = Picture_Short_Name(Res_Short_Name(Drop(4)))
                image_GU_Drop_6.Source = Picture_Short_Name(Res_Short_Name(Drop(5)))
                image_GU_Drop_7.Source = Picture_Short_Name(Res_Short_Name(Drop(6)))
                image_GU_Drop_8.Source = Picture_Short_Name(Res_Short_Name(Drop(7)))
                G_WrapPanel_DropButton_U.Height = 160
        End Select
        GS_UnlockDrop_Select_1 = Drop(0)
        REM ------------------解锁人物-------------------
        GS_UnlockCharacter_Select = G_UnlockCharacter
        image_GU_UnlockCharacter.Source = Picture_Short_Name(Res_Short_Name(G_UnlockCharacter))
        REM ------------------物品简介-------------------
        TextBlock_GU_Introduce.Text = G_Introduce
        TextBlock_GU_Introduce.Height = SetTextBlockHeight(G_Introduce, 13)
        G_WrapPanel_Introduce_U.Height = SetTextBlockHeight(G_Introduce, 13)
    End Sub

    Private Sub button_GU_UnlockCharacter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GU_UnlockCharacter.Click
        ButtonJump(GS_UnlockCharacter_Select)
    End Sub

    Private Sub button_GU_Drop_1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GU_Drop_1.Click
        ButtonJump(GS_UnlockDrop_Select_1)
    End Sub

    Private Sub button_GU_Drop_2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GU_Drop_2.Click
        ButtonJump(GS_UnlockDrop_Select_2)
    End Sub

    Private Sub button_GU_Drop_3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GU_Drop_3.Click
        ButtonJump(GS_UnlockDrop_Select_3)
    End Sub

    Private Sub button_GU_Drop_4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GU_Drop_4.Click
        ButtonJump(GS_UnlockDrop_Select_4)
    End Sub

    Private Sub button_GU_Drop_5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GU_Drop_5.Click
        ButtonJump(GS_UnlockDrop_Select_5)
    End Sub

    Private Sub button_GU_Drop_6_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GU_Drop_6.Click
        ButtonJump(GS_UnlockDrop_Select_6)
    End Sub

    Private Sub button_GU_Drop_7_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GU_Drop_7.Click
        ButtonJump(GS_UnlockDrop_Select_7)
    End Sub

    Private Sub button_GU_Drop_8_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GU_Drop_8.Click
        ButtonJump(GS_UnlockDrop_Select_8)
    End Sub

    Private Sub button_G_webbers_skull_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_webbers_skull.Click
        G_Show_U("维伯的头颅", "Webbers Skull", "G_webber's_skull", "ROG", 1, 0, 0, "C_webber", "每次击杀任何蜘蛛或拆蜘蛛巢和蛛网岩都有5%的几率掉落维伯的头颅，把维伯的头颅放进挖开的坟墓中，坟墓会被闪电劈中，维伯会出现，然后跳回坟墓消失，并出现6只蜘蛛，维伯就解锁了(不需要杀死蜘蛛)。", {"A_spider", "A_spider_warrior", "A_spitter", "A_cave_spider", "A_dangling_depth_dweller", "A_spider_queen", "N_spider_den", "N_spilagmite"})
    End Sub

    Private Sub button_G_iron_key_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_iron_key.Click
        G_Show_U("铁制钥匙", "Iron Key", "G_iron_key", "SW", 0, 1, 0, "C_woodlegs", "第一次击杀呱肯乌贼就会获得骨制钥匙。集齐三把钥匙即可解锁火山里的伍德莱格。", {"A_quacken"})
    End Sub

    Private Sub button_G_golden_key_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_golden_key.Click
        G_Show_U("金制钥匙", "Golden Key", "G_golden_key", "SW", 0, 1, 0, "C_woodlegs", "与亚克章鱼交易有10%的几率获得金制钥匙。集齐三把钥匙即可解锁火山里的伍德莱格。", {"A_yaarctopus"})
    End Sub

    Private Sub button_G_bone_key_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_bone_key.Click
        G_Show_U("骨制钥匙", "Bone Key", "G_bone_key", "SW", 0, 1, 0, "C_woodlegs", "在潮湿的坟墓钓鱼有几率获得铁制钥匙，每次钓鱼都会增加获得的几率。集齐三把钥匙即可解锁火山里的伍德莱格。", {"N_watery_grave"})
    End Sub

    Private Sub button_G_tarnished_crown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_tarnished_crown.Click
        G_Show_U("旧王冠", "Tarnished Crown", "G_tarnished_crown", "SW", 0, 1, 0, "C_wilbur", "每次击杀猿猴或摧毁猿猴小屋都有10%的几率掉落旧王冠，把旧王冠给在浅海的竹筏上的威尔伯即可解锁威尔伯人物。", {"A_prime_ape", "N_Prime_ape_hut"})
    End Sub


    REM ------------------左侧面板(物品_零件)------------------
    Private Sub G_Show_C(G_Name As String, G_EnName As String, G_picture As String, G_DLC As String, G_DLC_ROG As SByte, G_DLC_SW As SByte, G_DLC_DST As SByte, G_Introduce As String)
        REM ------------------初始化------------------
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_Component.Visibility = Visibility.Visible
        REM ------------------物品名字------------------
        GL_textBlock_GoodsName_C.Text = G_Name
        GL_textBlock_GoodsName_C.UpdateLayout()
        Dim G_N_MarginLeft As Integer
        G_N_MarginLeft = (Canvas_GoodsLeft_C.ActualWidth - GL_textBlock_GoodsName_C.ActualWidth) / 2
        Dim G_N_T As New Thickness()
        G_N_T.Top = 80
        G_N_T.Left = G_N_MarginLeft
        GL_textBlock_GoodsName_C.Margin = G_N_T

        GL_textBlock_GoodsEnName_C.Text = G_EnName
        GL_textBlock_GoodsEnName_C.UpdateLayout()
        Dim G_EnN_MarginLeft As Integer
        G_EnN_MarginLeft = (Canvas_GoodsLeft_C.ActualWidth - GL_textBlock_GoodsEnName_C.ActualWidth) / 2
        Dim G_EnN_T As New Thickness()
        G_EnN_T.Top = 100
        G_EnN_T.Left = G_EnN_MarginLeft
        GL_textBlock_GoodsEnName_C.Margin = G_EnN_T
        REM ------------------物品图片------------------
        GL_image_GoodsPicture_C.Source = Picture_Short_Name(Res_Short_Name(G_picture))
        REM ------------------物品DLC-------------------
        If G_DLC = "ROG" Then
            GL_image_GC_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_ROG"))
        ElseIf G_DLC = "SW" Then
            GL_image_GC_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_SW"))
        ElseIf G_DLC = "DST" Then
            GL_image_GC_DLC.Source = Picture_Short_Name(Res_Short_Name("DLC_DST"))
        Else
            GL_image_GC_DLC.Source = Picture_Short_Name()
        End If
        REM ------------------存在版本-------------------
        GL_textBlock_GC_DLC_1.Foreground = Brushes.Black
        GL_textBlock_GC_DLC_2.Foreground = Brushes.Black
        GL_textBlock_GC_DLC_3.Foreground = Brushes.Black
        If G_DLC_ROG = 0 Then
            GL_textBlock_GC_DLC_1.Foreground = Brushes.Silver
        End If
        If G_DLC_SW = 0 Then
            GL_textBlock_GC_DLC_2.Foreground = Brushes.Silver
        End If
        If G_DLC_DST = 0 Then
            GL_textBlock_GC_DLC_3.Foreground = Brushes.Silver
        End If
        REM ------------------物品简介-------------------
        TextBlock_GC_Introduce.Text = G_Introduce
        TextBlock_GC_Introduce.Height = SetTextBlockHeight(G_Introduce, 13)
        G_WrapPanel_Introduce_C.Height = SetTextBlockHeight(G_Introduce, 13)
    End Sub

    Private Sub button_GC_Switch_Left_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GC_Switch_Left.Click
        button_GC_Switch_Right.IsEnabled = True
        If G_ComponentListIndex <> 0 Then
            G_ComponentListIndex -= 1
            If G_ComponentListIndex = 0 Then
                button_GC_Switch_Left.IsEnabled = False
            End If
            Select Case G_ComponentList(G_ComponentListIndex)
                Case "Boxthing"
                    G_Show_C("盒状传送机零件", "Box Thing", "G_box_thing_1", "ROG", 1, 0, 0, "盒状传送机零件是用来制作传送机的四个零件之一。")
                Case "BoxthingAdvanture"
                    G_Show_C("盒状传送机零件(冒险模式)", "Box Thing", "G_box_thing_2", "ROG", 1, 0, 0, "盒状传送机零件是用来制作传送机的四个零件之一。")
                Case "Crankthing"
                    G_Show_C("曲柄状传送机零件", "Crank Thing", "G_crank_thing_1", "ROG", 1, 0, 0, "曲柄状送机零件是用来制作传送机的四个零件之一。")
                Case "CrankthingAdvanture"
                    G_Show_C("曲柄状传送机零件(冒险模式)", "Crank Thing", "G_crank_thing_2", "ROG", 1, 0, 0, "曲柄状送机零件是用来制作传送机的四个零件之一。")
                Case "Metalpotatothing"
                    G_Show_C("球状传送机零件", "Metal Potato Thing", "G_metal_potato_thing_1", "ROG", 1, 0, 0, "球状传送机零件是用来制作传送机的四个零件之一。")
                Case "MetalpotatothingAdvanture"
                    G_Show_C("球状传送机零件(冒险模式)", "Metal Potato Thing", "G_metal_potato_thing_2", "ROG", 1, 0, 0, "球状传送机零件是用来制作传送机的四个零件之一。")
                Case "Ringthing"
                    G_Show_C("环状传送机零件", "Ring Thing", "G_ring_thing_1", "ROG", 1, 0, 0, "环状传送机零件是用来制作传送机的四个零件之一。")
                Case "RingthingAdvanture"
                    G_Show_C("环状传送机零件(冒险模式)", "Ring Thing", "G_ring_thing_2", "ROG", 1, 0, 0, "环状传送机零件是用来制作传送机的四个零件之一。")
            End Select
        End If
    End Sub

    Private Sub button_GC_Switch_Right_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GC_Switch_Right.Click
        button_GC_Switch_Left.IsEnabled = True
        If G_ComponentListIndex <> G_ComponentListIndexMax Then
            G_ComponentListIndex += 1
            If G_ComponentListIndex = G_ComponentListIndexMax Then
                button_GC_Switch_Right.IsEnabled = False
            End If
            Select Case G_ComponentList(G_ComponentListIndex)
                Case "Boxthing"
                    G_Show_C("盒状传送机零件", "Box Thing", "G_box_thing_1", "ROG", 1, 0, 0, "盒状传送机零件是用来制作传送机的四个零件之一。")
                Case "BoxthingAdvanture"
                    G_Show_C("盒状传送机零件(冒险模式)", "Box Thing", "G_box_thing_2", "ROG", 1, 0, 0, "盒状传送机零件是用来制作传送机的四个零件之一。")
                Case "Crankthing"
                    G_Show_C("曲柄状传送机零件", "Crank Thing", "G_crank_thing_1", "ROG", 1, 0, 0, "曲柄状送机零件是用来制作传送机的四个零件之一。")
                Case "CrankthingAdvanture"
                    G_Show_C("曲柄状传送机零件(冒险模式)", "Crank Thing", "G_crank_thing_2", "ROG", 1, 0, 0, "曲柄状送机零件是用来制作传送机的四个零件之一。")
                Case "Metalpotatothing"
                    G_Show_C("球状传送机零件", "Metal Potato Thing", "G_metal_potato_thing_1", "ROG", 1, 0, 0, "球状传送机零件是用来制作传送机的四个零件之一。")
                Case "MetalpotatothingAdvanture"
                    G_Show_C("球状传送机零件(冒险模式)", "Metal Potato Thing", "G_metal_potato_thing_2", "ROG", 1, 0, 0, "球状传送机零件是用来制作传送机的四个零件之一。")
                Case "Ringthing"
                    G_Show_C("环状传送机零件", "Ring Thing", "G_ring_thing_1", "ROG", 1, 0, 0, "环状传送机零件是用来制作传送机的四个零件之一。")
                Case "RingthingAdvanture"
                    G_Show_C("环状传送机零件(冒险模式)", "Ring Thing", "G_ring_thing_2", "ROG", 1, 0, 0, "环状传送机零件是用来制作传送机的四个零件之一。")
            End Select
        End If
    End Sub

    Private Sub button_GC_Switch_initialization()
        button_GC_Switch_Left.Visibility = Visibility.Visible
        button_GC_Switch_Right.Visibility = Visibility.Visible
        button_GC_Switch_Left.IsEnabled = False
        button_GC_Switch_Right.IsEnabled = True
        ReDim G_ComponentList(1)
        G_ComponentList(0) = ""
        G_ComponentListIndex = 0
        G_ComponentListIndexMax = -128
    End Sub

    Private Sub G_ComponentListAddComponent(ComponentName As String)
        If G_ComponentListIndex >= G_ComponentListIndexMax Then
            G_ComponentListIndexMax = G_ComponentListIndex
            G_ComponentListIndex += 1
            ReDim Preserve G_ComponentList(G_ComponentListIndex)
            G_ComponentList(G_ComponentListIndex - 1) = ComponentName
        End If
    End Sub

    Private Sub button_G_box_thing_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_box_thing.Click
        button_GC_Switch_initialization()
        G_ComponentListAddComponent("Boxthing")
        G_ComponentListAddComponent("BoxthingAdvanture")
        G_ComponentListIndex = 0
        G_Show_C("盒状传送机零件", "Box Thing", "G_box_thing_1", "ROG", 1, 0, 0, "盒状传送机零件是用来制作传送机的四个零件之一。")
    End Sub

    Private Sub button_G_crank_thing_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_crank_thing.Click
        button_GC_Switch_initialization()
        G_ComponentListAddComponent("Crankthing")
        G_ComponentListAddComponent("CrankthingAdvanture")
        G_ComponentListIndex = 0
        G_Show_C("曲柄状传送机零件", "Crank Thing", "G_crank_thing_1", "ROG", 1, 0, 0, "曲柄状传送机零件是用来制作传送机的四个零件之一。")
    End Sub

    Private Sub button_G_metal_potato_thing_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_metal_potato_thing.Click
        button_GC_Switch_initialization()
        G_ComponentListAddComponent("Metalpotatothing")
        G_ComponentListAddComponent("MetalpotatothingAdvanture")
        G_ComponentListIndex = 0
        G_Show_C("球状传送机零件", "Metal Potato Thing", "G_metal_potato_thing_1", "ROG", 1, 0, 0, "球状传送机零件是用来制作传送机的四个零件之一。")
    End Sub

    Private Sub button_G_ring_thing_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_ring_thing.Click
        button_GC_Switch_initialization()
        G_ComponentListAddComponent("Ringthing")
        G_ComponentListAddComponent("RingthingAdvanture")
        G_ComponentListIndex = 0
        G_Show_C("环状传送机零件", "Ring Thing", "G_ring_thing_1", "ROG", 1, 0, 0, "环状传送机零件是用来制作传送机的四个零件之一。")
    End Sub

    Private Sub button_G_grassy_thing_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_grassy_thing.Click
        G_Show_C("长满草的玩意", "Grassy Thing", "G_grassy_thing", "SW", 0, 1, 0, "长满草的玩意是用来制作传送机的四个零件之一。")
        button_GC_Switch_Left.Visibility = Visibility.Collapsed
        button_GC_Switch_Right.Visibility = Visibility.Collapsed
    End Sub

    Private Sub button_G_ring_thing_sw_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_ring_thing_sw.Click
        G_Show_C("类似戒指的玩意", "Ring Thing", "G_ring_thing", "SW", 0, 1, 0, "类似戒指的玩意是用来制作传送机的四个零件之一。")
        button_GC_Switch_Left.Visibility = Visibility.Collapsed
        button_GC_Switch_Right.Visibility = Visibility.Collapsed
    End Sub

    Private Sub button_G_screw_thing_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_screw_thing.Click
        G_Show_C("螺丝钉", "Screw Thing", "G_screw_thing", "SW", 0, 1, 0, "螺丝钉是用来制作传送机的四个零件之一。")
        button_GC_Switch_Left.Visibility = Visibility.Collapsed
        button_GC_Switch_Right.Visibility = Visibility.Collapsed
    End Sub

    Private Sub button_G_wooden_potato_thing_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_wooden_potato_thing.Click
        G_Show_C("木质的类似土豆的玩意", "Wooden Potato Thing", "G_wooden_potato_thing", "SW", 0, 1, 0, "木质的类似土豆的玩意是用来制作传送机的四个零件之一。")
        button_GC_Switch_Left.Visibility = Visibility.Collapsed
        button_GC_Switch_Right.Visibility = Visibility.Collapsed
    End Sub

    REM ------------------左侧面板(物品_其他)------------------
    Private Sub button_G_blueprint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_blueprint.Click
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_Blueprint.Visibility = Visibility.Visible
    End Sub

    Private Sub button_G_ballphin_free_tuna_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_ballphin_free_tuna.Click
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_BallphinFreeTuna.Visibility = Visibility.Visible
    End Sub

    Private Sub button_GBFT_FoodButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_GBFT_FoodButton.Click
        ButtonJump("F_fish_steak")
    End Sub

    Private Sub button_G_message_in_a_bottle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles button_G_message_in_a_bottle.Click
        G_LeftPanel_Initialization()
        ScrollViewer_GoodsLeft_MessageInABottle.Visibility = Visibility.Visible
    End Sub

    REM 生物DLC检测初始化
    Private Sub G_DLC_Check_initialization()
        REM 材料
        button_G_marble.Visibility = Visibility.Collapsed
        button_G_beefalo_horn.Visibility = Visibility.Collapsed
        button_G_guano.Visibility = Visibility.Collapsed
        button_G_glommers_flower.Visibility = Visibility.Collapsed
        button_G_mosquito_sack.Visibility = Visibility.Collapsed
        button_G_volt_goat_horn.Visibility = Visibility.Collapsed
        button_G_walrus_tusk.Visibility = Visibility.Collapsed
        button_G_light_bulb.Visibility = Visibility.Collapsed
        button_G_bunny_puff.Visibility = Visibility.Collapsed
        button_G_foliage.Visibility = Visibility.Collapsed
        button_G_broken_shell.Visibility = Visibility.Collapsed
        button_G_slurtle_slime.Visibility = Visibility.Collapsed
        button_G_beard_hair.Visibility = Visibility.Collapsed
        button_G_slurper_pelt.Visibility = Visibility.Collapsed
        button_G_thulecite_fragments.Visibility = Visibility.Collapsed
        button_G_down_feather.Visibility = Visibility.Collapsed
        button_G_scales.Visibility = Visibility.Collapsed
        button_G_thick_fur.Visibility = Visibility.Collapsed
        button_G_bamboo_patch.Visibility = Visibility.Collapsed
        button_G_cut_grass_SW.Visibility = Visibility.Collapsed
        button_G_vine.Visibility = Visibility.Collapsed
        button_G_sand.Visibility = Visibility.Collapsed
        button_G_snakeskin.Visibility = Visibility.Collapsed
        button_G_snake_oil.Visibility = Visibility.Collapsed
        button_G_palm_leaf.Visibility = Visibility.Collapsed
        button_G_venom_gland.Visibility = Visibility.Collapsed
        button_G_yellow_mosquito_sack.Visibility = Visibility.Collapsed
        button_G_seashell.Visibility = Visibility.Collapsed
        button_G_doydoy_feather.Visibility = Visibility.Collapsed
        button_G_dubloons.Visibility = Visibility.Collapsed
        button_G_hail.Visibility = Visibility.Collapsed
        button_G_horn.Visibility = Visibility.Collapsed
        button_G_coral.Visibility = Visibility.Collapsed
        button_G_obsidian.Visibility = Visibility.Collapsed
        button_G_cactus_spike.Visibility = Visibility.Collapsed
        button_G_dragoon_heart.Visibility = Visibility.Collapsed
        button_G_turbine_blades.Visibility = Visibility.Collapsed
        button_G_magic_seal.Visibility = Visibility.Collapsed
        button_G_shark_gills.Visibility = Visibility.Collapsed
        button_G_moon_rock.Visibility = Visibility.Collapsed
        button_G_steel_wool.Visibility = Visibility.Collapsed
        button_G_phlegm.Visibility = Visibility.Collapsed
        button_G_fur_tuft.Visibility = Visibility.Collapsed
        REM 装备
        button_G_pickaxe_1.Visibility = Visibility.Collapsed
        button_G_walking_cane.Visibility = Visibility.Collapsed
        button_G_pretty_parasol.Visibility = Visibility.Collapsed
        button_G_rain_hat.Visibility = Visibility.Collapsed
        button_G_rain_coat.Visibility = Visibility.Collapsed
        button_G_eyebrella.Visibility = Visibility.Collapsed
        button_G_rabbit_earmuffs.Visibility = Visibility.Collapsed
        button_G_beefalo_hat.Visibility = Visibility.Collapsed
        button_G_winter_hat.Visibility = Visibility.Collapsed
        button_G_cat_cap.Visibility = Visibility.Collapsed
        button_G_dapper_vest.Visibility = Visibility.Collapsed
        button_G_breezy_vest.Visibility = Visibility.Collapsed
        button_G_puffy_vest.Visibility = Visibility.Collapsed
        button_G_hibearnation_vest.Visibility = Visibility.Collapsed
        button_G_morning_star.Visibility = Visibility.Collapsed
        button_G_weather_pain.Visibility = Visibility.Collapsed
        button_G_bat_bat.Visibility = Visibility.Collapsed
        button_G_belt_of_hunger.Visibility = Visibility.Collapsed
        button_G_thulecite_club.Visibility = Visibility.Collapsed
        button_G_grass_suit.Visibility = Visibility.Collapsed
        button_G_marble_suit.Visibility = Visibility.Collapsed
        button_G_scalemail.Visibility = Visibility.Collapsed
        button_G_thulecite_crown.Visibility = Visibility.Collapsed
        button_G_thulecite_suit.Visibility = Visibility.Collapsed
        button_G_shelmet.Visibility = Visibility.Collapsed
        button_G_snurtle_shell_armor.Visibility = Visibility.Collapsed
        button_G_tam_o_shanter.Visibility = Visibility.Collapsed
        button_G_spiderhat.Visibility = Visibility.Collapsed
        button_G_slurper.Visibility = Visibility.Collapsed
        button_G_machete.Visibility = Visibility.Collapsed
        button_G_luxury_machete.Visibility = Visibility.Collapsed
        button_G_obsidian_machete.Visibility = Visibility.Collapsed
        button_G_obsidian_axe.Visibility = Visibility.Collapsed
        button_G_rawling.Visibility = Visibility.Collapsed
        button_G_pirate_hat.Visibility = Visibility.Collapsed
        button_G_tropical_parasol.Visibility = Visibility.Collapsed
        button_G_snakeskin_hat.Visibility = Visibility.Collapsed
        button_G_snakeskin_jacket.Visibility = Visibility.Collapsed
        button_G_blubber_suit.Visibility = Visibility.Collapsed
        button_G_dumbrella.Visibility = Visibility.Collapsed
        button_G_windbreaker.Visibility = Visibility.Collapsed
        button_G_boat_cannon.Visibility = Visibility.Collapsed
        button_G_coconade.Visibility = Visibility.Collapsed
        button_G_obsidian_coconade.Visibility = Visibility.Collapsed
        button_G_poison_spear.Visibility = Visibility.Collapsed
        button_G_poison_dart.Visibility = Visibility.Collapsed
        button_G_spear_gun.Visibility = Visibility.Collapsed
        button_G_cutlass_supreme.Visibility = Visibility.Collapsed
        button_G_obsidian_spear.Visibility = Visibility.Collapsed
        button_G_eyeshot.Visibility = Visibility.Collapsed
        button_G_harpoon.Visibility = Visibility.Collapsed
        button_G_peg_leg.Visibility = Visibility.Collapsed
        button_G_trident.Visibility = Visibility.Collapsed
        button_G_seashell_suit.Visibility = Visibility.Collapsed
        button_G_limestone_suit.Visibility = Visibility.Collapsed
        button_G_cactus_armour.Visibility = Visibility.Collapsed
        button_G_horned_helmet.Visibility = Visibility.Collapsed
        button_G_obsidian_armour.Visibility = Visibility.Collapsed
        button_G_portable_crock_pot.Visibility = Visibility.Collapsed
        button_G_saddlehorn.Visibility = Visibility.Collapsed
        button_G_brush.Visibility = Visibility.Collapsed
        button_G_whirly_fan.Visibility = Visibility.Collapsed
        button_G_tail_o_three_cats.Visibility = Visibility.Collapsed
        REM 树苗
        button_G_birchnut.Visibility = Visibility.Collapsed
        button_G_pine_cone.Visibility = Visibility.Collapsed
        button_G_sapling.Visibility = Visibility.Collapsed
        button_G_grass_tuft.Visibility = Visibility.Collapsed
        button_G_berry_bush.Visibility = Visibility.Collapsed
        button_G_berry_bush_2.Visibility = Visibility.Collapsed
        button_G_spiky_bushes.Visibility = Visibility.Collapsed
        button_G_bamboo_root.Visibility = Visibility.Collapsed
        button_G_grass_tuft_SW.Visibility = Visibility.Collapsed
        button_G_coconut.Visibility = Visibility.Collapsed
        button_G_jungle_tree_seed.Visibility = Visibility.Collapsed
        button_G_viney_bush_root.Visibility = Visibility.Collapsed
        button_G_coffee_plant.Visibility = Visibility.Collapsed
        button_G_elephant_cactus_stump.Visibility = Visibility.Collapsed
        button_G_juicy_berry_bush.Visibility = Visibility.Collapsed
        button_G_twiggy_tree_cone.Visibility = Visibility.Collapsed
        REM 生物
        button_G_rabbit.Visibility = Visibility.Collapsed
        button_G_winter_rabbit.Visibility = Visibility.Collapsed
        button_G_beardling.Visibility = Visibility.Collapsed
        button_G_moleworm.Visibility = Visibility.Collapsed
        button_G_bee.Visibility = Visibility.Collapsed
        button_G_killer_bee.Visibility = Visibility.Collapsed
        button_G_butterfly.Visibility = Visibility.Collapsed
        button_G_mosquito.Visibility = Visibility.Collapsed
        button_G_redbird.Visibility = Visibility.Collapsed
        button_G_snowbird.Visibility = Visibility.Collapsed
        button_G_crow.Visibility = Visibility.Collapsed
        button_G_fireflies.Visibility = Visibility.Collapsed
        button_G_crabbit.Visibility = Visibility.Collapsed
        button_G_beardling_sw.Visibility = Visibility.Collapsed
        button_G_dead_dogfish.Visibility = Visibility.Collapsed
        button_G_dead_swordfish.Visibility = Visibility.Collapsed
        button_G_dead_wobster.Visibility = Visibility.Collapsed
        button_G_bioluminescence.Visibility = Visibility.Collapsed
        button_G_jellyfish.Visibility = Visibility.Collapsed
        button_G_dead_jellyfish.Visibility = Visibility.Collapsed
        button_G_butterfly_sw.Visibility = Visibility.Collapsed
        button_G_mosquito_sw.Visibility = Visibility.Collapsed
        button_G_parrot.Visibility = Visibility.Collapsed
        button_G_parrot_pirate.Visibility = Visibility.Collapsed
        button_G_toucan.Visibility = Visibility.Collapsed
        button_G_seagull.Visibility = Visibility.Collapsed
        button_G_blue_spore.Visibility = Visibility.Collapsed
        button_G_green_spore.Visibility = Visibility.Collapsed
        button_G_red_spore.Visibility = Visibility.Collapsed
        REM 玩具
        button_G_air_unfreshener.Visibility = Visibility.Collapsed
        button_G_back_scratcher.Visibility = Visibility.Collapsed
        button_G_ball_and_cup.Visibility = Visibility.Collapsed
        button_G_beaten_beater.Visibility = Visibility.Collapsed
        button_G_bent_spork.Visibility = Visibility.Collapsed
        button_G_black_bishop.Visibility = Visibility.Collapsed
        button_G_dessicated_tentacle.Visibility = Visibility.Collapsed
        button_G_fake_kazoo.Visibility = Visibility.Collapsed
        button_G_frayed_yarn.Visibility = Visibility.Collapsed
        button_G_frazzled_wires.Visibility = Visibility.Collapsed
        button_G_gnome_1.Visibility = Visibility.Collapsed
        button_G_gnome_2.Visibility = Visibility.Collapsed
        button_G_gords_knot.Visibility = Visibility.Collapsed
        button_G_hardened_rubber_bung.Visibility = Visibility.Collapsed
        button_G_leaky_teacup.Visibility = Visibility.Collapsed
        button_G_lucky_cat_jar.Visibility = Visibility.Collapsed
        button_G_lying_robot.Visibility = Visibility.Collapsed
        button_G_melty_marbles.Visibility = Visibility.Collapsed
        button_G_mismatched_buttons.Visibility = Visibility.Collapsed
        button_G_potato_cup.Visibility = Visibility.Collapsed
        button_G_second_hand_dentures.Visibility = Visibility.Collapsed
        button_G_shoe_horn.Visibility = Visibility.Collapsed
        button_G_tiny_rocketship.Visibility = Visibility.Collapsed
        button_G_toy_trojan_horse.Visibility = Visibility.Collapsed
        button_G_unbalanced_top.Visibility = Visibility.Collapsed
        button_G_white_bishop.Visibility = Visibility.Collapsed
        button_G_wire_hanger.Visibility = Visibility.Collapsed
        button_G_ancient_vase.Visibility = Visibility.Collapsed
        button_G_brain_cloud_pill.Visibility = Visibility.Collapsed
        button_G_broken_AAC_device.Visibility = Visibility.Collapsed
        button_G_license_plate.Visibility = Visibility.Collapsed
        button_G_old_boot.Visibility = Visibility.Collapsed
        button_G_one_true_earring.Visibility = Visibility.Collapsed
        button_G_orange_soda.Visibility = Visibility.Collapsed
        button_G_sea_worther.Visibility = Visibility.Collapsed
        button_G_sextant.Visibility = Visibility.Collapsed
        button_G_soaked_candle.Visibility = Visibility.Collapsed
        button_G_toy_boat.Visibility = Visibility.Collapsed
        button_G_ukulele.Visibility = Visibility.Collapsed
        button_G_voodoo_doll.Visibility = Visibility.Collapsed
        button_G_wine_bottle_candle.Visibility = Visibility.Collapsed
        REM 草皮
        button_G_checkered_flooring.Visibility = Visibility.Collapsed
        button_G_carpeted_flooring.Visibility = Visibility.Collapsed
        button_G_deciduous_turf.Visibility = Visibility.Collapsed
        button_G_forest_turf.Visibility = Visibility.Collapsed
        button_G_grass_turf.Visibility = Visibility.Collapsed
        button_G_marsh_turf.Visibility = Visibility.Collapsed
        button_G_meadow_turf.Visibility = Visibility.Collapsed
        button_G_rocky_turf.Visibility = Visibility.Collapsed
        button_G_sandy_turf.Visibility = Visibility.Collapsed
        button_G_savanna_turf.Visibility = Visibility.Collapsed
        button_G_cave_rock_turf.Visibility = Visibility.Collapsed
        button_G_fungal_turf_blue.Visibility = Visibility.Collapsed
        button_G_fungal_turf_green.Visibility = Visibility.Collapsed
        button_G_fungal_turf_red.Visibility = Visibility.Collapsed
        button_G_guano_turf.Visibility = Visibility.Collapsed
        button_G_mud_turf.Visibility = Visibility.Collapsed
        button_G_slimey_turf.Visibility = Visibility.Collapsed
        button_G_snakeskin_rug.Visibility = Visibility.Collapsed
        button_G_jungle_turf.Visibility = Visibility.Collapsed
        button_G_magma_turf.Visibility = Visibility.Collapsed
        button_G_tidal_marsh_turf.Visibility = Visibility.Collapsed
        button_G_ashy_turf.Visibility = Visibility.Collapsed
        button_G_volcano_turf.Visibility = Visibility.Collapsed
        button_G_beach_turf.Visibility = Visibility.Collapsed
        button_G_scaled_flooring.Visibility = Visibility.Collapsed
        REM 宠物
        button_G_eye_bone.Visibility = Visibility.Collapsed
        button_G_fishbone.Visibility = Visibility.Collapsed
        button_G_lavae_egg.Visibility = Visibility.Collapsed
        button_G_lavae_tooth.Visibility = Visibility.Collapsed
        button_G_star_sky.Visibility = Visibility.Collapsed
        REM 解锁
        button_G_webbers_skull.Visibility = Visibility.Collapsed
        button_G_iron_key.Visibility = Visibility.Collapsed
        button_G_golden_key.Visibility = Visibility.Collapsed
        button_G_bone_key.Visibility = Visibility.Collapsed
        button_G_tarnished_crown.Visibility = Visibility.Collapsed
        button_G_grassy_thing.Visibility = Visibility.Collapsed
        button_G_ring_thing_sw.Visibility = Visibility.Collapsed
        button_G_screw_thing.Visibility = Visibility.Collapsed
        button_G_wooden_potato_thing.Visibility = Visibility.Collapsed
        REM 零件
        button_G_box_thing.Visibility = Visibility.Collapsed
        button_G_crank_thing.Visibility = Visibility.Collapsed
        button_G_metal_potato_thing.Visibility = Visibility.Collapsed
        button_G_ring_thing.Visibility = Visibility.Collapsed
        REM 其他
        button_G_ballphin_free_tuna.Visibility = Visibility.Collapsed
        button_G_message_in_a_bottle.Visibility = Visibility.Collapsed
    End Sub

    Private Sub G_DLC_ROG_SHOW()
        REM 材料
        button_G_marble.Visibility = Visibility.Visible
        button_G_beefalo_horn.Visibility = Visibility.Visible
        button_G_guano.Visibility = Visibility.Visible
        button_G_glommers_flower.Visibility = Visibility.Visible
        button_G_mosquito_sack.Visibility = Visibility.Visible
        button_G_volt_goat_horn.Visibility = Visibility.Visible
        button_G_walrus_tusk.Visibility = Visibility.Visible
        button_G_light_bulb.Visibility = Visibility.Visible
        button_G_bunny_puff.Visibility = Visibility.Visible
        button_G_foliage.Visibility = Visibility.Visible
        button_G_broken_shell.Visibility = Visibility.Visible
        button_G_slurtle_slime.Visibility = Visibility.Visible
        button_G_beard_hair.Visibility = Visibility.Visible
        button_G_slurper_pelt.Visibility = Visibility.Visible
        button_G_thulecite_fragments.Visibility = Visibility.Visible
        button_G_down_feather.Visibility = Visibility.Visible
        button_G_scales.Visibility = Visibility.Visible
        button_G_thick_fur.Visibility = Visibility.Visible
        REM 装备
        button_G_pickaxe_1.Visibility = Visibility.Visible
        button_G_walking_cane.Visibility = Visibility.Visible
        button_G_pretty_parasol.Visibility = Visibility.Visible
        button_G_rain_hat.Visibility = Visibility.Visible
        button_G_rain_coat.Visibility = Visibility.Visible
        button_G_eyebrella.Visibility = Visibility.Visible
        button_G_rabbit_earmuffs.Visibility = Visibility.Visible
        button_G_beefalo_hat.Visibility = Visibility.Visible
        button_G_winter_hat.Visibility = Visibility.Visible
        button_G_cat_cap.Visibility = Visibility.Visible
        button_G_dapper_vest.Visibility = Visibility.Visible
        button_G_breezy_vest.Visibility = Visibility.Visible
        button_G_puffy_vest.Visibility = Visibility.Visible
        button_G_hibearnation_vest.Visibility = Visibility.Visible
        button_G_morning_star.Visibility = Visibility.Visible
        button_G_weather_pain.Visibility = Visibility.Visible
        button_G_bat_bat.Visibility = Visibility.Visible
        button_G_belt_of_hunger.Visibility = Visibility.Visible
        button_G_thulecite_club.Visibility = Visibility.Visible
        button_G_grass_suit.Visibility = Visibility.Visible
        button_G_marble_suit.Visibility = Visibility.Visible
        button_G_scalemail.Visibility = Visibility.Visible
        button_G_thulecite_crown.Visibility = Visibility.Visible
        button_G_thulecite_suit.Visibility = Visibility.Visible
        button_G_shelmet.Visibility = Visibility.Visible
        button_G_snurtle_shell_armor.Visibility = Visibility.Visible
        button_G_tam_o_shanter.Visibility = Visibility.Visible
        button_G_spiderhat.Visibility = Visibility.Visible
        button_G_slurper.Visibility = Visibility.Visible
        REM 树苗
        button_G_birchnut.Visibility = Visibility.Visible
        button_G_pine_cone.Visibility = Visibility.Visible
        button_G_sapling.Visibility = Visibility.Visible
        button_G_grass_tuft.Visibility = Visibility.Visible
        button_G_berry_bush.Visibility = Visibility.Visible
        button_G_berry_bush_2.Visibility = Visibility.Visible
        button_G_spiky_bushes.Visibility = Visibility.Visible
        REM 生物
        button_G_rabbit.Visibility = Visibility.Visible
        button_G_winter_rabbit.Visibility = Visibility.Visible
        button_G_beardling.Visibility = Visibility.Visible
        button_G_moleworm.Visibility = Visibility.Visible
        button_G_bee.Visibility = Visibility.Visible
        button_G_killer_bee.Visibility = Visibility.Visible
        button_G_butterfly.Visibility = Visibility.Visible
        button_G_mosquito.Visibility = Visibility.Visible
        button_G_redbird.Visibility = Visibility.Visible
        button_G_snowbird.Visibility = Visibility.Visible
        button_G_crow.Visibility = Visibility.Visible
        button_G_fireflies.Visibility = Visibility.Visible
        REM 玩具
        button_G_ball_and_cup.Visibility = Visibility.Visible
        button_G_dessicated_tentacle.Visibility = Visibility.Visible
        button_G_fake_kazoo.Visibility = Visibility.Visible
        button_G_frazzled_wires.Visibility = Visibility.Visible
        button_G_gnome_1.Visibility = Visibility.Visible
        button_G_gords_knot.Visibility = Visibility.Visible
        button_G_hardened_rubber_bung.Visibility = Visibility.Visible
        button_G_lying_robot.Visibility = Visibility.Visible
        button_G_melty_marbles.Visibility = Visibility.Visible
        button_G_mismatched_buttons.Visibility = Visibility.Visible
        button_G_second_hand_dentures.Visibility = Visibility.Visible
        button_G_tiny_rocketship.Visibility = Visibility.Visible
        REM 草皮
        button_G_checkered_flooring.Visibility = Visibility.Visible
        button_G_carpeted_flooring.Visibility = Visibility.Visible
        button_G_deciduous_turf.Visibility = Visibility.Visible
        button_G_forest_turf.Visibility = Visibility.Visible
        button_G_grass_turf.Visibility = Visibility.Visible
        button_G_marsh_turf.Visibility = Visibility.Visible
        button_G_meadow_turf.Visibility = Visibility.Visible
        button_G_rocky_turf.Visibility = Visibility.Visible
        button_G_sandy_turf.Visibility = Visibility.Visible
        button_G_savanna_turf.Visibility = Visibility.Visible
        button_G_cave_rock_turf.Visibility = Visibility.Visible
        button_G_fungal_turf_blue.Visibility = Visibility.Visible
        button_G_fungal_turf_green.Visibility = Visibility.Visible
        button_G_fungal_turf_red.Visibility = Visibility.Visible
        button_G_guano_turf.Visibility = Visibility.Visible
        button_G_mud_turf.Visibility = Visibility.Visible
        button_G_slimey_turf.Visibility = Visibility.Visible
        REM 宠物
        button_G_eye_bone.Visibility = Visibility.Visible
        REM 解锁
        button_G_webbers_skull.Visibility = Visibility.Visible
        REM 零件
        button_G_box_thing.Visibility = Visibility.Visible
        button_G_crank_thing.Visibility = Visibility.Visible
        button_G_metal_potato_thing.Visibility = Visibility.Visible
        button_G_ring_thing.Visibility = Visibility.Visible
        REM 其他

    End Sub

    Private Sub G_DLC_SW_SHOW()
        REM 材料
        button_G_bamboo_patch.Visibility = Visibility.Visible
        button_G_cut_grass_SW.Visibility = Visibility.Visible
        button_G_vine.Visibility = Visibility.Visible
        button_G_sand.Visibility = Visibility.Visible
        button_G_snakeskin.Visibility = Visibility.Visible
        button_G_snake_oil.Visibility = Visibility.Visible
        button_G_palm_leaf.Visibility = Visibility.Visible
        button_G_venom_gland.Visibility = Visibility.Visible
        button_G_yellow_mosquito_sack.Visibility = Visibility.Visible
        button_G_seashell.Visibility = Visibility.Visible
        button_G_doydoy_feather.Visibility = Visibility.Visible
        button_G_dubloons.Visibility = Visibility.Visible
        button_G_hail.Visibility = Visibility.Visible
        button_G_horn.Visibility = Visibility.Visible
        button_G_coral.Visibility = Visibility.Visible
        button_G_obsidian.Visibility = Visibility.Visible
        button_G_cactus_spike.Visibility = Visibility.Visible
        button_G_dragoon_heart.Visibility = Visibility.Visible
        button_G_turbine_blades.Visibility = Visibility.Visible
        button_G_magic_seal.Visibility = Visibility.Visible
        button_G_shark_gills.Visibility = Visibility.Visible
        REM 装备
        button_G_machete.Visibility = Visibility.Visible
        button_G_luxury_machete.Visibility = Visibility.Visible
        button_G_obsidian_machete.Visibility = Visibility.Visible
        button_G_obsidian_axe.Visibility = Visibility.Visible
        button_G_rawling.Visibility = Visibility.Visible
        button_G_pirate_hat.Visibility = Visibility.Visible
        button_G_tropical_parasol.Visibility = Visibility.Visible
        button_G_snakeskin_hat.Visibility = Visibility.Visible
        button_G_snakeskin_jacket.Visibility = Visibility.Visible
        button_G_blubber_suit.Visibility = Visibility.Visible
        button_G_dumbrella.Visibility = Visibility.Visible
        button_G_windbreaker.Visibility = Visibility.Visible
        button_G_boat_cannon.Visibility = Visibility.Visible
        button_G_coconade.Visibility = Visibility.Visible
        button_G_obsidian_coconade.Visibility = Visibility.Visible
        button_G_poison_spear.Visibility = Visibility.Visible
        button_G_poison_dart.Visibility = Visibility.Visible
        button_G_spear_gun.Visibility = Visibility.Visible
        button_G_cutlass_supreme.Visibility = Visibility.Visible
        button_G_obsidian_spear.Visibility = Visibility.Visible
        button_G_eyeshot.Visibility = Visibility.Visible
        button_G_harpoon.Visibility = Visibility.Visible
        button_G_peg_leg.Visibility = Visibility.Visible
        button_G_trident.Visibility = Visibility.Visible
        button_G_seashell_suit.Visibility = Visibility.Visible
        button_G_limestone_suit.Visibility = Visibility.Visible
        button_G_cactus_armour.Visibility = Visibility.Visible
        button_G_horned_helmet.Visibility = Visibility.Visible
        button_G_obsidian_armour.Visibility = Visibility.Visible
        button_G_portable_crock_pot.Visibility = Visibility.Visible
        REM 树苗
        button_G_bamboo_root.Visibility = Visibility.Visible
        button_G_grass_tuft_SW.Visibility = Visibility.Visible
        button_G_coconut.Visibility = Visibility.Visible
        button_G_jungle_tree_seed.Visibility = Visibility.Visible
        button_G_viney_bush_root.Visibility = Visibility.Visible
        button_G_coffee_plant.Visibility = Visibility.Visible
        button_G_elephant_cactus_stump.Visibility = Visibility.Visible
        REM 生物
        button_G_crabbit.Visibility = Visibility.Visible
        button_G_beardling_sw.Visibility = Visibility.Visible
        button_G_dead_dogfish.Visibility = Visibility.Visible
        button_G_dead_swordfish.Visibility = Visibility.Visible
        button_G_dead_wobster.Visibility = Visibility.Visible
        button_G_bioluminescence.Visibility = Visibility.Visible
        button_G_jellyfish.Visibility = Visibility.Visible
        button_G_dead_jellyfish.Visibility = Visibility.Visible
        button_G_butterfly_sw.Visibility = Visibility.Visible
        button_G_mosquito_sw.Visibility = Visibility.Visible
        button_G_parrot.Visibility = Visibility.Visible
        button_G_parrot_pirate.Visibility = Visibility.Visible
        button_G_toucan.Visibility = Visibility.Visible
        button_G_seagull.Visibility = Visibility.Visible
        REM 玩具
        button_G_ancient_vase.Visibility = Visibility.Visible
        button_G_brain_cloud_pill.Visibility = Visibility.Visible
        button_G_broken_AAC_device.Visibility = Visibility.Visible
        button_G_license_plate.Visibility = Visibility.Visible
        button_G_old_boot.Visibility = Visibility.Visible
        button_G_one_true_earring.Visibility = Visibility.Visible
        button_G_orange_soda.Visibility = Visibility.Visible
        button_G_sea_worther.Visibility = Visibility.Visible
        button_G_sextant.Visibility = Visibility.Visible
        button_G_soaked_candle.Visibility = Visibility.Visible
        button_G_toy_boat.Visibility = Visibility.Visible
        button_G_ukulele.Visibility = Visibility.Visible
        button_G_voodoo_doll.Visibility = Visibility.Visible
        button_G_wine_bottle_candle.Visibility = Visibility.Visible
        REM 草皮
        button_G_snakeskin_rug.Visibility = Visibility.Visible
        button_G_jungle_turf.Visibility = Visibility.Visible
        button_G_magma_turf.Visibility = Visibility.Visible
        button_G_tidal_marsh_turf.Visibility = Visibility.Visible
        button_G_ashy_turf.Visibility = Visibility.Visible
        button_G_volcano_turf.Visibility = Visibility.Visible
        button_G_beach_turf.Visibility = Visibility.Visible
        REM 宠物
        button_G_fishbone.Visibility = Visibility.Visible
        REM 解锁
        button_G_iron_key.Visibility = Visibility.Visible
        button_G_golden_key.Visibility = Visibility.Visible
        button_G_bone_key.Visibility = Visibility.Visible
        button_G_tarnished_crown.Visibility = Visibility.Visible
        REM 零件
        button_G_grassy_thing.Visibility = Visibility.Visible
        button_G_ring_thing_sw.Visibility = Visibility.Visible
        button_G_screw_thing.Visibility = Visibility.Visible
        button_G_wooden_potato_thing.Visibility = Visibility.Visible
        REM 其他
        button_G_ballphin_free_tuna.Visibility = Visibility.Visible
        button_G_message_in_a_bottle.Visibility = Visibility.Visible
    End Sub

    Private Sub G_DLC_DST_SHOW()
        REM 材料
        button_G_marble.Visibility = Visibility.Visible
        button_G_moon_rock.Visibility = Visibility.Visible
        button_G_beefalo_horn.Visibility = Visibility.Visible
        button_G_guano.Visibility = Visibility.Visible
        button_G_glommers_flower.Visibility = Visibility.Visible
        button_G_mosquito_sack.Visibility = Visibility.Visible
        button_G_volt_goat_horn.Visibility = Visibility.Visible
        button_G_walrus_tusk.Visibility = Visibility.Visible
        button_G_steel_wool.Visibility = Visibility.Visible
        button_G_phlegm.Visibility = Visibility.Visible
        button_G_light_bulb.Visibility = Visibility.Visible
        button_G_bunny_puff.Visibility = Visibility.Visible
        button_G_foliage.Visibility = Visibility.Visible
        button_G_broken_shell.Visibility = Visibility.Visible
        button_G_slurtle_slime.Visibility = Visibility.Visible
        button_G_beard_hair.Visibility = Visibility.Visible
        button_G_slurper_pelt.Visibility = Visibility.Visible
        button_G_thulecite_fragments.Visibility = Visibility.Visible
        button_G_down_feather.Visibility = Visibility.Visible
        button_G_scales.Visibility = Visibility.Visible
        button_G_fur_tuft.Visibility = Visibility.Visible
        button_G_thick_fur.Visibility = Visibility.Visible
        REM 装备
        button_G_saddlehorn.Visibility = Visibility.Visible
        button_G_brush.Visibility = Visibility.Visible
        button_G_pickaxe_1.Visibility = Visibility.Visible
        button_G_walking_cane.Visibility = Visibility.Visible
        button_G_pretty_parasol.Visibility = Visibility.Visible
        button_G_rain_hat.Visibility = Visibility.Visible
        button_G_rain_coat.Visibility = Visibility.Visible
        button_G_eyebrella.Visibility = Visibility.Visible
        button_G_whirly_fan.Visibility = Visibility.Visible
        button_G_rabbit_earmuffs.Visibility = Visibility.Visible
        button_G_beefalo_hat.Visibility = Visibility.Visible
        button_G_winter_hat.Visibility = Visibility.Visible
        button_G_cat_cap.Visibility = Visibility.Visible
        button_G_dapper_vest.Visibility = Visibility.Visible
        button_G_breezy_vest.Visibility = Visibility.Visible
        button_G_puffy_vest.Visibility = Visibility.Visible
        button_G_hibearnation_vest.Visibility = Visibility.Visible
        button_G_morning_star.Visibility = Visibility.Visible
        button_G_tail_o_three_cats.Visibility = Visibility.Visible
        button_G_weather_pain.Visibility = Visibility.Visible
        button_G_bat_bat.Visibility = Visibility.Visible
        button_G_belt_of_hunger.Visibility = Visibility.Visible
        button_G_thulecite_club.Visibility = Visibility.Visible
        button_G_grass_suit.Visibility = Visibility.Visible
        button_G_marble_suit.Visibility = Visibility.Visible
        button_G_scalemail.Visibility = Visibility.Visible
        button_G_thulecite_crown.Visibility = Visibility.Visible
        button_G_thulecite_suit.Visibility = Visibility.Visible
        button_G_shelmet.Visibility = Visibility.Visible
        button_G_snurtle_shell_armor.Visibility = Visibility.Visible
        button_G_tam_o_shanter.Visibility = Visibility.Visible
        button_G_spiderhat.Visibility = Visibility.Visible
        button_G_slurper.Visibility = Visibility.Visible
        REM 树苗
        button_G_birchnut.Visibility = Visibility.Visible
        button_G_pine_cone.Visibility = Visibility.Visible
        button_G_sapling.Visibility = Visibility.Visible
        button_G_grass_tuft.Visibility = Visibility.Visible
        button_G_berry_bush.Visibility = Visibility.Visible
        button_G_berry_bush_2.Visibility = Visibility.Visible
        button_G_spiky_bushes.Visibility = Visibility.Visible
        button_G_juicy_berry_bush.Visibility = Visibility.Visible
        button_G_twiggy_tree_cone.Visibility = Visibility.Visible
        REM 生物
        button_G_rabbit.Visibility = Visibility.Visible
        button_G_winter_rabbit.Visibility = Visibility.Visible
        button_G_beardling.Visibility = Visibility.Visible
        button_G_moleworm.Visibility = Visibility.Visible
        button_G_bee.Visibility = Visibility.Visible
        button_G_killer_bee.Visibility = Visibility.Visible
        button_G_butterfly.Visibility = Visibility.Visible
        button_G_mosquito.Visibility = Visibility.Visible
        button_G_redbird.Visibility = Visibility.Visible
        button_G_snowbird.Visibility = Visibility.Visible
        button_G_crow.Visibility = Visibility.Visible
        button_G_fireflies.Visibility = Visibility.Visible
        button_G_blue_spore.Visibility = Visibility.Visible
        button_G_green_spore.Visibility = Visibility.Visible
        button_G_red_spore.Visibility = Visibility.Visible
        REM 玩具
        button_G_air_unfreshener.Visibility = Visibility.Visible
        button_G_back_scratcher.Visibility = Visibility.Visible
        button_G_ball_and_cup.Visibility = Visibility.Visible
        button_G_beaten_beater.Visibility = Visibility.Visible
        button_G_bent_spork.Visibility = Visibility.Visible
        button_G_black_bishop.Visibility = Visibility.Visible
        button_G_dessicated_tentacle.Visibility = Visibility.Visible
        button_G_fake_kazoo.Visibility = Visibility.Visible
        button_G_frayed_yarn.Visibility = Visibility.Visible
        button_G_frazzled_wires.Visibility = Visibility.Visible
        button_G_gnome_1.Visibility = Visibility.Visible
        button_G_gnome_2.Visibility = Visibility.Visible
        button_G_gords_knot.Visibility = Visibility.Visible
        button_G_hardened_rubber_bung.Visibility = Visibility.Visible
        button_G_leaky_teacup.Visibility = Visibility.Visible
        button_G_lucky_cat_jar.Visibility = Visibility.Visible
        button_G_lying_robot.Visibility = Visibility.Visible
        button_G_melty_marbles.Visibility = Visibility.Visible
        button_G_mismatched_buttons.Visibility = Visibility.Visible
        button_G_potato_cup.Visibility = Visibility.Visible
        button_G_second_hand_dentures.Visibility = Visibility.Visible
        button_G_shoe_horn.Visibility = Visibility.Visible
        button_G_tiny_rocketship.Visibility = Visibility.Visible
        button_G_toy_trojan_horse.Visibility = Visibility.Visible
        button_G_unbalanced_top.Visibility = Visibility.Visible
        button_G_white_bishop.Visibility = Visibility.Visible
        button_G_wire_hanger.Visibility = Visibility.Visible
        REM 草皮
        button_G_checkered_flooring.Visibility = Visibility.Visible
        button_G_carpeted_flooring.Visibility = Visibility.Visible
        button_G_scaled_flooring.Visibility = Visibility.Visible
        button_G_deciduous_turf.Visibility = Visibility.Visible
        button_G_forest_turf.Visibility = Visibility.Visible
        button_G_grass_turf.Visibility = Visibility.Visible
        button_G_marsh_turf.Visibility = Visibility.Visible
        button_G_meadow_turf.Visibility = Visibility.Visible
        button_G_rocky_turf.Visibility = Visibility.Visible
        button_G_sandy_turf.Visibility = Visibility.Visible
        button_G_savanna_turf.Visibility = Visibility.Visible
        button_G_cave_rock_turf.Visibility = Visibility.Visible
        button_G_fungal_turf_blue.Visibility = Visibility.Visible
        button_G_fungal_turf_green.Visibility = Visibility.Visible
        button_G_fungal_turf_red.Visibility = Visibility.Visible
        button_G_guano_turf.Visibility = Visibility.Visible
        button_G_mud_turf.Visibility = Visibility.Visible
        REM 宠
        button_G_eye_bone.Visibility = Visibility.Visible
        button_G_lavae_egg.Visibility = Visibility.Visible
        button_G_lavae_tooth.Visibility = Visibility.Visible
        button_G_star_sky.Visibility = Visibility.Visible
        REM 解锁

        REM 零件

        REM 其他
    End Sub

    REM 物品DLC检测
    Private Sub G_DLC_Check()

        Dim G_ROG_SW_DST As SByte
        Dim G_ROG__ As SByte
        Dim G_SW__ As SByte
        Dim G_DST__ As SByte
        If checkBox_G_DLC_ROG.IsChecked = True Then
            G_ROG__ = 1
        Else
            G_ROG__ = 0
        End If
        If checkBox_G_DLC_SW.IsChecked = True Then
            G_SW__ = 2
        Else
            G_SW__ = 0
        End If
        If checkBox_G_DLC_DST.IsChecked = True Then
            G_DST__ = 4
        Else
            G_DST__ = 0
        End If
        G_ROG_SW_DST = G_ROG__ + G_SW__ + G_DST__
        If G_ROG_SW_DST = 0 Then
            MsgBox("至少选择一项！")
            checkBox_G_DLC_ROG.IsChecked = True
            G_DLC_Check()
        Else
            G_DLC_Check_initialization()
            Select Case G_ROG_SW_DST
                Case 1
                    G_DLC_ROG_SHOW()
                    WrapPanel_G_material.Height = 410
                    WrapPanel_G_equipment.Height = 570
                    WrapPanel_G_sapling.Height = 90
                    WrapPanel_G_animal.Height = 170
                    WrapPanel_G_toys.Height = 170
                    WrapPanel_G_turf.Height = 170
                    TextBlock_G_unlock.Visibility = Visibility.Visible
                    WrapPanel_G_unlock.Visibility = Visibility.Visible
                    TextBlock_G_component.Visibility = Visibility.Visible
                    WrapPanel_G_component.Visibility = Visibility.Visible
                    WrapPanel_Goods.Height = 3270 - 11 * 80
                    Reg_Write("Goods", 1)
                Case 2
                    G_DLC_SW_SHOW()
                    WrapPanel_G_material.Height = 490
                    WrapPanel_G_equipment.Height = 570
                    WrapPanel_G_sapling.Height = 90
                    WrapPanel_G_animal.Height = 170
                    WrapPanel_G_toys.Height = 170
                    WrapPanel_G_turf.Height = 90
                    TextBlock_G_unlock.Visibility = Visibility.Visible
                    WrapPanel_G_unlock.Visibility = Visibility.Visible
                    TextBlock_G_component.Visibility = Visibility.Visible
                    WrapPanel_G_component.Visibility = Visibility.Visible
                    WrapPanel_Goods.Height = 3270 - 11 * 80
                    Reg_Write("Goods", 2)
                Case 3
                    G_DLC_ROG_SHOW()
                    G_DLC_SW_SHOW()
                    WrapPanel_G_material.Height = 570
                    WrapPanel_G_equipment.Height = 730
                    WrapPanel_G_sapling.Height = 170
                    WrapPanel_G_animal.Height = 250
                    WrapPanel_G_toys.Height = 250
                    WrapPanel_G_turf.Height = 250
                    TextBlock_G_unlock.Visibility = Visibility.Visible
                    WrapPanel_G_unlock.Visibility = Visibility.Visible
                    TextBlock_G_component.Visibility = Visibility.Visible
                    WrapPanel_G_component.Visibility = Visibility.Visible
                    WrapPanel_Goods.Height = 3270 - 3 * 80
                    Reg_Write("Goods", 3)
                Case 4
                    G_DLC_DST_SHOW()
                    WrapPanel_G_material.Height = 490
                    WrapPanel_G_equipment.Height = 570
                    WrapPanel_G_sapling.Height = 90
                    WrapPanel_G_animal.Height = 170
                    WrapPanel_G_toys.Height = 250
                    WrapPanel_G_turf.Height = 170
                    TextBlock_G_unlock.Visibility = Visibility.Collapsed
                    WrapPanel_G_unlock.Visibility = Visibility.Collapsed
                    TextBlock_G_component.Visibility = Visibility.Collapsed
                    WrapPanel_G_component.Visibility = Visibility.Collapsed
                    WrapPanel_Goods.Height = 3270 - 11 * 80 - 35.4 * 2
                    Reg_Write("Goods", 4)
                Case 5
                    G_DLC_ROG_SHOW()
                    G_DLC_DST_SHOW()
                    WrapPanel_G_material.Height = 490
                    WrapPanel_G_equipment.Height = 570
                    WrapPanel_G_sapling.Height = 90
                    WrapPanel_G_animal.Height = 170
                    WrapPanel_G_toys.Height = 250
                    WrapPanel_G_turf.Height = 170
                    TextBlock_G_unlock.Visibility = Visibility.Visible
                    WrapPanel_G_unlock.Visibility = Visibility.Visible
                    TextBlock_G_component.Visibility = Visibility.Visible
                    WrapPanel_G_component.Visibility = Visibility.Visible
                    WrapPanel_Goods.Height = 3270 - 9 * 80
                    Reg_Write("Goods", 5)
                Case 6
                    G_DLC_SW_SHOW()
                    G_DLC_DST_SHOW()
                    WrapPanel_G_material.Height = 650
                    WrapPanel_G_equipment.Height = 810
                    WrapPanel_G_sapling.Height = 170
                    WrapPanel_G_animal.Height = 250
                    WrapPanel_G_toys.Height = 330
                    WrapPanel_G_turf.Height = 250
                    TextBlock_G_unlock.Visibility = Visibility.Visible
                    WrapPanel_G_unlock.Visibility = Visibility.Visible
                    TextBlock_G_component.Visibility = Visibility.Visible
                    WrapPanel_G_component.Visibility = Visibility.Visible
                    WrapPanel_Goods.Height = 3270
                    Reg_Write("Goods", 6)
                Case 7
                    G_DLC_ROG_SHOW()
                    G_DLC_SW_SHOW()
                    G_DLC_DST_SHOW()
                    WrapPanel_G_material.Height = 650
                    WrapPanel_G_equipment.Height = 810
                    WrapPanel_G_sapling.Height = 170
                    WrapPanel_G_animal.Height = 250
                    WrapPanel_G_toys.Height = 330
                    WrapPanel_G_turf.Height = 250
                    TextBlock_G_unlock.Visibility = Visibility.Visible
                    WrapPanel_G_unlock.Visibility = Visibility.Visible
                    TextBlock_G_component.Visibility = Visibility.Visible
                    WrapPanel_G_component.Visibility = Visibility.Visible
                    WrapPanel_Goods.Height = 3270
                    Reg_Write("Goods", 7)
            End Select
        End If
    End Sub

    Private Sub checkBox_G_DLC_ROG_click(sender As Object, e As RoutedEventArgs) Handles checkBox_G_DLC_ROG.Click
        G_DLC_Check()
    End Sub

    Private Sub GL_button_G_DLC_ROG_click(sender As Object, e As RoutedEventArgs) Handles GL_button_G_DLC_ROG.Click
        If checkBox_G_DLC_ROG.IsChecked = True Then
            checkBox_G_DLC_ROG.IsChecked = False
        Else
            checkBox_G_DLC_ROG.IsChecked = True
        End If
        checkBox_G_DLC_ROG_click(Nothing, Nothing)
    End Sub

    Private Sub checkBox_G_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles checkBox_G_DLC_SW.Click
        G_DLC_Check()
    End Sub

    Private Sub GL_button_G_DLC_SW_click(sender As Object, e As RoutedEventArgs) Handles GL_button_G_DLC_SW.Click
        If checkBox_G_DLC_SW.IsChecked = True Then
            checkBox_G_DLC_SW.IsChecked = False
        Else
            checkBox_G_DLC_SW.IsChecked = True
        End If
        checkBox_G_DLC_SW_click(Nothing, Nothing)
    End Sub

    Private Sub checkBox_G_DLC_DST_click(sender As Object, e As RoutedEventArgs) Handles checkBox_G_DLC_DST.Click
        G_DLC_Check()
    End Sub

    Private Sub GL_button_G_DLC_DST_click(sender As Object, e As RoutedEventArgs) Handles GL_button_G_DLC_DST.Click
        If checkBox_G_DLC_DST.IsChecked = True Then
            checkBox_G_DLC_DST.IsChecked = False
        Else
            checkBox_G_DLC_DST.IsChecked = True
        End If
        checkBox_G_DLC_DST_click(Nothing, Nothing)
    End Sub

    REM --------------------设置面板--------------------
    Private Sub Se_button_CreateShortCut_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Se_button_CreateShortCut.Click
        ' 使用COM导入库
        Dim wsh As New IWshShell_Class
        Dim desk As String = wsh.SpecialFolders.Item("Desktop") '从SHELL枚举中获得桌面路径
        Dim lnk As IWshShortcut = wsh.CreateShortcut(desk & "\饥荒百科全书.lnk") '在桌面上创建说明文件的路径，注意扩展名为 .lnk

        With lnk
            '.Arguments = "/?" '传递参数
            .Description = "饥荒百科全书"
            '.IconLocation = Application.StartupPath & "\Mac.ico" '快捷方式的图标，空表示使用默认文件图标，可使用ico或exe、dll shell.dll,23
            .TargetPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase & Process.GetCurrentProcess.ProcessName & ".exe" '目标文件路径
            .WindowStyle = 1 '打开窗体的风格
            .WorkingDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase '工作路径

            .Save() '保存快捷方式
        End With

        MsgBox("已创建桌面快捷方式!", MsgBoxStyle.Information)
    End Sub

    Private Sub Se_button_Background_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Se_button_Background.Click
        Dim OFD As New OpenFileDialog()
        OFD.FileName = "" ' 默认文件名
        OFD.DefaultExt = ".png" ' 默认文件扩展名
        OFD.Filter = "图像文件 (*.bmp;*.gif;*.jpg;*.jpeg;*.png)|*.bmp;*.gif;*.jpg;*.jpeg;*.png" ' 文件扩展名过滤器

        ' 显示打开文件对话框
        Dim result? As Boolean = OFD.ShowDialog()
        Dim LeftCanvas() As Canvas = {Canvas_CharacterLeft, Canvas_CharacterLeft_Wolfgang, Canvas_FoodLeft, Canvas_ScienceLeft, Canvas_CookingSimulatorLeft, Canvas_AnimalLeft, Canvas_AnimalLeft_Krampus, Canvas_AnimalLeft_Apackim_baggims, Canvas_AnimalLeft_PigKing, Canvas_AnimalLeft_Yaarctopus, Canvas_NaturalLeft_B, Canvas_NaturalLeft_P, Canvas_GoodsLeft_M, Canvas_GoodsLeft_E, Canvas_GoodsLeft_S, Canvas_GoodsLeft_A, Canvas_GoodsLeft_T, Canvas_GoodsLeft_P, Canvas_GoodsLeft_PL, Canvas_GoodsLeft_U, Canvas_GoodsLeft_C, Canvas_GoodsLeft_B, Canvas_GoodsLeft_BFT, Canvas_GoodsLeft_MIAB, Canvas_Setting}
        Dim RightWrapPanel() As WrapPanel = {WrapPanel_Character, WrapPanel_Food, WrapPanel_Science, WrapPanel_CookingSimulator, WrapPanel_Animal, WrapPanel_Natural, WrapPanel_Goods}

        '' 处理文件对话框结果
        'If result = True Then
        '    ' 打开文档
        '    Dim filename As String = OFD.FileName
        'End If

        Try
            BackGroundBorder.Visibility = Visibility.Visible
            Dim PictruePath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\JiHuangBaiKe\"
            If (Directory.Exists(PictruePath)) = False Then
                Directory.CreateDirectory(PictruePath)
            End If
            Dim filename = Path.GetFileName(OFD.FileName)
            Try
                FileIO.FileSystem.CopyFile(OFD.FileName, PictruePath & filename, True)
            Catch ex As Exception
            End Try
            Dim brush As New ImageBrush
            brush.ImageSource = New BitmapImage(New Uri(PictruePath & filename))
            BackGroundBorder.Background = brush
            Reg_Write_string("Background", PictruePath & filename)
        Catch ex As Exception
            MsgBox("没有选择正确的图片")
            For i = 0 To LeftCanvas.Length - 1
                LeftCanvas(i).Background.Opacity = 1
            Next
            For i = 0 To RightWrapPanel.Length - 1
                RightWrapPanel(i).Background.Opacity = 1
            Next
        End Try

        Dim RegReadBG As String = Reg_Read_string("Background")
        If RegReadBG <> "" Then
            For i = 0 To LeftCanvas.Length - 1
                LeftCanvas(i).Background.Opacity = PanelAlpha
            Next
            For i = 0 To RightWrapPanel.Length - 1
                RightWrapPanel(i).Background.Opacity = PanelAlpha
            Next
            Se_TextBlock_Alpha.Foreground = Brushes.Black
            Setting_slider_Alpha.IsEnabled = True
        Else
            Se_TextBlock_Alpha.Foreground = Brushes.Silver
            Setting_slider_Alpha.IsEnabled = False
        End If

    End Sub

    Private Sub Se_button_Background_Clear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Se_button_Background_Clear.Click
        BackGroundBorder.Visibility = Visibility.Collapsed
        Reg_Write_string("Background", "")
        Dim LeftCanvas() As Canvas = {Canvas_CharacterLeft, Canvas_CharacterLeft_Wolfgang, Canvas_FoodLeft, Canvas_ScienceLeft, Canvas_CookingSimulatorLeft, Canvas_AnimalLeft, Canvas_AnimalLeft_Krampus, Canvas_AnimalLeft_Apackim_baggims, Canvas_AnimalLeft_PigKing, Canvas_AnimalLeft_Yaarctopus, Canvas_NaturalLeft_B, Canvas_NaturalLeft_P, Canvas_GoodsLeft_M, Canvas_GoodsLeft_E, Canvas_GoodsLeft_S, Canvas_GoodsLeft_A, Canvas_GoodsLeft_T, Canvas_GoodsLeft_P, Canvas_GoodsLeft_PL, Canvas_GoodsLeft_U, Canvas_GoodsLeft_C, Canvas_GoodsLeft_B, Canvas_GoodsLeft_BFT, Canvas_GoodsLeft_MIAB, Canvas_Setting}
        For i = 0 To LeftCanvas.Length - 1
            LeftCanvas(i).Background.Opacity = 1
        Next
        Dim RightWrapPanel() As WrapPanel = {WrapPanel_Character, WrapPanel_Food, WrapPanel_Science, WrapPanel_CookingSimulator, WrapPanel_Natural, WrapPanel_Animal, WrapPanel_Goods}
        For i = 0 To RightWrapPanel.Length - 1
            RightWrapPanel(i).Background.Opacity = 1
        Next
        Se_TextBlock_Alpha.Foreground = Brushes.Silver
        Setting_slider_Alpha.IsEnabled = False
    End Sub

    Private Sub Setting_slider_Alpha_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles Setting_slider_Alpha.ValueChanged
        Dim Alpha As Integer = Setting_slider_Alpha.Value
        PanelAlpha = Alpha / 100
        Se_TextBlock_Alpha.Text = "面板不透明度 : " & Alpha & "%"
        Dim LeftCanvas() As Canvas = {Canvas_CharacterLeft, Canvas_CharacterLeft_Wolfgang, Canvas_FoodLeft, Canvas_ScienceLeft, Canvas_CookingSimulatorLeft, Canvas_AnimalLeft, Canvas_AnimalLeft_Krampus, Canvas_AnimalLeft_Apackim_baggims, Canvas_AnimalLeft_PigKing, Canvas_AnimalLeft_Yaarctopus, Canvas_NaturalLeft_B, Canvas_NaturalLeft_P, Canvas_GoodsLeft_M, Canvas_GoodsLeft_E, Canvas_GoodsLeft_S, Canvas_GoodsLeft_A, Canvas_GoodsLeft_T, Canvas_GoodsLeft_P, Canvas_GoodsLeft_PL, Canvas_GoodsLeft_U, Canvas_GoodsLeft_C, Canvas_GoodsLeft_B, Canvas_GoodsLeft_BFT, Canvas_GoodsLeft_MIAB, Canvas_Setting}
        For i = 0 To LeftCanvas.Length - 1
            LeftCanvas(i).Background.Opacity = PanelAlpha
        Next
        Dim RightWrapPanel() As WrapPanel = {WrapPanel_Character, WrapPanel_Food, WrapPanel_Science, WrapPanel_CookingSimulator, WrapPanel_Animal, WrapPanel_Natural, WrapPanel_Goods}
        For i = 0 To RightWrapPanel.Length - 1
            RightWrapPanel(i).Background.Opacity = PanelAlpha
        Next
    End Sub

End Class