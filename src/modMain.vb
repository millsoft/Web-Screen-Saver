Module modMain
    Public defaultUrl As String = "http://www.millsoft.de"


    Sub Main(ByVal args As String())

        Dim frmMain As New frmMain
        Dim frmOpt As New frmOptions
        Dim frmPrv As New frmPreview
    
        Try
            'Make sure the screen saver is not already running. This can happen
            If Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName).Length > 1 Then Application.Exit()

            'Command Line Arguments
            Dim gOpenArgs() As String = Environment.GetCommandLineArgs()

            'get arguments
            Select Case gOpenArgs.Length
                Case 0, 1  'Started from IDE or Explorer
                    frmMain.sDisplay = gOpenArgs
                    Application.Run(frmMain)
                Case Else
                    Select Case Mid(gOpenArgs(1).ToLower, 1, 2)
                        Case "/p"   'Preview Screensaver
                            If gOpenArgs.Length > 1 Then
                                SetForm(frmPrv, gOpenArgs(2))
                                Application.Run(frmPrv)
                            Else
                                frmMain.sDisplay = gOpenArgs
                                Application.Run(frmMain)
                            End If
                        Case "/c"   'Configure Screensaver
                            Application.Run(frmOpt)
                        Case "/s"   'Normal Screensaver
                            Application.Run(frmMain)
                        Case Else
                            frmMain.sDisplay = gOpenArgs
                            Application.Run(frmMain)
                    End Select
            End Select

        Catch Ex As Exception
            MessageBox.Show(Ex.ToString, "Error Loading Web Screen Saver", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub SetForm(ByRef frm As Form, ByRef arg As String)

        Dim style As Integer
        Dim previewHandle As Integer = Int32.Parse(CType(arg, String))
        Dim clsAPI As New clsWinAPI
        Dim R As New clsWinAPI.RECT

        'get dimensions of preview window
        clsWinAPI.GetClientRect(previewHandle, R)

        With frm
            .WindowState = FormWindowState.Normal
            .FormBorderStyle = FormBorderStyle.None
            .Width = R.right
            .Height = R.bottom
        End With

        'get and set new window style
        style = clsAPI.GetWindowLong(frm.Handle.ToInt32, clsAPI.GWL_STYLE)
        style = style Or clsAPI.WS_CHILD
        clsAPI.SetWindowLong(frm.Handle.ToInt32, clsAPI.GWL_STYLE, style)

        'set parent window (preview window)
        clsAPI.SetParent(frm.Handle.ToInt32, previewHandle)

        'save preview in forms window structure
        clsAPI.SetWindowLong(frm.Handle.ToInt32, clsAPI.GWL_HWNDPARENT, previewHandle)
        clsAPI.SetWindowPos(frm.Handle.ToInt32, 0, R.left, 0, R.right, R.bottom, clsAPI.SWP_NOACTIVATE Or clsAPI.SWP_NOZORDER Or clsAPI.SWP_SHOWWINDOW)

    End Sub

    Sub _saveSettings(key As String, value As String)
        SaveSetting("websaver", "settings", key, value)
    End Sub

    Function _getSettings(key As String, Optional defaultValue As String = "") As String
        Return GetSetting("websaver", "settings", key, defaultValue)
    End Function

End Module
