Dim isTrigger

isTrigger = (p.VPV Mod 0.5) = 0

If isTrigger Then
    p.PumpAActive = 0
ElseIf p.LvlPV >= 500 then
    p.PumpAActive = 1
Else
    p.PumpAActive = 0
End If