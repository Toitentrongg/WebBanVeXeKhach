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
    PRIMARY KEY (TenDangNhap)
);

CREATE TABLE NhanVien (
    MaNV nvarchar(10),
    TenNV nvarchar(30),
    DiaChi nvarchar(30),
    SoDT nvarchar(10),
    PRIMARY KEY (MaNV)
);

CREATE TABLE TuyenXe(
	MaTuyen nvarchar (10),
	DiemBatDau nvarchar(30),
	DiemKetThuc nvarchar(30),
	PRIMARY KEY (MaTuyen),
);
CREATE TABLE ChuyenXe (
    MaChuyenXe nvarchar(10),
	MaTuyen nvarchar (10),
    ThoiGianXuatPhat date,
    ThoiGianDi time,
    SoLuongGhe int,    
    SoVeCon int,
    PRIMARY KEY (MaChuyenXe),
    FOREIGN KEY (MaTuyen) REFERENCES TuyenXe(MaTuyen),
);

CREATE TABLE KhachHang (
    MaKH nvarchar(10),
    TenKH nvarchar(30),
    DiaChi nvarchar(30),
    SoDT nvarchar(10),
    SoVeMua int,
    PRIMARY KEY (MaKH)
);

CREATE TABLE VeXe (
    MaVeXe nvarchar(10),
    MaChuyenXe nvarchar(10),
    MaNV nvarchar(10),
    MaKH nvarchar(10),
    NgayMua datetime,
    TrangThai int,
    SoGhe int,
    PRIMARY KEY (MaVeXe, MaChuyenXe),
    FOREIGN KEY (MaChuyenXe) REFERENCES ChuyenXe(MaChuyenXe),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
);


CREATE TABLE QLGiaVe (
	MaTuyen nvarchar(10),
    GiaVe int,
    NgayApDung date,
    PRIMARY KEY (NgayApDung,MaTuyen),
    FOREIGN KEY (MaTuyen) REFERENCES TuyenXe(MaTuyen),
);


SET DATEFORMAT dmy 
GO
-- Ràng buộc QuyenHan chỉ được là 'Quan Tri' hoặc 'Nhan Vien'
ALTER TABLE Users
ADD CONSTRAINT CK_QuyenHan CHECK (QuyenHan IN ('Quan Tri', 'Khach Hang'));

-- Ràng buộc MaNV phải bắt đầu bằng 'NV'
ALTER TABLE NhanVien
ADD CONSTRAINT CK_MaNV CHECK (MaNV LIKE 'NV%');

-- Ràng buộc MaKH phải bắt đầu bằng 'KH'
ALTER TABLE KhachHang
ADD CONSTRAINT CK_MaKH CHECK (MaKH LIKE 'KH%' or MaKH LIKE 'kh%' or MaKH LIKE 'kH%' or MaKH like 'Kh');

-- Ràng buộc MaChuyenXe phải bắt đầu bằng 'C' và tiếp theo là 3 số
ALTER TABLE ChuyenXe
ADD CONSTRAINT CK_MaChuyenXe CHECK (MaChuyenXe LIKE 'CX[0-9][0-9][0-9]');

-- Ràng buộc SoLuongGhe phải nhỏ hơn 40
ALTER TABLE ChuyenXe
ADD CONSTRAINT CK_SoLuongGhe CHECK (SoLuongGhe <= 40);

-- Ràng buộc MaVeXe phải bắt đầu bằng 'V' và tiếp theo là 3 số
INSERT INTO Users VALUES ('Admin','12345','Quan Tri');
-- Thêm dữ liệu vào bảng NhanVien
INSERT INTO NhanVien (MaNV, TenNV, DiaChi, SoDT)
VALUES
    ('NV001', N'Nguyễn Văn A', N'123 Đường Chính', '0123456789'),
    ('NV002', N'Trần Thị B', N'456 Phố Dài', '0987654321'),
    ('NV003', N'Lê Văn C', N'789 Hẻm Nhỏ', '0123456789'),
    ('NV004', N'Phạm Thị D', N'101 Ngõ Xanh', '0987654321'),
    ('NV005', N'Hoàng Văn E', N'202 Đường Mộc', '0123456789');
INSERT INTO TuyenXe (MaTuyen, DiemBatDau, DiemKetThuc)
VALUES
    ('T001', N'Hà Nội', N'Hồ Chí Minh'),
    ('T002', N'Đà Nẵng', N'Nha Trang'),
    ('T003', N'Huế', N'Quy Nhơn'),
    ('T004', N'Đà Lạt', N'Vũng Tàu'),
    ('T005', N'Cần Thơ', N'Long Xuyên');

