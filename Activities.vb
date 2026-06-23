Imports System.IO

Public Class Activities
    Dim Fichier As String = "C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\activites.txt"

    Private Sub ChargerActivités()
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DataGridView1.Columns(DataGridView1.Columns.Count - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DataGridView1.Rows.Clear()
        Dim fs As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)
            If t.Length >= 4 Then
                DataGridView1.Rows.Add(t(0), t(1), t(2), t(3))
            End If
        End While

        sr.Close()
        fs.Close()
    End Sub

    Private Sub Activities_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerActivités()
        ComboBox1.Items.Clear()

        Dim fs1 As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr1 As New StreamReader(fs1)
        Dim line As String
        Dim parts() As String

        While sr1.Peek > -1
            line = sr1.ReadLine()
            parts = line.Split(";"c)
            If parts.Length > 2 AndAlso Not ComboBox1.Items.Contains(parts(2)) Then
                ComboBox1.Items.Add(parts(2))
            End If
        End While

        sr1.Close()
        fs1.Close()
    End Sub

    Private Sub TextBox1_LostFocus(sender As Object, e As EventArgs) Handles TextBox1.LostFocus
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MessageBox.Show("L'ID ne peut pas être vide !", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox1.Focus()
            Exit Sub
        End If

        If Not TextBox1.Text.StartsWith("ACT") Then
            MessageBox.Show("L'ID doit commencer par 'ACT' !", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox1.Focus()
            Exit Sub
        End If

        If TextBox1.Text.Length < 4 Then
            MessageBox.Show("L'ID doit être sous la forme ACT suivi de chiffres (exemple ACT1234) !", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox1.Focus()
            Exit Sub
        End If

        Dim partieNumerique As String = TextBox1.Text.Substring(3)
        If Not IsNumeric(partieNumerique) Then
            MessageBox.Show("L'ID doit être sous la forme ACT suivi de chiffres (exemple ACT1234) !", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox1.Focus()
            Exit Sub
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not ((Asc(e.KeyChar) >= 65 And Asc(e.KeyChar) <= 90) Or (Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122) Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 32) Then
            e.KeyChar = ""
        End If
    End Sub

    Private Sub TextBox2_LostFocus(sender As Object, e As EventArgs) Handles TextBox2.LostFocus
        If String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MessageBox.Show("donne le nom de l'activité !", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox2.Focus()
        End If
    End Sub

    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        If (Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57) And Asc(e.KeyChar) <> 8 Then
            e.KeyChar = ""
        End If
    End Sub

    Private Sub TextBox4_LostFocus(sender As Object, e As EventArgs) Handles TextBox4.LostFocus
        If String.IsNullOrWhiteSpace(TextBox4.Text) Then
            TextBox4.Focus()
            Exit Sub
        End If
        If CInt(TextBox4.Text) > 180 Then
            MessageBox.Show("La durée maximale de la séance ne doit pas dépasser 3 heures !", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TextBox4.Focus()
        End If
    End Sub

    'bouton ajouter
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MsgBox("Veuillez saisir l'ID de l'activité.", vbExclamation, "Erreur")
            TextBox1.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MsgBox("Veuillez saisir le nom de l'activité.", vbExclamation, "Erreur")
            TextBox2.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(ComboBox1.Text) Then
            MsgBox("Veuillez sélectionner le type d'activité.", vbExclamation, "Erreur")
            ComboBox1.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox4.Text) Then
            MsgBox("Veuillez saisir la durée.", vbExclamation, "Erreur")
            TextBox4.Focus()
            Exit Sub
        End If

        Dim idActivite As String = TextBox1.Text.Trim()
        Dim nomActivite As String = TextBox2.Text.Trim()
        Dim typeActivite As String = ComboBox1.Text.Trim()
        Dim duree As String = TextBox4.Text.Trim()

        Dim fs As New FileStream(Fichier, FileMode.OpenOrCreate, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 4 Then
                If t(0) = idActivite Then
                    sr.Close()
                    fs.Close()
                    MessageBox.Show("Erreur : Cet ID d'activité existe déjà !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    TextBox1.Focus()
                    Exit Sub
                End If

                If t(1).ToLower() = nomActivite.ToLower() Then
                    sr.Close()
                    fs.Close()
                    MessageBox.Show("Erreur : Cette activité existe déjà !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    TextBox2.Focus()
                    Exit Sub
                End If
            End If
        End While

        sr.Close()
        fs.Close()

        Dim fs1 As New FileStream(Fichier, FileMode.Append, FileAccess.Write)
        Dim sw As New StreamWriter(fs1)
        sw.WriteLine(idActivite & ";" & nomActivite & ";" & typeActivite & ";" & duree)
        sw.Close()
        fs1.Close()

        MessageBox.Show("Activité ajoutée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ChargerActivités()
        ViderChamps()
    End Sub

    'bouton supprimer
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim idActivite As String = InputBox("Veuillez saisir l'ID de l'activité à supprimer :", "Suppression activité")

        If String.IsNullOrWhiteSpace(idActivite) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        If Not idActivite.StartsWith("ACT") Then
            MsgBox("L'ID doit commencer par ACT.", vbExclamation, "Erreur")
            Exit Sub
        End If

        If MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette activité ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If

        Dim fs As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim trouve As Integer = 0
        Dim nouveaufichier As String = ""

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 4 AndAlso t(0) = idActivite Then
                trouve = 1
            Else
                nouveaufichier = nouveaufichier & s & vbCrLf
            End If
        End While

        sr.Close()
        fs.Close()

        If trouve = 0 Then
            MessageBox.Show("Erreur : Aucune activité trouvée avec cet ID !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim fs1 As New FileStream(Fichier, FileMode.Create, FileAccess.Write)
        Dim sw As New StreamWriter(fs1)
        sw.Write(nouveaufichier)
        sw.Close()
        fs1.Close()

        MessageBox.Show("Activité supprimée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ChargerActivités()
        ViderChamps()
    End Sub

    'bouton modifier
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim idActivite As String = InputBox("Veuillez saisir l'ID de l'activité à modifier :", "Modification activité")

        If String.IsNullOrWhiteSpace(idActivite) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        If Not idActivite.StartsWith("ACT") Then
            MsgBox("L'ID doit commencer par ACT.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim fs As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim trouve As Integer = 0
        Dim anciennesValeurs() As String = Nothing

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 4 AndAlso t(0) = idActivite Then
                trouve = 1
                anciennesValeurs = t
                Exit While
            End If
        End While

        sr.Close()
        fs.Close()

        If trouve = 0 Then
            MessageBox.Show("Erreur : Aucune activité trouvée avec cet ID !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim nouvelID As String = InputBox("Nouvel ID :", "Modification", anciennesValeurs(0))
        If String.IsNullOrWhiteSpace(nouvelID) Then
            MsgBox("L'ID ne peut pas être vide.", vbExclamation, "Erreur")
            Exit Sub
        End If
        If Not nouvelID.StartsWith("ACT") Then
            MsgBox("L'ID doit commencer par ACT.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim nouveauNom As String = InputBox("Nouveau nom :", "Modification", anciennesValeurs(1))
        If String.IsNullOrWhiteSpace(nouveauNom) Then
            MsgBox("Le nom ne peut pas être vide.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim nouveauType As String = InputBox("Nouveau type :", "Modification", anciennesValeurs(2))
        If String.IsNullOrWhiteSpace(nouveauType) Then
            MsgBox("Le type ne peut pas être vide.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim nouvelleDuree As String = InputBox("Nouvelle durée (en minutes) :", "Modification", anciennesValeurs(3))
        If String.IsNullOrWhiteSpace(nouvelleDuree) Then
            MsgBox("La durée ne peut pas être vide.", vbExclamation, "Erreur")
            Exit Sub
        End If
        If Not IsNumeric(nouvelleDuree) Then
            MsgBox("La durée doit être un nombre.", vbExclamation, "Erreur")
            Exit Sub
        End If
        If CInt(nouvelleDuree) > 180 Then
            MsgBox("La durée ne doit pas dépasser 180 minutes.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim fs2 As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr2 As New StreamReader(fs2)
        Dim lignes As String = ""

        While sr2.Peek > -1
            s = sr2.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 4 AndAlso t(0) = idActivite Then
                lignes = lignes & nouvelID & ";" & nouveauNom & ";" & nouveauType & ";" & nouvelleDuree & vbCrLf
            Else
                lignes = lignes & s & vbCrLf
            End If
        End While

        sr2.Close()
        fs2.Close()

        Dim fs1 As New FileStream(Fichier, FileMode.Create, FileAccess.Write)
        Dim sw As New StreamWriter(fs1)
        sw.Write(lignes)
        sw.Close()
        fs1.Close()

        MessageBox.Show("Activité modifiée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ChargerActivités()
        ViderChamps()
    End Sub

    Private Sub ViderChamps()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox4.Clear()
        If ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Me.Hide()
        Form1.Show()
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        If MessageBox.Show("Quitter l'application ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
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