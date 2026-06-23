Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Planning
    Dim FichierPlanning As String = "C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\planning.txt"
    Dim FichierActivites As String = "C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\activites.txt"
    Dim FichierPersonnel As String = "C:\Users\Ahmed\Desktop\1ere LNBC ISGT\Peven\C#\Projet\packs\personnel.txt"
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
    Private Sub ViderChamps()
        If ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        End If


        If ComboBox3.Items.Count > 0 Then
            ComboBox3.SelectedIndex = 0
        End If

        TextBox1.Clear()
        TextBox2.Clear()

        If ComboBox4.Items.Count > 0 Then
            ComboBox4.SelectedIndex = 0
        End If
    End Sub

    Private Sub ChargerPlanning()
        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        DataGridView1.Columns(DataGridView1.Columns.Count - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DataGridView1.Rows.Clear()

        Dim fs As New FileStream(FichierPlanning, FileMode.Open, FileAccess.Read)
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

    Private Sub planning_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox2.ReadOnly = True
        ChargerPlanning()
        ComboBox3.Items.Clear()
        Dim fs1 As New FileStream(FichierActivites, FileMode.Open, FileAccess.Read)
        Dim sr1 As New StreamReader(fs1)

        While sr1.Peek > -1
            Dim line As String = sr1.ReadLine()
            Dim parts() As String = line.Split(";"c)
            If parts.Length > 1 Then
                ComboBox3.Items.Add(parts(1))
            End If
        End While

        sr1.Close()
        fs1.Close()

        ComboBox4.Items.Clear()
        Dim fs2 As New FileStream(FichierPersonnel, FileMode.Open, FileAccess.Read)
        Dim sr2 As New StreamReader(fs2)

        While sr2.Peek > -1
            Dim line As String = sr2.ReadLine()
            Dim parts() As String = line.Split(";"c)
            If parts.Length >= 4 AndAlso parts(3) = "Coach" Then
                ComboBox4.Items.Add(parts(1) & " " & parts(2))
            End If
        End While

        sr2.Close()
        fs2.Close()
    End Sub



    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        CalculerDuree()
    End Sub

    Private Sub DateTimePicker2_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker2.ValueChanged
        CalculerDuree()
    End Sub

    Private Sub CalculerDuree()
        Dim heureDebut As DateTime = DateTimePicker1.Value
        Dim heureFin As DateTime = DateTimePicker2.Value

        If heureFin > heureDebut Then
            Dim duree As TimeSpan = heureFin - heureDebut
            TextBox1.Text = CInt(duree.TotalMinutes).ToString()
        Else
            TextBox1.Text = ""
        End If
    End Sub



    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedIndex <> -1 Then
            Dim activiteSelectionnee As String = ComboBox3.Text

            TextBox2.Clear()

            Dim fs As New FileStream(FichierActivites, FileMode.Open, FileAccess.Read)
            Dim sr As New StreamReader(fs)

            While sr.Peek > -1
                Dim line As String = sr.ReadLine()
                Dim parts() As String = line.Split(";"c)

                If parts.Length >= 4 AndAlso parts(1) = activiteSelectionnee Then
                    TextBox2.Text = parts(2)
                    Exit While
                End If
            End While

            sr.Close()
            fs.Close()
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrWhiteSpace(ComboBox1.Text) Then
            MsgBox("Veuillez sélectionner un jour.", vbExclamation, "Erreur")
            ComboBox1.Focus()
            Exit Sub
        End If

        Dim heureDebut As DateTime = DateTimePicker1.Value
        Dim heureFin As DateTime = DateTimePicker2.Value

        If heureFin <= heureDebut Then
            MsgBox("L'heure de fin doit être supérieure à l'heure de début.", vbExclamation, "Erreur")
            DateTimePicker2.Focus()
            Exit Sub
        End If

        Dim duree As TimeSpan = heureFin - heureDebut
        If duree.TotalHours > 3 Then
            MsgBox("La période ne peut pas dépasser 3 heures.", vbExclamation, "Erreur")
            DateTimePicker2.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(ComboBox3.Text) Then
            MsgBox("Veuillez sélectionner une activité.", vbExclamation, "Erreur")
            ComboBox3.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox2.Text) Then
            MsgBox("Le type doit être renseigné.", vbExclamation, "Erreur")
            TextBox2.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(TextBox1.Text) Then
            MsgBox("La durée doit être calculée.", vbExclamation, "Erreur")
            TextBox1.Focus()
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(ComboBox4.Text) Then
            MsgBox("Veuillez sélectionner un coach.", vbExclamation, "Erreur")
            ComboBox4.Focus()
            Exit Sub
        End If

        Dim jour As String = ComboBox1.Text.Trim()
        Dim dateDeb As String = heureDebut.ToString("HH:mm")
        Dim dateFin As String = heureFin.ToString("HH:mm")
        Dim activite As String = ComboBox3.Text.Trim()
        Dim type As String = TextBox2.Text.Trim()
        Dim dureeMin As String = TextBox1.Text.Trim()
        Dim coach As String = ComboBox4.Text.Trim()

        Dim fs As New FileStream(FichierPlanning, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim existe As Boolean = False

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 7 Then
                If t(0).Trim() = jour AndAlso t(1).Trim() = dateDeb AndAlso t(2).Trim() = dateFin AndAlso t(3).Trim() = activite Then
                    existe = True
                    Exit While
                End If
            End If
        End While

        sr.Close()
        fs.Close()

        If existe Then
            MessageBox.Show("Ce planning existe déjà !" & vbCrLf & "Jour: " & jour & vbCrLf & "Début: " & dateDeb & vbCrLf & "Fin: " & dateFin & vbCrLf & "Activité: " & activite, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim fs1 As New FileStream(FichierPlanning, FileMode.Append, FileAccess.Write)
        Dim sw As New StreamWriter(fs1)
        sw.WriteLine(jour & ";" & dateDeb & ";" & dateFin & ";" & activite & ";" & type & ";" & dureeMin & ";" & coach)
        sw.Close()
        fs1.Close()

        MessageBox.Show("Planning ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ChargerPlanning()
        ViderChamps()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim jour As String = InputBox("Veuillez saisir le jour du planning à modifier :", "Modification planning")

        If String.IsNullOrWhiteSpace(jour) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        Dim heureDebut As String = InputBox("Veuillez saisir l'heure de début (HH:mm) :", "Modification planning")

        If String.IsNullOrWhiteSpace(heureDebut) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        Dim fs As New FileStream(FichierPlanning, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim trouve As Boolean = False
        Dim anciennesValeurs() As String = Nothing

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 7 AndAlso t(0).Trim() = jour.Trim() AndAlso t(1).Trim() = heureDebut.Trim() Then
                trouve = True
                anciennesValeurs = t
                Exit While
            End If
        End While

        sr.Close()
        fs.Close()

        If Not trouve Then
            MessageBox.Show("Erreur : Aucun planning trouvé !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim nouveauJour As String = InputBox("Nouveau jour :", "Modification", anciennesValeurs(0))
        Dim nouvelleHeureDeb As String = InputBox("Nouvelle heure de début (HH:mm) :", "Modification", anciennesValeurs(1))
        Dim nouvelleHeureFin As String = InputBox("Nouvelle heure de fin (HH:mm) :", "Modification", anciennesValeurs(2))
        Dim nouvelleActivite As String = InputBox("Nouvelle activité :", "Modification", anciennesValeurs(3))
        Dim nouveauType As String = InputBox("Nouveau type :", "Modification", anciennesValeurs(4))
        Dim nouvelleDuree As String = InputBox("Nouvelle durée :", "Modification", anciennesValeurs(5))
        Dim nouveauCoach As String = InputBox("Nouveau coach :", "Modification", anciennesValeurs(6))

        If String.IsNullOrWhiteSpace(nouveauJour) Or String.IsNullOrWhiteSpace(nouvelleHeureDeb) Or String.IsNullOrWhiteSpace(nouvelleHeureFin) Then
            MsgBox("Les champs ne peuvent pas être vides.", vbExclamation, "Erreur")
            Exit Sub
        End If

        Dim fs2 As New FileStream(FichierPlanning, FileMode.Open, FileAccess.Read)
        Dim sr2 As New StreamReader(fs2)
        Dim lignes As String = ""

        While sr2.Peek > -1
            s = sr2.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 7 AndAlso t(0).Trim() = jour.Trim() AndAlso t(1).Trim() = heureDebut.Trim() Then
                lignes = lignes & nouveauJour & ";" & nouvelleHeureDeb & ";" & nouvelleHeureFin & ";" & nouvelleActivite & ";" & nouveauType & ";" & nouvelleDuree & ";" & nouveauCoach & vbCrLf
            Else
                lignes = lignes & s & vbCrLf
            End If
        End While

        sr2.Close()
        fs2.Close()

        Dim fs3 As New FileStream(FichierPlanning, FileMode.Create, FileAccess.Write)
        Dim sw As New StreamWriter(fs3)
        sw.Write(lignes)
        sw.Close()
        fs3.Close()

        MessageBox.Show("Planning modifié avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ChargerPlanning()
        ViderChamps()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim jour As String = InputBox("Veuillez saisir le jour du planning :", "Suppression")

        If String.IsNullOrWhiteSpace(jour) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        Dim heureDebut As String = InputBox("Veuillez saisir l'heure de début (HH:mm) :", "Suppression")

        If String.IsNullOrWhiteSpace(heureDebut) Then
            MsgBox("Opération annulée.", vbInformation, "Annulation")
            Exit Sub
        End If

        Dim fs As New FileStream(FichierPlanning, FileMode.Open, FileAccess.Read)
        Dim sr As New StreamReader(fs)
        Dim s As String
        Dim t() As String
        Dim trouve As Boolean = False

        While sr.Peek > -1
            s = sr.ReadLine()
            t = s.Split(";"c)

            If t.Length >= 7 AndAlso t(0).Trim() = jour.Trim() AndAlso t(1).Trim() = heureDebut.Trim() Then
                trouve = True
                Exit While
            End If
        End While

        sr.Close()
        fs.Close()

        If Not trouve Then
            MessageBox.Show("Erreur : Aucun planning trouvé !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce planning ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Exit Sub
        End If


        Dim fs2 As New FileStream(FichierPlanning, FileMode.Open, FileAccess.Read)
        Dim sr2 As New StreamReader(fs2)
        Dim nouveaufichier As String = ""

        While sr2.Peek > -1
            s = sr2.ReadLine()
            t = s.Split(";"c)

            If Not (t.Length >= 7 AndAlso t(0).Trim() = jour.Trim() AndAlso t(1).Trim() = heureDebut.Trim()) Then
                nouveaufichier = nouveaufichier & s & vbCrLf
            End If
        End While

        sr2.Close()
        fs2.Close()

        Dim fs3 As New FileStream(FichierPlanning, FileMode.Create, FileAccess.Write)
        Dim sw As New StreamWriter(fs3)
        sw.Write(nouveaufichier)
        sw.Close()
        fs3.Close()

        MessageBox.Show("Planning supprimé avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ChargerPlanning()
        ViderChamps()
    End Sub


End Class