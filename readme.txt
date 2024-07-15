세팅
	NTP Setting
	Sysmon install 우선
	WSD Disabled
	ReName WGName
		wmic computersystem where name="%computername%" call joindomainorworkgruop name="WG_Name"



수집정보; Registry, Wmic, Powershell
	@엣지 버전 info_edge
		"HKCU\Software\Microsoft\Edge\BLBeacon"
			version
	
	@윈도우 빌드 정보 info_win_build
		"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion"
			CompositionEditionID
			CurrentBuild
			CurrentBuildNumber
			DisplayVersion
			EditionID
			InstallDate
			InstallTime
			ProductId
			ProductName
			ReleaseId
			UBR
			
	백신 설치 엔진정보 info_hauri
		"HKLM\SOFTWARE\HAURI\ViRobot Security\Base"
			EngineVersion
	
	모니터 정보 info_monitor
		Get-WmiObject WmiMonitorID -Namespace root\wmi | ForEach-Object { [PSCustomObject]@{ ManufacturerName = [System.Text.Encoding]::ASCII.GetString($_.ManufacturerName); ProductCodeID = [System.Text.Encoding]::ASCII.GetString($_.ProductCodeID); SerialNumberID = [System.Text.Encoding]::ASCII.GetString($_.SerialNumberID); UserFriendlyName = [System.Text.Encoding]::ASCII.GetString($_.UserFriendlyName); Active = $_.Active; WeekOfManufacture = $_.WeekOfManufacture; YearOfManufacture = $_.YearOfManufacture} } | ConvertTo-Json

	PC 모델명,제조사,시리얼번호; info_pc_product
		wmic csproduct get Name,Vendor,IdentifyingNumber
	
	메인보드 info_baseboard
		wmic baseboard get Product
	
	bios 정보 info_bios
		wmic bios get SerialNumber
	
	프린터 정보 info_printer
		wmic printer get name,PortName,DeviceID,DriverName,SystemName,Shared
	
	CPU info_cpu
		wmic cpu get Name,Caption,Description,NumberOfCores,NumberOfEnabledCore,NumberOfLogicalProcessors,ThreadCount
	
	메모리 정보 info_memory
		wmic memorychip get Caption,Capacity,ConfiguredClockSpeed,CreationClassName,Manufacturer,Name,PartNumber,SerialNumber,Speed,Tag,Version
	
	저장장치 정보 info_hdd
		wmic diskdrive get Description,Index,CreationClassName,Caption,DeviceID,InterfaceType,Manufacturer,MediaType,Model,MediaLoaded,Name,Partitions,SerialNumber,Size,SystemName
	
	NIC 정보 info_nic
		wmic nic get Caption,CreationClassName,Description,DeviceID,Index,MACAddress,Manufacturer,NetConnectionID,Name,NetConnectionStatus,ProductName,SystemName
		
	OS 정보 info_os
		wmic os get BuildNumber,Caption,CSName,CurrentTimeZone,FreePhysicalMemory,FreeSpaceInPagingFiles,FreeVirtualMemory,InstallDate,LocalDateTime,MaxProcessMemorySize,Name,OSArchitecture,SerialNumber,Version
	
	Domain 정보 info_domain
		wmic computersystem where name="%computername%" get Caption,DNSHostName,Domain,Model,Name,UserName,Workgroup

