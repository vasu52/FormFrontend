﻿Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class Form1

    Dim mm, ss, ms As Integer
    Dim isTimerEnabled As Boolean = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.T Then
            toggleStopwatch.PerformClick()
        End If
        If e.Control AndAlso e.KeyCode = Keys.S Then
            Submit.PerformClick()
        End If
    End Sub


    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Submit.Click
        Dim a As String = nameText.Text
        Dim b As String = email.Text
        Dim c As String = phone.Text
        Dim d As String = github_link.Text
        Dim f As String = stopwatch_time.Text
        Dim formData As New Dictionary(Of String, String) From {
            {"name", a},
            {"email", b},
            {"phone", c},
            {"github_link", d},
            {"stopwatch_timer", f}
        }

        Await SendDataAsync(formData)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        stopwatch_time.Text = Format(mm, "00:") & Format(ss, "00:") & Format(ms, "00")

        ms = ms + 1
        If (ms > 99) Then
            ms = 0
            ss = ss + 1
        End If
        If (ss > 59) Then
            ss = 0
            mm = mm + 1
        End If
        If (mm > 59) Then
            mm = 0
            ss = 0
            ms = 0
            Timer1.Enabled = False
        End If
    End Sub

    Private Sub toggleStopwatch_Click(sender As Object, e As EventArgs) Handles toggleStopwatch.Click
        If isTimerEnabled.Equals(True) Then
            Timer1.Enabled = False
            isTimerEnabled = False
        Else
            Timer1.Enabled = True
            isTimerEnabled = True
        End If

    End Sub

    Private Async Function SendDataAsync(formData As Dictionary(Of String, String)) As Task
        Using client As New HttpClient()

            Dim apiUrl As String = "http://localhost:3000/submit"

            Dim jsonData As String = JsonConvert.SerializeObject(formData)
            Dim content As New StringContent(jsonData, Encoding.UTF8, "application/json")

            Dim response As HttpResponseMessage = Await client.PostAsync(apiUrl, content)

            If response.IsSuccessStatusCode Then
                MessageBox.Show("Data submitted successfully.")
            Else
                MessageBox.Show("Error submitting data: " & response.ReasonPhrase)
            End If
        End Using
    End Function
End Class
