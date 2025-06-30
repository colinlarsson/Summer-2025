        ' === Anti-Foam Logic Start ===
            Dim AF_CURRENT_CASE
            Dim AF_NUMBER_SHOTS
            Dim AF_LAST_CASE
            Dim AF_PREV_VAPV
            Dim AF_LAST_RESET

            Dim AF_MAX_SHOTS
            Dim AF_THRESHOLD
            Dim AF_SHOT_VOLUME
            Dim AF_DELAY_START
            Dim AF_DELAY_DURATION
            AF_MAX_SHOTS = 5
            AF_THRESHOLD = 500
            AF_SHOT_VOLUME = 0.1
            AF_DELAY_DURATION = 10 / 3600

            Const AF_CASE_WAITING_PRE = 10
            Const AF_CASE_WAITING_POST = 20

            If IsNumeric(s(30)) Then
                AF_CURRENT_CASE = s(30)
            Else
                AF_CURRENT_CASE = 0
            End If

            If IsNumeric(s(31)) Then
                AF_NUMBER_SHOTS = s(31)
            Else
                AF_NUMBER_SHOTS = 0
            End If

            If IsNumeric(s(32)) Then
                AF_LAST_CASE = s(32)
            Else
                AF_LAST_CASE = 0
            End If

            If IsNumeric(s(33)) Then
                AF_PREV_VAPV = s(33)
            Else 
                AF_PREV_VAPV = 0
            End If 

            If IsNumeric(s(34)) then
                AF_DELAY_START = CDbl(s(34))
            Else
                AF_DELAY_START = 0
            End If

            If IsNumeric(s(35)) Then
                AF_LAST_RESET = CDbl(s(35))
            Else
                AF_LAST_RESET = 0
            End If

            ' === 24 hour shot counter reset ===
            If .runtime_H - AF_LAST_RESET >= 24 then
                AF_NUMBER_SHOTS = 0
                AF_LAST_RESET = .runtime_H
                .LogWarning("24 hour anti-foam shot counter reset.")
            End If

            ' Anti-Foam main logic
            If AF_CURRENT_CASE = 0 Then
                If AF_LAST_CASE <> 0 Then
                    .logwarning("Foam level low, no anti-foam needed.")
                    AF_LAST_CASE = 0
                End If
                If .LvlPV >= AF_THRESHOLD Then
                    .LogWarning("Foam rising, starting anti-foam.")
                    AF_CURRENT_CASE = 1
                End If

            ElseIf AF_CURRENT_CASE = 1 Then
                If AF_NUMBER_SHOTS < AF_MAX_SHOTS Then
                    If AF_DELAY_START = 0 Then
                        AF_DELAY_START = .runtime_H
                        AF_CURRENT_CASE = AF_CASE_WAITING_PRE
                        .LogMessage("Starting pre-shot delay timer.")
                    End If
                Else
                    AF_CURRENT_CASE = 3
                End If
            ElseIf AF_CURRENT_CASE = AF_CASE_WAITING_PRE Then
                If AF_DELAY_START = 0 Then
                    AF_DELAY_START = .runtime_H
                    .LogMessage("Starting pre-shot delay timer.")
                ElseIf .runtime_H - AF_DELAY_START >= AF_DELAY_DURATION Then
                    .LogWarning("Dispensing anti-foam.")
                    .PumpAActive = 1
                    AF_PREV_VAPV = .VAPV
                    AF_DELAY_START = 0
                    AF_CURRENT_CASE = 2
                End If

            ElseIf AF_CURRENT_CASE = 2 Then
                If .VAPV - AF_PREV_VAPV >= AF_SHOT_VOLUME Then
                    .PumpAActive = 0
                    AF_NUMBER_SHOTS = AF_NUMBER_SHOTS + 1
                    .LogWarning("Shot dispensed: " & AF_SHOT_VOLUME & " mL.")
                    .LogWarning("Shot #" & AF_NUMBER_SHOTS & " dispensed")
                    AF_DELAY_START = .runtime_H
                    AF_CURRENT_CASE = AF_CASE_WAITING_POST
                    .LogMessage("Starting post-shot delay timer.")
                End If
            ElseIf AF_CURRENT_CASE = AF_CASE_WAITING_POST Then
                If AF_DELAY_START = 0 Then
                    AF_DELAY_START = .runtime_H
                    .LogMessage("Starting post-shot delay timer.")
                ElseIf .runtime_H - AF_DELAY_START >= AF_DELAY_DURATION Then
                    .LogMessage("Post-shot delay complete, resetting state.")
                    AF_LAST_CASE = 2
                    AF_CURRENT_CASE = 0
                    AF_DELAY_START = 0
                End If

            ElseIf AF_CURRENT_CASE = 3 Then
                If AF_LAST_CASE <> 3 Then
                    .LogWarning("Maximum amount of anti-foam shots reached.")
                    AF_LAST_CASE = 3
                End If
            End If

            ' Save anti-foam state
            s(30) = AF_CURRENT_CASE
            s(31) = AF_NUMBER_SHOTS
            s(32) = AF_LAST_CASE
            s(33) = AF_PREV_VAPV
            s(34) = CStr(AF_DELAY_START)
            s(35) = CStr(AF_LAST_RESET)
        ' === Anti-Foam Logic End ===
