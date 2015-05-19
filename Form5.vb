﻿
Imports System
Imports System.IO

Public Class frmApps



    Private Sub CLBExercise_DragLeave(sender As Object, e As System.EventArgs) Handles CLBExercise.DoubleClick
        CLBExercise.Items.Remove(CLBExercise.SelectedItem)
    End Sub

    Private Sub CBVitalSubDomTask_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CBVitalSubDomTask.CheckedChanged
        If Form1.FormLoading = False Then
            My.Settings.VitalSubAssignments = CBVitalSubDomTask.Checked
            My.Settings.Save()
        End If
    End Sub

    Private Sub BTNVitalSub_Click(sender As System.Object, e As System.EventArgs) Handles BTNVitalSub.Click

        If Form1.SaidHello = True Then
            MessageBox.Show(Me, "Please wait until you are not engaged with the domme to make VitalSub reports!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Form1.SaidHello = True
        Form1.TeaseOver = False


        Dim VitalSubFail As Boolean = False

        If CLBExercise.Items.Count > 0 Then
            For i As Integer = 0 To CLBExercise.Items.Count - 1
                If CLBExercise.GetItemChecked(i) = False Then VitalSubFail = True
            Next
        End If

        If Val(LBLCalorie.Text) > Val(TBCalorie.Text) Then VitalSubFail = True

        Dim VitalSubState As String

        If VitalSubFail = True Then
            VitalSubState = "Punishments"
        Else
            VitalSubState = "Rewards"
        End If
        VitalSubFail = False




        Dim VitalList As New List(Of String)

        For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\VitalSub\" & VitalSubState & "\", FileIO.SearchOption.SearchTopLevelOnly, "*.txt")
            VitalList.Add(foundFile)
        Next

        If VitalList.Count > 0 Then

            For i As Integer = 0 To CLBExercise.Items.Count - 1
                CLBExercise.SetItemChecked(i, False)
            Next
            Form1.SaveExercise()

            LBCalorie.Items.Clear()
            If File.Exists(Application.StartupPath & "\System\VitalSub\CalorieItems.txt") Then My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\System\VitalSub\CalorieItems.txt")
            LBLCalorie.Text = 0
            Form1.CaloriesConsumed = 0
            My.Settings.CaloriesConsumed = 0
            My.Settings.Save()

            Form1.FileText = VitalList(Form1.randomizer.Next(0, VitalList.Count))

            Form1.StrokeTauntVal = -1
            Form1.ScriptTick = 3
            Form1.ScriptTimer.Start()

        Else

            MessageBox.Show(Me, "No " & VitalSubState & " were found! Please make sure you have files in the VitaSub directory for this personality type!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If


    End Sub


    Private Sub Button4_Click_1(sender As System.Object, e As System.EventArgs) Handles Button4.Click




        If My.Settings.ClearWishlist = True Then
          
            MessageBox.Show(Me, "You have already purchased " & Form1.domName.Text & "'s Wishlist item for today!" & Environment.NewLine & Environment.NewLine & _
                            "Please check back again tomorrow!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If








        If DateTime.Now.ToString("MM/dd/yyyy") <> Form1.GetLastWishlistStamp().ToString("MM/dd/yyyy") Or Not File.Exists(Application.StartupPath & "\System\Wishlist") Then

            'If CDate(DateString) <> GetLastWishlistStamp() Or Not File.Exists(Application.StartupPath & "\System\Wishlist") Then

            'If CDate(DateString) <> GetLastWishlistStamp() Or Not File.Exists(Application.StartupPath & "\System\Wishlist") Then



            Dim WishList As New List(Of String)
            WishList.Clear()

            For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Wishlist\Items\", FileIO.SearchOption.SearchTopLevelOnly, "*.txt")
                WishList.Add(foundFile)
            Next

            If WishList.Count < 1 Then
                MessageBox.Show(Me, "No Wishlist items found!" & Environment.NewLine & Environment.NewLine & _
                            "Please make sure you have item scripts located in Apps\Wishlist\Items.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                Return

            End If

            LBLWishlistDom.Text = Form1.domName.Text & "'s Wishlist"
            LBLWishlistDate.Text = DateTime.Now.ToString("MM/dd/yyyy")
            WishlistCostGold.Visible = False
            WishlistCostSilver.Visible = False
            LBLWishlistBronze.Text = Form1.BronzeTokens
            LBLWishlistSilver.Text = Form1.SilverTokens
            LBLWishlistGold.Text = Form1.GoldTokens
            LBLWishListText.Text = ""

        

            Dim WishDir As String = WishList(Form1.randomizer.Next(0, WishList.Count))

            Dim WishReader As New StreamReader(WishDir)

            WishList.Clear()

            While WishReader.Peek <> -1
                WishList.Add(WishReader.ReadLine())
            End While

            WishReader.Close()
            WishReader.Dispose()

            LBLWishListName.Text = WishList(0)
            My.Settings.WishlistName = LBLWishListName.Text


            WishlistPreview.Load(WishList(1))
            WishlistPreview.Visible = True
            My.Settings.WishlistPreview = WishList(1)

            If WishList(2).Contains("Silver") Then
                WishlistCostSilver.Visible = True
                LBLWishlistCost.Text = WishList(2)
                LBLWishlistCost.Text = LBLWishlistCost.Text.Replace(" Silver", "")
                My.Settings.WishlistTokenType = "Silver"
            End If

            If WishList(2).Contains("Gold") Then
                WishlistCostGold.Visible = True
                LBLWishlistCost.Text = WishList(2)
                LBLWishlistCost.Text = LBLWishlistCost.Text.Replace(" Gold", "")
                My.Settings.WishlistTokenType = "Gold"
            End If

            My.Settings.WishlistCost = Val(LBLWishlistCost.Text)


            LBLWishListText.Text = WishList(3)
            My.Settings.WishlistNote = WishList(3)


            If WishlistCostGold.Visible = True Then
                If Form1.GoldTokens >= Val(LBLWishlistCost.Text) Then
                    BTNWishlist.Enabled = True
                    BTNWishlist.Text = "Purchase for " & Form1.domName.Text
                Else
                    BTNWishlist.Enabled = False
                    BTNWishlist.Text = "Not Enough Tokens!"
                End If
            End If

            If WishlistCostSilver.Visible = True Then
                If Form1.SilverTokens >= Val(LBLWishlistCost.Text) Then
                    BTNWishlist.Enabled = True
                    BTNWishlist.Text = "Purchase for " & Form1.domName.Text
                Else
                    BTNWishlist.Enabled = False
                    BTNWishlist.Text = "Not Enough Tokens!"
                End If
            End If

            My.Settings.Save()



            System.IO.File.WriteAllText(Application.StartupPath & "\System\Wishlist", DateString)


        Else

            'Debug.Print("CDate = GetLastWishlistStamp")

            LBLWishlistDom.Text = Form1.domName.Text & "'s Wishlist"
            LBLWishlistDate.Text = DateTime.Now.ToString("MM/dd/yyyy")
            LBLWishlistBronze.Text = Form1.BronzeTokens
            LBLWishlistSilver.Text = Form1.SilverTokens
            LBLWishlistGold.Text = Form1.GoldTokens


            LBLWishListName.Text = My.Settings.WishlistName
            Try
                WishlistPreview.Load(My.Settings.WishlistPreview)
            Catch
                WishlistPreview.Load(Application.StartupPath & "\Images\System\NoPreview.png")
            End Try

            If My.Settings.WishlistTokenType = "Silver" Then WishlistCostSilver.Visible = True
            If My.Settings.WishlistTokenType = "Gold" Then WishlistCostGold.Visible = True
            LBLWishlistCost.Text = My.Settings.WishlistCost
            LBLWishListText.Text = My.Settings.WishlistNote

            If WishlistCostGold.Visible = True Then
                If Form1.GoldTokens >= Val(LBLWishlistCost.Text) Then
                    BTNWishlist.Text = "????? Gold"
                    BTNWishlist.Enabled = True
                Else
                    BTNWishlist.Text = "Not Enough Tokens!"
                    BTNWishlist.Enabled = False
                End If
            End If

            If WishlistCostSilver.Visible = True Then
                Debug.Print("Silver Caled PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP")
                If Form1.SilverTokens >= Val(LBLWishlistCost.Text) Then
                    BTNWishlist.Text = "???? Silver"
                    BTNWishlist.Enabled = True
                Else
                    BTNWishlist.Text = "Not Enough Tokens!"
                    BTNWishlist.Enabled = False
                End If
            End If

        End If





        PNLWishList.Visible = True
        PNLAppHome.Visible = False

        LBLWishlistBronze.Text = Form1.BronzeTokens
        LBLWishlistSilver.Text = Form1.SilverTokens
        LBLWishlistGold.Text = Form1.GoldTokens

        If WishlistCostGold.Visible = True Then
            If Form1.GoldTokens >= Val(LBLWishlistCost.Text) Then
                BTNWishlist.Text = "Purchase for " & Form1.domName.Text
                BTNWishlist.Enabled = True
            Else
                BTNWishlist.Text = "Not Enough Tokens!"
                BTNWishlist.Enabled = False
            End If
        End If

        If WishlistCostSilver.Visible = True Then
            Debug.Print("Silver Called")
            If Form1.SilverTokens >= Val(LBLWishlistCost.Text) Then
                BTNWishlist.Text = "Purchase for " & Form1.domName.Text
                BTNWishlist.Enabled = True
            Else
                BTNWishlist.Text = "Not Enough Tokens!"
                BTNWishlist.Enabled = False
            End If
        End If


    End Sub

    Private Sub BTNWishlist_Click(sender As System.Object, e As System.EventArgs) Handles BTNWishlist.Click

        If Form1.SaidHello = True Then
            MessageBox.Show(Me, "Please wait until you are not engaged with your domme to use this feature!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Debug.Print(WishlistCostSilver.Visible)
        Debug.Print(Val(LBLWishlistCost.Text))

        If WishlistCostSilver.Visible = True And Form1.SilverTokens >= Val(LBLWishlistCost.Text) Then

            Form1.SilverTokens -= Val(LBLWishlistCost.Text)
            My.Settings.SilverTokens = Form1.SilverTokens

            'LBLWishListText.Text = "You purchased this item for " & domName.Text & " on " & CDate(DateString) & "."
            'My.Settings.WishlistNote = LBLWishListText.Text

            My.Settings.ClearWishlist = True

            My.Settings.Save()

            WishlistCostGold.Visible = False
            WishlistCostSilver.Visible = False
            LBLWishlistBronze.Text = Form1.BronzeTokens
            LBLWishlistSilver.Text = Form1.SilverTokens
            LBLWishlistGold.Text = Form1.GoldTokens
            LBLWishListName.Text = ""
            WishlistPreview.Visible = False
            LBLWishlistCost.Text = ""
            LBLWishListText.Text = "Thank you for your purchase! " & Form1.domName.Text & " has been notified of your generous gift. Please check back again tomorrow for a new item!"
            BTNWishlist.Enabled = False
            BTNWishlist.Text = ""


            Dim SilverList As New List(Of String)

            For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Wishlist\Silver Rewards\", FileIO.SearchOption.SearchTopLevelOnly, "*.txt")
                SilverList.Add(foundFile)
            Next

            If SilverList.Count < 1 Then
                MessageBox.Show(Me, "No Silver Reward scripts were found!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                Return
            End If

            Form1.SaidHello = True
            Form1.ShowModule = True

            Form1.FileText = SilverList(Form1.randomizer.Next(0, SilverList.Count))

            Form1.StrokeTauntVal = -1
            Form1.ScriptTick = 2
            Form1.ScriptTimer.Start()
            Return

        End If


        If WishlistCostGold.Visible = True And Form1.GoldTokens >= Val(LBLWishlistCost.Text) Then

            Form1.GoldTokens -= Val(LBLWishlistCost.Text)
            My.Settings.GoldTokens = Form1.GoldTokens

            My.Settings.ClearWishlist = True

            My.Settings.Save()

            Dim GoldList As New List(Of String)

            For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Wishlist\Gold Rewards\", FileIO.SearchOption.SearchTopLevelOnly, "*.txt")
                GoldList.Add(foundFile)
            Next

            If GoldList.Count < 1 Then
                MessageBox.Show(Me, "No Gold Reward scripts were found!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                Return
            End If

            Form1.SaidHello = True
            Form1.ShowModule = True

            Form1.FileText = GoldList(Form1.randomizer.Next(0, GoldList.Count))

            Form1.StrokeTauntVal = -1
            Form1.ScriptTick = 2
            Form1.ScriptTimer.Start()

        End If


    End Sub


    'Private Sub Button29_Click_1(sender As System.Object, e As System.EventArgs) Handles button29.click
    ' If frmApps.AppPanelGlitter.Visible = True Then
    ' frmApps.AppPanelGlitter.Visible = False
    'AppSelectLeft.Visible = False
    ' Me.ActiveControl = Me.chatBox
    ' Return
    ' End If

    ' If frmApps.AppPanelVitalSub.Visible = True Then
    ' frmApps.AppPanelVitalSub.Visible = False
    ' frmApps.AppPanelGlitter.Visible = True
    'AppSelectRight.Visible = True
    ' Return
    'End If
    ' End Sub





    Private Sub Button23_Click_1(sender As System.Object, e As System.EventArgs) Handles Button23.Click
        PNLAppHome.Visible = False
        AppPanelGlitter.Visible = True
    End Sub

    Private Sub Button5_Click_1(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        'PNLScriptGuide.Visible = False
        AppPanelGlitter.Visible = False
        AppPanelVitalSub.Visible = False
        PNLAppRandomizer.Visible = False
        PNLHypnoGen.Visible = False
        PNLWishList.Visible = False
        PNLAppHome.Visible = True
    End Sub

    Private Sub Button30_Click_1(sender As System.Object, e As System.EventArgs) Handles BTNHomeVitalSub.Click
        PNLAppHome.Visible = False
        AppPanelVitalSub.Visible = True

        If File.Exists(Application.StartupPath & "\System\VitalSub\CalorieList.txt") Then
            Debug.Print("called itttttttt")
            Dim CalReader As New StreamReader(Application.StartupPath & "\System\VitalSub\CalorieList.txt")
            Dim CalList As New List(Of String)
            While CalReader.Peek <> -1
                CalList.Add(CalReader.ReadLine())
            End While
            CalReader.Close()
            CalReader.Dispose()
            For i As Integer = 0 To CalList.Count - 1
                ComboBoxCalorie.Items.Add(CalList(i))
            Next
        End If

    End Sub

    Private Sub Button42_Click(sender As System.Object, e As System.EventArgs) Handles Button42.Click
        PNLAppHome.Visible = False
        PNLAppRandomizer.Visible = True
    End Sub


    Private Sub Button47_Click(sender As System.Object, e As System.EventArgs) Handles Button47.Click

        If Form1.OpenScriptDialog.ShowDialog() = DialogResult.OK Then

            Form1.StrokeTauntVal = -1

            Form1.FileText = Form1.OpenScriptDialog.FileName
            Form1.BeforeTease = False
            Form1.ShowModule = True
            Form1.SaidHello = True
            Form1.ScriptTick = 1
            Form1.ScriptTimer.Start()


        End If

    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles BTNHypnoGenStart.Click


        If Form1.HypnoGen = False Then

            If CBHypnoGenInduction.Checked = True Then
                If File.Exists(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Hypnotic Guide\Inductions\" & LBHypnoGenInduction.SelectedItem & ".txt") Then
                    Form1.Induction = True
                    Form1.FileText = Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Hypnotic Guide\Inductions\" & LBHypnoGenInduction.SelectedItem & ".txt"
                Else
                    MessageBox.Show(Me, "Please select a valid Hypno Induction File or deselect the Induction option!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                    Return
                End If
            End If



            If File.Exists(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Hypnotic Guide\Hypno Files\" & LBHypnoGen.SelectedItem & ".txt") Then
                If Form1.Induction = False Then
                    Form1.FileText = Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Hypnotic Guide\Hypno Files\" & LBHypnoGen.SelectedItem & ".txt"
                Else
                    Form1.TempHypno = Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Hypnotic Guide\Hypno Files\" & LBHypnoGen.SelectedItem & ".txt"
                End If
            Else
                MessageBox.Show(Me, "Please select a valid Hypno File!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                Return
            End If

            Form1.StrokeTauntVal = -1
            Form1.ScriptTick = 1
            Form1.ScriptTimer.Start()
            Dim HypnoTrack As String = Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Hypnotic Guide\" & ComboBoxHypnoGenTrack.SelectedItem
            If File.Exists(HypnoTrack) Then Form1.DomWMP.URL = HypnoTrack
            Form1.HypnoGen = True
            Form1.AFK = True
            Form1.SaidHello = True

            BTNHypnoGenStart.Text = "End Session"

        Else

            Form1.mciSendString("CLOSE Speech1", String.Empty, 0, 0)
            Form1.mciSendString("CLOSE Echo1", String.Empty, 0, 0)
            Form1.DomWMP.Ctlcontrols.stop()

            Form1.ScriptTimer.Stop()
            Form1.StrokeTauntVal = -1
            Form1.HypnoGen = False
            Form1.Induction = False
            Form1.AFK = False
            Form1.SaidHello = False

            BTNHypnoGenStart.Text = "Guide Me!"

        End If





    End Sub


    Private Sub Button1_Click_2(sender As System.Object, e As System.EventArgs) Handles BTNHomeHypnoGen.Click


        LBHypnoGenInduction.Items.Clear()

        For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Hypnotic Guide\Inductions\", FileIO.SearchOption.SearchTopLevelOnly, "*.txt")

            Dim TempUrl As String = foundFile
            TempUrl = TempUrl.Replace(".txt", "")
            Do Until Not TempUrl.Contains("\")
                TempUrl = TempUrl.Remove(0, 1)
            Loop
            LBHypnoGenInduction.Items.Add(TempUrl)

        Next

        For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Hypnotic Guide\", FileIO.SearchOption.SearchTopLevelOnly, "*.mp3")
            Dim TempUrl As String = foundFile
            Do Until Not TempUrl.Contains("\")
                TempUrl = TempUrl.Remove(0, 1)
            Loop
            ComboBoxHypnoGenTrack.Items.Add(TempUrl)
        Next



        LBHypnoGen.Items.Clear()

        For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Hypnotic Guide\Hypno Files\", FileIO.SearchOption.SearchTopLevelOnly, "*.txt")

            Dim TempUrl As String = foundFile
            TempUrl = TempUrl.Replace(".txt", "")
            Do Until Not TempUrl.Contains("\")
                TempUrl = TempUrl.Remove(0, 1)
            Loop
            LBHypnoGen.Items.Add(TempUrl)

        Next

        PNLAppHome.Visible = False
        PNLHypnoGen.Visible = True
    End Sub

    Private Sub CBHypnoGenSlideshow_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CBHypnoGenSlideshow.CheckedChanged
        If Form1.FormLoading = False Then
            If CBHypnoGenSlideshow.Checked = True Then
                LBHypnoGenSlideshow.Enabled = True
            Else
                LBHypnoGenSlideshow.Enabled = False
            End If
        End If
    End Sub

    Private Sub CBHypnoGenInduction_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CBHypnoGenInduction.CheckedChanged
        If Form1.FormLoading = False Then
            If CBHypnoGenInduction.Checked = True Then
                LBHypnoGenInduction.Enabled = True
            Else
                LBHypnoGenInduction.Enabled = False
            End If
        End If
    End Sub

    Private Sub BTNExercise_Click(sender As System.Object, e As System.EventArgs) Handles BTNExercise.Click
        If TBExercise.Text <> "" Then
            CLBExercise.Items.Add(TBExercise.Text)
            TBExercise.Text = ""
            Form1.SaveExercise()
        End If
    End Sub

    Private Sub BTNCalorie_Click(sender As System.Object, e As System.EventArgs) Handles BTNCalorie.Click
        If TBCalorieItem.Text <> "" And TBCalorieAmount.Text <> "" Then
            Dim CalorieString As String
            CalorieString = TBCalorieItem.Text & " " & TBCalorieAmount.Text & " Calories"
            Dim Dupecheck As Boolean = False
            For i As Integer = 0 To ComboBoxCalorie.Items.Count - 1
                If CalorieString = ComboBoxCalorie.Items(i) Then Dupecheck = True
            Next
            ComboBoxCalorie.Items.Add(CalorieString)
            LBCalorie.Items.Add(CalorieString)

            If File.Exists(Application.StartupPath & "\System\VitalSub\CalorieItems.txt") Then My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\System\VitalSub\CalorieItems.txt")
            For i As Integer = 0 To LBCalorie.Items.Count - 1
                If Not File.Exists(Application.StartupPath & "\System\VitalSub\CalorieItems.txt") Then
                    My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\System\VitalSub\CalorieItems.txt", LBCalorie.Items(i), False)
                Else
                    My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\System\VitalSub\CalorieItems.txt", Environment.NewLine & LBCalorie.Items(i), True)
                End If
            Next




            If Dupecheck = False Then
                If File.Exists(Application.StartupPath & "\System\VitalSub\CalorieList.txt") Then
                    My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\System\VitalSub\CalorieList.txt", Environment.NewLine & CalorieString, True)
                Else
                    My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\System\VitalSub\CalorieList.txt", CalorieString, False)
                End If
            End If
            Dupecheck = False
            Form1.CaloriesConsumed += TBCalorieAmount.Text
            LBLCalorie.Text = Form1.CaloriesConsumed
            My.Settings.CaloriesConsumed = Form1.CaloriesConsumed
            TBCalorieItem.Text = ""
            TBCalorieAmount.Text = ""
            My.Settings.Save()
        End If
    End Sub

    Private Sub ComboBoxCalorie_Click(sender As Object, e As System.EventArgs) Handles ComboBoxCalorie.SelectionChangeCommitted

        If Not ComboBoxCalorie.SelectedItem Is Nothing Then
            Dim CalorieString As String = ComboBoxCalorie.SelectedItem
            LBCalorie.Items.Add(CalorieString)
            CalorieString = CalorieString.Replace(" Calories", "")
            Dim CalorieSplit As String() = Split(CalorieString)
            Dim TempCal As Integer = Val(CalorieSplit(CalorieSplit.Count - 1))
            Form1.CaloriesConsumed += TempCal
            LBLCalorie.Text = Form1.CaloriesConsumed
            My.Settings.CaloriesConsumed = Form1.CaloriesConsumed
            My.Settings.Save()
            If File.Exists(Application.StartupPath & "\System\VitalSub\CalorieItems.txt") Then My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\System\VitalSub\CalorieItems.txt")
            For i As Integer = 0 To LBCalorie.Items.Count - 1
                If Not File.Exists(Application.StartupPath & "\System\VitalSub\CalorieItems.txt") Then
                    My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\System\VitalSub\CalorieItems.txt", LBCalorie.Items(i), False)
                Else
                    My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\System\VitalSub\CalorieItems.txt", Environment.NewLine & LBCalorie.Items(i), True)
                End If
            Next
        End If
    End Sub



    Private Sub CLBExercise_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CLBExercise.SelectedIndexChanged, CLBExercise.LostFocus

        Form1.SaveExercise()

    End Sub


    Private Sub CBVitalSub_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CBVitalSub.CheckedChanged
        If CBVitalSub.Checked = True Then
            CBVitalSub.ForeColor = Color.LightGreen
            CBVitalSub.Text = "VitalSub Active"
        Else
            CBVitalSub.ForeColor = Color.Red
            CBVitalSub.Text = "VitalSub Inactive"
        End If
    End Sub

    Private Sub CBVitalSub_LostFocus(sender As Object, e As System.EventArgs) Handles CBVitalSub.LostFocus
        If CBVitalSub.Checked = True Then
            My.Settings.VitalSub = True
        Else
            My.Settings.VitalSub = False
        End If
        My.Settings.Save()
    End Sub

    Private Sub LBCalorie_DoubleClick(sender As Object, e As System.EventArgs) Handles LBCalorie.DoubleClick


        Dim CalorieString As String = LBCalorie.SelectedItem
        CalorieString = CalorieString.Replace(" Calories", "")
        Dim CalorieSplit As String() = Split(CalorieString)
        Dim TempCal As Integer = Val(CalorieSplit(CalorieSplit.Count - 1))
        Form1.CaloriesConsumed -= TempCal
        LBLCalorie.Text = Form1.CaloriesConsumed
        My.Settings.CaloriesConsumed = Form1.CaloriesConsumed
        My.Settings.Save()
        LBCalorie.Items.Remove(LBCalorie.SelectedItem)
        If File.Exists(Application.StartupPath & "\System\VitalSub\CalorieItems.txt") Then My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\System\VitalSub\CalorieItems.txt")
        If LBCalorie.Items.Count > 0 Then
            For i As Integer = 0 To LBCalorie.Items.Count - 1
                If Not File.Exists(Application.StartupPath & "\System\VitalSub\CalorieItems.txt") Then
                    My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\System\VitalSub\CalorieItems.txt", LBCalorie.Items(i), False)
                Else
                    My.Computer.FileSystem.WriteAllText(Application.StartupPath & "\System\VitalSub\CalorieItems.txt", Environment.NewLine & LBCalorie.Items(i), True)
                End If
            Next
        End If
    End Sub

    Private Sub Button1_Click_3(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        If FrmSettings.BP1.Image Is Nothing Or FrmSettings.BP2.Image Is Nothing Or FrmSettings.BP3.Image Is Nothing Or FrmSettings.BP4.Image Is Nothing Or FrmSettings.BP5.Image Is Nothing Or FrmSettings.BP6.Image Is Nothing Or _
            FrmSettings.SP1.Image Is Nothing Or FrmSettings.SP2.Image Is Nothing Or FrmSettings.SP3.Image Is Nothing Or FrmSettings.SP4.Image Is Nothing Or FrmSettings.SP5.Image Is Nothing Or FrmSettings.SP6.Image Is Nothing Or _
            FrmSettings.GP1.Image Is Nothing Or FrmSettings.GP2.Image Is Nothing Or FrmSettings.GP3.Image Is Nothing Or FrmSettings.GP4.Image Is Nothing Or FrmSettings.GP5.Image Is Nothing Or FrmSettings.GP6.Image Is Nothing Or _
            FrmSettings.CardBack.Image Is Nothing Then
            MessageBox.Show(Me, "Please set all of the pictures in the Apps\Games tab before launching this app!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Hand)
            Return
        End If


        FrmCardList.Show()
        Form1.RefreshCards()
        FrmCardList.InitializeCards()
    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click

      

        Form1.LargeUI()
        'Form1.PNLChatBox.Location = New Point(0, 214)
        My.Settings.LargeUI = True
        My.Settings.SmallUI = False
        My.Settings.UI768 = False
        My.Settings.Save()

        Try
            Form1.ChatText.Document.Window.ScrollTo(Int16.MaxValue, Int16.MaxValue)
        Catch
        End Try



    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click


        Form1.GetBlogImage()


    End Sub

    Private Sub Button16_Click(sender As System.Object, e As System.EventArgs) Handles Button16.Click

        Form1.GetLocalImage()

    End Sub

    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click

        Form1.RandomizerVideo = True
        Form1.RandomVideo()
        Form1.RandomizerVideo = False


    End Sub

    Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles Button11.Click

        Form1.PlayRandomJOI()

    End Sub

    Private Sub Button15_Click(sender As System.Object, e As System.EventArgs) Handles Button15.Click

        Form1.SaidHello = True
        Form1.RandomizerVideoTease = True

        Form1.ScriptVideoTease = "Censorship Sucks"
        Form1.ScriptVideoTeaseFlag = True
        Form1.RandomVideo()
        Form1.ScriptVideoTeaseFlag = False
        Form1.CensorshipGame = True
        Form1.VideoTease = True
        Form1.CensorshipTick = Form1.randomizer.Next(FrmSettings.NBCensorHideMin.Value, FrmSettings.NBCensorHideMax.Value + 1)
        Form1.CensorshipTimer.Start()



    End Sub

    Private Sub Button14_Click(sender As System.Object, e As System.EventArgs) Handles Button14.Click

        Form1.SaidHello = True
        Form1.RandomizerVideoTease = True

        Form1.ScriptTimer.Stop()
        Form1.SubStroking = True
        Form1.TempStrokeTauntVal = Form1.StrokeTauntVal
        Form1.TempFileText = Form1.FileText
        Form1.ScriptVideoTease = "Avoid The Edge"
        Form1.ScriptVideoTeaseFlag = True
        Form1.AvoidTheEdgeStroking = True
        Form1.AvoidTheEdgeGame = True
        Form1.RandomVideo()
        Form1.ScriptVideoTeaseFlag = False
        Form1.VideoTease = True
        Form1.StartStrokingCount += 1
        Form1.StopMetronome = False
        Form1.StrokePace = Form1.randomizer.Next(3, 8) * 10
        Form1.StrokePaceTimer.Interval = Form1.StrokePace
        Form1.StrokePaceTimer.Start()
        Form1.AvoidTheEdgeTick = 120 / FrmSettings.TauntSlider.Value
        Form1.AvoidTheEdgeTaunts.Start()


    End Sub

    Private Sub Button13_Click(sender As System.Object, e As System.EventArgs) Handles Button13.Click

        Form1.SaidHello = True
        Form1.RandomizerVideoTease = True

        Form1.ScriptTimer.Stop()
        Form1.SubStroking = True
        Form1.ScriptVideoTease = "RLGL"
        Form1.ScriptVideoTeaseFlag = True
        'AvoidTheEdgeStroking = True
        Form1.RLGLGame = True
        Form1.RandomVideo()
        Form1.ScriptVideoTeaseFlag = False
        Form1.VideoTease = True
        Form1.RLGLTick = Form1.randomizer.Next(FrmSettings.NBGreenLightMin.Value, FrmSettings.NBGreenLightMax.Value + 1)
        Form1.RLGLTimer.Start()
        Form1.StartStrokingCount += 1
        Form1.StopMetronome = False
        Form1.StrokePace = Form1.randomizer.Next(3, 8) * 10
        Form1.StrokePaceTimer.Interval = Form1.StrokePace
        Form1.StrokePaceTimer.Start()
        'VideoTauntTick = randomizer.Next(20, 31)
        'VideoTauntTimer.Start()

    End Sub

    Private Sub Button12_Click_1(sender As System.Object, e As System.EventArgs) Handles Button12.Click

        Form1.PlayRandomCH()


    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click

        Form1.UI768()
        'Form1.PNLChatBox.Location = New Point(0, 173)
        My.Settings.LargeUI = False
        My.Settings.SmallUI = False
        My.Settings.UI768 = True
        My.Settings.Save()

        Try
            Form1.ChatText.Document.Window.ScrollTo(Int16.MaxValue, Int16.MaxValue)
        Catch
        End Try



    End Sub

    Private Sub Button3_Click_1(sender As System.Object, e As System.EventArgs) Handles Button3.Click

        Form1.SmallUI()
        'Form1.PNLChatBox.Location = New Point(0, 172)
        My.Settings.LargeUI = False
        My.Settings.SmallUI = True
        My.Settings.UI768 = False
        My.Settings.Save()

        Try
            Form1.ChatText.Document.Window.ScrollTo(Int16.MaxValue, Int16.MaxValue)
        Catch
        End Try


    End Sub


    Private Sub Button35_Click_3(sender As System.Object, e As System.EventArgs) Handles Button35.Click
        Application.Exit()
    End Sub

    Private Sub Button18_Click_3(sender As System.Object, e As System.EventArgs) Handles Button18.Click

        My.Settings.TCAgreed = True
        My.Settings.Save()

        ClearAgree()

    End Sub

    Public Sub ClearAgree()

        Form1.PNLTerms.Visible = False
        Button23.Enabled = True
        BTNHomeVitalSub.Enabled = True
        BTNHomeHypnoGen.Enabled = True
        Button1.Enabled = True
        Button4.Enabled = True
        Button42.Enabled = True
        Button7.Enabled = True
        Button3.Enabled = True
        Button9.Enabled = True
        Button47.Enabled = True

        Button18.Visible = False
        Button35.Visible = False


    End Sub

    Private Sub frmApps_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Form1.BTNShowHideApps.Text = "Show Apps"
    End Sub

    Private Sub frmApps_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If My.Settings.TCAgreed = True Then
            ClearAgree()
        End If
    End Sub
End Class