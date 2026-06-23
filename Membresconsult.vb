Imports System.IO

Public Class Membresconsult
    Private Sub Membresconsult_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DataGridView1.Columns(DataGridView1.Columns.Count - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DataGridView1.Rows.Clear()


        Dim fs As New FileStream("C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\membres.txt", FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 7 Then
                DataGridView1.Rows.Add(t(0), t(1), t(2), t(3), t(4), t(5), t(6))
            End If
        End While

        sr.Close()
        fs.Close()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        If MessageBox.Show("Quitter l'application ?", "Confirmation",
                                  MessageBoxButtons.YesNo,
                                  MessageBoxIcon.Question) = DialogResult.Yes Then

            ' Animation de fermeture (fade out)
            Dim timer As New Timer
            timer.Interval = 10
            AddHandler timer.Tick, Sub()
                                       Opacity -= 0.05
                                       If Opacity <= 0 Then
                                           timer.Stop()
                                           Application.Exit()
                                       End If
                                   End Sub
            timer.Start()
        End If
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Me.Hide()
        Form1.Show()
    End Sub

End Class