-- Thêm dữ liệu vào bảng ChuyenXe
INSERT INTO ChuyenXe (MaChuyenXe, MaTuyen, ThoiGianXuatPhat, ThoiGianDi, SoLuongGhe, SoVeCon)
VALUES
    ('CX001', 'T001', '2024-01-03', '08:00:00', 40, 40),
    ('CX002', 'T002', '2024-05-04', '10:30:00', 40, 40),
    ('CX003', 'T003', '2024-01-05', '12:15:00', 40, 40),
    ('CX004', 'T004', '2024-08-06', '14:45:00', 36, 36),
    ('CX005', 'T005', '2024-12-07', '16:20:00', 36, 36);

-- Thêm dữ liệu vào bảng KhachHang
INSERT INTO KhachHang (MaKH, TenKH, DiaChi, SoDT, SoVeMua)
VALUES
    ('KH001', N'Nguyễn Văn Khách', N'123 Đường Mới', '0123456789', 2),
    ('KH002', N'Trần Thị Hành', N'456 Hẻm Xanh', '0987654321', 1),
    ('KH003', N'Lê Minh Tuấn', N'789 Đường Lạ', '0123456789', 3),
    ('KH004', N'Phạm Thị Ngọc', N'101 Phố Cổ', '0987654321', 2),
    ('KH005', N'Hoàng Minh Anh', N'202 Hẻm Nhỏ', '0123456789', 1);

-- Thêm dữ liệu vào bảng QLGiaVe
INSERT INTO QLGiaVe (MaTuyen, GiaVe, NgayApDung)
VALUES 
	('T001', 150000, '2024-01-01'),
	('T001', 150000, '2023-01-01'),
    ('T002', 200000, '2023-01-01'),
    ('T003', 180000, '2023-01-01'),
    ('T004', 250000, '2022-01-01'),
    ('T005', 120000, '2023-01-10');

SELECT * 
FROM Users

SELECT * 
FROM KhachHang

SELECT * 
FROM ChuyenXe

SELECT * 
FROM TuyenXe

-- ===================PROCEDURE TAO VE XE ==================================
CREATE PROCEDURE dbo.InsertVeXe
    @MaCX NVARCHAR(10),
    @SLGhe INT
AS
BEGIN
    DECLARE @Temp INT
    SET @Temp = (SELECT ISNULL(MAX(CAST(SUBSTRING(MaVeXe, 2, LEN(MaVeXe) - 1) AS INT)), 0) FROM VeXe)

    DECLARE @I INT = 1

    WHILE @I <= @SLGhe
    BEGIN
        DECLARE @Temp2 INT
        SET @Temp2 = @I + @Temp

        INSERT INTO VeXe (MaVeXe, MaChuyenXe, MaNV, MaKH, NgayMua, TrangThai, SoGhe)
        VALUES ('V' + CAST(@Temp2 AS NVARCHAR(10)), @MaCX, NULL, NULL, NULL, 0, @I)

        SET @I = @I + 1
    END
END

-- Gọi lại Procedure và truyền các tham số
-- Ví dụ: gọi lại Procedure InsertVeXe với MaCX là 'CX001' và SLGhe là 10
EXEC dbo.InsertVeXe @MaCX = 'CX001', @SLGhe = 40;

-- Gọi lại Procedure cho các chuyến xe còn lại
EXEC dbo.InsertVeXe @MaCX = 'CX002', @SLGhe = 40;
EXEC dbo.InsertVeXe @MaCX = 'CX003', @SLGhe = 40;
EXEC dbo.InsertVeXe @MaCX = 'CX006', @SLGhe = 36;
EXEC dbo.InsertVeXe @MaCX = 'CX007', @SLGhe = 40;
EXEC dbo.InsertVeXe @MaCX = 'CX005', @SLGhe = 36;
EXEC dbo.InsertVeXe @MaCX = 'CX004', @SLGhe = 36;


SELECT C.ThoiGianXuatPhat , C.MaChuyenXe , SUM (IIF (V.TrangThai = 1,1,0)) SoLuongVe ,
		DoanhThu = SUM (IIF (V.TrangThai = 1,1,0)) * (SELECT TOP 1 GiaVe
					FROM QLGiaVe 
					WHERE CONVERT(DATE , NgayApDung,103) <= C.ThoiGianXuatPhat 
					ORDER BY CONVERT(DATE , NgayApDung,103) DESC)
