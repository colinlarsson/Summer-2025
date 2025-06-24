'Abbvie | Dynamic time trigger and gravimetric response feed | JDL CL 03-13-2025
'Version 4.09 Working on 03-27-2025 
'Version for testing!

'A = Feed B volumetric
'B = Base not controlled
'C = Feed A gravimetric
'D = Glucose gravimetric

'*************************************************************************************************************
'Feed A 
Dim MajorFeed_Fed as Double '[g]
Dim glucoseFeed_Fed as Double '[g]

Dim Scale_Weight(6) as Double 
Scale_Weight(1) = p.MAPV 'time zero bottle weight 
Scale_Weight(2) = p.MAPV 'interval starting bottleweight 
Scale_Weight(3) = p.MAPV 'current weight feed A
Scale_Weight(4) = p.MAPV 'glucose interval starting bottleweight
Scale_Weight(5) = p.MAPV 'current weight glucose

'Process Day
Dim procDay As Integer 'Day used for logmessages and acknowledge glucose sample
procDay = p.offlineR 'int(p.inoculationTime_H / 24)

Dim Active_Feed_TimePoint(1) as Double 
Active_Feed_TimePoint(1) = 0

Dim Feed_StartTime as Double = 0

'All Feed Times in hours
Dim Feed_Time(13) as Double 

'Major Feed active target weight 
Dim Major_Feed_TargetWeight as Double 

'Major Feed - pump C
Dim Major_Feed_TargetWeight_Value(13) as Double 

'Feed B active target volume
Dim FeedB_VolumeTarget as Double = 0

'Feed B Target Volumes - pump A
Dim FeedB_VolumeTarget_Value(13) as Double 

Feed_Time(1) = 5/60
Feed_Time(2) = 10/60
Feed_Time(3) = 20/60
Feed_Time(4) = 60
Feed_Time(5) = 100
Feed_Time(6) = 204
Feed_Time(7) = 228
Feed_Time(8) = 252
Feed_Time(9) = 500
Feed_Time(10) = 501
Feed_Time(11) = 502
Feed_Time(12) = 503

Major_Feed_TargetWeight_Value(1) = 20
Major_Feed_TargetWeight_Value(2) = 25
Major_Feed_TargetWeight_Value(3) = 0
Major_Feed_TargetWeight_Value(4) = 80
Major_Feed_TargetWeight_Value(5) = 96.3
Major_Feed_TargetWeight_Value(6) = 96.3
Major_Feed_TargetWeight_Value(7) = 96.3
Major_Feed_TargetWeight_Value(8) = 96.3
Major_Feed_TargetWeight_Value(9) = 96.3
Major_Feed_TargetWeight_Value(10) = 10
Major_Feed_TargetWeight_Value(11) = 11
Major_Feed_TargetWeight_Value(12) = 12


FeedB_VolumeTarget_Value(1) = 1.2
FeedB_VolumeTarget_Value(2) = 1.1
FeedB_VolumeTarget_Value(3) = 0
FeedB_VolumeTarget_Value(4) = 1
FeedB_VolumeTarget_Value(5) = 3
FeedB_VolumeTarget_Value(6) = 0.0  
FeedB_VolumeTarget_Value(7) = 0.0
FeedB_VolumeTarget_Value(8) = 0.0
FeedB_VolumeTarget_Value(9) = 0.0
FeedB_VolumeTarget_Value(10) = 0.0
FeedB_VolumeTarget_Value(11) = 0.0
FeedB_VolumeTarget_Value(12) = 0.0


'Major Feed SetPoint (SP) variables
Dim Major_Feed_SP as Double = 400 '[mL/H] - starting pump flow setpoint 
Dim Major_Feed_Percent_Value as Double = 0.25 'reduce pump flow rate to 25% of SP 
Dim Major_Feed_SP_percent as Double = Major_Feed_SP * Major_Feed_Percent_Value  'stores the new reduced pump flow sp
Dim cut_pumpValue_Percent as Double = 0.75 'slow down when reached 75% of target value 

Dim Major_Feed_Counter as Double = 0

