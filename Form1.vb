Imports System.IO

Public Class Form1
    Dim fichierPaiment As String = "C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\Paiment.txt"
    Dim revenuTotal As Decimal = 0

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If MessageBox.Show("Quitter l'application ?", "Confirmation",
                          MessageBoxButtons.YesNo,
                          MessageBoxIcon.Question) = DialogResult.Yes Then

            ' Animation de fermeture (fade out)
            Dim timer As New Timer()
            timer.Interval = 10
            AddHandler timer.Tick, Sub()
                                       Me.Opacity -= 0.05
                                       If Me.Opacity <= 0 Then
                                           timer.Stop()
                                           Application.Exit()
                                       End If
                                   End Sub
            timer.Start()
        End If
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub ConsulterToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ConsulterToolStripMenuItem1.Click
        Me.Hide()
        Membresconsult.Show()
    End Sub

    Private Sub MiseAJourToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles MiseAJourToolStripMenuItem1.Click
        Me.Hide()
        Miseajourmembre.Show()
    End Sub

    Private Sub CalculerRevenuMensuel()
        revenuTotal = 0
        Dim moisActuel As Integer = DateTime.Now.Month
        Dim anneeActuelle As Integer = DateTime.Now.Year

        Dim fs As New FileStream(fichierPaiment, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)

        While sr.Peek > -1
            Dim ligne As String = sr.ReadLine()
            Dim parts() As String = ligne.Split(";"c)

            If parts.Length >= 5 Then
                Dim montantStr As String = parts(2).Replace(",", ".")
                Dim montant As Decimal

                If Decimal.TryParse(montantStr, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, montant) Then
                    Dim datePaiement As String = parts(4)
                    Dim dateparts() As String = datePaiement.Split("/"c)

                    If dateparts.Length = 3 Then
                        Dim jour As Integer
                        Dim mois As Integer
                        Dim annee As Integer

                        If Integer.TryParse(dateparts(0), jour) AndAlso Integer.TryParse(dateparts(1), mois) AndAlso Integer.TryParse(dateparts(2), annee) Then
                            If mois = moisActuel AndAlso annee = anneeActuelle Then
                                revenuTotal += montant
                            End If
                        End If
                    End If
                End If
            End If
        End While

        sr.Close()
        fs.Close()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label3.Visible = True
        Label3.Text = System.IO.File.ReadAllLines("C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\membres.txt").Length.ToString()

        CalculerRevenuMensuel()

        Label4.Text = revenuTotal.ToString("N2") & " DT"
        Label4.Visible = True
    End Sub

    Private Sub ActivitésToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActivitésToolStripMenuItem.Click
        Me.Hide()
        Activities.Show()
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub PlanningsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PlanningsToolStripMenuItem.Click
        Me.Hide()
        Planning.Show()
    End Sub

    Private Sub StaffToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StaffToolStripMenuItem.Click
        Me.Hide()
        Staff.Show()
    End Sub

    Private Sub PaimentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PaimentsToolStripMenuItem.Click
        Me.Hide()
        paiement.Show()
    End Sub

End Class