FROM ChuyenXe C LEFT JOIN VeXe V ON C.MaChuyenXe = V.MaChuyenXe
GROUP BY C.ThoiGianXuatPhat , C.MaChuyenXe 

-- THONG KE DOANH THU THEO TUYEN
CREATE PROCEDURE GetDoanhThuTheoTuyen
AS
BEGIN
    SELECT
        TX.DiemBatDau,
        TX.DiemKetThuc,
        MONTH(C.ThoiGianXuatPhat) AS 'Thang',
        YEAR(C.ThoiGianXuatPhat) AS 'Nam',
        SUM(IIF(V.TrangThai = 1, 1, 0)) AS 'SoLuongVeBanRa',
        DoanhThu = SUM(IIF(V.TrangThai = 1, 1, 0)) *
            (SELECT TOP 1 GiaVe
             FROM QLGiaVe
             WHERE MONTH(CONVERT(DATE, NgayApDung, 103)) <= MONTH(C.ThoiGianXuatPhat)
               AND YEAR(CONVERT(DATE, NgayApDung, 103)) <= YEAR(C.ThoiGianXuatPhat)
               AND MaTuyen = TX.MaTuyen -- Thêm điều kiện mã tuyến
             ORDER BY CONVERT(DATE, NgayApDung, 103) DESC)
    FROM
        ChuyenXe C
        LEFT JOIN VeXe V ON C.MaChuyenXe = V.MaChuyenXe
        LEFT JOIN TuyenXe TX ON C.MaTuyen = TX.MaTuyen
    WHERE
        TX.MaTuyen IS NOT NULL -- Thêm điều kiện để loại bỏ các chuyến xe không có mã tuyến
    GROUP BY
        TX.DiemBatDau, TX.DiemKetThuc, MONTH(C.ThoiGianXuatPhat), YEAR(C.ThoiGianXuatPhat), TX.MaTuyen;
END;


EXEC GetDoanhThuTheoTuyen

--THONG KE THEO CHUYEN
CREATE PROCEDURE GetDoanhThuTheoChuyenXe
AS
BEGIN
    SELECT
        C.MaChuyenXe,
        TX.DiemBatDau,
        TX.DiemKetThuc,
        MONTH(C.ThoiGianXuatPhat) AS 'Thang',
        YEAR(C.ThoiGianXuatPhat) AS 'Nam',
        SUM(IIF(V.TrangThai = 1, 1, 0)) AS 'SoLuongVeBanRa',
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
        TX.MaTuyen IS NOT NULL
    GROUP BY
        C.MaChuyenXe, TX.DiemBatDau, TX.DiemKetThuc, MONTH(C.ThoiGianXuatPhat), YEAR(C.ThoiGianXuatPhat), TX.MaTuyen;
END;

EXEC GetDoanhThuTheoChuyenXe
---------------------------------------
CREATE PROCEDURE GetDoanhThuTheoChuyenXe1
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
        TX.MaTuyen IS NOT NULL
    GROUP BY
        C.MaChuyenXe, MONTH(C.ThoiGianXuatPhat), YEAR(C.ThoiGianXuatPhat), TX.MaTuyen;
END;

EXEC GetDoanhThuTheoChuyenXe1

--THONG KE THEO THANG VA NAM
CREATE PROCEDURE GetDoanhThuTheoThang
AS
BEGIN
    SELECT
        MONTH(C.ThoiGianXuatPhat) AS 'Thang',
        YEAR(C.ThoiGianXuatPhat) AS 'Nam',
        SUM(IIF(V.TrangThai = 1, 1, 0)) AS 'TongSoLuongVeBanRa',
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
        TX.MaTuyen IS NOT NULL
    GROUP BY
        MONTH(C.ThoiGianXuatPhat), YEAR(C.ThoiGianXuatPhat);
END;

EXEC GetDoanhThuTheoThang

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
		SUM(IIF(V.TrangThai = 1, 1, 0)) AS 'TongSoLuongVeBanRa',
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
		TX.MaTuyen IS NOT NULL
	GROUP BY
		MONTH(C.ThoiGianXuatPhat), YEAR(C.ThoiGianXuatPhat),C.MaChuyenXe;
END;

EXEC GetDoanhThuTheoNam

SELECT * 
FROM ChuyenXe INNER JOIN TuyenXe ON ChuyenXe.MaTuyen = TuyenXe.MaTuyen

SELECT *
FROM ChuyenXe INNER JOIN TuyenXe ON ChuyenXe.MaTuyen = TuyenXe.MaTuyen

SELECT * 
FRom VeXe