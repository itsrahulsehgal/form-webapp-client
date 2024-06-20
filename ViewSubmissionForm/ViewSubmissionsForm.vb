Imports Newtonsoft.Json
Imports System.Net.Http
Imports System.Text

Public Class ViewSubmissionsForm
    Private submissions As New List(Of Submission)()
    Private currentIndex As Integer = 0

    Private txtName As TextBox
    Private txtEmail As TextBox
    Private txtPhone As TextBox
    Private txtGithubLink As TextBox
    Private txtStopwatchTime As TextBox

    Private btnEditName As Button
    Private btnEditEmail As Button
    Private btnEditPhone As Button
    Private btnEditGithubLink As Button
    Private btnEditStopwatchTime As Button

    Private btnPrevious As Button
    Private btnNext As Button
    Private btnDelete As Button
    Private btnUpdate As Button

    Public Sub New()
        InitializeComponent()
        InitializeAsync()
    End Sub

    Private Async Sub InitializeAsync()
        Try
            Await LoadSubmissions()
            If submissions.Count > 0 Then
                DisplaySubmission()
            Else
                MessageBox.Show("No Submissions Available. Create New Submissions to View Submissions.")
                Me.Close()
            End If

        Catch ex As Exception
            MessageBox.Show($"Failed to initialize! Exception: {ex.Message}")
        End Try
    End Sub

    Private Sub InitializeComponent()
        Me.Text = "Rahul Sehgal, Slidely Task 2 - Slidely Form App"
        Me.Width = 600
        Me.Height = 600
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.LightGray
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False

        ' Name
        Dim lblName As New Label()
        lblName.Text = "Name"
        lblName.Location = New Point(20, 20)
        lblName.Size = New Size(200, 20)
        lblName.Font = New Font("Arial", 10, FontStyle.Bold)
        Me.Controls.Add(lblName)

        txtName = New TextBox()
        txtName.Location = New Point(20, 50)
        txtName.Size = New Size(400, 30)
        txtName.Font = New Font("Arial", 10)
        txtName.ReadOnly = True
        Me.Controls.Add(txtName)


        ' Email
        Dim lblEmail As New Label()
        lblEmail.Text = "Email"
        lblEmail.Location = New Point(20, 90)
        lblEmail.Size = New Size(200, 20)
        lblEmail.Font = New Font("Arial", 10, FontStyle.Bold)
        Me.Controls.Add(lblEmail)

        txtEmail = New TextBox()
        txtEmail.Location = New Point(20, 120)
        txtEmail.Size = New Size(400, 30)
        txtEmail.Font = New Font("Arial", 10)
        txtEmail.ReadOnly = True
        Me.Controls.Add(txtEmail)

        ' Phone
        Dim lblPhone As New Label()
        lblPhone.Text = "Phone Number"
        lblPhone.Location = New Point(20, 160)
        lblPhone.Size = New Size(200, 20)
        lblPhone.Font = New Font("Arial", 10, FontStyle.Bold)
        Me.Controls.Add(lblPhone)

        txtPhone = New TextBox()
        txtPhone.Location = New Point(20, 190)
        txtPhone.Size = New Size(400, 30)
        txtPhone.Font = New Font("Arial", 10)
        txtPhone.ReadOnly = True
        Me.Controls.Add(txtPhone)


        ' GitHub Link
        Dim lblGithubLink As New Label()
        lblGithubLink.Text = "Github Link For Task 2"
        lblGithubLink.Location = New Point(20, 230)
        lblGithubLink.Size = New Size(200, 20)
        lblGithubLink.Font = New Font("Arial", 10, FontStyle.Bold)
        Me.Controls.Add(lblGithubLink)

        txtGithubLink = New TextBox()
        txtGithubLink.Location = New Point(20, 260)
        txtGithubLink.Size = New Size(400, 30)
        txtGithubLink.Font = New Font("Arial", 10)
        txtGithubLink.ReadOnly = True
        Me.Controls.Add(txtGithubLink)

        ' Stopwatch Time
        Dim lblStopwatchTime As New Label()
        lblStopwatchTime.Text = "Stopwatch Time"
        lblStopwatchTime.Location = New Point(20, 300)
        lblStopwatchTime.Size = New Size(200, 20)
        lblStopwatchTime.Font = New Font("Arial", 10, FontStyle.Bold)
        Me.Controls.Add(lblStopwatchTime)

        txtStopwatchTime = New TextBox()
        txtStopwatchTime.Location = New Point(20, 330)
        txtStopwatchTime.Size = New Size(400, 30)
        txtStopwatchTime.Font = New Font("Arial", 10)
        txtStopwatchTime.ReadOnly = True
        Me.Controls.Add(txtStopwatchTime)


        ' Previous button
        btnPrevious = New Button()
        btnPrevious.Text = "PREVIOUS (CTRL + P)"
        btnPrevious.Size = New Size(200, 50)
        btnPrevious.Location = New Point(50, 400)
        btnPrevious.Font = New Font("Arial", 12, FontStyle.Bold)
        btnPrevious.BackColor = Color.Yellow
        btnPrevious.ForeColor = Color.Black
        AddHandler btnPrevious.Click, AddressOf BtnPrevious_Click
        Me.Controls.Add(btnPrevious)

        ' Next button
        btnNext = New Button()
        btnNext.Text = "NEXT (CTRL + N)"
        btnNext.Size = New Size(200, 50)
        btnNext.Location = New Point(300, 400)
        btnNext.Font = New Font("Arial", 12, FontStyle.Bold)
        btnNext.BackColor = Color.LightBlue
        btnNext.ForeColor = Color.Black
        AddHandler btnNext.Click, AddressOf BtnNext_Click
        Me.Controls.Add(btnNext)

    End Sub

    Private Async Function LoadSubmissions() As Task
        Try
            Using client As New HttpClient()
                Dim response As HttpResponseMessage = Await client.GetAsync("http://localhost:3000/read")
                Debug.WriteLine($"Response -> {response}")
                If response.IsSuccessStatusCode Then
                    Dim json As String = Await response.Content.ReadAsStringAsync()
                    Debug.WriteLine($"Received JSON -> {json}")
                    submissions = JsonConvert.DeserializeObject(Of List(Of Submission))(json)
                    Debug.WriteLine($"Number of submissions loaded: {submissions.Count}")
                Else
                    MessageBox.Show("Failed to load submissions.")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Failed to get Submissions! Exception: {ex.Message}")
        End Try
    End Function

    Private Sub DisplaySubmission()
        If submissions IsNot Nothing AndAlso submissions.Count > 0 Then
            Dim submission = submissions(currentIndex)
            txtName.Text = submission.Name
            txtEmail.Text = submission.Email
            txtPhone.Text = submission.Phone
            txtGithubLink.Text = submission.GithubLink
            txtStopwatchTime.Text = submission.StopwatchTime
        End If
    End Sub

    Private Sub BtnPrevious_Click(sender As Object, e As EventArgs)
        If currentIndex > 0 Then
            currentIndex -= 1
            DisplaySubmission()
        Else
            MessageBox.Show("No Previous Submissions Available.")
        End If
    End Sub

    Private Sub BtnNext_Click(sender As Object, e As EventArgs)
        If currentIndex < submissions.Count - 1 Then
            currentIndex += 1
            DisplaySubmission()
        Else
            MessageBox.Show("No Next Submissions Available.")
        End If
    End Sub


End Class