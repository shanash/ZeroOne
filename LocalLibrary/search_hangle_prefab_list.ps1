#Powershell로 실행
# PS > .\search_hangle_prefab_list.ps1

# 디렉토리 설정
$directory = ".\..\Assets"
$outputFile = ".\outputFile.txt"

# .prefab 파일들을 찾고 내용에서 유니코드로 인코딩된 한글이 있는지 검사 및 변환
Get-ChildItem -Path $directory -Filter *.prefab -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    # 유니코드 이스케이프 시퀀스로 인코딩된 한글 문자열을 포함하는 전체 문자열 찾기
    $pattern = '"[^"]*\\u([ACD][0-9A-F]{2}[0-9A-F]|[D][0-9A-F]{2}[0-9A][0-9A-F])[^"]*"'
    $matchess = [regex]::Matches($content, $pattern)

    if ($matchess.Count > 0)
    {
        Write-Host "Checking file: $($_.FullName)" | Out-File $outputFile -Append
    }
    foreach ($match in $matchess) {
        $unicodeString = $match.Value
        # 유니코드 이스케이프 시퀀스를 실제 문자로 변환
        $decodedString = [regex]::Replace($unicodeString, '\\u([a-fA-F0-9]{4})', {
            param($m)
            [char][Convert]::ToInt32($m.Groups[1].Value, 16)
        })
        "Korean String in $($_.FullName): $decodedString" | Out-File $outputFile -Append
    }
}