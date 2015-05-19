﻿Imports System.IO
Imports System
Imports System.Globalization





Public Class FrmCardList

    Dim randomizer As New Random
    Dim TempVal As Integer


    Dim MatchPhase As Integer = 0
    Dim Match1 As String
    Dim Match2 As String
    Dim MatchTemp As String
    Dim MatchChance As Integer
    Dim MatchesMade As Integer
    Dim GameOn As Boolean
    Dim MatchPot As Integer
    Dim ShuffleTick As Integer

    Dim SlotBetTemp As Integer

    Dim MatchList As New List(Of String)

    Dim Pair1 As String
    Dim Pair2 As String
    Dim Pair3 As String
    Dim Pair4 As String
    Dim Pair5 As String
    Dim Pair6 As String
    Dim Pair7 As String
    Dim Pair8 As String
    Dim Pair9 As String

    Dim Match1A As String
    Dim Match2A As String
    Dim Match3A As String
    Dim Match4A As String
    Dim Match5A As String
    Dim Match6A As String

    Dim Match1B As String
    Dim Match2B As String
    Dim Match3B As String
    Dim Match4B As String
    Dim Match5B As String
    Dim Match6B As String

    Dim Match1C As String
    Dim Match2C As String
    Dim Match3C As String
    Dim Match4C As String
    Dim Match5C As String
    Dim Match6C As String

    Dim CardTick As Integer
    Dim CardBackImage As String

    Dim SlotTick1 As Integer
    Dim SlotTick2 As Integer
    Dim SlotTick3 As Integer

    Dim SlotList As New List(Of String)

    Dim Slot1Val As Integer
    Dim Slot2Val As Integer
    Dim Slot3Val As Integer

    Dim SlotBet As Integer
    Dim Payout As Integer

    Dim BoosterTick As Integer
    Dim BoosterListBronze As New List(Of String)
    Dim BoosterListSilver As New List(Of String)
    Dim BoosterListGold As New List(Of String)

    Public B1 As Integer
    Public B2 As Integer
    Public B3 As Integer
    Public B4 As Integer
    Public B5 As Integer
    Public B6 As Integer

    Public S1 As Integer
    Public S2 As Integer
    Public S3 As Integer
    Public S4 As Integer
    Public S5 As Integer
    Public S6 As Integer

    Public G1 As Integer
    Public G2 As Integer
    Public G3 As Integer
    Public G4 As Integer
    Public G5 As Integer
    Public G6 As Integer



    Dim fileName1 As String

    Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" _
  (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, _
  ByVal uReturnLength As Integer, ByVal hwndCallback As Integer) As Integer



    Public Sub InitializeCards()


        M1A.Enabled = True
        M2A.Enabled = True
        M3A.Enabled = True
        M4A.Enabled = True
        M5A.Enabled = True
        M6A.Enabled = True

        M1B.Enabled = True
        M2B.Enabled = True
        M3B.Enabled = True
        M4B.Enabled = True
        M5B.Enabled = True
        M6B.Enabled = True

        M1C.Enabled = True
        M2C.Enabled = True
        M3C.Enabled = True
        M4C.Enabled = True
        M5C.Enabled = True
        M6C.Enabled = True

        If File.Exists(My.Settings.CardBack) Then
            CardBackImage = My.Settings.CardBack
        Else
            CardBackImage = Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Games\_CardBackPicture.png"
        End If

        ClearMatchCache()

        M1A.Load(CardBackImage)
        M2A.Load(CardBackImage)
        M3A.Load(CardBackImage)
        M4A.Load(CardBackImage)
        M5A.Load(CardBackImage)
        M6A.Load(CardBackImage)

        M1B.Load(CardBackImage)
        M2B.Load(CardBackImage)
        M3B.Load(CardBackImage)
        M4B.Load(CardBackImage)
        M5B.Load(CardBackImage)
        M6B.Load(CardBackImage)

        M1C.Load(CardBackImage)
        M2C.Load(CardBackImage)
        M3C.Load(CardBackImage)
        M4C.Load(CardBackImage)
        M5C.Load(CardBackImage)
        M6C.Load(CardBackImage)


        MatchList.Clear()


        For i As Integer = 0 To FrmSettings.URLFileList.Items.Count - 1


            If File.Exists(Application.StartupPath & "\Images\System\URL Files\" & FrmSettings.URLFileList.Items(i) & ".txt") Then

                If FrmSettings.URLFileList.GetItemCheckState(i) = CheckState.Checked Then

                    Dim URLString As String = Application.StartupPath & "\Images\System\URL Files\" & FrmSettings.URLFileList.Items(i) & ".txt"
                    Dim CardReader As New System.IO.StreamReader(URLString)

                    While CardReader.Peek <> -1
                        MatchList.Add(CardReader.ReadLine())
                    End While


                    CardReader.Close()
                    CardReader.Dispose()

                End If

            End If
        Next

        If FrmSettings.CBIncludeGifs.Checked = False Then
            For i As Integer = MatchList.Count - 1 To 0 Step -1
                If MatchList(i).Contains(".gif") Then MatchList.Remove(MatchList(i))
            Next
        End If


        Dim supportedExtensions As String

        If FrmSettings.CBIncludeGifs.Checked = True Then
            supportedExtensions = "*.png,*.jpg,*.gif,*.bmp,*.jpeg"
        Else
            supportedExtensions = "*.png,*.jpg,*.bmp,*.jpeg"
        End If

        Dim files As String()


        If FrmSettings.CBIHardcore.Checked = True And Directory.Exists(FrmSettings.LBLIHardcore.Text) Then
            If FrmSettings.CBIHardcoreSD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLIHardcore.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLIHardcore.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If

        If FrmSettings.CBISoftcore.Checked = True And Directory.Exists(FrmSettings.LBLISoftcore.Text) Then
            If FrmSettings.CBISoftcoreSD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLISoftcore.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLISoftcore.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If

        If FrmSettings.CBILesbian.Checked = True And Directory.Exists(FrmSettings.LBLILesbian.Text) Then
            If FrmSettings.CBILesbianSD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLILesbian.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLILesbian.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If

        If FrmSettings.CBIBlowjob.Checked = True And Directory.Exists(FrmSettings.LBLIBlowjob.Text) Then
            If FrmSettings.CBIBlowjobSD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLIBlowjob.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLIBlowjob.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If

        If FrmSettings.CBIFemdom.Checked = True And Directory.Exists(FrmSettings.LBLIFemdom.Text) Then
            If FrmSettings.CBIFemdomSD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLIFemdom.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLIFemdom.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If

        If FrmSettings.CBILezdom.Checked = True And Directory.Exists(FrmSettings.LBLILezdom.Text) Then
            If FrmSettings.CBILezdomSD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLILezdom.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLILezdom.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If

        If FrmSettings.CBIHentai.Checked = True And Directory.Exists(FrmSettings.LBLIHentai.Text) Then
            If FrmSettings.CBIHentaiSD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLIHentai.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLIHentai.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If

        If FrmSettings.CBIGay.Checked = True And Directory.Exists(FrmSettings.LBLIGay.Text) Then
            If FrmSettings.CBIGaySD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLIGay.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLIGay.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If

        If FrmSettings.CBIMaledom.Checked = True And Directory.Exists(FrmSettings.LBLIMaledom.Text) Then
            If FrmSettings.CBIMaledomSD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLIMaledom.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLIMaledom.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If

        If FrmSettings.CBICaptions.Checked = True And Directory.Exists(FrmSettings.LBLICaptions.Text) Then
            If FrmSettings.CBICaptionsSD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLICaptions.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLICaptions.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If

        If FrmSettings.CBIGeneral.Checked = True And Directory.Exists(FrmSettings.LBLIGeneral.Text) Then
            If FrmSettings.CBIGeneralSD.Checked = True Then
                files = Directory.GetFiles(FrmSettings.LBLIGeneral.Text, "*.*", SearchOption.AllDirectories)
            Else
                files = Directory.GetFiles(FrmSettings.LBLIGeneral.Text, "*.*")
            End If
            Array.Sort(files)
            For Each fi As String In files
                If supportedExtensions.Contains(Path.GetExtension(fi)) Then
                    MatchList.Add(fi)
                End If
            Next
        End If




        If MatchList.Count < 1 Then Return



        Dim CardVal As Integer


        CardVal = Form1.randomizer.Next(0, MatchList.Count)
        Pair1 = MatchList(CardVal)
        MatchList.Remove(MatchList(CardVal))

        CardVal = Form1.randomizer.Next(0, MatchList.Count)
        Pair2 = MatchList(CardVal)
        MatchList.Remove(MatchList(CardVal))

        CardVal = Form1.randomizer.Next(0, MatchList.Count)
        Pair3 = MatchList(CardVal)
        MatchList.Remove(MatchList(CardVal))

        CardVal = Form1.randomizer.Next(0, MatchList.Count)
        Pair4 = MatchList(CardVal)
        MatchList.Remove(MatchList(CardVal))

        CardVal = Form1.randomizer.Next(0, MatchList.Count)
        Pair5 = MatchList(CardVal)
        MatchList.Remove(MatchList(CardVal))

        CardVal = Form1.randomizer.Next(0, MatchList.Count)
        Pair6 = MatchList(CardVal)
        MatchList.Remove(MatchList(CardVal))

        CardVal = Form1.randomizer.Next(0, MatchList.Count)
        Pair7 = MatchList(CardVal)
        MatchList.Remove(MatchList(CardVal))

        CardVal = Form1.randomizer.Next(0, MatchList.Count)
        Pair8 = MatchList(CardVal)
        MatchList.Remove(MatchList(CardVal))

        CardVal = Form1.randomizer.Next(0, MatchList.Count)
        Pair9 = MatchList(CardVal)
        'MatchList.Remove(MatchList(CardVal))

        MatchList.Clear()

        MatchList.Add(Pair1)
        MatchList.Add(Pair1)
        MatchList.Add(Pair2)
        MatchList.Add(Pair2)
        MatchList.Add(Pair3)
        MatchList.Add(Pair3)
        MatchList.Add(Pair4)
        MatchList.Add(Pair4)
        MatchList.Add(Pair5)
        MatchList.Add(Pair5)
        MatchList.Add(Pair6)
        MatchList.Add(Pair6)
        MatchList.Add(Pair7)
        MatchList.Add(Pair7)
        MatchList.Add(Pair8)
        MatchList.Add(Pair8)
        MatchList.Add(Pair9)
        MatchList.Add(Pair9)

        For I As Integer = 0 To MatchList.Count - 1
            Debug.Print(MatchList(I))
        Next

        Match1A = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match1A)
        Match2A = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match2A)
        Match3A = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match3A)
        Match4A = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match4A)
        Match5A = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match5A)
        Match6A = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match6A)

        Match1B = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match1B)
        Match2B = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match2B)
        Match3B = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match3B)
        Match4B = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match4B)
        Match5B = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match5B)
        Match6B = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match6B)

        Match1C = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match1C)
        Match2C = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match2C)
        Match3C = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match3C)
        Match4C = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match4C)
        Match5C = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        MatchList.Remove(Match5C)
        Match6C = MatchList(Form1.randomizer.Next(0, MatchList.Count))
        'MatchList.Remove(Match6C)


        Debug.Print(Match4A)


    End Sub



 

    Public Sub CheckMatchTemp()

        'If MatchTemp = "M1A" Then M1A.Enabled = False
        'If MatchTemp = "M2A" Then M2A.Enabled = False
        'If MatchTemp = "M3A" Then M3A.Enabled = False
        'If MatchTemp = "M4A" Then M4A.Enabled = False
        'If MatchTemp = "M5A" Then M5A.Enabled = False
        'If MatchTemp = "M6A" Then M6A.Enabled = False

        'If MatchTemp = "M1B" Then M1B.Enabled = False
        'If MatchTemp = "M2B" Then M2B.Enabled = False
        'If MatchTemp = "M3B" Then M3B.Enabled = False
        'If MatchTemp = "M4B" Then M4B.Enabled = False
        'If MatchTemp = "M5B" Then M5B.Enabled = False
        'If MatchTemp = "M6B" Then M6B.Enabled = False

        'If MatchTemp = "M1C" Then M1C.Enabled = False
        'If MatchTemp = "M2C" Then M2C.Enabled = False
        'If MatchTemp = "M3C" Then M3C.Enabled = False
        'If MatchTemp = "M4C" Then M4C.Enabled = False
        'If MatchTemp = "M5C" Then M5C.Enabled = False
        'If MatchTemp = "M6C" Then M6C.Enabled = False

        If Match1 <> Match2 Then

            MatchChance -= 1
            LBLMatchChance.Text = MatchChance & " Chances Left"
            If MatchChance = 1 Then LBLMatchChance.Text = MatchChance & " Chance Left"

            CardTick = 2
            CardTimer.Start()

            Do
                Application.DoEvents()
            Loop Until CardTimer.Enabled = False

        Else

            MatchesMade += 1

        End If


        If M1A.Enabled = True Then
            Try
                M1A.Image.Dispose()
                M1A.Image = Nothing
            Catch
            End Try
            M1A.Load(CardBackImage)
        End If
        If M2A.Enabled = True Then
            Try
                M2A.Image.Dispose()
                M2A.Image = Nothing
            Catch
            End Try
            M2A.Load(CardBackImage)
        End If
        If M3A.Enabled = True Then
            Try
                M3A.Image.Dispose()
                M3A.Image = Nothing
            Catch
            End Try
            M3A.Load(CardBackImage)
        End If
        If M4A.Enabled = True Then
            Try
                M4A.Image.Dispose()
                M4A.Image = Nothing
            Catch
            End Try
            M4A.Load(CardBackImage)
        End If
        If M5A.Enabled = True Then
            Try
                M5A.Image.Dispose()
                M5A.Image = Nothing
            Catch
            End Try
            M5A.Load(CardBackImage)
        End If
        If M6A.Enabled = True Then
            Try
                M6A.Image.Dispose()
                M6A.Image = Nothing
            Catch
            End Try
            M6A.Load(CardBackImage)
        End If

        If M1B.Enabled = True Then
            Try
                M1B.Image.Dispose()
                M1B.Image = Nothing
            Catch
            End Try
            M1B.Load(CardBackImage)
        End If
        If M2B.Enabled = True Then
            Try
                M2B.Image.Dispose()
                M2B.Image = Nothing
            Catch
            End Try
            M2B.Load(CardBackImage)
        End If
        If M3B.Enabled = True Then
            Try
                M3B.Image.Dispose()
                M3B.Image = Nothing
            Catch
            End Try
            M3B.Load(CardBackImage)
        End If
        If M4B.Enabled = True Then
            Try
                M4B.Image.Dispose()
                M4B.Image = Nothing
            Catch
            End Try
            M4B.Load(CardBackImage)
        End If
        If M5B.Enabled = True Then
            Try
                M5B.Image.Dispose()
                M5B.Image = Nothing
            Catch
            End Try
            M5B.Load(CardBackImage)
        End If
        If M6B.Enabled = True Then
            Try
                M6B.Image.Dispose()
                M6B.Image = Nothing
            Catch
            End Try
            M6B.Load(CardBackImage)
        End If
       
        If M1C.Enabled = True Then
            Try
                M1C.Image.Dispose()
                M1C.Image = Nothing
            Catch
            End Try
            M1C.Load(CardBackImage)
        End If
        If M2C.Enabled = True Then
            Try
                M2C.Image.Dispose()
                M2C.Image = Nothing
            Catch
            End Try
            M2C.Load(CardBackImage)
        End If
        If M3C.Enabled = True Then
            Try
                M3C.Image.Dispose()
                M3C.Image = Nothing
            Catch
            End Try
            M3C.Load(CardBackImage)
        End If
        If M4C.Enabled = True Then
            Try
                M4C.Image.Dispose()
                M4C.Image = Nothing
            Catch
            End Try
            M4C.Load(CardBackImage)
        End If
        If M5C.Enabled = True Then
            Try
                M5C.Image.Dispose()
                M5C.Image = Nothing
            Catch
            End Try
            M5C.Load(CardBackImage)
        End If
        If M6C.Enabled = True Then
            Try
                M6C.Image.Dispose()
                M6C.Image = Nothing
            Catch
            End Try
            M6C.Load(CardBackImage)
        End If
      
        Try
            GC.Collect()
        Catch
        End Try

        If MatchChance = 0 Then

            LBLMatchChance.Text = "Game Over"
            GameOn = False
            BTNMatchEasy.Enabled = True
            BTNMatchNormal.Enabled = True
            BTNMatchHard.Enabled = True

            If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\NoPayout.wav") Then


                GameWMP.settings.setMode("loop", False)
                GameWMP.settings.volume = 20
                GameWMP.URL = Application.StartupPath & "\Audio\System\NoPayout.wav"


            End If
            'My.Computer.Audio.Play(Application.StartupPath & "\Audio\System\NoPayout.wav")



        End If

        If MatchesMade = 9 Then


            LBLMatchChance.Text = "You Win!"
            GameOn = False
            Form1.BronzeTokens = Form1.BronzeTokens + MatchPot
            My.Settings.BronzeTokens = Form1.BronzeTokens
            My.Settings.Save()
            LBLMatchTokens.Text = Form1.BronzeTokens


            If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\PayoutSmall.wav") Then


                GameWMP.settings.setMode("loop", False)
                GameWMP.settings.volume = 20
                GameWMP.URL = Application.StartupPath & "\Audio\System\PayoutSmall.wav"


            End If

            'If FrmSettings.CBGameSounds.Checked = True Then My.Computer.Audio.Play(Application.StartupPath & "\Audio\System\PayoutSmall.wav")

            BTNMatchEasy.Enabled = True
            BTNMatchNormal.Enabled = True
            BTNMatchHard.Enabled = True

        End If




    End Sub

    Public Sub FlipCards()

        If MatchTemp = "M1A" Then M1A.Enabled = True
        If MatchTemp = "M2A" Then M2A.Enabled = True
        If MatchTemp = "M3A" Then M3A.Enabled = True
        If MatchTemp = "M4A" Then M4A.Enabled = True
        If MatchTemp = "M5A" Then M5A.Enabled = True
        If MatchTemp = "M6A" Then M6A.Enabled = True

        If MatchTemp = "M1B" Then M1B.Enabled = True
        If MatchTemp = "M2B" Then M2B.Enabled = True
        If MatchTemp = "M3B" Then M3B.Enabled = True
        If MatchTemp = "M4B" Then M4B.Enabled = True
        If MatchTemp = "M5B" Then M5B.Enabled = True
        If MatchTemp = "M6B" Then M6B.Enabled = True

        If MatchTemp = "M1C" Then M1C.Enabled = True
        If MatchTemp = "M2C" Then M2C.Enabled = True
        If MatchTemp = "M3C" Then M3C.Enabled = True
        If MatchTemp = "M4C" Then M4C.Enabled = True
        If MatchTemp = "M5C" Then M5C.Enabled = True
        If MatchTemp = "M6C" Then M6C.Enabled = True






    End Sub

    Public Sub PlayCardFlip()


        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\CardFlip.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\CardFlip.wav"
        End If

    End Sub

    Private Sub M1A_Click(sender As System.Object, e As System.EventArgs) Handles M1A.Click


        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()





        If MatchPhase = 0 Then
            MatchPhase = 1
            M1A.Load(Match1A)
            Match1 = M1A.ImageLocation
            M1A.Enabled = False
            MatchTemp = "M1A"
        Else
            MatchPhase = 0
            M1A.Load(Match1A)
            Match2 = M1A.ImageLocation
            If Match1 = Match2 Then
                M1A.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M2A_Click(sender As System.Object, e As System.EventArgs) Handles M2A.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M2A.Load(Match2A)
            Match1 = M2A.ImageLocation
            M2A.Enabled = False
            MatchTemp = "M2A"
        Else
            MatchPhase = 0
            M2A.Load(Match2A)
            Match2 = M2A.ImageLocation
            If Match1 = Match2 Then
                M2A.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M3A_Click(sender As System.Object, e As System.EventArgs) Handles M3A.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M3A.Load(Match3A)
            Match1 = M3A.ImageLocation
            M3A.Enabled = False
            MatchTemp = "M3A"
        Else
            MatchPhase = 0
            M3A.Load(Match3A)
            Match2 = M3A.ImageLocation
            If Match1 = Match2 Then
                M3A.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M4A_Click(sender As System.Object, e As System.EventArgs) Handles M4A.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M4A.Load(Match4A)
            Match1 = M4A.ImageLocation
            M4A.Enabled = False
            MatchTemp = "M4A"
        Else
            MatchPhase = 0
            M4A.Load(Match4A)
            Match2 = M4A.ImageLocation
            If Match1 = Match2 Then
                M4A.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M5A_Click(sender As System.Object, e As System.EventArgs) Handles M5A.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M5A.Load(Match5A)
            Match1 = M5A.ImageLocation
            M5A.Enabled = False
            MatchTemp = "M5A"
        Else
            MatchPhase = 0
            M5A.Load(Match5A)
            Match2 = M5A.ImageLocation
            If Match1 = Match2 Then
                M5A.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M6A_Click(sender As System.Object, e As System.EventArgs) Handles M6A.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M6A.Load(Match6A)
            Match1 = M6A.ImageLocation
            M6A.Enabled = False
            MatchTemp = "M6A"
        Else
            MatchPhase = 0
            M6A.Load(Match6A)
            Match2 = M6A.ImageLocation
            If Match1 = Match2 Then
                M6A.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub

    Private Sub M1B_Click(sender As System.Object, e As System.EventArgs) Handles M1B.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M1B.Load(Match1B)
            Match1 = M1B.ImageLocation
            M1B.Enabled = False
            MatchTemp = "M1B"
        Else
            MatchPhase = 0
            M1B.Load(Match1B)
            Match2 = M1B.ImageLocation
            If Match1 = Match2 Then
                M1B.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M2B_Click(sender As System.Object, e As System.EventArgs) Handles M2B.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M2B.Load(Match2B)
            Match1 = M2B.ImageLocation
            M2B.Enabled = False
            MatchTemp = "M2B"
        Else
            MatchPhase = 0
            M2B.Load(Match2B)
            Match2 = M2B.ImageLocation
            If Match1 = Match2 Then
                M2B.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M3B_Click(sender As System.Object, e As System.EventArgs) Handles M3B.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M3B.Load(Match3B)
            Match1 = M3B.ImageLocation
            M3B.Enabled = False
            MatchTemp = "M3B"
        Else
            MatchPhase = 0
            M3B.Load(Match3B)
            Match2 = M3B.ImageLocation
            If Match1 = Match2 Then
                M3B.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M4B_Click(sender As System.Object, e As System.EventArgs) Handles M4B.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M4B.Load(Match4B)
            Match1 = M4B.ImageLocation
            M4B.Enabled = False
            MatchTemp = "M4B"
        Else
            MatchPhase = 0
            M4B.Load(Match4B)
            Match2 = M4B.ImageLocation
            If Match1 = Match2 Then
                M4B.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M5B_Click(sender As System.Object, e As System.EventArgs) Handles M5B.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M5B.Load(Match5B)
            Match1 = M5B.ImageLocation
            M5B.Enabled = False
            MatchTemp = "M5B"
        Else
            MatchPhase = 0
            M5B.Load(Match5B)
            Match2 = M5B.ImageLocation
            If Match1 = Match2 Then
                M5B.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M6B_Click(sender As System.Object, e As System.EventArgs) Handles M6B.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M6B.Load(Match6B)
            Match1 = M6B.ImageLocation
            M6B.Enabled = False
            MatchTemp = "M6B"
        Else
            MatchPhase = 0
            M6B.Load(Match6B)
            Match2 = M6B.ImageLocation
            If Match1 = Match2 Then
                M6B.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub

    Private Sub M1C_Click(sender As System.Object, e As System.EventArgs) Handles M1C.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M1C.Load(Match1C)
            Match1 = M1C.ImageLocation
            M1C.Enabled = False
            MatchTemp = "M1C"
        Else
            MatchPhase = 0
            M1C.Load(Match1C)
            Match2 = M1C.ImageLocation
            If Match1 = Match2 Then
                M1C.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M2C_Click(sender As System.Object, e As System.EventArgs) Handles M2C.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M2C.Load(Match2C)
            Match1 = M2C.ImageLocation
            M2C.Enabled = False
            MatchTemp = "M2C"
        Else
            MatchPhase = 0
            M2C.Load(Match2C)
            Match2 = M2C.ImageLocation
            If Match1 = Match2 Then
                M2C.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M3C_Click(sender As System.Object, e As System.EventArgs) Handles M3C.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M3C.Load(Match3C)
            Match1 = M3C.ImageLocation
            M3C.Enabled = False
            MatchTemp = "M3C"
        Else
            MatchPhase = 0
            M3C.Load(Match3C)
            Match2 = M3C.ImageLocation
            If Match1 = Match2 Then
                M3C.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M4C_Click(sender As System.Object, e As System.EventArgs) Handles M4C.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M4C.Load(Match4C)
            Match1 = M4C.ImageLocation
            M4C.Enabled = False
            MatchTemp = "M4C"
        Else
            MatchPhase = 0
            M4C.Load(Match4C)
            Match2 = M4C.ImageLocation
            If Match1 = Match2 Then
                M4C.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M5C_Click(sender As System.Object, e As System.EventArgs) Handles M5C.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M5C.Load(Match5C)
            Match1 = M5C.ImageLocation
            M5C.Enabled = False
            MatchTemp = "M5C"
        Else
            MatchPhase = 0
            M5C.Load(Match5C)
            Match2 = M5C.ImageLocation
            If Match1 = Match2 Then
                M5C.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub
    Private Sub M6C_Click(sender As System.Object, e As System.EventArgs) Handles M6C.Click

        If CardTimer.Enabled = True Or GameOn = False Then Return

        PlayCardFlip()

        If MatchPhase = 0 Then
            MatchPhase = 1
            M6C.Load(Match6C)
            Match1 = M6C.ImageLocation
            M6C.Enabled = False
            MatchTemp = "M6C"
        Else
            MatchPhase = 0
            M6C.Load(Match6C)
            Match2 = M6C.ImageLocation
            If Match1 = Match2 Then
                M6C.Enabled = False
            Else
                FlipCards()
            End If
            CheckMatchTemp()
        End If

    End Sub

    Private Sub CardTimer_Tick(sender As System.Object, e As System.EventArgs) Handles CardTimer.Tick

        CardTick -= 1

        If CardTick < 1 Then
            CardTimer.Stop()
        End If

    End Sub

  

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click

        If SlotTimer3.Enabled = True Or LBLSlotBet.Text = 0 Then Return


        SlotTick1 = 4
        SlotTick2 = 12
        SlotTick3 = 36



        SlotList.Clear()

        SlotList.Add(FrmSettings.BP1.ImageLocation)
        SlotList.Add(FrmSettings.BP2.ImageLocation)
        SlotList.Add(FrmSettings.BP3.ImageLocation)
        SlotList.Add(FrmSettings.BP4.ImageLocation)
        SlotList.Add(FrmSettings.BP5.ImageLocation)
        SlotList.Add(FrmSettings.BP6.ImageLocation)

        SlotList.Add(FrmSettings.SP1.ImageLocation)
        SlotList.Add(FrmSettings.SP2.ImageLocation)
        SlotList.Add(FrmSettings.SP3.ImageLocation)
        SlotList.Add(FrmSettings.SP4.ImageLocation)
        SlotList.Add(FrmSettings.SP5.ImageLocation)
        SlotList.Add(FrmSettings.SP6.ImageLocation)

        SlotList.Add(FrmSettings.GP1.ImageLocation)
        SlotList.Add(FrmSettings.GP2.ImageLocation)
        SlotList.Add(FrmSettings.GP3.ImageLocation)
        SlotList.Add(FrmSettings.GP4.ImageLocation)
        SlotList.Add(FrmSettings.GP5.ImageLocation)
        SlotList.Add(FrmSettings.GP6.ImageLocation)

        SlotBack1.BackColor = Color.Gainsboro
        SlotBack2.BackColor = Color.Gainsboro
        SlotBack3.BackColor = Color.Gainsboro

        LBLSlotPayout.Text = "---"

        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\Slots.wav") Then
            ' mciSendString("close myWAV", Nothing, 0, 0)

            fileName1 = Application.StartupPath & "\Audio\System\Slots.wav"
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = fileName1


            'fileName1 = Form1.GetShortPathName(fileName1)

            'Dim Volume As Integer = 50
            'mciSendString("setaudio myWAV volume to " & Volume, Nothing, 0, 0)
            'waveoutsetvolume(-1, 5)

            'mciSendString("open " & fileName1 & " type WAVEAUDIO alias myWAV", Nothing, 0, 0)
            'mciSendString("play myWAV", Nothing, 0, 0)
        End If
        'My.Computer.Audio.Play(Application.StartupPath & "\Audio\System\Slots.wav", AudioPlayMode.Background)


        Form1.BronzeTokens -= LBLSlotBet.Text
        LBLSlotTokens.Text = Form1.BronzeTokens

        If Val(LBLSlotBet.Text) > Val(LBLSlotTokens.Text) Then
            LBLSlotBet.Text = LBLSlotTokens.Text
            SlotBet = Val(LBLSlotBet.Text)
            SlotBetTemp = Val(LBLSlotBet.Text)
        End If

        'Slot1.Image.Dispose()
        'Slot1.Image = Nothing

        'Slot2.Image.Dispose()
        'Slot2.Image = Nothing

        'Slot3.Image.Dispose()
        'Slot3.Image = Nothing

        'GC.Collect()

        SlotTimer1.Start()
        SlotTImer2.Start()
        SlotTimer3.Start()


    End Sub

    Private Sub SlotTimer_Tick(sender As System.Object, e As System.EventArgs) Handles SlotTimer1.Tick

        SlotTick1 -= 1

        Slot1Val = randomizer.Next(0, 18)
        Try
            Slot1.Image.Dispose()
            GC.Collect()
        Catch
        End Try
        Slot1.Load(SlotList(Slot1Val))

        If SlotTick1 < 1 Then
            SlotTimer1.Stop()
            SlotBack1.BackColor = Color.Silver
            If Slot1Val < 6 Then SlotBack1.BackColor = Color.Peru
            If Slot1Val > 11 Then SlotBack1.BackColor = Color.Gold
        End If







    End Sub

    Private Sub SlotTImer2_Tick(sender As System.Object, e As System.EventArgs) Handles SlotTImer2.Tick


        SlotTick2 -= 1

        Slot2Val = randomizer.Next(0, 18)
        Try
            Slot2.Image.Dispose()
        GC.Collect()
        catch
        End Try
        Slot2.Load(SlotList(Slot2Val))

        If SlotTick2 < 1 Then
            SlotTImer2.Stop()
            SlotBack2.BackColor = Color.Silver
            If Slot2Val < 6 Then SlotBack2.BackColor = Color.Peru
            If Slot2Val > 11 Then SlotBack2.BackColor = Color.Gold
        End If




    End Sub

    Private Sub SlotTimer3_Tick(sender As System.Object, e As System.EventArgs) Handles SlotTimer3.Tick

        SlotTick3 -= 1

        Slot3Val = randomizer.Next(0, 18)
        Try
            Slot3.Image.Dispose()
            GC.Collect()
        Catch
        End Try
        Slot3.Load(SlotList(Slot3Val))

        If SlotTick3 < 1 Then
            SlotTimer3.Stop()
            SlotBack3.BackColor = Color.Silver
            If Slot3Val < 6 Then SlotBack3.BackColor = Color.Peru
            If Slot3Val > 11 Then SlotBack3.BackColor = Color.Gold


            Payout = 0

            If SlotBack1.BackColor = Color.Gold Then Payout = 1
            If SlotBack1.BackColor = Color.Gold And SlotBack2.BackColor = Color.Gold Then Payout = 2

            If SlotBack1.BackColor = Color.Peru And SlotBack2.BackColor = Color.Peru And SlotBack3.BackColor = Color.Peru Then Payout = 3
            If SlotBack1.BackColor = Color.Silver And SlotBack2.BackColor = Color.Silver And SlotBack3.BackColor = Color.Silver Then Payout = 5
            If SlotBack1.BackColor = Color.Gold And SlotBack2.BackColor = Color.Gold And SlotBack3.BackColor = Color.Gold Then Payout = 7

            If SlotBack1.BackColor = Color.Peru And SlotBack2.BackColor = Color.Peru And Slot1Val = Slot2Val Then Payout = 10
            If SlotBack1.BackColor = Color.Silver And SlotBack2.BackColor = Color.Silver And Slot1Val = Slot2Val Then Payout = 15
            If SlotBack1.BackColor = Color.Gold And SlotBack2.BackColor = Color.Gold And Slot1Val = Slot2Val Then Payout = 20

            If SlotBack1.BackColor = Color.Peru And SlotBack2.BackColor = Color.Peru And SlotBack3.BackColor = Color.Peru And Slot1Val = Slot2Val And Slot2Val = Slot3Val Then Payout = 30
            If SlotBack1.BackColor = Color.Silver And SlotBack2.BackColor = Color.Silver And SlotBack3.BackColor = Color.Silver And Slot1Val = Slot2Val And Slot2Val = Slot3Val Then Payout = 40
            If SlotBack1.BackColor = Color.Gold And SlotBack2.BackColor = Color.Gold And SlotBack3.BackColor = Color.Gold And Slot1Val = Slot2Val And Slot2Val = Slot3Val Then Payout = 50

            Payout *= SlotBet
            LBLSlotPayout.Text = Payout

            If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\PayoutOne.wav") Then
                ' Dim SlotSound As String

                'mciSendString("close myWAV", Nothing, 0, 0)


                If Payout < 4 Then
                    'SlotSound = Application.StartupPath & "\Audio\System\PayoutOne.wav"


                    fileName1 = Application.StartupPath & "\Audio\System\PayoutOne.wav"
                    'fileName1 = Form1.GetShortPathName(fileName1)




                    'min Volume is 1, max Volume is 1000



                End If

                If Payout > 3 And Payout < 26 And File.Exists(Application.StartupPath & "\Audio\System\PayoutSmall.wav") Then
                    'SlotSound = Application.StartupPath & "\Audio\System\PayoutSmall.wav"

                    fileName1 = Application.StartupPath & "\Audio\System\PayoutSmall.wav"
                    '   fileName1 = Form1.GetShortPathName(fileName1)
                End If

                If Payout > 25 And File.Exists(Application.StartupPath & "\Audio\System\PayoutBig.wav") Then
                    'SlotSound = Application.StartupPath & "\Audio\System\PayoutBig.wav"

                    fileName1 = Application.StartupPath & "\Audio\System\PayoutBig.wav"
                    '  fileName1 = Form1.GetShortPathName(fileName1)
                End If

                If Payout = 0 And File.Exists(Application.StartupPath & "\Audio\System\NoPayout.wav") Then
                    'SlotSound = Application.StartupPath & "\Audio\System\NoPayout.wav"

                    fileName1 = Application.StartupPath & "\Audio\System\NoPayout.wav"
                    ' fileName1 = Form1.GetShortPathName(fileName1)
                End If

                GameWMP.settings.setMode("loop", False)
                GameWMP.settings.volume = 20
                GameWMP.URL = fileName1


                'Dim Volume As Integer = 50
                'mciSendString("setaudio myWAV volume to " & Volume, Nothing, 0, 0)

                '                mciSendString("open " & fileName1 & " type WAVEAUDIO alias myWAV", Nothing, 0, 0)
                '               mciSendString("play myWAV", Nothing, 0, 0)

                'My.Computer.Audio.Play(SlotSound)
            End If


            Form1.BronzeTokens += Payout

            LBLSlotTokens.Text = Form1.BronzeTokens



        End If








    End Sub

    Private Sub Panel5_Paint(sender As System.Object, e As System.Windows.Forms.PaintEventArgs) Handles Panel5.Paint

    End Sub

    Private Sub FrmCardList_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        InitializeSlots()

    End Sub

    Public Sub InitializeSlots()

        SlotList.Clear()

        SlotList.Add(FrmSettings.BP1.ImageLocation)
        SlotList.Add(FrmSettings.BP2.ImageLocation)
        SlotList.Add(FrmSettings.BP3.ImageLocation)
        SlotList.Add(FrmSettings.BP4.ImageLocation)
        SlotList.Add(FrmSettings.BP5.ImageLocation)
        SlotList.Add(FrmSettings.BP6.ImageLocation)

        SlotList.Add(FrmSettings.SP1.ImageLocation)
        SlotList.Add(FrmSettings.SP2.ImageLocation)
        SlotList.Add(FrmSettings.SP3.ImageLocation)
        SlotList.Add(FrmSettings.SP4.ImageLocation)
        SlotList.Add(FrmSettings.SP5.ImageLocation)
        SlotList.Add(FrmSettings.SP6.ImageLocation)

        SlotList.Add(FrmSettings.GP1.ImageLocation)
        SlotList.Add(FrmSettings.GP2.ImageLocation)
        SlotList.Add(FrmSettings.GP3.ImageLocation)
        SlotList.Add(FrmSettings.GP4.ImageLocation)
        SlotList.Add(FrmSettings.GP5.ImageLocation)
        SlotList.Add(FrmSettings.GP6.ImageLocation)



        Slot1.Load(SlotList(randomizer.Next(0, SlotList.Count)))
        Slot2.Load(SlotList(randomizer.Next(0, SlotList.Count)))
        Slot3.Load(SlotList(randomizer.Next(0, SlotList.Count)))



        Dim SlotImage As String

        If File.Exists(My.Settings.CardBack) Then
            SlotImage = My.Settings.CardBack
        Else
            SlotImage = Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Games\_CardBackPicture.png"
        End If

        SlotLeft2.Load(SlotImage)
        SlotLeft1.Load(SlotImage)
        SlotRight1.Load(SlotImage)
        SlotRight2.Load(SlotImage)

        LBLSlotTokens.Text = Form1.BronzeTokens

    End Sub

    Public Sub ClearSlots()

        Try
            Slot1.Image.Dispose()
            Slot1.Image = Nothing

            Slot2.Image.Dispose()
            Slot2.Image = Nothing

            Slot3.Image.Dispose()
            Slot3.Image = Nothing

            SlotLeft1.Image.Dispose()
            SlotLeft1.Image = Nothing

            SlotLeft2.Image.Dispose()
            SlotLeft2.Image = Nothing

            SlotRight1.Image.Dispose()
            SlotRight1.Image = Nothing

            SlotRight2.Image.Dispose()
            SlotRight2.Image = Nothing
        Catch
        End Try

        Try
            GC.Collect()
        Catch
        End Try

    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click

        If SlotTimer3.Enabled = True Then Return

        If SlotBet < 3 Then
            SlotBet += 1
            If SlotBet > LBLSlotTokens.Text Then SlotBet -= 1
            LBLSlotBet.Text = SlotBet
        End If

    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click

        If SlotTimer3.Enabled = True Then Return

        If SlotBet > 0 Then
            SlotBet -= 1
            LBLSlotBet.Text = SlotBet
        End If


    End Sub



    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs)

        Form1.BronzeTokens += 5
        LBLSlotTokens.Text = Form1.BronzeTokens
    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles BTNMatchEasy.Click




        If Form1.BronzeTokens < 1 Then Return

        InitializeCards()

        If MatchList.Count < 1 Then
            MessageBox.Show(Me, "You will need to select at least 1 local image folder or URL File before you can play the Match Game!", "Caution!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If


        EraseCards()
        Form1.BronzeTokens -= 1
        My.Settings.BronzeTokens = Form1.BronzeTokens
        My.Settings.Save()
        LBLMatchTokens.Text = Form1.BronzeTokens

        InitializeCards()


        MatchChance = 15
        MatchesMade = 0
        MatchPot = 5

        LBLMatchChance.Text = MatchChance & " Chances Left"

        BTNMatchEasy.Enabled = False
        BTNMatchNormal.Enabled = False
        BTNMatchHard.Enabled = False



        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\CardShuffle.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\CardShuffle.wav"
        End If
        'My.Computer.Audio.Play(Application.StartupPath & "\Audio\System\CardShuffle.wav")

        ShuffleTick = 19
        ShuffleTimer.Start()


    End Sub

    Private Sub BTNMatchNormal_Click(sender As System.Object, e As System.EventArgs) Handles BTNMatchNormal.Click

        If Form1.BronzeTokens < 1 Then Return

        InitializeCards()

        If MatchList.Count < 1 Then
            MessageBox.Show(Me, "You will need to select at least 1 local image folder or URL File before you can play the Match Game!", "Caution!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If


        EraseCards()
        Form1.BronzeTokens -= 1
        My.Settings.BronzeTokens = Form1.BronzeTokens
        My.Settings.Save()
        LBLMatchTokens.Text = Form1.BronzeTokens

        InitializeCards()

        MatchChance = 10
        MatchesMade = 0
        MatchPot = 25

        LBLMatchChance.Text = MatchChance & " Chances Left"

        BTNMatchEasy.Enabled = False
        BTNMatchNormal.Enabled = False
        BTNMatchHard.Enabled = False



        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\CardShuffle.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\CardShuffle.wav"
        End If
        'My.Computer.Audio.Play(Application.StartupPath & "\Audio\System\CardShuffle.wav")

        ShuffleTick = 19
        ShuffleTimer.Start()

    End Sub

    Private Sub BTNMatchHard_Click(sender As System.Object, e As System.EventArgs) Handles BTNMatchHard.Click

        If Form1.BronzeTokens < 1 Then Return

        InitializeCards()

        If MatchList.Count < 1 Then
            MessageBox.Show(Me, "You will need to select at least 1 local image folder or URL File before you can play the Match Game!", "Caution!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If


        EraseCards()
        Form1.BronzeTokens -= 1
        My.Settings.BronzeTokens = Form1.BronzeTokens
        My.Settings.Save()
        LBLMatchTokens.Text = Form1.BronzeTokens

        InitializeCards()

        MatchChance = 7
        MatchesMade = 0
        MatchPot = 100

        LBLMatchChance.Text = MatchChance & " Chances Left"

        BTNMatchEasy.Enabled = False
        BTNMatchNormal.Enabled = False
        BTNMatchHard.Enabled = False



        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\CardShuffle.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\CardShuffle.wav"
        End If
        'My.Computer.Audio.Play(Application.StartupPath & "\Audio\System\CardShuffle.wav")

        ShuffleTick = 19
        ShuffleTimer.Start()


    End Sub

    Public Sub ClearMatchCache()

        Try
            M1A.Image.Dispose()
            M2A.Image.Dispose()
            M3A.Image.Dispose()
            M4A.Image.Dispose()
            M5A.Image.Dispose()
            M6A.Image.Dispose()
        Catch
        End Try

        Try
            M1B.Image.Dispose()
            M2B.Image.Dispose()
            M3B.Image.Dispose()
            M4B.Image.Dispose()
            M5B.Image.Dispose()
            M6B.Image.Dispose()
        Catch
        End Try

        Try
            M1C.Image.Dispose()
            M2C.Image.Dispose()
            M3C.Image.Dispose()
            M4C.Image.Dispose()
            M5C.Image.Dispose()
            M6C.Image.Dispose()
        Catch
        End Try

        Try
            M1A.Image = Nothing
            M2A.Image = Nothing
            M3A.Image = Nothing
            M4A.Image = Nothing
            M5A.Image = Nothing
            M6A.Image = Nothing
        Catch
        End Try

        Try
            M1B.Image = Nothing
            M2B.Image = Nothing
            M3B.Image = Nothing
            M4B.Image = Nothing
            M5B.Image = Nothing
            M6B.Image = Nothing
        Catch
        End Try

        Try
            M1C.Image = Nothing
            M2C.Image = Nothing
            M3C.Image = Nothing
            M4C.Image = Nothing
            M5C.Image = Nothing
            M6C.Image = Nothing
        Catch
        End Try

        Try
            GC.Collect()
        Catch
        End Try

    End Sub

    Public Sub EraseCards()

        M1A.Visible = False
        M2A.Visible = False
        M3A.Visible = False
        M4A.Visible = False
        M5A.Visible = False
        M6A.Visible = False

        M1B.Visible = False
        M2B.Visible = False
        M3B.Visible = False
        M4B.Visible = False
        M5B.Visible = False
        M6B.Visible = False

        M1C.Visible = False
        M2C.Visible = False
        M3C.Visible = False
        M4C.Visible = False
        M5C.Visible = False
        M6C.Visible = False



    End Sub

    Private Sub TCGames_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles TCGames.SelectedIndexChanged

        If TCGames.SelectedIndex <> 0 Then
            ClearSlots()
        End If

        If TCGames.SelectedIndex = 0 Then
            InitializeSlots()
            LBLSlotTokens.Text = Form1.BronzeTokens
        End If

        If TCGames.SelectedIndex <> 1 Then
            ClearMatchCache()
        End If

        If TCGames.SelectedIndex = 1 Then
            InitializeCards()
            LBLMatchTokens.Text = Form1.BronzeTokens
        End If

        If TCGames.SelectedIndex = 2 Then

            If DateTime.Now.ToString("MM/dd/yyyy") <> GetLastTokenTaskStamp().ToString("MM/dd/yyyy") Then
                BTNTokenRequest.Enabled = True
            Else
                BTNTokenRequest.Enabled = False
            End If

            ClearExchange()
            CheckExchange()

            If File.Exists(My.Settings.CardBack) Then
                Try
                    BoosterBack.Image.Dispose()
                    BoosterBack.Image = Nothing
                    GC.Collect()
                Catch
                End Try

                BoosterBack.Load(My.Settings.CardBack)
            Else
                BoosterBack.Load(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Games\_CardBackPicture.png")
            End If
            LBLExchangeBronze.Text = Form1.BronzeTokens
            LBLExchangeSilver.Text = Form1.SilverTokens
            LBLExchangeGold.Text = Form1.GoldTokens
        End If

        If TCGames.SelectedIndex = 3 Then

        

            BronzeQ1.Text = My.Settings.B1
            BronzeQ2.Text = My.Settings.B2
            BronzeQ3.Text = My.Settings.B3
            BronzeQ4.Text = My.Settings.B4
            BronzeQ5.Text = My.Settings.B5
            BronzeQ6.Text = My.Settings.B6

            SilverQ1.Text = My.Settings.S1
            SilverQ2.Text = My.Settings.S2
            SilverQ3.Text = My.Settings.S3
            SilverQ4.Text = My.Settings.S4
            SilverQ5.Text = My.Settings.S5
            SilverQ6.Text = My.Settings.S6

            GoldQ1.Text = My.Settings.G1
            GoldQ2.Text = My.Settings.G2
            GoldQ3.Text = My.Settings.G3
            GoldQ4.Text = My.Settings.G4
            GoldQ5.Text = My.Settings.G5
            GoldQ6.Text = My.Settings.G6

            If GoldQ1.Text <> 0 Then
                GoldN1.Text = FrmSettings.GN1.Text
                GoldP1.Load(FrmSettings.GP1.ImageLocation)
                GoldP1.Visible = True
            Else
                GoldP1.Visible = False
                GoldN1.Text = ""
            End If

            If GoldQ2.Text <> 0 Then
                GoldN2.Text = FrmSettings.GN2.Text
                GoldP2.Load(FrmSettings.GP2.ImageLocation)
                GoldP2.Visible = True
            Else
                GoldP2.Visible = False
                GoldN2.Text = ""
            End If

            If GoldQ3.Text <> 0 Then
                GoldN3.Text = FrmSettings.GN3.Text
                GoldP3.Load(FrmSettings.GP3.ImageLocation)
                GoldP3.Visible = True
            Else
                GoldP3.Visible = False
                GoldN3.Text = ""
            End If

            If GoldQ4.Text <> 0 Then
                GoldN4.Text = FrmSettings.GN4.Text
                GoldP4.Load(FrmSettings.GP4.ImageLocation)
                GoldP4.Visible = True
            Else
                GoldP4.Visible = False
                GoldN4.Text = ""
            End If

            If GoldQ5.Text <> 0 Then
                GoldN5.Text = FrmSettings.GN5.Text
                GoldP5.Load(FrmSettings.GP5.ImageLocation)
                GoldP5.Visible = True
            Else
                GoldP5.Visible = False
                GoldN5.Text = ""
            End If

            If GoldQ6.Text <> 0 Then
                GoldN6.Text = FrmSettings.GN6.Text
                GoldP6.Load(FrmSettings.GP6.ImageLocation)
                GoldP6.Visible = True
            Else
                GoldP6.Visible = False
                GoldN6.Text = ""
            End If


            If SilverQ1.Text <> 0 Then
                SilverN1.Text = FrmSettings.SN1.Text
                SilverP1.Load(FrmSettings.SP1.ImageLocation)
                SilverP1.Visible = True
            Else
                SilverP1.Visible = False
                SilverN1.Text = ""
            End If

            If SilverQ2.Text <> 0 Then
                SilverN2.Text = FrmSettings.SN2.Text
                SilverP2.Load(FrmSettings.SP2.ImageLocation)
                SilverP2.Visible = True
            Else
                SilverP2.Visible = False
                SilverN2.Text = ""
            End If

            If SilverQ3.Text <> 0 Then
                SilverN3.Text = FrmSettings.SN3.Text
                SilverP3.Load(FrmSettings.SP3.ImageLocation)
                SilverP3.Visible = True
            Else
                SilverP3.Visible = False
                SilverN3.Text = ""
            End If

            If SilverQ4.Text <> 0 Then
                SilverN4.Text = FrmSettings.SN4.Text
                SilverP4.Load(FrmSettings.SP4.ImageLocation)
                SilverP4.Visible = True
            Else
                SilverP4.Visible = False
                SilverN4.Text = ""
            End If

            If SilverQ5.Text <> 0 Then
                SilverN5.Text = FrmSettings.SN5.Text
                SilverP5.Load(FrmSettings.SP5.ImageLocation)
                SilverP5.Visible = True
            Else
                SilverP5.Visible = False
                SilverN5.Text = ""
            End If

            If SilverQ6.Text <> 0 Then
                SilverN6.Text = FrmSettings.SN6.Text
                SilverP6.Load(FrmSettings.SP6.ImageLocation)
                SilverP6.Visible = True
            Else
                SilverP6.Visible = False
                SilverN6.Text = ""
            End If


            If BronzeQ1.Text <> 0 Then
                BronzeN1.Text = FrmSettings.BN1.Text
                BronzeP1.Load(FrmSettings.BP1.ImageLocation)
                BronzeP1.Visible = True
            Else
                BronzeP1.Visible = False
                BronzeN1.Text = ""
            End If

            If BronzeQ2.Text <> 0 Then
                BronzeN2.Text = FrmSettings.BN2.Text
                BronzeP2.Load(FrmSettings.BP2.ImageLocation)
                BronzeP2.Visible = True
            Else
                BronzeP2.Visible = False
                BronzeN2.Text = ""
            End If

            If BronzeQ3.Text <> 0 Then
                BronzeN3.Text = FrmSettings.BN3.Text
                BronzeP3.Load(FrmSettings.BP3.ImageLocation)
                BronzeP3.Visible = True
            Else
                BronzeP3.Visible = False
                BronzeN3.Text = ""
            End If

            If BronzeQ4.Text <> 0 Then
                BronzeN4.Text = FrmSettings.BN4.Text
                BronzeP4.Load(FrmSettings.BP4.ImageLocation)
                BronzeP4.Visible = True
            Else
                BronzeP4.Visible = False
                BronzeN4.Text = ""
            End If

            If BronzeQ5.Text <> 0 Then
                BronzeN5.Text = FrmSettings.BN5.Text
                BronzeP5.Load(FrmSettings.BP5.ImageLocation)
                BronzeP5.Visible = True
            Else
                BronzeP5.Visible = False
                BronzeN5.Text = ""
            End If

            If BronzeQ6.Text <> 0 Then
                BronzeN6.Text = FrmSettings.BN6.Text
                BronzeP6.Load(FrmSettings.BP6.ImageLocation)
                BronzeP6.Visible = True
            Else
                BronzeP6.Visible = False
                BronzeN6.Text = ""
            End If







        End If

        

    End Sub




    Private Sub ShuffleTimer_Tick(sender As System.Object, e As System.EventArgs) Handles ShuffleTimer.Tick

        ShuffleTick -= 1

        If ShuffleTick = 18 Then M1A.Visible = True
        If ShuffleTick = 17 Then M2A.Visible = True
        If ShuffleTick = 16 Then M3A.Visible = True
        If ShuffleTick = 15 Then M4A.Visible = True
        If ShuffleTick = 14 Then M5A.Visible = True
        If ShuffleTick = 13 Then M6A.Visible = True

        If ShuffleTick = 12 Then M1B.Visible = True
        If ShuffleTick = 11 Then M2B.Visible = True
        If ShuffleTick = 10 Then M3B.Visible = True
        If ShuffleTick = 9 Then M4B.Visible = True
        If ShuffleTick = 8 Then M5B.Visible = True
        If ShuffleTick = 7 Then M6B.Visible = True

        If ShuffleTick = 6 Then M1C.Visible = True
        If ShuffleTick = 5 Then M2C.Visible = True
        If ShuffleTick = 4 Then M3C.Visible = True
        If ShuffleTick = 3 Then M4C.Visible = True
        If ShuffleTick = 2 Then M5C.Visible = True
        If ShuffleTick = 1 Then M6C.Visible = True

        If ShuffleTick = 0 Then
            GameOn = True
            ShuffleTimer.Stop()
        End If




    End Sub


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles BTNBoosterBuy.Click

        If Form1.BronzeTokens < 25 Then Return

        Form1.BronzeTokens -= 25
        LBLExchangeBronze.Text = Form1.BronzeTokens
        My.Settings.BronzeTokens = Form1.BronzeTokens
        My.Settings.Save()

        BoosterListBronze.Clear()
        BoosterListSilver.Clear()
        BoosterListGold.Clear()

        BoosterListBronze.Add(FrmSettings.BP1.ImageLocation)
        BoosterListBronze.Add(FrmSettings.BP2.ImageLocation)
        BoosterListBronze.Add(FrmSettings.BP3.ImageLocation)
        BoosterListBronze.Add(FrmSettings.BP4.ImageLocation)
        BoosterListBronze.Add(FrmSettings.BP5.ImageLocation)
        BoosterListBronze.Add(FrmSettings.BP6.ImageLocation)

        BoosterListSilver.Add(FrmSettings.SP1.ImageLocation)
        BoosterListSilver.Add(FrmSettings.SP2.ImageLocation)
        BoosterListSilver.Add(FrmSettings.SP3.ImageLocation)
        BoosterListSilver.Add(FrmSettings.SP4.ImageLocation)
        BoosterListSilver.Add(FrmSettings.SP5.ImageLocation)
        BoosterListSilver.Add(FrmSettings.SP6.ImageLocation)

        BoosterListGold.Add(FrmSettings.GP1.ImageLocation)
        BoosterListGold.Add(FrmSettings.GP2.ImageLocation)
        BoosterListGold.Add(FrmSettings.GP3.ImageLocation)
        BoosterListGold.Add(FrmSettings.GP4.ImageLocation)
        BoosterListGold.Add(FrmSettings.GP5.ImageLocation)
        BoosterListGold.Add(FrmSettings.GP6.ImageLocation)

        BoosterTick = 6
        BTNBoosterBuy.Enabled = False

        Booster1.Visible = False
        Booster2.Visible = False
        Booster3.Visible = False
        Booster4.Visible = False
        Booster5.Visible = False

   

        Try
            Booster1.Image.Dispose()
            Booster1.Image = Nothing
        Catch
        End Try

        Try
            Booster2.Image.Dispose()
            Booster2.Image = Nothing
        Catch
        End Try

        Try
            Booster3.Image.Dispose()
            Booster3.Image = Nothing
        Catch
        End Try

        Try
            Booster4.Image.Dispose()
            Booster4.Image = Nothing
        Catch
        End Try

        Try
            Booster5.Image.Dispose()
            Booster5.Image = Nothing
        Catch
        End Try

        Try
            GC.Collect()
        Catch
        End Try

        Booster1Frame.BackColor = Color.DimGray
        Booster1Plate.BackColor = Color.DimGray
        Booster2Frame.BackColor = Color.DimGray
        Booster2Plate.BackColor = Color.DimGray
        Booster3Frame.BackColor = Color.DimGray
        Booster3Plate.BackColor = Color.DimGray
        Booster4Frame.BackColor = Color.DimGray
        Booster4Plate.BackColor = Color.DimGray
        Booster5Frame.BackColor = Color.DimGray
        Booster5Plate.BackColor = Color.DimGray

        Booster1Name.Text = ""
        Booster2Name.Text = ""
        Booster3Name.Text = ""
        Booster4Name.Text = ""
        Booster5Name.Text = ""



        BoosterTimer.Start()
        CheckExchange()



    End Sub


    Private Sub BoosterTimer_Tick(sender As System.Object, e As System.EventArgs) Handles BoosterTimer.Tick

        BoosterTick -= 1
        Dim ColorVal As Integer



        If BoosterTick = 5 Then

            TempVal = randomizer.Next(1, 101)

            If TempVal > 20 Then
                Booster1Frame.BackColor = Color.Peru
                Booster1Plate.BackColor = Color.Peru
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster1.Load(BoosterListBronze(0))
                    Booster1Name.Text = FrmSettings.BN1.Text
                    My.Settings.B1 += 1
                End If
                If ColorVal = 2 Then
                    Booster1.Load(BoosterListBronze(1))
                    Booster1Name.Text = FrmSettings.BN2.Text
                    My.Settings.B2 += 1
                End If
                If ColorVal = 3 Then
                    Booster1.Load(BoosterListBronze(2))
                    Booster1Name.Text = FrmSettings.BN3.Text
                    My.Settings.B3 += 1
                End If
                If ColorVal = 4 Then
                    Booster1.Load(BoosterListBronze(3))
                    Booster1Name.Text = FrmSettings.BN4.Text
                    My.Settings.B4 += 1
                End If
                If ColorVal = 5 Then
                    Booster1.Load(BoosterListBronze(4))
                    Booster1Name.Text = FrmSettings.BN5.Text
                    My.Settings.B5 += 1
                End If
                If ColorVal = 6 Then
                    Booster1.Load(BoosterListBronze(5))
                    Booster1Name.Text = FrmSettings.BN6.Text
                    My.Settings.B6 += 1
                End If
            End If

            If TempVal > 5 And TempVal < 21 Then
                Booster1Frame.BackColor = Color.Silver
                Booster1Plate.BackColor = Color.Silver
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster1.Load(BoosterListSilver(0))
                    Booster1Name.Text = FrmSettings.SN1.Text
                    My.Settings.S1 += 1
                End If
                If ColorVal = 2 Then
                    Booster1.Load(BoosterListSilver(1))
                    Booster1Name.Text = FrmSettings.SN2.Text
                    My.Settings.S2 += 1
                End If
                If ColorVal = 3 Then
                    Booster1.Load(BoosterListSilver(2))
                    Booster1Name.Text = FrmSettings.SN3.Text
                    My.Settings.S3 += 1
                End If
                If ColorVal = 4 Then
                    Booster1.Load(BoosterListSilver(3))
                    Booster1Name.Text = FrmSettings.SN4.Text
                    My.Settings.S4 += 1
                End If
                If ColorVal = 5 Then
                    Booster1.Load(BoosterListSilver(4))
                    Booster1Name.Text = FrmSettings.SN5.Text
                    My.Settings.S5 += 1
                End If
                If ColorVal = 6 Then
                    Booster1.Load(BoosterListSilver(5))
                    Booster1Name.Text = FrmSettings.SN6.Text
                    My.Settings.S6 += 1
                End If
            End If

            If TempVal < 5 Then
                Booster1Frame.BackColor = Color.Gold
                Booster1Plate.BackColor = Color.Gold
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster1.Load(BoosterListGold(0))
                    Booster1Name.Text = FrmSettings.GN1.Text
                    My.Settings.G1 += 1
                End If
                If ColorVal = 2 Then
                    Booster1.Load(BoosterListGold(1))
                    Booster1Name.Text = FrmSettings.GN2.Text
                    My.Settings.G2 += 1
                End If
                If ColorVal = 3 Then
                    Booster1.Load(BoosterListGold(2))
                    Booster1Name.Text = FrmSettings.GN3.Text
                    My.Settings.G3 += 1
                End If
                If ColorVal = 4 Then
                    Booster1.Load(BoosterListGold(3))
                    Booster1Name.Text = FrmSettings.GN4.Text
                    My.Settings.G4 += 1
                End If
                If ColorVal = 5 Then
                    Booster1.Load(BoosterListGold(4))
                    Booster1Name.Text = FrmSettings.GN5.Text
                    My.Settings.G5 += 1
                End If
                If ColorVal = 6 Then
                    Booster1.Load(BoosterListGold(5))
                    Booster1Name.Text = FrmSettings.GN6.Text
                    My.Settings.G6 += 1
                End If
            End If
            Booster1.Visible = True
        End If


        If BoosterTick = 4 Then

            TempVal = randomizer.Next(1, 101)

            If TempVal > 20 Then
                Booster2Frame.BackColor = Color.Peru
                Booster2Plate.BackColor = Color.Peru
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster2.Load(BoosterListBronze(0))
                    Booster2Name.Text = FrmSettings.BN1.Text
                    My.Settings.B1 += 1
                End If
                If ColorVal = 2 Then
                    Booster2.Load(BoosterListBronze(1))
                    Booster2Name.Text = FrmSettings.BN2.Text
                    My.Settings.B2 += 1
                End If
                If ColorVal = 3 Then
                    Booster2.Load(BoosterListBronze(2))
                    Booster2Name.Text = FrmSettings.BN3.Text
                    My.Settings.B3 += 1
                End If
                If ColorVal = 4 Then
                    Booster2.Load(BoosterListBronze(3))
                    Booster2Name.Text = FrmSettings.BN4.Text
                    My.Settings.B4 += 1
                End If
                If ColorVal = 5 Then
                    Booster2.Load(BoosterListBronze(4))
                    Booster2Name.Text = FrmSettings.BN5.Text
                    My.Settings.B5 += 1
                End If
                If ColorVal = 6 Then
                    Booster2.Load(BoosterListBronze(5))
                    Booster2Name.Text = FrmSettings.BN6.Text
                    My.Settings.B6 += 1
                End If
            End If

            If TempVal > 5 And TempVal < 21 Then
                Booster2Frame.BackColor = Color.Silver
                Booster2Plate.BackColor = Color.Silver
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster2.Load(BoosterListSilver(0))
                    Booster2Name.Text = FrmSettings.SN1.Text
                    My.Settings.S1 += 1
                End If
                If ColorVal = 2 Then
                    Booster2.Load(BoosterListSilver(1))
                    Booster2Name.Text = FrmSettings.SN2.Text
                    My.Settings.S2 += 1
                End If
                If ColorVal = 3 Then
                    Booster2.Load(BoosterListSilver(2))
                    Booster2Name.Text = FrmSettings.SN3.Text
                    My.Settings.S3 += 1
                End If
                If ColorVal = 4 Then
                    Booster2.Load(BoosterListSilver(3))
                    Booster2Name.Text = FrmSettings.SN4.Text
                    My.Settings.S4 += 1
                End If
                If ColorVal = 5 Then
                    Booster2.Load(BoosterListSilver(4))
                    Booster2Name.Text = FrmSettings.SN5.Text
                    My.Settings.S5 += 1
                End If
                If ColorVal = 6 Then
                    Booster2.Load(BoosterListSilver(5))
                    Booster2Name.Text = FrmSettings.SN6.Text
                    My.Settings.S6 += 1
                End If
            End If

            If TempVal < 5 Then
                Booster2Frame.BackColor = Color.Gold
                Booster2Plate.BackColor = Color.Gold
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster2.Load(BoosterListGold(0))
                    Booster2Name.Text = FrmSettings.GN1.Text
                    My.Settings.G1 += 1
                End If
                If ColorVal = 2 Then
                    Booster2.Load(BoosterListGold(1))
                    Booster2Name.Text = FrmSettings.GN2.Text
                    My.Settings.G2 += 1
                End If
                If ColorVal = 3 Then
                    Booster2.Load(BoosterListGold(2))
                    Booster2Name.Text = FrmSettings.GN3.Text
                    My.Settings.G3 += 1
                End If
                If ColorVal = 4 Then
                    Booster2.Load(BoosterListGold(3))
                    Booster2Name.Text = FrmSettings.GN4.Text
                    My.Settings.G4 += 1
                End If
                If ColorVal = 5 Then
                    Booster2.Load(BoosterListGold(4))
                    Booster2Name.Text = FrmSettings.GN5.Text
                    My.Settings.G5 += 1
                End If
                If ColorVal = 6 Then
                    Booster2.Load(BoosterListGold(5))
                    Booster2Name.Text = FrmSettings.GN6.Text
                    My.Settings.G6 += 1
                End If
            End If
            Booster2.Visible = True
        End If

        If BoosterTick = 3 Then

            TempVal = randomizer.Next(1, 101)

            If TempVal > 20 Then
                Booster3Frame.BackColor = Color.Peru
                Booster3Plate.BackColor = Color.Peru
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster3.Load(BoosterListBronze(0))
                    Booster3Name.Text = FrmSettings.BN1.Text
                    My.Settings.B1 += 1
                End If
                If ColorVal = 2 Then
                    Booster3.Load(BoosterListBronze(1))
                    Booster3Name.Text = FrmSettings.BN2.Text
                    My.Settings.B2 += 1
                End If
                If ColorVal = 3 Then
                    Booster3.Load(BoosterListBronze(2))
                    Booster3Name.Text = FrmSettings.BN3.Text
                    My.Settings.B3 += 1
                End If
                If ColorVal = 4 Then
                    Booster3.Load(BoosterListBronze(3))
                    Booster3Name.Text = FrmSettings.BN4.Text
                    My.Settings.B4 += 1
                End If
                If ColorVal = 5 Then
                    Booster3.Load(BoosterListBronze(4))
                    Booster3Name.Text = FrmSettings.BN5.Text
                    My.Settings.B5 += 1
                End If
                If ColorVal = 6 Then
                    Booster3.Load(BoosterListBronze(5))
                    Booster3Name.Text = FrmSettings.BN6.Text
                    My.Settings.B6 += 1
                End If
            End If

            If TempVal > 5 And TempVal < 21 Then
                Booster3Frame.BackColor = Color.Silver
                Booster3Plate.BackColor = Color.Silver
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster3.Load(BoosterListSilver(0))
                    Booster3Name.Text = FrmSettings.SN1.Text
                    My.Settings.S1 += 1
                End If
                If ColorVal = 2 Then
                    Booster3.Load(BoosterListSilver(1))
                    Booster3Name.Text = FrmSettings.SN2.Text
                    My.Settings.S2 += 1
                End If
                If ColorVal = 3 Then
                    Booster3.Load(BoosterListSilver(2))
                    Booster3Name.Text = FrmSettings.SN3.Text
                    My.Settings.S3 += 1
                End If
                If ColorVal = 4 Then
                    Booster3.Load(BoosterListSilver(3))
                    Booster3Name.Text = FrmSettings.SN4.Text
                    My.Settings.S4 += 1
                End If
                If ColorVal = 5 Then
                    Booster3.Load(BoosterListSilver(4))
                    Booster3Name.Text = FrmSettings.SN5.Text
                    My.Settings.S5 += 1
                End If
                If ColorVal = 6 Then
                    Booster3.Load(BoosterListSilver(5))
                    Booster3Name.Text = FrmSettings.SN6.Text
                    My.Settings.S6 += 1
                End If
            End If

            If TempVal < 5 Then
                Booster3Frame.BackColor = Color.Gold
                Booster3Plate.BackColor = Color.Gold
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster3.Load(BoosterListGold(0))
                    Booster3Name.Text = FrmSettings.GN1.Text
                    My.Settings.G1 += 1
                End If
                If ColorVal = 2 Then
                    Booster3.Load(BoosterListGold(1))
                    Booster3Name.Text = FrmSettings.GN2.Text
                    My.Settings.G2 += 1
                End If
                If ColorVal = 3 Then
                    Booster3.Load(BoosterListGold(2))
                    Booster3Name.Text = FrmSettings.GN3.Text
                    My.Settings.G3 += 1
                End If
                If ColorVal = 4 Then
                    Booster3.Load(BoosterListGold(3))
                    Booster3Name.Text = FrmSettings.GN4.Text
                    My.Settings.G4 += 1
                End If
                If ColorVal = 5 Then
                    Booster3.Load(BoosterListGold(4))
                    Booster3Name.Text = FrmSettings.GN5.Text
                    My.Settings.G5 += 1
                End If
                If ColorVal = 6 Then
                    Booster3.Load(BoosterListGold(5))
                    Booster3Name.Text = FrmSettings.GN6.Text
                    My.Settings.G6 += 1
                End If
            End If
            Booster3.Visible = True
        End If

        If BoosterTick = 2 Then

            TempVal = randomizer.Next(1, 101)

            If TempVal > 20 Then
                Booster4Frame.BackColor = Color.Peru
                Booster4Plate.BackColor = Color.Peru
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster4.Load(BoosterListBronze(0))
                    Booster4Name.Text = FrmSettings.BN1.Text
                    My.Settings.B1 += 1
                End If
                If ColorVal = 2 Then
                    Booster4.Load(BoosterListBronze(1))
                    Booster4Name.Text = FrmSettings.BN2.Text
                    My.Settings.B2 += 1
                End If
                If ColorVal = 3 Then
                    Booster4.Load(BoosterListBronze(2))
                    Booster4Name.Text = FrmSettings.BN3.Text
                    My.Settings.B3 += 1
                End If
                If ColorVal = 4 Then
                    Booster4.Load(BoosterListBronze(3))
                    Booster4Name.Text = FrmSettings.BN4.Text
                    My.Settings.B4 += 1
                End If
                If ColorVal = 5 Then
                    Booster4.Load(BoosterListBronze(4))
                    Booster4Name.Text = FrmSettings.BN5.Text
                    My.Settings.B5 += 1
                End If
                If ColorVal = 6 Then
                    Booster4.Load(BoosterListBronze(5))
                    Booster4Name.Text = FrmSettings.BN6.Text
                    My.Settings.B6 += 1
                End If
            End If

            If TempVal > 5 And TempVal < 21 Then
                Booster4Frame.BackColor = Color.Silver
                Booster4Plate.BackColor = Color.Silver
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster4.Load(BoosterListSilver(0))
                    Booster4Name.Text = FrmSettings.SN1.Text
                    My.Settings.S1 += 1
                End If
                If ColorVal = 2 Then
                    Booster4.Load(BoosterListSilver(1))
                    Booster4Name.Text = FrmSettings.SN2.Text
                    My.Settings.S2 += 1
                End If
                If ColorVal = 3 Then
                    Booster4.Load(BoosterListSilver(2))
                    Booster4Name.Text = FrmSettings.SN3.Text
                    My.Settings.S3 += 1
                End If
                If ColorVal = 4 Then
                    Booster4.Load(BoosterListSilver(3))
                    Booster4Name.Text = FrmSettings.SN4.Text
                    My.Settings.S4 += 1
                End If
                If ColorVal = 5 Then
                    Booster4.Load(BoosterListSilver(4))
                    Booster4Name.Text = FrmSettings.SN5.Text
                    My.Settings.S5 += 1
                End If
                If ColorVal = 6 Then
                    Booster4.Load(BoosterListSilver(5))
                    Booster4Name.Text = FrmSettings.SN6.Text
                    My.Settings.S6 += 1
                End If
            End If

            If TempVal < 5 Then
                Booster4Frame.BackColor = Color.Gold
                Booster4Plate.BackColor = Color.Gold
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster4.Load(BoosterListGold(0))
                    Booster4Name.Text = FrmSettings.GN1.Text
                    My.Settings.G1 += 1
                End If
                If ColorVal = 2 Then
                    Booster4.Load(BoosterListGold(1))
                    Booster4Name.Text = FrmSettings.GN2.Text
                    My.Settings.G2 += 1
                End If
                If ColorVal = 3 Then
                    Booster4.Load(BoosterListGold(2))
                    Booster4Name.Text = FrmSettings.GN3.Text
                    My.Settings.G3 += 1
                End If
                If ColorVal = 4 Then
                    Booster4.Load(BoosterListGold(3))
                    Booster4Name.Text = FrmSettings.GN4.Text
                    My.Settings.G4 += 1
                End If
                If ColorVal = 5 Then
                    Booster4.Load(BoosterListGold(4))
                    Booster4Name.Text = FrmSettings.GN5.Text
                    My.Settings.G5 += 1
                End If
                If ColorVal = 6 Then
                    Booster4.Load(BoosterListGold(5))
                    Booster4Name.Text = FrmSettings.GN6.Text
                    My.Settings.G6 += 1
                End If
            End If
            Booster4.Visible = True
        End If

        If BoosterTick = 1 Then

            TempVal = randomizer.Next(1, 101)

            If TempVal > 20 Then
                Booster5Frame.BackColor = Color.Peru
                Booster5Plate.BackColor = Color.Peru
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster5.Load(BoosterListBronze(0))
                    Booster5Name.Text = FrmSettings.BN1.Text
                    My.Settings.B1 += 1
                End If
                If ColorVal = 2 Then
                    Booster5.Load(BoosterListBronze(1))
                    Booster5Name.Text = FrmSettings.BN2.Text
                    My.Settings.B2 += 1
                End If
                If ColorVal = 3 Then
                    Booster5.Load(BoosterListBronze(2))
                    Booster5Name.Text = FrmSettings.BN3.Text
                    My.Settings.B3 += 1
                End If
                If ColorVal = 4 Then
                    Booster5.Load(BoosterListBronze(3))
                    Booster5Name.Text = FrmSettings.BN4.Text
                    My.Settings.B4 += 1
                End If
                If ColorVal = 5 Then
                    Booster5.Load(BoosterListBronze(4))
                    Booster5Name.Text = FrmSettings.BN5.Text
                    My.Settings.B5 += 1
                End If
                If ColorVal = 6 Then
                    Booster5.Load(BoosterListBronze(5))
                    Booster5Name.Text = FrmSettings.BN6.Text
                    My.Settings.B6 += 1
                End If
            End If

            If TempVal > 5 And TempVal < 21 Then
                Booster5Frame.BackColor = Color.Silver
                Booster5Plate.BackColor = Color.Silver
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster5.Load(BoosterListSilver(0))
                    Booster5Name.Text = FrmSettings.SN1.Text
                    My.Settings.S1 += 1
                End If
                If ColorVal = 2 Then
                    Booster5.Load(BoosterListSilver(1))
                    Booster5Name.Text = FrmSettings.SN2.Text
                    My.Settings.S2 += 1
                End If
                If ColorVal = 3 Then
                    Booster5.Load(BoosterListSilver(2))
                    Booster5Name.Text = FrmSettings.SN3.Text
                    My.Settings.S3 += 1
                End If
                If ColorVal = 4 Then
                    Booster5.Load(BoosterListSilver(3))
                    Booster5Name.Text = FrmSettings.SN4.Text
                    My.Settings.S4 += 1
                End If
                If ColorVal = 5 Then
                    Booster5.Load(BoosterListSilver(4))
                    Booster5Name.Text = FrmSettings.SN5.Text
                    My.Settings.S5 += 1
                End If
                If ColorVal = 6 Then
                    Booster5.Load(BoosterListSilver(5))
                    Booster5Name.Text = FrmSettings.SN6.Text
                    My.Settings.S6 += 1
                End If
            End If

            If TempVal < 5 Then
                Booster5Frame.BackColor = Color.Gold
                Booster5Plate.BackColor = Color.Gold
                ColorVal = randomizer.Next(1, 7)
                If ColorVal = 1 Then
                    Booster5.Load(BoosterListGold(0))
                    Booster5Name.Text = FrmSettings.GN1.Text
                    My.Settings.G1 += 1
                End If
                If ColorVal = 2 Then
                    Booster5.Load(BoosterListGold(1))
                    Booster5Name.Text = FrmSettings.GN2.Text
                    My.Settings.G2 += 1
                End If
                If ColorVal = 3 Then
                    Booster5.Load(BoosterListGold(2))
                    Booster5Name.Text = FrmSettings.GN3.Text
                    My.Settings.G3 += 1
                End If
                If ColorVal = 4 Then
                    Booster5.Load(BoosterListGold(3))
                    Booster5Name.Text = FrmSettings.GN4.Text
                    My.Settings.G4 += 1
                End If
                If ColorVal = 5 Then
                    Booster5.Load(BoosterListGold(4))
                    Booster5Name.Text = FrmSettings.GN5.Text
                    My.Settings.G5 += 1
                End If
                If ColorVal = 6 Then
                    Booster5.Load(BoosterListGold(5))
                    Booster5Name.Text = FrmSettings.GN6.Text
                    My.Settings.G6 += 1
                End If
            End If
            Booster5.Visible = True
        End If





        My.Settings.Save()
        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\CardFlip.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\CardFlip.wav"
        End If
        'My.Computer.Audio.Play(Application.StartupPath & "\Audio\System\CardFlip.wav")

        If BoosterTick = 1 Then
            BoosterTimer.Stop()
            CheckExchange()
        End If


    End Sub

    Public Sub UpdateBronzeTokens()

        LBLSlotTokens.Text = Form1.BronzeTokens
        LBLMatchTokens.Text = Form1.BronzeTokens
        LBLExchangeBronze.Text = Form1.BronzeTokens

    End Sub

  
   

    Public Sub CheckExchange()

        If My.Settings.B1 > 0 And My.Settings.B2 > 0 And My.Settings.B3 > 0 And My.Settings.B4 > 0 And My.Settings.B5 > 0 And My.Settings.B6 > 0 Then
            BTNExchange1.Enabled = True
            BTNExchange2.Enabled = True
        Else
            BTNExchange1.Enabled = False
            BTNExchange2.Enabled = False
        End If

        If My.Settings.S1 > 0 And My.Settings.S2 > 0 And My.Settings.S3 > 0 And My.Settings.S4 > 0 And My.Settings.S5 > 0 And My.Settings.S6 > 0 Then
            BTNExchange3.Enabled = True
            BTNExchange4.Enabled = True
        Else
            BTNExchange3.Enabled = False
            BTNExchange4.Enabled = False
        End If

        If My.Settings.G1 > 0 And My.Settings.G2 > 0 And My.Settings.G3 > 0 And My.Settings.G4 > 0 And My.Settings.G5 > 0 And My.Settings.G6 > 0 Then
            BTNExchange5.Enabled = True
        Else
            BTNExchange5.Enabled = False
        End If

        If Form1.SilverTokens > 0 Then
            BTNExchange6.Enabled = True
        Else
            BTNExchange6.Enabled = False
        End If

        If Form1.BronzeTokens > 24 And BoosterTimer.Enabled = False Then
            BTNBoosterBuy.Enabled = True
        Else
            BTNBoosterBuy.Enabled = False
        End If

    End Sub

    Public Sub ClearExchange()

        ExchangeCard.Visible = False
        ExchangeFrame.BackColor = Color.DimGray
        ExchangePlate.BackColor = Color.DimGray
        LBLExchange.Text = ""
        ExchangeName.Text = ""


    End Sub

    Private Sub BTNExchange1_Click(sender As System.Object, e As System.EventArgs) Handles BTNExchange1.Click


        My.Settings.B1 -= 1
        My.Settings.B2 -= 1
        My.Settings.B3 -= 1
        My.Settings.B4 -= 1
        My.Settings.B5 -= 1
        My.Settings.B6 -= 1

        Dim SilverDraw As New List(Of String)
        SilverDraw.Clear()

        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\CardFlip.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\CardFlip.wav"
        End If

        ExchangeCard.Visible = True
        ExchangeFrame.BackColor = Color.Silver
        ExchangePlate.BackColor = Color.Silver
        LBLExchange.Text = "You've received a Silver card!"

        SilverDraw.Add(FrmSettings.SP1.ImageLocation)
        SilverDraw.Add(FrmSettings.SP2.ImageLocation)
        SilverDraw.Add(FrmSettings.SP3.ImageLocation)
        SilverDraw.Add(FrmSettings.SP4.ImageLocation)
        SilverDraw.Add(FrmSettings.SP5.ImageLocation)
        SilverDraw.Add(FrmSettings.SP6.ImageLocation)

        TempVal = randomizer.Next(1, 7)

        If TempVal = 1 Then
            My.Settings.S1 += 1
            ExchangeCard.Load(SilverDraw(0))
            ExchangeName.Text = FrmSettings.SN1.Text
        End If

        If TempVal = 2 Then
            My.Settings.S2 += 1
            ExchangeCard.Load(SilverDraw(1))
            ExchangeName.Text = FrmSettings.SN2.Text
        End If

        If TempVal = 3 Then
            My.Settings.S3 += 1
            ExchangeCard.Load(SilverDraw(2))
            ExchangeName.Text = FrmSettings.SN3.Text
        End If

        If TempVal = 4 Then
            My.Settings.S4 += 1
            ExchangeCard.Load(SilverDraw(3))
            ExchangeName.Text = FrmSettings.SN4.Text
        End If

        If TempVal = 5 Then
            My.Settings.S5 += 1
            ExchangeCard.Load(SilverDraw(4))
            ExchangeName.Text = FrmSettings.SN5.Text
        End If

        If TempVal = 6 Then
            My.Settings.S6 += 1
            ExchangeCard.Load(SilverDraw(5))
            ExchangeName.Text = FrmSettings.SN6.Text
        End If

        My.Settings.Save()

        CheckExchange()

       
    End Sub

    Private Sub BTNExchange2_Click(sender As System.Object, e As System.EventArgs) Handles BTNExchange2.Click


        My.Settings.B1 -= 1
        My.Settings.B2 -= 1
        My.Settings.B3 -= 1
        My.Settings.B4 -= 1
        My.Settings.B5 -= 1
        My.Settings.B6 -= 1

        ClearExchange()

        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\PayoutSmall.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\PayoutSmall.wav"
        End If

        Form1.BronzeTokens += 12
        My.Settings.BronzeTokens = Form1.BronzeTokens
        LBLExchangeBronze.Text = Form1.BronzeTokens



        My.Settings.Save()

        CheckExchange()


    End Sub

    Private Sub BTNExchange3_Click(sender As System.Object, e As System.EventArgs) Handles BTNExchange3.Click

        My.Settings.S1 -= 1
        My.Settings.S2 -= 1
        My.Settings.S3 -= 1
        My.Settings.S4 -= 1
        My.Settings.S5 -= 1
        My.Settings.S6 -= 1

        Dim GoldDraw As New List(Of String)
        GoldDraw.Clear()

        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\CardFlip.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\CardFlip.wav"
        End If

        ExchangeCard.Visible = True
        ExchangeFrame.BackColor = Color.Gold
        ExchangePlate.BackColor = Color.Gold
        LBLExchange.Text = "You've received a Gold card!"

        GoldDraw.Add(FrmSettings.GP1.ImageLocation)
        GoldDraw.Add(FrmSettings.GP2.ImageLocation)
        GoldDraw.Add(FrmSettings.GP3.ImageLocation)
        GoldDraw.Add(FrmSettings.GP4.ImageLocation)
        GoldDraw.Add(FrmSettings.GP5.ImageLocation)
        GoldDraw.Add(FrmSettings.GP6.ImageLocation)

        TempVal = randomizer.Next(1, 7)

        If TempVal = 1 Then
            My.Settings.G1 += 1
            ExchangeCard.Load(GoldDraw(0))
            ExchangeName.Text = FrmSettings.GN1.Text
        End If

        If TempVal = 2 Then
            My.Settings.G2 += 1
            ExchangeCard.Load(GoldDraw(1))
            ExchangeName.Text = FrmSettings.GN2.Text
        End If

        If TempVal = 3 Then
            My.Settings.G3 += 1
            ExchangeCard.Load(GoldDraw(2))
            ExchangeName.Text = FrmSettings.GN3.Text
        End If

        If TempVal = 4 Then
            My.Settings.G4 += 1
            ExchangeCard.Load(GoldDraw(3))
            ExchangeName.Text = FrmSettings.GN4.Text
        End If

        If TempVal = 5 Then
            My.Settings.G5 += 1
            ExchangeCard.Load(GoldDraw(4))
            ExchangeName.Text = FrmSettings.GN5.Text
        End If

        If TempVal = 6 Then
            My.Settings.G6 += 1
            ExchangeCard.Load(GoldDraw(5))
            ExchangeName.Text = FrmSettings.GN6.Text
        End If

        My.Settings.Save()

        CheckExchange()



    End Sub

    Private Sub BTNExchange4_Click(sender As System.Object, e As System.EventArgs) Handles BTNExchange4.Click

        My.Settings.S1 -= 1
        My.Settings.S2 -= 1
        My.Settings.S3 -= 1
        My.Settings.S4 -= 1
        My.Settings.S5 -= 1
        My.Settings.S6 -= 1

        ClearExchange()

        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\PayoutOne.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\PayoutOne.wav"
        End If

        Form1.SilverTokens += 1
        My.Settings.SilverTokens = Form1.SilverTokens
        LBLExchangeSilver.Text = Form1.SilverTokens



        My.Settings.Save()

        CheckExchange()


    End Sub

    Private Sub BTNExchange5_Click(sender As System.Object, e As System.EventArgs) Handles BTNExchange5.Click

        My.Settings.G1 -= 1
        My.Settings.G2 -= 1
        My.Settings.G3 -= 1
        My.Settings.G4 -= 1
        My.Settings.G5 -= 1
        My.Settings.G6 -= 1

        ClearExchange()

        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\PayoutOne.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\PayoutOne.wav"
        End If

        Form1.GoldTokens += 1
        My.Settings.GoldTokens = Form1.GoldTokens
        LBLExchangeGold.Text = Form1.GoldTokens



        My.Settings.Save()

        CheckExchange()

    End Sub

    Private Sub BTNExchange6_Click(sender As System.Object, e As System.EventArgs) Handles BTNExchange6.Click

        ClearExchange()

        If FrmSettings.CBGameSounds.Checked = True And File.Exists(Application.StartupPath & "\Audio\System\PayoutSmall.wav") Then
            GameWMP.settings.setMode("loop", False)
            GameWMP.settings.volume = 20
            GameWMP.URL = Application.StartupPath & "\Audio\System\PayoutSmall.wav"
        End If

        Form1.SilverTokens -= 1
        Form1.BronzeTokens += 50
        My.Settings.BronzeTokens = Form1.BronzeTokens
        My.Settings.BronzeTokens = Form1.BronzeTokens

        LBLExchangeBronze.Text = Form1.BronzeTokens
        LBLExchangeSilver.Text = Form1.SilverTokens

        My.Settings.Save()

        CheckExchange()

    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles BTNTokenRequest.Click

        If DateTime.Now.ToString("MM/dd/yyyy") <> GetLastTokenTaskStamp().ToString("MM/dd/yyyy") Then
            System.IO.File.WriteAllText(Application.StartupPath & "\System\TokenTasks", DateString)

            Dim TokenList As New List(Of String)
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\Scripts\" & FrmSettings.dompersonalityComboBox.Text & "\Apps\Games\Token Tasks\", FileIO.SearchOption.SearchAllSubDirectories, "*.txt")
                TokenList.Add(foundFile)
            Next
            If TokenList.Count > 0 Then

                Form1.SaidHello = True
                Form1.ShowModule = True
                Form1.FileText = TokenList(randomizer.Next(0, TokenList.Count))
                Form1.StrokeTauntVal = -1
                Form1.ScriptTick = 2
                Form1.ScriptTimer.Start()

                BTNTokenRequest.Enabled = False
            Else
                MessageBox.Show(Me, "No tasks found in Token Tasks folder!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand)
                Return
            End If
        End If










    End Sub


    Private Function GetLastTokenTaskStamp() As DateTime
        Try
            Dim lsts As String = System.IO.File.ReadAllText(Application.StartupPath & "\System\TokenTasks")
            Return Date.ParseExact(lsts.Trim, "MM-dd-yyyy", CultureInfo.InvariantCulture)
            Debug.Print(lsts.Trim & " Worked")
        Catch
            'Return DateTime.Now
        End Try
    End Function

    Private Sub Button1_Click_2(sender As System.Object, e As System.EventArgs)

        Form1.BronzeTokens += 50
        My.Settings.BronzeTokens = Form1.BronzeTokens

        My.Settings.Save()

        LBLExchangeBronze.Text = Form1.BronzeTokens
        BTNBoosterBuy.Enabled = True

    End Sub
End Class