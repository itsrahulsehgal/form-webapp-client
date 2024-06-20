Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class CreateSubmissionForm
    Private stopwatch As New Stopwatch()
    Private httpClient As New HttpClient()
    Private WithEvents timer As New Timer()

    ' Declare form controls
    Private txtName As TextBox
    Private txtEmail As TextBox
    Private txtPhone As TextBox
    Private txtGithubLink As TextBox
    Private txtStopwatchTime As TextBox
    Private btnToggleStopwatch As Button
    Private btnSubmit As Button

    Public Sub New()
        InitializeComponent()
        timer.Interval = 1000 ' Update every second
        AddHandler timer.Tick, AddressOf Timer_Tick

    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs)
        txtStopwatchTime.Text = stopwatch.Elapsed.ToString("hh\:mm\:ss")
    End Sub

    Private Sub InitializeComponent()
        Me.ClientSize = New Size(600, 500)
        Me.Text = "Rahul Sehgal, Slidely Task 2 - Slidely Form App"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.LightGray
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False

        ' Initialize and add txtName
        txtName = New TextBox()
        txtName.Location = New Point(20, 50)
        txtName.Size = New Size(400, 30)
        txtName.Font = New Font("Arial", 10)
        Me.Controls.Add(txtName)

        Dim lblName As New Label()
        lblName.Text = "Name"
        lblName.Location = New Point(20, 20)
        lblName.Size = New Size(200, 20)
        lblName.Font = New Font("Arial", 10, FontStyle.Bold)
        Me.Controls.Add(lblName)

        ' Initialize and add txtEmail
        txtEmail = New TextBox()
        txtEmail.Location = New Point(20, 110)
        txtEmail.Size = New Size(400, 30)
        txtEmail.Font = New Font("Arial", 10)
        Me.Controls.Add(txtEmail)

        Dim lblEmail As New Label()
        lblEmail.Text = "Email"
        lblEmail.Location = New Point(20, 80)
        lblEmail.Size = New Size(200, 20)
        lblEmail.Font = New Font("Arial", 10, FontStyle.Bold)
        Me.Controls.Add(lblEmail)

        ' Initialize and add txtPhone
        txtPhone = New TextBox()
        txtPhone.Location = New Point(20, 170)
        txtPhone.Size = New Size(400, 30)
        txtPhone.Font = New Font("Arial", 10)
        Me.Controls.Add(txtPhone)

        Dim lblPhone As New Label()
        lblPhone.Text = "Phone Number"
        lblPhone.Location = New Point(20, 140)
        lblPhone.Size = New Size(200, 20)
        lblPhone.Font = New Font("Arial", 10, FontStyle.Bold)
        Me.Controls.Add(lblPhone)

        ' Initialize and add txtGithubLink
        txtGithubLink = New TextBox()
        txtGithubLink.Location = New Point(20, 230)
        txtGithubLink.Size = New Size(400, 30)
        txtGithubLink.Font = New Font("Arial", 10)
        Me.Controls.Add(txtGithubLink)

        Dim lblGithubLink As New Label()
        lblGithubLink.Text = "Github Link For Task 2"
        lblGithubLink.Location = New Point(20, 200)
        lblGithubLink.Size = New Size(200, 20)
        lblGithubLink.Font = New Font("Arial", 10, FontStyle.Bold)
        Me.Controls.Add(lblGithubLink)

        ' Initialize and add txtStopwatchTime
        txtStopwatchTime = New TextBox()
        txtStopwatchTime.Location = New Point(20, 290)
        txtStopwatchTime.Size = New Size(400, 30)
        txtStopwatchTime.ReadOnly = True
        txtStopwatchTime.Font = New Font("Arial", 10)
        Me.Controls.Add(txtStopwatchTime)

        Dim lblStopwatchTime As New Label()
        lblStopwatchTime.Text = "Stopwatch Time"
        lblStopwatchTime.Location = New Point(20, 260)
        lblStopwatchTime.Size = New Size(200, 20)
        lblStopwatchTime.Font = New Font("Arial", 10, FontStyle.Bold)
        Me.Controls.Add(lblStopwatchTime)

        ' Initialize and add btnToggleStopwatch
        btnToggleStopwatch = New Button()
        btnToggleStopwatch.Location = New Point(240, 350)
        btnToggleStopwatch.Size = New Size(220, 50)
        btnToggleStopwatch.Text = "Toggle Stopwatch (CTRL + T)"
        btnToggleStopwatch.Font = New Font("Arial", 10, FontStyle.Bold)
        btnToggleStopwatch.BackColor = Color.LightBlue
        AddHandler btnToggleStopwatch.Click, AddressOf BtnToggleStopwatch_Click
        Me.Controls.Add(btnToggleStopwatch)

        ' Initialize and add btnSubmit
        btnSubmit = New Button()
        btnSubmit.Location = New Point(20, 350)
        btnSubmit.Size = New Size(200, 50)
        btnSubmit.Text = "Submit (CTRL + S)"
        btnSubmit.Font = New Font("Arial", 10, FontStyle.Bold)
        btnSubmit.BackColor = Color.Yellow
        AddHandler btnSubmit.Click, AddressOf BtnSubmit_Click
        Me.Controls.Add(btnSubmit)
    End Sub

    Private Sub BtnToggleStopwatch_Click(sender As Object, e As EventArgs)
        If stopwatch.IsRunning Then
            stopwatch.Stop()
            timer.Stop()
            btnToggleStopwatch.Text = "START (CTRL + T)"
        Else
            stopwatch.Start()
            timer.Start()
            btnToggleStopwatch.Text = "STOP (CTRL + T)"
        End If
    End Sub

    Private Async Sub BtnSubmit_Click(sender As Object, e As EventArgs)
        Dim submission As New Submission With {
            .Name = txtName.Text,
            .Email = txtEmail.Text,
            .Phone = txtPhone.Text,
            .GithubLink = txtGithubLink.Text,
            .StopwatchTime = txtStopwatchTime.Text
        }
        stopwatch.Stop()
        btnToggleStopwatch.Text = "Start (CTRL + T)"
        Dim jsonContent = JsonConvert.SerializeObject(submission)
        Dim content = New StringContent(jsonContent, Encoding.UTF8, "application/json")

        Try
            Dim response = Await httpClient.PostAsync("http://localhost:3000/submit", content)
            If response.IsSuccessStatusCode Then
                MessageBox.Show("Submission successful!")
                txtName.Text = ""
                txtPhone.Text = ""
                txtEmail.Text = ""
                txtGithubLink.Text = ""
                txtStopwatchTime.Clear()
                stopwatch.Reset()
            Else
                Dim errorMessage = Await response.Content.ReadAsStringAsync()
                MessageBox.Show($"Failed to submit! Server returned: {errorMessage}")
            End If
        Catch ex As Exception
            MessageBox.Show($"Failed to submit! Exception: {ex.Message}")
        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.S) Then
            BtnSubmit_Click(Nothing, Nothing)
            Return True
        ElseIf keyData = (Keys.Control Or Keys.T) Then
            BtnToggleStopwatch_Click(Nothing, Nothing)
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
End Class

Public Class Submission
    Public Property Name As String
    Public Property Email As String
    Public Property Phone As String
    Public Property GithubLink As String
    Public Property StopwatchTime As String
End Class