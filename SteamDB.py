import requests
import pandas as pd

# SteamSpy API URL
url = "https://steamspy.com/api.php?request=all"

# 요청 보내기
print("SteamSpy 데이터 가져오는 중...")
res = requests.get(url)
res.raise_for_status()

data = res.json()

# 결과 저장
results = []

for appid, info in data.items():
    positive = info['positive']
    negative = info['negative']
    total_reviews = positive + negative
    
    if total_reviews >= 10000:
        # 긍정률 계산
        if total_reviews > 0:
            positive_rate = positive / total_reviews
        else:
            positive_rate = 0
        
        # '압도적으로 긍정적' 기준 (보통 95% 이상으로 간주)
        if positive_rate >= 0.95:
            # 가격 처리
            price_raw = info['price']
            try:
                price = int(price_raw) / 100  # 숫자면 계산
            except (ValueError, TypeError):
                price = "정보 없음"  # 숫자가 아니면 "정보 없음" 처리
            
            results.append({
                '게임명': info['name'],
                '평가 수': total_reviews,
                '긍정 비율(%)': round(positive_rate * 100, 2),
                '장르': info.get('genre', '없음'),
                '가격(USD)': price
            })

# DataFrame 만들기
df = pd.DataFrame(results)

# 평가수 많은 순 정렬
df = df.sort_values(by='평가 수', ascending=False)

# 출력
print(df)

# CSV로 저장
df.to_csv('steam_overwhelmingly_positive_games.csv', index=False, encoding='utf-8-sig')
print("CSV 파일로 저장 완료!")
