USE master
GO 
IF EXISTS (SELECT * FROM sysdatabases WHERE name='QLBVX')
    DROP DATABASE QLBVX
GO

CREATE DATABASE QLBVX
GO

USE QLBVX
GO

CREATE TABLE Users (
    TenDangNhap nvarchar(50),
    MatKhau nvarchar(50),
    QuyenHan nvarchar(50),
    TenUser nvarchar(30),
    DiaChi nvarchar(30),
    SoDT nvarchar(10),
    SoVeMua int,
    PRIMARY KEY (TenDangNhap)
);
CREATE TABLE TuyenXe(
	MaTuyen nvarchar (10),
	DiemBatDau nvarchar(30),
	DiemKetThuc nvarchar(30),
	PRIMARY KEY (MaTuyen),
);
CREATE TABLE ChuyenXe (
    MaChuyenXe nvarchar(10),
    MaTuyen nvarchar(10),
    ThoiGianXuatPhat date,
    SoLuongGhe int,    
    SoVeCon int,
    PRIMARY KEY (MaChuyenXe, ThoiGianXuatPhat),
    FOREIGN KEY (MaTuyen) REFERENCES TuyenXe(MaTuyen)
);

CREATE TABLE VeXe (
    MaVeXe nvarchar(10),
    MaChuyenXe nvarchar(10),
    ThoiGianXuatPhat date,
    TenDangNhap nvarchar(50),
    NgayMua datetime,
    TrangThai int,
    SoGhe int,
    PRIMARY KEY (MaVeXe),
    FOREIGN KEY (MaChuyenXe, ThoiGianXuatPhat) REFERENCES ChuyenXe(MaChuyenXe, ThoiGianXuatPhat),
    FOREIGN KEY (TenDangNhap) REFERENCES Users(TenDangNhap)
);

CREATE TABLE QLGiaVe (
	MaTuyen nvarchar(10),
    GiaVe int,
    NgayApDung date,
    PRIMARY KEY (NgayApDung,MaTuyen),
    FOREIGN KEY (MaTuyen) REFERENCES TuyenXe(MaTuyen),
);
CREATE TABLE ThanhToan (
    MaGiaoDich int IDENTITY(1,1) PRIMARY KEY,
    MaVeXe nvarchar(10),
    TenDangNhap nvarchar(50),
    ThoiGianGiaoDich datetime,
    TongTien int,
    TrangThai nvarchar(50),
    CONSTRAINT FK_VeXe FOREIGN KEY (MaVeXe) REFERENCES VeXe(MaVeXe),
    CONSTRAINT FK_Users FOREIGN KEY (TenDangNhap) REFERENCES Users(TenDangNhap)
);
SET DATEFORMAT dmy
GO
-- Ràng buộc QuyenHan chỉ được là 'KhachHang' hoặc 'QuanTri'
ALTER TABLE Users
ADD CONSTRAINT CK_QuyenHan CHECK (QuyenHan IN ('QuanTri', 'KhachHang'));

-- Thêm dữ liệu vào bảng Users
INSERT INTO Users (TenDangNhap, MatKhau, QuyenHan, TenUser, DiaChi, SoDT, SoVeMua)
VALUES 
    ('user1', 'pass123', 'QuanTri', 'Nguyen Van A', '123 ABC Street', '0123456789', 3),
    ('user2', 'pass456', 'KhachHang', 'Tran Thi B', '456 XYZ Street', '0987654321', 2),
    ('user3', 'pass789', 'KhachHang', 'Le Van C', '789 QWE Street', '0123456789', 1),
    ('user4', 'passabc', 'QuanTri', 'Pham Thi D', '456 ZXC Street', '0987654321', 5),
    ('user5', 'passdef', 'KhachHang', 'Hoang Van E', '789 RTY Street', '0123456789', 0);

-- Thêm dữ liệu vào bảng TuyenXe
INSERT INTO TuyenXe (MaTuyen, DiemBatDau, DiemKetThuc)
VALUES 
    ('T1', N'SaiGon', N'NhaTrang'),
    ('T2', 'Da Nang', 'Ho Chi Minh'),
    ('T3', 'Ho Chi Minh', 'Hue'),
    ('T4', 'Hue', 'Hai Phong'),
    ('T5', 'Hai Phong', 'Nha Trang');