'Glucose feed 
Dim Glucose_Feed_SP as Double = 300
Dim Glucose_Feed_Percent_Value as Double = 0.25 'reduce pump flow rate to 25% of SP 
Dim Glucose_Feed_SP_percent as Double = Glucose_Feed_SP * Glucose_Feed_Percent_Value  'stores the new reduced pump flow sp
Dim Glucose_Cut_pumpValue_Percent as Double = 0.75 'slow down when reached 75% of target value 

'Feed B 
Dim FeedB_FlowRate as Double = 15 '[mL/H]
Dim FeedB_FCal as Double = 105 '[1/mL]
Dim FeedB_Counter as Double = 0 '#
Dim FeedB_CounterIncrement as Double = 0
Dim FeedB_Totalizer as Double = 0'[mL]
Dim FeedB_Totalizer_Count as Double = 0 

'Define array - indexing starts at 0.
If s is Nothing then
Dim a(22) as Double
a(1) = 0 'time zero bottle weight 
a(2) = 0 'interval starting bottleweight 
a(3) = 0 'current weight 
a(4) = Major_Feed_TargetWeight
a(5) = Major_Feed_Counter
a(6) = Feed_StartTime 'initialized to 0
a(7) = FeedB_Counter 'initialized to 0
a(8) = FeedB_Totalizer 'initialized to 0
a(9) = FeedB_VolumeTarget 'initialized to 0
a(10) = 0 'Glucose interval starting bottleweight
a(11) = 0 'current weight
a(12) = 0 'user input glucose
s = a
End If

'*************************************************************************************************************
'Tornado Ramen Script 
'Dim GlucoseReading as Double = p.ExtE 
'Dim GlucoseTarget as Double = 1 '[g/L]
'Dim GlucosePumpSP as Double = 10 '[mL/H]

'If GlucoseReading < GlucoseTarget Then 
'    p.pumpDActive = True 
'    p.FDSP = GlucosePumpSP '[mL/H]
'ElseIf GlucoseReading > GlucoseTarget Then 
 '   p.pumpDActive = False
'End If 

'*************************************************************************************************************

If P isNot Nothing Then
With P 

    MajorFeed_Fed = (s(2) - s(3)) '
    glucoseFeed_Fed = (s(10) - s(11)) '

    .intA = .phase 'phase
    .intB = .runtime_H - .phaseStart_H 'phase time 
    .intC = s(5) 'Feed Counter
    .intD = procDay 'feed b counter 
    .intE = s(4) 'major feed target weight  
    .intF = s(2) - s(3) 'major feed fed 
    .intG = s(12) 'glucose target
    .intH = s(10) - s(11) 'glucose fed
    .intI = s(9) + s(8) 'feed b volume target
    .IntJ = s(1) 'static 
    .intK = s(2) 'static interval feed
    .intL = s(3) 'dynamic interval
    .intQ = procDay
    .intS = s(9)
    .intT = s(6) - .inoculationTime_H 'time to feed in hours

