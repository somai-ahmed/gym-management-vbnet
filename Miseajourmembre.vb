Imports System.IO

Public Class Miseajourmembre
    Dim Fichier As String = "C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\membres.txt"

    Private Sub ChargerMembres()
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DataGridView1.Columns(DataGridView1.Columns.Count - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DataGridView1.Rows.Clear()

        Dim fs As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)

        While sr.Peek > -1
            Dim c As String = sr.ReadLine()
            Dim t() As String = c.Split(";")
            If t.Length >= 7 Then
                DataGridView1.Rows.Add(t(0), t(1), t(2), t(3), t(4), t(5), t(6))
            End If
        End While

        sr.Close()
        fs.Close()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Me.Hide()
        Form1.Show()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
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

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargerMembres()
        ComboBox1.Items.Clear()

        Dim fs1 As New FileStream("C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\activites.txt", FileMode.Open, FileAccess.Read)
        Dim sr1 As New StreamReader(fs1)
        Dim line As String
        Dim parts() As String

        While sr1.Peek > -1
            line = sr1.ReadLine()
            parts = line.Split(";")
            If parts.Length > 1 AndAlso Not ComboBox1.Items.Contains(parts(1)) Then
                ComboBox1.Items.Add(parts(1))
            End If
        End While

        sr1.Close()
        fs1.Close()
    End Sub

    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        If (Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57) And Asc(e.KeyChar) <> 8 Then
            e.KeyChar = ""
        End If
        If TextBox5.Text.Length >= 8 And Asc(e.KeyChar) <> 8 Then
            e.KeyChar = ""
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MsgBox("Veuillez saisir le nom du membre.", vbExclamation, "Erreur")
            TextBox2.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox3.Text) Then
            MsgBox("Veuillez saisir le prénom du membre.", vbExclamation, "Erreur")
            TextBox3.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox5.Text) Then
            MsgBox("Veuillez saisir le numéro de CIN.", vbExclamation, "Erreur")
            TextBox5.Focus()
            Exit Sub
        End If

        If TextBox5.Text.Length < 8 Then
            MsgBox("Le CIN doit contenir 8 chiffres.", vbExclamation, "Erreur")
            TextBox5.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox4.Text) Then
            MsgBox("Veuillez saisir l'adresse.", vbExclamation, "Erreur")
            TextBox4.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox6.Text) Then
            MsgBox("Veuillez saisir le numéro de téléphone.", vbExclamation, "Erreur")
            TextBox6.Focus()
            Exit Sub
        End If

        If TextBox6.Text.Length < 8 Then
            MsgBox("Le téléphone doit contenir 8 chiffres.", vbExclamation, "Erreur")
            TextBox6.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(ComboBox1.Text) Then
            MsgBox("Veuillez sélectionner un abonnement.", vbExclamation, "Erreur")
            ComboBox1.Focus()
            Exit Sub
        End If

        Dim nom As String = TextBox2.Text.Trim()
        Dim prenom As String = TextBox3.Text.Trim()
        Dim dateNaissance As String = DateTimePicker1.Value.ToString("dd/MM/yyyy")
        Dim cin As String = TextBox5.Text.Trim()
        Dim adresse As String = TextBox4.Text.Trim()
        Dim telephone As String = TextBox6.Text.Trim()

        Dim fs As New FileStream(Fichier, FileMode.OpenOrCreate, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim membreExiste As Boolean = False
        Dim ancienHistorique As String = ""
        Dim lignesExistantes As String = ""

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 7 Then
                If t(0).ToLower() = nom.ToLower() And t(1).ToLower() = prenom.ToLower() And t(2) = dateNaissance Then
                    membreExiste = True
                    ancienHistorique = t(6)
                    If MessageBox.Show("Ce membre existe déjà. Voulez-vous ajouter un nouvel abonnement ?", "Membre existant", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        Dim nouvelHistorique As String = ancienHistorique & " | " & ComboBox1.Text
                        lignesExistantes = lignesExistantes & t(0) & ";" & t(1) & ";" & t(2) & ";" & t(3) & ";" & t(4) & ";" & t(5) & ";" & nouvelHistorique & vbCrLf
                    Else
                        sr.Close()
                        fs.Close()
                        Exit Sub
                    End If
                Else
                    If t(3) = cin Then
                        sr.Close()
                        fs.Close()
                        MessageBox.Show("Erreur : CIN existe déjà pour un autre membre !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        TextBox5.Focus()
                        Exit Sub
                    End If

                    If t(5) = telephone Then
                        sr.Close()
                        fs.Close()
                        MessageBox.Show("Erreur : Téléphone existe déjà pour un autre membre !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        TextBox6.Focus()
                        Exit Sub
                    End If

                    lignesExistantes = lignesExistantes & s & vbCrLf
                End If
            End If
        End While

        sr.Close()
        fs.Close()

        If membreExiste Then
            Dim fs1 As New FileStream(Fichier, FileMode.Create, FileAccess.Write)
            Dim sw As New StreamWriter(fs1)
            sw.Write(lignesExistantes)
            sw.Close()
            fs1.Close()
            MessageBox.Show("Nouvel abonnement ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim historique As String = ComboBox1.Text
            Dim fs1 As New FileStream(Fichier, FileMode.Append, FileAccess.Write)
            Dim sw As New StreamWriter(fs1)
            sw.WriteLine(nom & ";" & prenom & ";" & dateNaissance & ";" & cin & ";" & adresse & ";" & telephone & ";" & historique)
            sw.Close()
            fs1.Close()
            MessageBox.Show("Membre ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        ChargerMembres()
        ViderChamps()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim cin As String = InputBox("Veuillez saisir le CIN du membre :", "Suppression")

        If String.IsNullOrWhiteSpace(cin) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        If cin.Length <> 8 Then
            MsgBox("Le CIN doit contenir exactement 8 chiffres.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim fs As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim trouve As Boolean = False
        Dim historique As String = ""
        Dim membreInfo() As String = Nothing

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 7 AndAlso t(3) = cin Then
                trouve = True
                membreInfo = t
                historique = t(6)
                Exit While
            End If
        End While

        sr.Close()
        fs.Close()

        If Not trouve Then
            MessageBox.Show("Erreur : Aucun membre trouvé avec ce CIN !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim abonnements() As String = historique.Split("|")

        If abonnements.Length > 1 Then
            Dim listeAbonnements As String = "Abonnements disponibles :" & vbCrLf & vbCrLf
            For i As Integer = 0 To abonnements.Length - 1
                listeAbonnements &= (i + 1).ToString() & ". " & abonnements(i).Trim() & vbCrLf
            Next
            listeAbonnements &= vbCrLf & "0. Supprimer tout le membre"

            Dim choix As String = InputBox(listeAbonnements & vbCrLf & vbCrLf & "Entrez le numéro de l'abonnement à supprimer :", "Choix de suppression")

            If String.IsNullOrWhiteSpace(choix) Then
                MsgBox("Opération annulée.", vbInformation, "Annulation")
                Exit Sub
            End If

            Dim numChoix As Integer
            If Not Integer.TryParse(choix, numChoix) OrElse numChoix < 0 OrElse numChoix > abonnements.Length Then
                MsgBox("Choix invalide.", vbExclamation, "Erreur")
                Exit Sub
            End If

            If numChoix = 0 Then
                If MessageBox.Show("Êtes-vous sûr de vouloir supprimer complètement ce membre ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    Exit Sub
                End If

                Dim fs2 As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
                Dim sr2 As New StreamReader(fs2)
                Dim nouveaufichier As String = ""

                While sr2.Peek > -1
                    s = sr2.ReadLine()
                    t = s.Split(";"c)

                    If Not (t.Length >= 7 AndAlso t(3) = cin) Then
                        nouveaufichier = nouveaufichier & s & vbCrLf
                    End If
                End While

                sr2.Close()
                fs2.Close()

                Dim fs3 As New FileStream(Fichier, FileMode.Create, FileAccess.Write)
                Dim sw As New StreamWriter(fs3)
                sw.Write(nouveaufichier)
                sw.Close()
                fs3.Close()

                MessageBox.Show("Membre supprimé avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim nouvelHistorique As String = ""
                For i As Integer = 0 To abonnements.Length - 1
                    If i <> (numChoix - 1) Then
                        If nouvelHistorique <> "" Then
                            nouvelHistorique &= " | "
                        End If
                        nouvelHistorique &= abonnements(i).Trim()
                    End If
                Next

                If MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet abonnement ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                    Exit Sub
                End If

                Dim fs2 As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
                Dim sr2 As New StreamReader(fs2)
                Dim lignes As String = ""

                While sr2.Peek > -1
                    s = sr2.ReadLine()
                    t = s.Split(";"c)

                    If t.Length >= 7 AndAlso t(3) = cin Then
                        lignes = lignes & t(0) & ";" & t(1) & ";" & t(2) & ";" & t(3) & ";" & t(4) & ";" & t(5) & ";" & nouvelHistorique & vbCrLf
                    Else
                        lignes = lignes & s & vbCrLf
                    End If
                End While

                sr2.Close()
                fs2.Close()

                Dim fs3 As New FileStream(Fichier, FileMode.Create, FileAccess.Write)
                Dim sw As New StreamWriter(fs3)
                sw.Write(lignes)
                sw.Close()
                fs3.Close()

                MessageBox.Show("Abonnement supprimé avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            If MessageBox.Show("Ce membre n'a qu'un seul abonnement. Voulez-vous supprimer complètement ce membre ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Exit Sub
            End If

            Dim fs2 As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
            Dim sr2 As New StreamReader(fs2)
            Dim nouveaufichier As String = ""

            While sr2.Peek > -1
                s = sr2.ReadLine()
                t = s.Split(";"c)

                If Not (t.Length >= 7 AndAlso t(3) = cin) Then
                    nouveaufichier = nouveaufichier & s & vbCrLf
                End If
            End While

            sr2.Close()
            fs2.Close()

            Dim fs3 As New FileStream(Fichier, FileMode.Create, FileAccess.Write)
            Dim sw As New StreamWriter(fs3)
            sw.Write(nouveaufichier)
            sw.Close()
            fs3.Close()

            MessageBox.Show("Membre supprimé avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        ChargerMembres()
        ViderChamps()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim cin As String = InputBox("Veuillez saisir le CIN du membre à modifier :", "Modification membre")

        If String.IsNullOrWhiteSpace(cin) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        If cin.Length <> 8 Then
            MsgBox("Le CIN doit contenir exactement 8 chiffres.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim fs As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim trouve As Integer = 0
        Dim anciennesValeurs() As String = Nothing
        Dim historique As String = ""

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 7 AndAlso t(3) = cin Then
                trouve = 1
                anciennesValeurs = t
                historique = t(6)
                Exit While
            End If
        End While

        sr.Close()
        fs.Close()

        If trouve = 0 Then
            MessageBox.Show("Erreur : Aucun membre trouvé avec ce CIN !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim abonnements() As String = historique.Split("|")

        If abonnements.Length > 1 Then
            Dim listeAbonnements As String = "Abonnements disponibles :" & vbCrLf & vbCrLf
            For i As Integer = 0 To abonnements.Length - 1
                listeAbonnements &= (i + 1).ToString() & ". " & abonnements(i).Trim() & vbCrLf
            Next

            Dim choix As String = InputBox(listeAbonnements & vbCrLf & vbCrLf & "Entrez le numéro de l'abonnement à modifier (ou 0 pour modifier les infos personnelles) :", "Choix de modification")

            If String.IsNullOrWhiteSpace(choix) Then
                MsgBox("Opération annulée.", vbInformation, "Annulation")
                Exit Sub
            End If

            Dim numChoix As Integer
            If Not Integer.TryParse(choix, numChoix) OrElse numChoix < 0 OrElse numChoix > abonnements.Length Then
                MsgBox("Choix invalide.", vbExclamation, "Erreur")
                Exit Sub
            End If

            If numChoix = 0 Then
                ModifierInfosPersonnelles(cin, anciennesValeurs)
            Else
                Dim nouvelAbonnement As String = InputBox("Nouvel abonnement :", "Modification", "")
                If String.IsNullOrWhiteSpace(nouvelAbonnement) Then
                    MsgBox("L'abonnement ne peut pas être vide.", vbExclamation, "Erreur")
                    Exit Sub
                End If

                abonnements(numChoix - 1) = nouvelAbonnement

                Dim nouvelHistorique As String = String.Join(" | ", abonnements)

                Dim fs2 As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
                Dim sr2 As New StreamReader(fs2)
                Dim lignes As String = ""

                While sr2.Peek > -1
                    s = sr2.ReadLine()
                    t = s.Split(";"c)

                    If t.Length >= 7 AndAlso t(3) = cin Then
                        lignes = lignes & t(0) & ";" & t(1) & ";" & t(2) & ";" & t(3) & ";" & t(4) & ";" & t(5) & ";" & nouvelHistorique & vbCrLf
                    Else
                        lignes = lignes & s & vbCrLf
                    End If
                End While

                sr2.Close()
                fs2.Close()

                Dim fs3 As New FileStream(Fichier, FileMode.Create, FileAccess.Write)
                Dim sw As New StreamWriter(fs3)
                sw.Write(lignes)
                sw.Close()
                fs3.Close()

                MessageBox.Show("Abonnement modifié avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            Dim choix As Integer = MessageBox.Show("Voulez-vous modifier l'abonnement ?" & vbCrLf & vbCrLf & "Oui = Modifier l'abonnement" & vbCrLf & "Non = Modifier les infos personnelles", "Choix", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

            If choix = DialogResult.Cancel Then
                Exit Sub
            ElseIf choix = DialogResult.Yes Then
                Dim nouvelAbonnement As String = InputBox("Nouvel abonnement :", "Modification", "")
                If String.IsNullOrWhiteSpace(nouvelAbonnement) Then
                    MsgBox("L'abonnement ne peut pas être vide.", vbExclamation, "Erreur")
                    Exit Sub
                End If

                Dim nouvelHistorique As String = nouvelAbonnement

                Dim fs2 As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
                Dim sr2 As New StreamReader(fs2)
                Dim lignes As String = ""

                While sr2.Peek > -1
                    s = sr2.ReadLine()
                    t = s.Split(";"c)

                    If t.Length >= 7 AndAlso t(3) = cin Then
                        lignes = lignes & t(0) & ";" & t(1) & ";" & t(2) & ";" & t(3) & ";" & t(4) & ";" & t(5) & ";" & nouvelHistorique & vbCrLf
                    Else
                        lignes = lignes & s & vbCrLf
                    End If
                End While

                sr2.Close()
                fs2.Close()

                Dim fs3 As New FileStream(Fichier, FileMode.Create, FileAccess.Write)
                Dim sw As New StreamWriter(fs3)
                sw.Write(lignes)
                sw.Close()
                fs3.Close()

                MessageBox.Show("Abonnement modifié avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ModifierInfosPersonnelles(cin, anciennesValeurs)
            End If
        End If

        ChargerMembres()
        ViderChamps()
    End Sub

    Private Sub ModifierInfosPersonnelles(cin As String, anciennesValeurs() As String)
        Dim nouveauNom As String = InputBox("Nouveau nom :", "Modification", anciennesValeurs(0))
        If String.IsNullOrWhiteSpace(nouveauNom) Then
            MsgBox("Le nom ne peut pas être vide.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim nouveauPrenom As String = InputBox("Nouveau prénom :", "Modification", anciennesValeurs(1))
        If String.IsNullOrWhiteSpace(nouveauPrenom) Then
            MsgBox("Le prénom ne peut pas être vide.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim nouvelleDateNaissance As String = InputBox("Nouvelle date de naissance (dd/MM/yyyy) :", "Modification", anciennesValeurs(2))
        If String.IsNullOrWhiteSpace(nouvelleDateNaissance) Then
            MsgBox("La date de naissance ne peut pas être vide.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim nouveauCIN As String = InputBox("Nouveau CIN :", "Modification", anciennesValeurs(3))
        If String.IsNullOrWhiteSpace(nouveauCIN) Then
            MsgBox("Le CIN ne peut pas être vide.", vbExclamation, "Erreur")
            Exit Sub
        End If
        If nouveauCIN.Length <> 8 Then
            MsgBox("Le CIN doit contenir exactement 8 chiffres.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim nouvelleAdresse As String = InputBox("Nouvelle adresse :", "Modification", anciennesValeurs(4))
        If String.IsNullOrWhiteSpace(nouvelleAdresse) Then
            MsgBox("L'adresse ne peut pas être vide.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim nouveauTelephone As String = InputBox("Nouveau téléphone :", "Modification", anciennesValeurs(5))
        If String.IsNullOrWhiteSpace(nouveauTelephone) Then
            MsgBox("Le téléphone ne peut pas être vide.", vbExclamation, "Erreur")
            Exit Sub
        End If
        If nouveauTelephone.Length <> 8 Then
            MsgBox("Le téléphone doit contenir exactement 8 chiffres.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim fs2 As New FileStream(Fichier, FileMode.Open, FileAccess.Read)
        Dim sr2 As New StreamReader(fs2)
        Dim lignes As String = ""
        Dim s As String
        Dim t() As String

        While sr2.Peek > -1
            s = sr2.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 7 AndAlso t(3) = cin Then
                Dim ancienHistorique As String = t(6)
                lignes = lignes & nouveauNom & ";" & nouveauPrenom & ";" & nouvelleDateNaissance & ";" & nouveauCIN & ";" & nouvelleAdresse & ";" & nouveauTelephone & ";" & ancienHistorique & vbCrLf
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

        MessageBox.Show("Informations personnelles modifiées avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub ViderChamps()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        DateTimePicker1.Value = Date.Today
        If ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not ((Asc(e.KeyChar) >= 65 And Asc(e.KeyChar) <= 90) Or (Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122) Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 32) Then
            e.KeyChar = ""
        End If
    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not ((Asc(e.KeyChar) >= 65 And Asc(e.KeyChar) <= 90) Or (Asc(e.KeyChar) >= 97 And Asc(e.KeyChar) <= 122) Or Asc(e.KeyChar) = 8 Or Asc(e.KeyChar) = 32) Then
            e.KeyChar = ""
        End If
    End Sub

    Private Sub TextBox5_LostFocus(sender As Object, e As EventArgs) Handles TextBox5.LostFocus
        If TextBox5.Text.Length > 0 And TextBox5.Text.Length < 8 Then
            MessageBox.Show("CIN doit obligatoirement être composé de 8 chiffres", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub TextBox6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox6.KeyPress
        If (Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57) And Asc(e.KeyChar) <> 8 Then
            e.KeyChar = ""
        End If
        If TextBox6.Text.Length >= 8 And Asc(e.KeyChar) <> 8 Then
            e.KeyChar = ""
        End If
    End Sub

    Private Sub TextBox6_LostFocus(sender As Object, e As EventArgs) Handles TextBox6.LostFocus
        If TextBox6.Text.Length > 0 And TextBox6.Text.Length < 8 Then
            MessageBox.Show("Numéro du téléphone doit obligatoirement être composé de 8 chiffres", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

End Class