-- Thêm dữ liệu vào bảng ChuyenXe
INSERT INTO ChuyenXe (MaChuyenXe, MaTuyen, ThoiGianXuatPhat, SoLuongGhe, SoVeCon)
VALUES 
    ('CX1', 'T2', '2024-04-11',  40, 15),
    ('CX1', 'T1', '2024-04-10', 30, 20),
    ('CX3', 'T3', '2024-04-12',  35, 25),
	('CX3', 'T3', '2024-04-14',  35, 25),
    ('CX4', 'T4', '2024-04-13',  25, 10),
    ('CX5', 'T5', '2024-04-14',  50, 30),
    ('CX7', 'T1', '2024-04-10',  40, 15),
    ('CX8', 'T1', '2024-04-10',  40, 15),
    ('CX9', 'T1', '2024-04-10', 40, 15),
    ('CX10', 'T1', '2024-04-10', 40, 15),
    ('CX6', 'T1', '2024-04-10',  40, 15);

-- Thêm dữ liệu vào bảng QLGiaVe
INSERT INTO QLGiaVe (MaTuyen, GiaVe, NgayApDung)
VALUES 
    ('T1', 200000, '2024-04-15'),
    ('T2', 300000, '2024-04-10'),
    ('T3', 250000, '2024-04-10'),
    ('T4', 180000, '2024-04-10'),
    ('T5', 350000, '2024-04-10');

CREATE PROCEDURE dbo.InsertVeXe
    @MaCX NVARCHAR(10),
    @ThoiGianXuatPhat DATE,
    @SLGhe INT
AS
BEGIN
    -- Kiểm tra xem chuyến xe có tồn tại không
    IF NOT EXISTS (SELECT 1 FROM ChuyenXe WHERE MaChuyenXe = @MaCX)
    BEGIN
        RETURN
    END

    -- Khai báo biến tạm thời để lưu trữ số lượng vé đã được tạo
    DECLARE @Count INT = 0

    -- Lặp qua từng ghế để tạo vé
    WHILE @Count < @SLGhe
    BEGIN
        -- Tạo mã vé mới
        DECLARE @MaVeXe NVARCHAR(10)
        DECLARE @MaxMaVeXe INT
        SELECT @MaxMaVeXe = ISNULL(MAX(CAST(SUBSTRING(MaVeXe, 2, LEN(MaVeXe) - 1) AS INT)) + 1, 1) FROM VeXe
        SET @MaVeXe = 'V' + CAST(@MaxMaVeXe AS NVARCHAR(10))

        -- Thêm vé vào bảng VeXe với thời gian xuất phát của chuyến xe và trạng thái mặc định là 0
        INSERT INTO VeXe (MaVeXe, MaChuyenXe, ThoiGianXuatPhat, TenDangNhap, NgayMua, TrangThai, SoGhe)
        VALUES (@MaVeXe, @MaCX, @ThoiGianXuatPhat, NULL, NULL, 0, @Count + 1)

        SET @Count = @Count + 1
    END
END

SELECT * FROM ChuyenXe
EXEC InsertVeXe @MaCX = 'CX1' , @ThoiGianXuatPhat = '2024-04-10' , @SLGhe = 40

--Doanh thu theo chuyen
CREATE PROCEDURE ThongKeTheoNgayCuaChuyen
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        C.ThoiGianXuatPhat, 
        C.MaChuyenXe, 
        C.MaTuyen, 
        SUM(IIF(V.TrangThai = 1, 1, 0))/2 AS SoLuongVe,
        DoanhThu = (SUM(IIF(V.TrangThai = 1, 1, 0))/2) * (
            SELECT TOP 1 GiaVe
            FROM QLGiaVe 
            WHERE CONVERT(DATE, NgayApDung, 103) <= C.ThoiGianXuatPhat 
                AND MaTuyen = C.MaTuyen
            ORDER BY CONVERT(DATE, NgayApDung, 103) DESC
        )
    FROM 
        ChuyenXe C 
    LEFT JOIN 
        VeXe V ON C.MaChuyenXe = V.MaChuyenXe
    WHERE 
        C.ThoiGianXuatPhat = V.ThoiGianXuatPhat
    GROUP BY 
        C.ThoiGianXuatPhat, 
        C.MaChuyenXe,
        C.MaTuyen;
END;


