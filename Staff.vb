Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Staff
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Dim Fichier As String = "C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\personnel.txt"

    Private Sub ChargerPersonnel()
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DataGridView1.Columns(DataGridView1.Columns.Count - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DataGridView1.Rows.Clear()

        Dim fs As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)

        While sr.Peek > -1
            Dim c As String = sr.ReadLine()
            Dim t() As String = c.Split(";")
            If t.Length >= 4 Then
                DataGridView1.Rows.Add(t(0), t(1), t(2), t(3))
            End If
        End While

        sr.Close()
        fs.Close()
    End Sub

    Private Sub Staff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerPersonnel()
    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If (Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57) And Asc(e.KeyChar) <> 8 Then
            e.KeyChar = ""
        End If
        If TextBox3.Text.Length >= 8 And Asc(e.KeyChar) <> 8 Then
            e.KeyChar = ""
        End If
    End Sub

    Private Sub ViderChamps()
        TextBox1.Clear()
        TextBox2.Clear()
        ComboBox1.SelectedIndex = -1
        ComboBox1.Text = ""
        TextBox3.Clear()

        TextBox1.Focus()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MsgBox("Veuillez saisir le nom du personnel.", vbExclamation, "Erreur")
            TextBox1.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MsgBox("Veuillez saisir le prénom du personnel.", vbExclamation, "Erreur")
            TextBox2.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(ComboBox1.Text) Then
            MsgBox("Veuillez sélectionner une fonction.", vbExclamation, "Erreur")
            ComboBox1.Focus()
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(TextBox3.Text) Then
            MsgBox("Veuillez saisir le numéro de téléphone.", vbExclamation, "Erreur")
            TextBox3.Focus()
            Exit Sub
        End If
        If TextBox3.Text.Length < 8 And TextBox3.Text.Length > 8 Then
            MsgBox("Le téléphone doit contenir exactement 8 chiffres.", vbExclamation, "Erreur")
            TextBox3.Focus()
            Exit Sub
        End If
        Dim nom As String = TextBox1.Text.Trim()
        Dim prenom As String = TextBox2.Text.Trim()
        Dim fonction As String = ComboBox1.Text.Trim()
        Dim telephone As String = TextBox3.Text.Trim()

        Dim fs As New FileStream(Fichier, FileMode.OpenOrCreate, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim personnelExiste As Integer = 0
        Dim lignesExistantes As String = ""

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";")

            If t.Length >= 4 Then
                If t(0).ToLower() = nom.ToLower() And t(1).ToLower() = prenom.ToLower() Then
                    personnelExiste = 1
                    sr.Close()
                    fs.Close()
                    MessageBox.Show("Ce membre du personnel existe déjà !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                If t(3) = telephone Then
                    sr.Close()
                    fs.Close()
                    MessageBox.Show("Erreur : Téléphone existe déjà pour un autre membre du personnel !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    TextBox3.Focus()
                    Exit Sub
                End If

                lignesExistantes = lignesExistantes & s & vbCrLf
            End If
        End While

        sr.Close()
        fs.Close()


        Dim fs1 As New FileStream(Fichier, FileMode.Append, FileAccess.Write)
        Dim sw As New StreamWriter(fs1)
        sw.WriteLine(nom & ";" & prenom & ";" & fonction & ";" & telephone & ";0")
        sw.Close()
        fs1.Close()

        MessageBox.Show("Personnel ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ChargerPersonnel()
        ViderChamps()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim nom As String = InputBox("Veuillez saisir le nom du personnel :", "Modification")

        If String.IsNullOrWhiteSpace(nom) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        Dim prenom As String = InputBox("Veuillez saisir le prénom du personnel :", "Modification")

        If String.IsNullOrWhiteSpace(prenom) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        Dim fs As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim trouve As Boolean = 0
        Dim lignesModifiees As String = ""

        While sr.Peek > -1
            Dim ligne As String = sr.ReadLine()
            Dim t() As String = ligne.Split(";"c)

            If t.Length >= 4 AndAlso t(0).ToLower() = nom.ToLower() And t(1).ToLower() = prenom.ToLower() Then
                trouve = 1
                TextBox1.Text = t(0)
                TextBox2.Text = t(1)
                ComboBox1.Text = t(2)
                TextBox3.Text = t(3)
            End If
        End While

        sr.Close()
        fs.Close()

        If trouve = 0 Then
            MessageBox.Show("Personnel introuvable !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Modifiez les informations puis cliquez sur OK", "Modification", MessageBoxButtons.OKCancel) = DialogResult.Cancel Then
            ViderChamps()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox1.Text) Or String.IsNullOrWhiteSpace(TextBox2.Text) Or
       String.IsNullOrWhiteSpace(ComboBox1.Text) Or TextBox3.Text.Length < 8 Then
            MsgBox("Veuillez remplir tous les champs correctement.", vbExclamation)
            Exit Sub
        End If

        Dim fs2 As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr2 As New StreamReader(fs2)
        lignesModifiees = ""

        While sr2.Peek > -1
            Dim ligne As String = sr2.ReadLine()
            Dim t() As String = ligne.Split(";"c)

            If t.Length >= 4 AndAlso t(0).ToLower() = nom.ToLower() And t(1).ToLower() = prenom.ToLower() Then
                lignesModifiees &= TextBox1.Text & ";" & TextBox2.Text & ";" & ComboBox1.Text & ";" & TextBox3.Text & ";0" & vbCrLf
            Else
                lignesModifiees &= ligne & vbCrLf
            End If
        End While

        sr2.Close()
        fs2.Close()

        Dim sw As New StreamWriter(Fichier)
        sw.Write(lignesModifiees)
        sw.Close()

        MessageBox.Show("Modifié avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ChargerPersonnel()
        ViderChamps()
    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim nom As String = InputBox("Veuillez saisir le nom du personnel :", "Suppression")

        If String.IsNullOrWhiteSpace(nom) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        Dim prenom As String = InputBox("Veuillez saisir le prénom du personnel :", "Suppression")

        If String.IsNullOrWhiteSpace(prenom) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        Dim fs As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim trouve As Integer = 0
        Dim lignesRestantes As String = ""

        While sr.Peek > -1
            Dim ligne As String = sr.ReadLine()
            Dim t() As String = ligne.Split(";"c)

            If t.Length >= 4 AndAlso t(0).ToLower() = nom.ToLower() And t(1).ToLower() = prenom.ToLower() Then
                trouve = 1
                If MessageBox.Show("Supprimer " & t(0) & " " & t(1) & " ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No Then
                    sr.Close()
                    fs.Close()
                    Exit Sub
                End If
            Else
                lignesRestantes &= ligne & vbCrLf
            End If
        End While

        sr.Close()
        fs.Close()

        If trouve = 0 Then
            MessageBox.Show("Personnel introuvable !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim sw As New StreamWriter(Fichier)
        sw.Write(lignesRestantes)
        sw.Close()

        MessageBox.Show("Supprimé avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ChargerPersonnel()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub TextBox3_LostFocus(sender As Object, e As EventArgs) Handles TextBox3.LostFocus
        If TextBox3.Text.Length > 0 And TextBox3.Text.Length < 8 Then
            MessageBox.Show("Numéro du téléphone doit obligatoirement être composé de 8 chiffres", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not ((Asc(e.KeyChar) >= 65 And Asc(e.KeyChar) <= 90) Or (Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122) Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 32) Then
            e.KeyChar = ""
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not ((Asc(e.KeyChar) >= 65 And Asc(e.KeyChar) <= 90) Or (Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122) Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 32) Then
            e.KeyChar = ""
        End If
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Me.Hide()
        Form1.Show()
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        If MessageBox.Show("Quitter l'application ?", "Confirmation",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question) = DialogResult.Yes Then

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