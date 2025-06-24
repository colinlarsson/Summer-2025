dim capCriteria as double = 10.8  ' capacitance target
dim timeLimit as double = 68  ' hour limit on the shift
dim initialTemp as double = 36 'initial temperature
dim tempTarget as double = 32  'temp to shift to
dim rampDuration as double = 2   'ramp the temp shift over time in hours
dim capOverTime as double = 0.5 'in hours, how long must the capacitance be over the limit?
dim rampEndTime as double = 1 ' empty for now but will contain the end time of the ramping
dim tempDiff as double = initialTemp - tempTarget
dim rampPercent as double = 50 ' how far along the ramp are we so far
'dim .phase as integer = 0 ' this is the stage of the temp shift, 0 = initial, 1 = wait, 2 = ramping, 3 = done

If P IsNot Nothing Then
    With P

    If IsEmpty(.intY) Then .intY = 0
    

    Select Case .intY
        Case 0
            If ext.k < capCriteria And .inoculationtime_h < timeLimit Then
                .TSP = initialTemp
            ElseIf ext.k > capCriteria Then
                .intY = 1
                .phasestart_h = .inoculationtime_h
            ElseIf .inoculationtime_h > timeLimit Then
                .intY = 2
                .phasestart_h = .inoculationtime_h
            End If

        Case 1
            If ext.k > capCriteria And (.inoculationtime_h - .phasestart_h) >= capOverTime Then
                .intY = 2
                .phasestart_h = .inoculationtime_h
            ElseIf ext.k < capCriteria Then
                .intY = 0
            End If

        Case 2
            If (.inoculationtime_h - .phasestart_h) < rampDuration Then
                rampPercent = (.inoculationtime_h - .phasestart_h) / rampDuration
                .TSP = initialTemp - rampPercent * tempDiff
            Else
                .TSP = tempTarget
                .intY = 3
            End If

        Case 3
            .TSP = tempTarget
    End Select

    End With
End If

End If  