-- THONG KE DOANH THU THEO TUYEN
CREATE PROCEDURE GetDoanhThuTheoTuyen
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        TX.DiemBatDau,
        TX.DiemKetThuc,
        MONTH(C.ThoiGianXuatPhat) AS Thang,
        YEAR(C.ThoiGianXuatPhat) AS Nam,
        (SUM(IIF(V.TrangThai = 1, 1, 0)))/2 AS SoLuongVeBanRa,
        SUM(IIF(V.TrangThai = 1, 1, 0)) *
            (SELECT TOP 1 GiaVe
             FROM QLGiaVe
             WHERE MONTH(CONVERT(DATE, NgayApDung, 103)) <= MONTH(C.ThoiGianXuatPhat)
               AND YEAR(CONVERT(DATE, NgayApDung, 103)) <= YEAR(C.ThoiGianXuatPhat)
               AND MaTuyen = TX.MaTuyen
             ORDER BY CONVERT(DATE, NgayApDung, 103) DESC) AS DoanhThu
    FROM
        ChuyenXe C
        LEFT JOIN VeXe V ON C.MaChuyenXe = V.MaChuyenXe
        LEFT JOIN TuyenXe TX ON C.MaTuyen = TX.MaTuyen
    WHERE
        TX.MaTuyen IS NOT NULL AND C.ThoiGianXuatPhat = V.ThoiGianXuatPhat
    GROUP BY
        TX.DiemBatDau,
        TX.DiemKetThuc,
        MONTH(C.ThoiGianXuatPhat),
        YEAR(C.ThoiGianXuatPhat),
        TX.MaTuyen;
END;


---------------------------------------
CREATE PROCEDURE GetDoanhThuTheoChuyenXe
AS
BEGIN
    SELECT
        C.MaChuyenXe,
        MONTH(C.ThoiGianXuatPhat) AS 'Thang',
        YEAR(C.ThoiGianXuatPhat) AS 'Nam',
        DoanhThu = SUM(IIF(V.TrangThai = 1, 1, 0)) *
            (SELECT TOP 1 GiaVe
             FROM QLGiaVe
             WHERE MONTH(CONVERT(DATE, NgayApDung, 103)) <= MONTH(C.ThoiGianXuatPhat)
               AND YEAR(CONVERT(DATE, NgayApDung, 103)) <= YEAR(C.ThoiGianXuatPhat)
               AND MaTuyen = TX.MaTuyen
             ORDER BY CONVERT(DATE, NgayApDung, 103) DESC)
    FROM
        ChuyenXe C
        LEFT JOIN VeXe V ON C.MaChuyenXe = V.MaChuyenXe
        LEFT JOIN TuyenXe TX ON C.MaTuyen = TX.MaTuyen
    WHERE
        TX.MaTuyen IS NOT NULL AND C.ThoiGianXuatPhat = V.ThoiGianXuatPhat
    GROUP BY
        C.MaChuyenXe, MONTH(C.ThoiGianXuatPhat), YEAR(C.ThoiGianXuatPhat), TX.MaTuyen;
END;


CREATE PROCEDURE GetDoanhThuTheoNam
AS
BEGIN
SELECT
		
		Quy = IIF(month(C.ThoiGianXuatPhat) IN (1,2,3),1,
		IIF(MONTH(C.ThoiGianXuatPhat)IN(4,5,6),2,
		IIF(MONTH(C.ThoiGianXuatPhat) IN (7,8,9),3,
		IIF(MONTH(C.ThoiGianXuatPhat) IN (10,11,12),4,0)))),
		MONTH(C.ThoiGianXuatPhat) AS 'Thang',
		YEAR(C.ThoiGianXuatPhat) AS 'Nam',
		C.MaChuyenXe,
		(SUM(IIF(V.TrangThai = 1, 1, 0))/2) AS 'TongSoLuongVeBanRa',
		SUM(IIF(V.TrangThai = 1, 1, 0) * GiaVe) AS 'TongDoanhThu'
	FROM
		ChuyenXe C
		LEFT JOIN VeXe V ON C.MaChuyenXe = V.MaChuyenXe
		LEFT JOIN TuyenXe TX ON C.MaTuyen = TX.MaTuyen
		OUTER APPLY (
			SELECT TOP 1 GiaVe
			FROM QLGiaVe
			WHERE MONTH(CONVERT(DATE, NgayApDung, 103)) <= MONTH(C.ThoiGianXuatPhat)
				AND YEAR(CONVERT(DATE, NgayApDung, 103)) <= YEAR(C.ThoiGianXuatPhat)
				AND MaTuyen = TX.MaTuyen
			ORDER BY CONVERT(DATE, NgayApDung, 103) DESC
		) AS GiaVe
	WHERE
		TX.MaTuyen IS NOT NULL AND C.ThoiGianXuatPhat = V.ThoiGianXuatPhat
	GROUP BY
		MONTH(C.ThoiGianXuatPhat), YEAR(C.ThoiGianXuatPhat),C.MaChuyenXe;
END;

EXEC GetDoanhThuTheoNam

SELECT TenDangNhap,MatKhau,QuyenHan,TenUser,DiaChi,SoDT,SoVeMua
FROM Users
