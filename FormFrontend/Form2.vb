Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Net.Http
Imports System.Text

Public Class Form2
    Dim ind As Integer = 0
    Dim mxInd As Integer = 1
    Private Sub Form2_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.N Then
            previous.PerformClick()
        End If
        If e.Control AndAlso e.KeyCode = Keys.P Then
            nxt.PerformClick()
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles previous.Click

    End Sub

    Private Function SetData(formData As Dictionary(Of String, String)) As Task
        nameText.Text = formData("name")
        email.Text = formData("email")
        phone.Text = formData("phone")
        github_link.Text = formData("github_link")
        stopwatch_time.Text = formData("stopwatch_time")
        Dim tmp As Integer = formData("mxInd")
        mxInd = tmp
    End Function

    Private Async Function FetchData() As Task
        Using client As New HttpClient()
            Try
                Dim apiUrl As String = "http://localhost:3000/read?index=" + ind.ToString()

                Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)

                If response.IsSuccessStatusCode Then
                    Dim jsonResponse As String = Await response.Content.ReadAsStringAsync()
                    Dim jsonObject As JObject = JObject.Parse(jsonResponse)

                    Dim dataObject As JObject = jsonObject("data")

                    Dim dataDict As Dictionary(Of String, String) = dataObject.ToObject(Of Dictionary(Of String, String))()
                    SetData(dataDict)
                Else
                    MessageBox.Show("Error fetching data: " & response.ReasonPhrase)
                End If
            Catch ex As Exception
                MessageBox.Show("Exception while fetching data: " & ex.Message)
            End Try
        End Using
    End Function

    Private Async Sub Nxt_Click(sender As Object, e As EventArgs) Handles nxt.Click
        If ind < mxInd Then
            ind += 1
        Else
            MessageBox.Show("No more data available")
        End If
        Await FetchData()
    End Sub
    Private Async Sub Previous_Click(sender As Object, e As EventArgs) Handles previous.Click
        If ind - 1 >= 0 Then
            ind -= 1
        Else
            MessageBox.Show("No more data available")
        End If
        Await FetchData()
    End Sub
End Class