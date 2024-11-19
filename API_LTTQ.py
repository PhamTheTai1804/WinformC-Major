import pandas as pd
from sqlalchemy import create_engine
from sklearn.preprocessing import LabelEncoder
from category_encoders import BinaryEncoder
from datetime import datetime
import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np
from sklearn.metrics.pairwise import cosine_similarity
from sklearn.preprocessing import MinMaxScaler
from sklearn.manifold import MDS
from flask import Flask, request, jsonify
from sklearn.neighbors import NearestNeighbors
from sklearn.preprocessing import MinMaxScaler

app = Flask(__name__)

province_mapping = {
     "Hà Giang": 0,
    "Cao Bằng": 1,
    "Lào Cai": 2,
    "Lai Châu": 3,
    "Bắc Kạn": 4,
    "Lạng Sơn": 5,
    "Tuyên Quang": 6,
    "Yên Bái": 7,
    "Điện Biên": 8,
    "Thái Nguyên": 9,
    "Phú Thọ": 10,
    "Bắc Giang": 11,
    "Quảng Ninh": 12,
    "Hòa Bình": 13,
    "Bắc Ninh": 14,
    "Hà Nội": 15,
    "Hà Nam": 16,
    "Hải Dương": 17,
    "Hải Phòng": 18,
    "Hưng Yên": 19,
    "Nam Định": 20,
    "Thái Bình": 21,
    "Ninh Bình": 22,
    "Thanh Hóa": 23,
    "Nghệ An": 24,
    "Hà Tĩnh": 25,
    "Quảng Bình": 26,
    "Quảng Trị": 27,
    "Thừa Thiên Huế": 28,
    "Đà Nẵng": 29,
    "Quảng Nam": 30,
    "Quảng Ngãi": 31,
    "Bình Định": 32,
    "Phú Yên": 33,
    "Khánh Hòa": 34,
    "Gia Lai": 35,
    "Kon Tum": 36,
    "Đắk Lắk": 37,
    "Đắk Nông": 38,
    "Ninh Thuận": 39,
    "Bình Thuận": 40,
    "Lâm Đồng": 41,
    "Bình Phước": 42,
    "Tây Ninh": 43,
    "Bình Dương": 44,
    "Đồng Nai": 45,
    "TP Hồ Chí Minh": 46,
    "Bà Rịa - Vũng Tàu": 47,
    "Long An": 48,
    "Đồng Tháp": 49,
    "Tiền Giang": 50,
    "Bến Tre": 51,
    "Vĩnh Long": 52,
    "Trà Vinh": 53,
    "Hậu Giang": 54,
    "An Giang": 55,
    "Kiên Giang": 56,
    "Sóc Trăng": 57,
    "Bạc Liêu": 58,
    "Cà Mau": 59,
    "Vĩnh Phúc": 60,
    "Hà Tĩnh": 61,
    "Quảng Ninh": 62
}


#connect database
server = 'PHAMTHETAI\\SQLEXPRESS'  
database = 'QLUser'
connection_string = f'mssql+pyodbc://{server}/{database}?driver=ODBC+Driver+17+for+SQL+Server&Integrated Security=SSPI'
engine = create_engine(connection_string)

#Encoding location to 6-bit binary
def encode_binary(n, bits=6):
    return list(map(int, format(n, f'0{bits}b')))

#read data
likes_query = "SELECT * FROM SoThich"
likes_df = pd.read_sql(likes_query, engine)

# API để tìm người dùng tương đồng
@app.route('/tim_nguoi_dung_tuong_dong', methods=['GET'])
def tim_nguoi_dung_tuong_dong():
    ma_nguoi_dung = int(request.args.get('MaNguoiDung'))
    users_query = f"Select [MaNguoiDung],[DiaChi],[NgaySinh] from [dbo].[NguoiDung] where [MaNguoiDung] NOT IN (SELECT [MaBanBe] FROM [dbo].[BanBe] WHERE [MaNguoiDung]= {ma_nguoi_dung})"
    user_likes_query =f"SELECT * FROM [dbo].[NguoiDung_SoThich] WHERE [MaNguoiDung] NOT IN (SELECT [MaBanBe] FROM [dbo].[BanBe] WHERE [MaNguoiDung]={ma_nguoi_dung})"
    user_likes_df = pd.read_sql(user_likes_query, engine)
    users_df = pd.read_sql(users_query, engine)

    #Encoding Birth to Age 
    users_df['ID'] = users_df['MaNguoiDung'].apply(lambda x: x)
    users_df['Tuoi'] = users_df['NgaySinh'].apply(lambda x: datetime.now().year - x.year)


    users_df['DiaChi_Ma'] = users_df['DiaChi'].apply(lambda x: province_mapping[x])  # 'province_mapping' là bảng ánh xạ
    diachi_binary_encoded = users_df['DiaChi_Ma'].apply(lambda x: encode_binary(x))

# Encoded Binary -> DataFrame -> match to main table
    diachi_binary_encoded = pd.DataFrame(diachi_binary_encoded.tolist(), index=users_df.index)
    users_df = pd.concat([users_df[['Tuoi']], diachi_binary_encoded], axis=1)
# Encoding hobby and match
    nguoidung_sothich_pivot = user_likes_df.pivot_table(index='MaNguoiDung', columns='MaSoThich', aggfunc='size', fill_value=0)
    nguoidung_sothich_df = nguoidung_sothich_pivot.reset_index()
    nguoidung_sothich_df.columns.name = None
    print(nguoidung_sothich_df)
# Final match to single table 
    nguoidung_encoded = pd.concat([users_df, nguoidung_sothich_df], axis=1)
    a = nguoidung_encoded.pop('MaNguoiDung')  # Xóa cột A và lưu vào biến a
    nguoidung_encoded.insert(0, 'MaNguoiDung', a) 

# result table of vector
    print(nguoidung_encoded)
# Separate data from variable nguoidung_encoded
    ages = nguoidung_encoded['Tuoi'].values.reshape(-1, 1)  # Age ( column 2)
    addresses = nguoidung_encoded.iloc[:, 2:8].values  # Location (Next 6 columns)
    preferences = nguoidung_encoded.iloc[:, 8:18].values  # hobby (per column of next 10 columns)

# chuẩn hóa dữ liệu 
    scaler = MinMaxScaler()
    ages_scaled = scaler.fit_transform(ages)
    addresses_scaled = scaler.fit_transform(addresses)
    preferences_scaled = scaler.fit_transform(preferences)

# ghép các đặc trưng thành 1 ma trận ( trọng số qua thử nghiệm lâm sàng)
    features = np.hstack([
        0.4 * ages_scaled,           # khía cạnh tuổi
        0.3 * addresses_scaled,       # khía cạnh địa chỉ
        0.1 * preferences_scaled     # khía cạnh sở thích
    ])

# KNN
    knn = NearestNeighbors(n_neighbors=6, metric='euclidean')
    knn.fit(features)
    
    
    user_index = nguoidung_encoded.index[nguoidung_encoded['MaNguoiDung'] == ma_nguoi_dung].tolist()
    
    if not user_index:
        return jsonify({"error": "MaNguoiDung không hợp lệ"}), 404

    user_index = user_index[0]
    
    # find nearest neighbors
    distances, indices = knn.kneighbors([features[user_index]])
    
    # remove self
    similar_indices = indices[0][1:6]
    
    # get list result @@
    similar_users = nguoidung_encoded.iloc[similar_indices]['MaNguoiDung'].tolist()
    
    return jsonify(similar_users)

# API đây rồi
if __name__ == '__main__':
    app.run(debug=True)

