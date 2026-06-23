Imports System.IO

Public Class paiement
    Dim FichierPaiment As String = "C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\Paiment.txt"
    Dim FichierMembres As String = "C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\membres.txt"
    Private Sub ChargerPaiement()
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DataGridView1.Columns(DataGridView1.Columns.Count - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DataGridView1.Rows.Clear()

        Dim fs As New FileStream(FichierPaiment, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)

        While sr.Peek > -1
            Dim c As String = sr.ReadLine()
            Dim t() As String = c.Split(";")
            If t.Length >= 5 Then
                DataGridView1.Rows.Add(t(0), t(1), t(2), t(3), t(4))
            End If
        End While

        sr.Close()
        fs.Close()
    End Sub

    Private Sub ViderChamps()
        If ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        End If

        If ComboBox2.Items.Count > 0 Then
            ComboBox2.SelectedIndex = 0
        End If

        If ComboBox3.Items.Count > 0 Then
            ComboBox3.SelectedIndex = 0
        End If

        TextBox1.Clear()
        DateTimePicker1.Value = DateTime.Now
    End Sub

    Private Sub Paiment_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.ReadOnly = True
        ChargerPaiement()

        ComboBox1.Items.Clear()
        Dim fs1 As New FileStream(FichierMembres, FileMode.Open, FileAccess.Read)
        Dim sr1 As New StreamReader(fs1)

        While sr1.Peek > -1
            Dim line As String = sr1.ReadLine()
            Dim parts() As String = line.Split(";"c)
            If parts.Length >= 2 Then
                ComboBox1.Items.Add(parts(1) & " " & parts(0))
            End If
        End While

        sr1.Close()
        fs1.Close()

        ComboBox2.Items.Clear()
        ComboBox2.Items.Add("Mensuel")
        ComboBox2.Items.Add("Trimestriel")
        ComboBox2.Items.Add("Semestriel")
        ComboBox2.Items.Add("Annuel")

        ComboBox3.Items.Clear()
        ComboBox3.Items.Add("Carte bancaire")
        ComboBox3.Items.Add("Virement")
        ComboBox3.Items.Add("Espèce")
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex <> -1 Then
            Select Case ComboBox2.Text
                Case "Mensuel"
                    TextBox1.Text = "90,00"
                Case "Trimestriel"
                    TextBox1.Text = "200,00"
                Case "Semestriel"
                    TextBox1.Text = "350,00"
                Case "Annuel"
                    TextBox1.Text = "600,00"
                Case Else
                    TextBox1.Text = ""
            End Select
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrWhiteSpace(ComboBox1.Text) Then
            MsgBox("Veuillez sélectionner un abonné.", vbExclamation, "Erreur")
            ComboBox1.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(ComboBox2.Text) Then
            MsgBox("Veuillez sélectionner un type d'abonnement.", vbExclamation, "Erreur")
            ComboBox2.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(ComboBox3.Text) Then
            MsgBox("Veuillez sélectionner un mode de paiement.", vbExclamation, "Erreur")
            ComboBox3.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MsgBox("Le montant doit être renseigné.", vbExclamation, "Erreur")
            TextBox1.Focus()
            Exit Sub
        End If

        Dim abonne As String = ComboBox1.Text.Trim()
        Dim typeAbonnement As String = ComboBox2.Text.Trim()
        Dim montant As String = TextBox1.Text.Trim()
        Dim modePaiement As String = ComboBox3.Text.Trim()
        Dim datePaiement As String = DateTimePicker1.Value.ToString("dd/MM/yyyy")


        Dim nomComplet() As String = abonne.Split(" "c)
        Dim nomFormate As String = ""
        If nomComplet.Length >= 2 Then
            nomFormate = nomComplet(1) & " " & nomComplet(0)
        Else
            nomFormate = abonne
        End If


        Dim fs As New FileStream(FichierPaiment, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim existe As Boolean = False

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 5 Then
                If t(0).Trim() = nomFormate AndAlso t(4).Trim() = datePaiement Then
                    existe = True
                    Exit While
                End If
            End If
        End While

        sr.Close()
        fs.Close()

        If existe Then
            MessageBox.Show("Ce paiement existe déjà pour cet abonné à cette date !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim fs1 As New FileStream(FichierPaiment, FileMode.Append, FileAccess.Write)
        Dim sw As New StreamWriter(fs1)
        sw.WriteLine(nomFormate & ";" & typeAbonnement & ";" & montant & ";" & modePaiement & ";" & datePaiement)
        sw.Close()
        fs1.Close()

        MessageBox.Show("Paiement ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ChargerPaiement()
        ViderChamps()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim nomAbonne As String = InputBox("Veuillez saisir le nom de l'abonné (Nom Prenom) :", "Suppression")

        If String.IsNullOrWhiteSpace(nomAbonne) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        Dim datePaiement As String = InputBox("Veuillez saisir la date du paiement (dd/MM/yyyy) :", "Suppression")

        If String.IsNullOrWhiteSpace(datePaiement) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        Dim fs As New FileStream(FichierPaiment, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim trouve As Boolean = False

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 5 AndAlso t(0).Trim() = nomAbonne.Trim() AndAlso t(4).Trim() = datePaiement.Trim() Then
                trouve = True
                Exit While
            End If
        End While

        sr.Close()
        fs.Close()

        If Not trouve Then
            MessageBox.Show("Erreur : Aucun paiement trouvé !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce paiement ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If

        Dim fs2 As New FileStream(FichierPaiment, FileMode.Open, FileAccess.Read)
        Dim sr2 As New StreamReader(fs2)
        Dim nouveaufichier As String = ""

        While sr2.Peek > -1
            s = sr2.ReadLine()
            t = s.Split(";"c)

            If Not (t.Length >= 5 AndAlso t(0).Trim() = nomAbonne.Trim() AndAlso t(4).Trim() = datePaiement.Trim()) Then
                nouveaufichier = nouveaufichier & s & vbCrLf
            End If
        End While

        sr2.Close()
        fs2.Close()

        Dim fs3 As New FileStream(FichierPaiment, FileMode.Create, FileAccess.Write)
        Dim sw As New StreamWriter(fs3)
        sw.Write(nouveaufichier)
        sw.Close()
        fs3.Close()

        MessageBox.Show("Paiement supprimé avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ChargerPaiement()
        ViderChamps()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Me.Hide()
        Form1.Show()
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
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
End Class