Select Case .phase
    Case 0 'Workflow Start 
        .logmessage("██████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████")
        .logmessage("██████████████-----AUTOFEED v.4.08 LAUNCHING...-------------------------------------------------------")
        .logmessage("██████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████")
        .logWarning("THIS VERSION IS FOR SIMULATION PURPOSES, DO NOT USE")
        .phase = .phase + 1

    Case 1 'Start Inoculation Timer  
    's(1) = Scale_Weight(1) 's(0) = MAPV0 'time zero bottlweight (before inoculation timer starts)

    If.InoculationTime_H > 0 and (.runtime_H - .phaseStart_H) > 0.1/60 Then
        .logmessage("███████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████")
        .logmessage("██████████████----AUTOMATED FEEDING INITIALIZED FOR UNIT#: " & .unit & ".----")
        .logmessage("███████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████")
        .phase = .phase + 1
    End If
    
    Case 2
        .pumpCActive = False 
        .pumpDActive = False 
        .pumpAActive = False
        .VCSP = 0 'reset pump major feed totalizer
        .VDSP = 0 'reset pump glucose totalizer

        If .OfflineM = 0 Then 'Update Counters Manually, M: 1 enter reset state, N: number of steps to skip, O: step up 1. M will intercept after every feed 
            If.InoculationTime_H > Feed_Time(1) and (.runtime_H - .phaseStart_H) > 0.1/60 Then 'waits for first feed time + 6s delay; 
                s(5) = s(5) + 1 'Major Feed Counter
                s(7) = s(7) + 1 'Feed B Counter
                .phase = .phase + 1
            End If

        Else If .OfflineM = 1 and .OfflineN > 0 and .OfflineO = 0 Then 
            s(5) = .OfflineN + s(5) + 1
            s(7) = .OfflineN + s(7) + 1
            .phase = .phase + 1
        ElseIf .OfflineM = 1 and .OfflineN > 0 and .OfflineO = 1 Then 
            s(5) = s(5) + 1 'Major Feed Counter
            s(7) = s(7) + 1 'Feed B Counter    
            .phase = .phase + 1
        End If 

    Case 3 'turn off pump and assign new values for interval time and specified target weight to feed
        
    'major feed - pump C
        If s(5) = 1 Then 
            Feed_StartTime = Feed_Time(1) 's(6)
            Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(1)
            'FeedB_Counter = 0

    ElseIf s(5) = 2 Then 
        Feed_StartTime = Feed_Time(2) 
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(2)
        'FeedB_Counter = 1

    ElseIf s(5) = 3 Then ' 
        Feed_StartTime = Feed_Time(3)
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(3)
       ' FeedB_Counter = 2
    ElseIf s(5) = 4
        Feed_StartTime = Feed_Time(4)
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(4)
       ' FeedB_Counter = 3    

    ElseIf s(5) = 5
        Feed_StartTime = Feed_Time(5)
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(5)
      '  FeedB_Counter = 4    

    ElseIf s(5) = 6
        Feed_StartTime = Feed_Time(6)
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(6)
       ' FeedB_Counter = 5    
    ElseIf s(5) = 7 Then 
        Feed_StartTime = Feed_Time(7) 
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(7)
        'FeedB_Counter = 6  
    ElseIf s(5) = 8 Then ' 
        Feed_StartTime = Feed_Time(8)
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(8)
       ' FeedB_Counter = 7  
    ElseIf s(5) = 9
        Feed_StartTime = Feed_Time(9)
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(9)
       ' FeedB_Counter = 8    

    ElseIf s(5) = 10
        Feed_StartTime = Feed_Time(10)
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(10)
      '  FeedB_Counter = 9    

    ElseIf s(5) = 11
        Feed_StartTime = Feed_Time(11)
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(11)
       ' FeedB_Counter = 10   
    ElseIf s(5) = 12
        Feed_StartTime = Feed_Time(12)
        Major_Feed_TargetWeight = Major_Feed_TargetWeight_Value(12)
       ' FeedB_Counter = 11    
    End If 

