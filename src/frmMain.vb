
Public Class frmMain
    Inherits System.Windows.Forms.Form
    Private lastPos As PointAPI
    Public sDisplay As String()
    Friend WithEvents wb1 As System.Windows.Forms.WebBrowser
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Dim gTolerance As Integer
    Structure PointAPI
        Public x As Int32
        Public y As Int32
    End Structure

    Public Declare Function GetCursorPos Lib "user32" (ByRef lpPoint As PointAPI) As Boolean

    Declare Function ScreenToClient Lib "user32" (ByVal hwnd As Int32, ByRef lpPoint As PointAPI) As Int32

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lblDisplay As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblDisplay = New System.Windows.Forms.Label()
        Me.wb1 = New System.Windows.Forms.WebBrowser()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 200
        '
        'lblDisplay
        '
        Me.lblDisplay.AutoSize = True
        Me.lblDisplay.BackColor = System.Drawing.Color.Transparent
        Me.lblDisplay.ForeColor = System.Drawing.Color.Yellow
        Me.lblDisplay.Location = New System.Drawing.Point(80, 120)
        Me.lblDisplay.Name = "lblDisplay"
        Me.lblDisplay.Size = New System.Drawing.Size(0, 13)
        Me.lblDisplay.TabIndex = 0
        '
        'wb1
        '
        Me.wb1.AllowNavigation = False
        Me.wb1.AllowWebBrowserDrop = False
        Me.wb1.IsWebBrowserContextMenuEnabled = False
        Me.wb1.Location = New System.Drawing.Point(162, 39)
        Me.wb1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wb1.Name = "wb1"
        Me.wb1.ScriptErrorsSuppressed = True
        Me.wb1.ScrollBarsEnabled = False
        Me.wb1.Size = New System.Drawing.Size(582, 414)
        Me.wb1.TabIndex = 1
        Me.wb1.Url = New System.Uri("http://www.millsoft.de", System.UriKind.Absolute)
        Me.wb1.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.DeepSkyBlue
        Me.Label1.Location = New System.Drawing.Point(16, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(139, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Loading Screensaver..."
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(1059, 608)
        Me.Controls.Add(Me.wb1)
        Me.Controls.Add(Me.lblDisplay)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmMain"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region " Form Events "

    Private Sub frmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'If any key is pressed, Close the application
        Me.Close()
    End Sub

#End Region

#Region " Controls "

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        checkPos()
    End Sub

#End Region

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Cursor.Hide()
        wb1.Dock = DockStyle.Fill
        wb1.Navigate(_getSettings("url", defaultUrl))




    End Sub

    'save the mouse position - used by launch - if it is changed, close the saver
    Sub setFirstMousePos()
        Dim lpPoint As PointAPI
        GetCursorPos(lpPoint)
        lastPos = lpPoint
    End Sub
    'Check the Mouse Position - close the saver if the mouse was moved 
    Private Sub checkPos()
        Dim lpPoint As PointAPI
        GetCursorPos(lpPoint)

        If lpPoint.x <> lastPos.x Or lpPoint.y <> lastPos.y Then
            gTolerance += 1

        End If

        'Close the screen saver if the mouse was moved by x steps
        If gTolerance > 2 Then
            Me.Close()
        End If

        lastPos = lpPoint
    End Sub

    'Do stuff after the page has been loaded
    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles wb1.DocumentCompleted

        'Display the Browser - this will also hide the "Please Wait..." label
        wb1.Visible = True
    End Sub

    Private Sub frmMain_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        setFirstMousePos()
    End Sub
End Class
