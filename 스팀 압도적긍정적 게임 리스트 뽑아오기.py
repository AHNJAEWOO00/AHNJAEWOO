import requests
import pandas as pd

# SteamSpy API URL
url = "https://steamspy.com/api.php?request=all"

# 요청 보내기
print("SteamSpy 데이터 가져오는 중...")
res = requests.get(url)
res.raise_for_status()

data = res.json()

# Steam에서 게임 정보를 가져오는 URL
def get_steam_game_details(appid):
    steam_url = f"https://store.steampowered.com/api/appdetails?appids={appid}&l=koreana"  # 한국어로 정보 요청
    response = requests.get(steam_url)
    try:
        details = response.json()
        if details[str(appid)]['success']:
            return details[str(appid)]['data']['name']
        else:
            return None
    except:
        return None

# 결과 저장
results = []

for appid, info in data.items():
    positive = info['positive']
    negative = info['negative']
    total_reviews = positive + negative
    
    if total_reviews >= 5000:
        # 긍정률 계산
        if total_reviews > 0:
            positive_rate = positive / total_reviews
        else:
            positive_rate = 0
        
        if positive_rate >= 0.95:
            price_raw = info['price']
            try:
                price = int(price_raw) / 100
            except (ValueError, TypeError):
                price = "정보 없음"
            
            # 게임 이름을 한국어로 가져오기
            korean_name = get_steam_game_details(appid)
            if korean_name:  # 한국어 이름이 있으면
                game_name = korean_name
            else:
                game_name = info['name']  # 한국어 이름이 없으면 영어 이름 사용

            results.append({
                '게임명(한국어)': game_name,  # 한국어 이름 추가
                '평가 수': total_reviews,
                '긍정 비율(%)': round(positive_rate * 100, 2),
                '장르': info.get('genre', '없음'),
                '가격(USD)': price
            })

# DataFrame 만들기
df = pd.DataFrame(results)

# 평가수 많은 순 정렬하고 1000개만 가져오기
df = df.sort_values(by='평가 수', ascending=False).head(1000)

# 출력
print(df)

# CSV로 저장
df.to_csv('steam_overwhelmingly_positive_top1000_korean.csv', index=False, encoding='utf-8-sig')
print("CSV 파일로 저장 완료!")