'Feed B | s(7) = feed B counter, s(8) = pump D totalizer, s(9) = Feed B volume target
    If s(7) = 1 Then 
        FeedB_Totalizer = .VDPV
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(1)
    ElseIf s(7) = 2
        FeedB_Totalizer = .VDPV    
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(2)
    ElseIf s(7) = 3
        FeedB_Totalizer = .VDPV          
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(3)
    ElseIf s(7) = 4
        FeedB_Totalizer = .VDPV  
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(4)
    ElseIf s(7) = 5
        FeedB_Totalizer = .VDPV  
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(5)
    ElseIf s(7) = 6
        FeedB_Totalizer = .VDPV  
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(6)
    ElseIf s(7) = 7
        FeedB_Totalizer = .VDPV    
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(7)
    ElseIf s(7) = 8
        FeedB_Totalizer = .VDPV          
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(8)
    ElseIf s(7) = 9
        FeedB_Totalizer = .VDPV  
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(9)
    ElseIf s(7) = 10
        FeedB_Totalizer = .VDPV  
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(10)
    ElseIf s(7) = 11
        FeedB_Totalizer = .VDPV  
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(11)
    ElseIf s(7) = 12
        FeedB_Totalizer = .VDPV  
        FeedB_VolumeTarget = FeedB_VolumeTarget_Value(12)
    End If 

    'Major Feed
    s(4) = Major_Feed_TargetWeight
    s(6) = Feed_StartTime
    
    'feedB
    s(8) = FeedB_Totalizer
    s(9) = FeedB_VolumeTarget

    If procDay = 0 Then ' First decision tree, skips to feed b on day O
        .phase = .phase + 2
    ElseIf .offlineA <> procDay and .inoculationTime_H < (s(6) + 8) and ((.runtime_H - .PhaseStart_H) > 0.1/60)  'Waiting for offline D to confirm sample
        .logmessage("█████████████████████--Phase 3: Day# " & procDay & " UNIT#: " & .unit & ".--███████████████████████████████████████████████████")	
        .logmessage("██--------------Enter Glucose Target in Offline Field, then set Day to the current Process Day.")
        .logmessage("██████████████████████████████████████████████████████████████████████████████████████████████████████████████████")
        .phase = .phase + 1
    ElseIf .offlineA = procDay and .inoculationTime_H > s(6) 'in case it was set beforehand
        .logmessage("█████████████████████--Phase 3: Day# " & procDay & " UNIT#: " & .unit & ".--██████████████████████████████████████████████████████████████████████")
        .logmessage("██--------------Sampling Confirmed: Next Feeding Event in " & formatNumber(.intT, 2) & " Hrs")
        .logmessage("██--------------Feed Targets: " & s(9) & " [mL] FB. " & S(4) & " [g]. FA and " & .offlineB & " [g] Glucose.")
        .logmessage("████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████▌")
        .phase = .phase + 2
    End If

    Case 4 'waiting for sample confirmation
        If .offlineA <> procDay and .inoculationTime_H > (s(6) + 8)
            .logwarning("Phase 9: Sample not verified, feeding late, no glucose entered")
            .phase = .phase + 1
        ElseIf .offlineA = procDay and .inoculationTime_H > s(6)'
            .logmessage("█████████████████████--Phase 4: Day# " & procDay & " UNIT#: " & .unit & ".--██████████████████████████████████████████████████████████████████████")
            .logmessage("█▌--------------Sampling Confirmed: Next Feeding Event in " & .intT & " Hrs.")
            .logmessage("█▌--------------Feed Targets: " & s(9) & " [mL] of Feed B. " & S(4) & " [g] of Feed A and " & .offlineB & " of [g] Glucose.")
            .logmessage("████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████▌")
            .phase = .phase + 1
        End If

    Case 5 'Waiting for feed B to start
    If ((.runtime_H - .PhaseStart_H) > 0.1/60) and .inoculationTime_H > s(6) and s(9) <> 0 Then '6 second delay to ensure values get stored to array. also accounts for hiccups in DWC, checks for fb entry
        .logmessage("█████████████████████--Phase 5: Day# " & procDay & " UNIT#: " & .unit & ".██████████████████████████████████████████████████████████████████████")
        .logmessage("██--------------Feed B Starting: " & s(9) & " [g] to be fed.")
        .logmessage("██████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████▌")
        .phase = .phase + 1
    ElseIf ((.runtime_H - .PhaseStart_H) > 0.1/60) and .inoculationTime_H > s(6) and s(9) > 0 
        .logmessage("█████████████████████--Phase 5: Day# " & procDay & " UNIT#: " & .unit & ". ██████████████████████████████████████████████████████████████████████")
        .logmessage("██--------------No Feed B To be fed, skipping to Feed A.")
        .logmessage("████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████")
        .phase = .phase + 2 'goes to phase 7
    End If

        
    Case 6 'turn on pump
    If .inoculationTime_H > s(6) Then 
        .pumpDActive = True 'turn feed B on 
    End If 

    If s(7) <= 12 Then 'checks if above max feeds -probably not necessary
        If .VDPV < s(8) + s(9) Then
            .FDSP = FeedB_FlowRate
            .FDCal = FeedB_FCal
        Else 'ends when totalize hits value
        .logWARNING("Phase 4: Day - " & procDay & " Feed B Finished: " & s(9) & " [g] Fed")
            .phase = .phase + 1
        End If 
    ElseIf s(7) > 12 and ((.runtime_H - .PhaseStart_H) > 0.1/60) Then '6 second delay to ensure values get stored to array. also accounts for hiccups in DWC
        .phase = 2 'finish him 
    End If 

    Case 7 '        
        .pumpDActive = False 'feed B off
        s(1) = Scale_Weight(1) 'static weight after feed B is finished 

        If .inoculationTime_H > Feed_Time(12) and s(4) > Major_Feed_TargetWeight_Value(12) Then 'stops loop after the final time and feed
            .phase = 2 'finish him 
        Else
            .phase = .phase + 1 'continue loop 
        End If 
    
    Case 8 'Record MajorA phase bottleweight 
        s(2) = Scale_Weight(2) 'Static of Interval scale weight 
        If.InoculationTime_H > s(6) and s(4) <> 0 and (.runtime_H - .phaseStart_H) > 0.1/60 Then 'waits for respective feed time + 6s delay; 
            .phase = .phase + 1
	    ElseIf s(4) = 0
        .logmessage("█████████████████████--Phase 8: Day# " & procDay & " UNIT#: " & .unit & ".--██████████████████████████████████████████████████████████████████████")
        .logmessage("██--------------No Feed A, Skipping to Glucose Phase.")
        .logmessage("████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████▌")
		.phase = .phase + 2
        End If

    Case 9 'Feed A runs
        s(3) = Scale_Weight(3) 'dynamic interval scale weight 
        ' put totalizer safety here s(13?) = static totalizer
        .pumpCActive = True 'replace with pump A for feed B 'issue here with offline reset 
        
        If abs(MajorFeed_Fed) > (s(4) * cut_pumpValue_Percent) Then  
            .FCSP = Major_Feed_SP_percent 'reduced pump flow setpoint 
        Else
            .FCSP = Major_Feed_SP 'starting pump flow rate
        End If 'put elseif totalizerdynamic s14? - totalizer static > s4 * 1.3 stop
        
        If abs(MajorFeed_Fed) > s(4) and (.runtime_H - .phaseStart_H) > 0.1/60 Then '(s(1) - s(2)) > s(12)
            .pumpCActive = False
            .Logwarning("Feed A Fed on Day " & procDay & ":  " & formatNumber(MajorFeed_Fed, 2) & " [g]")
            .phase = .phase + 1
        ElseIf .FCPV > (1.2 * s(4)) Then 'if dynamic totalizer is above 120% of target weight, then stop
            .pumpCActive = False
            .logwarning("Feed A Totalizer: " & formatNumber(.FCPV, 2) & " [g] is above 120% of target weight: " & s(4) & " [g]. Stopping Feed A.")
            .phase = .phase + 1
        End If

        'if need to reset loop, then set offline C = 2. when in phase 2, set offline C = 0 to assign phase, then once in phase 4 assign offline C = 1   
        If .OfflineO = 2 Then '0 = no error, 1 = resume normal count  , 2 = loop reset 
            .phase = 2
        End If

    Case 10 'Glucose Decision Tree
        If procDay = 0 Then 'should never happen
            .logmessage("█████████████████████--Phase 10: Day# " & procDay & " UNIT#: " & .unit & ".--██████████████████████████████████████████████████████████████████████")
            .logmessage("██--------------No Glucose addition needed, proceeding to Phase 13.")
            .logmessage("████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████")
            .phase = .phase + 5
        ElseIf .offlineA <> procDay and .inoculationTime_H < (s(6) + 8) 'Waiting for offline D to confirm sample
            .logmessage("█████████████████████--Phase 10: Day# " & procDay & " UNIT#: " & .unit & ".--██████████████████████████████████████████████████████████████████████")
            .logmessage("██--------------Enter Glucose Target in Offline Field, then set Day to the current Process Day.")
            .logmessage("████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████")
            .phase = .phase + 1
        ElseIf .offlineA = procDay
            .phase = .phase + 2
        End If

    Case 11 'potential skip of glucose

        If .offlineA <> procDay and .inoculationTime_H > (s(6) + 8)
            .logwarning("Phase 9: Skipping glucose feed, sample not verified")
            .phase = .phase + 3
        ElseIf .offlineA = procDay 'if not entered until after feed
            .phase = .phase + 1
        End If

    Case 12 'Record Glucose phase bottleweight and 
        s(10) = Scale_Weight(4) 'Static glucose interval starting scale weight
        s(12) = .offlineB 'recorded glucose value
        
        If .InoculationTime_H > s(6) and (.runtime_H - .phaseStart_H) > 0.1/60 Then 'waits for respective feed time + 6s delay; 
            '.Logwarning("Case 9: Glucose to be Fed today: " & s(12) & " [g]")
            .phase = .phase + 1
        End If 

    Case 13 'should skip over
        If s(12) = 0 Then
            .logwarning("No glucose addition required on day: " & procDay & ". " & s(12) & " [g] was Fed")
            .phase = .phase + 2
        ElseIf s(12) <> 0
            .phase = .phase + 1
        End If

    Case 14 'glucose feed 
        If .runtime_H - .phaseStart_H < 0.025/60 Then 
            .logmessage("█████████████████████--Phase 14: Day# " & procDay & " UNIT#: " & .unit & ".--██████████████████████████████████████████████████████████████████████")
            .logmessage("██--------------Sampling Confirmed: " & s(12) & " [g] Glucose to be fed.")
            .logmessage("████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████")
        End If
        
        s(11) = Scale_Weight(5) 'dynamic interval scale weight 
        .pumpAActive = True 'Glucose starts
        
        If abs(glucoseFeed_Fed) > (s(12) * Glucose_Cut_pumpValue_Percent) Then  's(12) = glucose value input to offline D; cut value % = 75
            .FASP = Glucose_Feed_SP_percent 'reduced pump flow setpoint when 75% of target weight achieved 
        Else
            .FASP = Glucose_Feed_SP 'starting pump flow rate
        End If 
        
        If abs(glucoseFeed_Fed) > s(12) and (.runtime_H - .phaseStart_H) > 0.1/60 or s(12) = 0 Then ' = recorded glucose value 
            .logmessage("Gravimetric Glucose Feed Interval completed on day: " & procDay & ".") 's(10) = sample #
            .logMessage("Feed Counter: [" & .IntC &"]")
            .phase = .phase + 1 'reset to phase 2; need to move to glucose feed phase 
        ElseIf .FAPV > (1.2 * s(12)) Then 'if dynamic totalizer is above 120% of target weight, then stop
            .pumpAActive = False
            .logwarning("Feed A Totalizer: " & formatNumber(.FAPV, 2) & " [g] is above 120% of target weight: " & s(12) & " [g]. Stopping Feed A.")
            .phase = .phase + 1
        End If

        'if need to reset loop, then set offline C = 2. when in phase 2, set offline C = 0 to assign phase, then once in phase 4 assign offline C = 1   
        If .offlineC = 2 Then '0 = no error, 1 = resume normal count  , 2 = loop reset 
            .logMessage("Need to reset counter. Set to phase 2")
            .phase = .phase + 1
        End If

    Case 15
        .pumpAActive = False
        If .runtime_H - .phaseStart_H < 0.1/60 Then 
            .logWarning("Phase 15: Total Glucose Dispensed: " & formatNumber(glucoseFeed_Fed, 2) & "[g] on Day: " & procDay & ".")
            .phase = 2
        End If

    Case 16 
    'insert stop commands 
    .pumpCActive = False
    .pumpDActive = False 
    .pumpAActive = False
