' Description: Persistent Array Assignments for Automated Feeding and Temperature Control Script
' Use: Use this file to copy/paste into any new scripts written for the 3L bioreactors
' 2025-06-24 Will want to better organize later - Colin Larsson

If s is Nothing Then
    Dim a(22) as Double
    a(1) = 0 'time zero bottle weight 
    a(2) = 0 'interval starting bottleweight 
    a(3) = 0 'current weight 
    a(4) = Major_Feed_TargetWeight '
    a(5) = Major_Feed_Counter '
    a(6) = Feed_StartTime 'initialized to 0 
    a(7) = FeedB_Counter 'initialized to 0 
    a(8) = FeedB_Totalizer 'initialized to 0 
    a(9) = FeedB_VolumeTarget 'initialized to 0 
    a(10) = 0 'Glucose interval starting bottleweight 
    a(11) = 0 'current weight 
    a(12) = 0 'user input glucose 
    a(15) = 0 ' Timestamp when capacitance wait period begins
    a(16) = 0 ' Timestamp when temperature ramp begins
    s = a ' s is a persistent variable that holds the state of the script
End If