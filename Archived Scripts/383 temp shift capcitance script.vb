dim  capcriteria as double = 10.8  ' capacitance target
dim timelimit as double = 68  ' hour limit on the shift
dim initialtemp as double = 36 'initial temeprature
dim temptarget as double = 33  'temp to shift to
dim rampduration as double = 6   'ramp the temp shift over time in hours?
dim capovertime as double = 1 'in Minutes, how long must the capacitance be over the limit?
dim rampendtime as double = 1 ' empty for now but will contain the end time of the tamping
dim tempdiff as double = initialtemp - temptarget
dim rampercent as double = 50 ' how far along the ramp are we so far


If P IsNot Nothing Then
    With P    'this bit is just at the start of every script

select case .phase
	case 0 'initialize
	if ext.k < capcriteria and .inoculationtime_h < timelimit then
		.TSP = initialtemp
	elseif ext.k > capcriteria then
		.phase = 1
	elseif .inoculationtime_h > timelimit then
		.phase = 2
	end if
	case 1 'wait time to see if capacitance is still good
	if ext.k > capcriteria and .inoculationtime_h - .phasestart_h  > capovertime then
		.phase = 2
	elseif ext.k < capcriteria then
		.phase = 0
	endif
	case 2 'the crieria has been met, start ramping
	if .inoculationtime_h - .phasestart_h < rampduration then
		rampendtime = .phasestart_h + rampduration
		ramppercent = ( .inoculationtime_h - .phasestart_h ) / rampduration
		.TSP = initialtemp - ramppercent * tempdiff
	elseif .inoculationtime_h - .phasestart_h > rampduration then	
		.TSP = temptarget
		.phase = 3
	end if
	case 3
	.TSP = temptarget
	end select

    End With  'this bit is at the end of every script
End If  