End Select
End With
End If

'Checks with logs - disable when not debugging
'p.LogMessage("Phase: " & p.phase)
'p.logmessage("FeedB Increment" & s(18))
'p.logmessage("static totalizer " & s(19))
'p.logmessage("feed b vol target " & s(20))
'p.logmessage("active totalizer " & p.VDPV)
'p.LogMessage("Inoc Time [h]: " & p.InoculationTime_H)
'p.LogMessage("Phase Interval Time [h]: " & (P.runtime_H-p.PhaseStart_H))

'p.LogMessage("Major Feed SP: " & Major_Feed_SP)
'p.LogMessage("pump C SP: " & p.FCSP)

'p.LogMessage("MajorFeed Fed: " & MajorFeed_Fed )
'p.logmessage("majorfeed target weight: " & Major_Feed_TargetWeight)
'p.LogMessage("MajorFeed Start Time: " & Major_Feed_StartTime)
'p.LogMessage("Array0 - time zero bw: " & s(0))
'p.LogMessage("Array1 - STATIC: " & s(1))
'p.LogMessage("Array2 - STATIC INTERVAL: " & s(2))
'p.LogMessage("Array3: -DYNAMIC INTERVAL" & s(3))
'p.LogMessage("Array4: " & s(4))
'p.LogMessage("Array5: " & s(5))
'p.LogMessage("Array6: " & s(6))
'p.LogMessage("Array7: " & s(7))
'p.LogMessage("array11: " & s(11))
'p.LogMessage("array10: " & s(10))
'p.LogMessage("array12: " & s(12))