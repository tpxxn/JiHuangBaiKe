Imports System.Windows.Forms

Public Class Splash

    Public splashTimer As New Timer()

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        REM 禁止重复运行
        If UBound(Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName)) > 0 Then
            MsgBox("请不要重复运行！(ノ｀Д)ノ")
            End
        End If
        REM 加载主窗体
        AddHandler splashTimer.Tick, AddressOf splashStop
        splashTimer.Interval = 1
        splashTimer.Start()
    End Sub

    Private Sub splashStop(ByVal sender As Object, ByVal e As EventArgs)
        Dim MainWindowShow As New MainWindow
        MainWindowShow.Show()
        splashTimer.Enabled = False
        Close()
    End Sub

End Class
