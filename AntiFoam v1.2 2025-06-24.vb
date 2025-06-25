Dim AFOAM_COUNTER As Integer ' Can use array, like a(17)
Dim SHOT_VOLUME As Double ' Can use array or an offline value
Dim MAX_SHOTS As Integer ' Maximum number of shots to add per day
Dim AFOAM_SHOT_FED As Double ' Record volumes before and after shot, store in array a(18), a(19)
Dim AFOAM_THRESHOLD As Double ' Keep in array, internal value or offline value

' Case 0 logic
Sub Case0()
    ' Initial waiting for level.pv to rise above threshold
    ' Check for counter number and total volume from totalizer
End Sub

' Case 1 logic
Sub Case1()
    ' Shot: pump on until a certain volume is hit
    ' Save volume fed
    LogMessage
End Sub

' Case 2 logic
Sub Case2()
    ' Advance counter
    ' Pump off
    ' Go to Case0
    LogMessage
End Sub

' Function to log messages
Sub LogMessage()
    ' Implementation for logging messages
End Sub