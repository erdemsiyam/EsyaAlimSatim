USE [ikincieldb]
GO
/****** Object:  Table [dbo].[AcikArtirma]    Script Date: 10.01.2019 08:14:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AcikArtirma](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_AcikArtirma_id]  DEFAULT (newsequentialid()),
	[olusturulmatarih] [datetime] NULL,
	[bitistarih] [datetime] NULL,
 CONSTRAINT [PK_AcikArtirma] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Favoriilan]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Favoriilan](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Favoriilan_id]  DEFAULT (newsequentialid()),
	[kullanici_id] [uniqueidentifier] NOT NULL,
	[ilan_id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Favoriilan] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ilan]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ilan](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ilan_id]  DEFAULT (newsequentialid()),
	[kullanici_id] [uniqueidentifier] NOT NULL,
	[kategori_id] [uniqueidentifier] NOT NULL,
	[acikartirma_id] [uniqueidentifier] NULL,
	[satildimi] [bit] NULL,
	[ilanacikmi] [bit] NULL,
	[ilantarih] [datetime] NULL,
	[baslik] [nvarchar](50) NULL,
	[aciklama] [nvarchar](500) NULL,
	[sorunu] [nvarchar](500) NULL,
	[kullanimsuresi] [int] NULL,
	[il] [nvarchar](50) NULL,
	[ilce] [nvarchar](50) NULL,
	[mahalle] [nvarchar](50) NULL,
	[konum] [nvarchar](50) NULL,
	[fiyat] [int] NULL,
 CONSTRAINT [PK_ilan] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ilanMesaj]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ilanMesaj](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ilanMesaj_id]  DEFAULT (newsequentialid()),
	[ilan_id] [uniqueidentifier] NOT NULL,
	[satici_kullanici_id] [uniqueidentifier] NOT NULL,
	[alici_kullanici_id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ilanMesaj] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ilanResim]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ilanResim](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ilanResim_id]  DEFAULT (newsequentialid()),
	[ilan_id] [uniqueidentifier] NOT NULL,
	[ad] [nvarchar](100) NOT NULL,
	[sirasi] [int] NULL,
 CONSTRAINT [PK_ilanResim] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Kategori]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kategori](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Kategori_id]  DEFAULT (newsequentialid()),
	[ad] [nvarchar](50) NULL,
 CONSTRAINT [PK_Kategori] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Kullanici]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kullanici](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Kullanici_id]  DEFAULT (newsequentialid()),
	[ad] [nvarchar](50) NULL,
	[soyad] [nvarchar](50) NULL,
	[sifre] [nvarchar](50) NULL,
	[mail] [nvarchar](50) NULL,
	[telefon] [nvarchar](50) NULL,
	[rol_id] [uniqueidentifier] NULL,
	[kullaniciresim_id] [uniqueidentifier] NULL,
	[olusturulmatarih] [datetime] NULL,
	[onaylimi] [bit] NULL,
	[engellimi] [bit] NULL,
 CONSTRAINT [PK_Kullanici] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KullaniciResim]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KullaniciResim](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_KullaniciResim_id]  DEFAULT (newsequentialid()),
	[ad] [nvarchar](100) NULL,
 CONSTRAINT [PK_KullaniciResim] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Mesaj]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mesaj](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Mesaj_id]  DEFAULT (newsequentialid()),
	[ilanmesaj_id] [uniqueidentifier] NULL,
	[mesaj] [text] NULL,
	[mesajalicininmi] [bit] NULL,
	[gordumu] [bit] NULL,
	[tarih] [datetime] NULL,
 CONSTRAINT [PK_Mesaj] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OneCikar]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OneCikar](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_OneCikar_id]  DEFAULT (newsequentialid()),
	[ilan_id] [uniqueidentifier] NOT NULL,
	[bitistarih] [datetime] NOT NULL,
 CONSTRAINT [PK_OneCikar] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Rol]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Rol_id]  DEFAULT (newsequentialid()),
	[adi] [nvarchar](50) NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Teklif]    Script Date: 10.01.2019 08:14:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teklif](
	[id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Teklif_id]  DEFAULT (newsequentialid()),
	[acikartirma_id] [uniqueidentifier] NOT NULL,
	[alici_kullanici_id] [uniqueidentifier] NOT NULL,
	[teklif] [int] NULL,
	[tekliftarih] [datetime] NULL,
 CONSTRAINT [PK_Teklif] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[Rol] ([id], [adi]) VALUES (N'c9246471-a962-43be-9847-30ab034845f0', N'Kullanici')
INSERT [dbo].[Rol] ([id], [adi]) VALUES (N'5653886d-e8d9-4cf9-ba6e-9cd28e6296a2', N'Admin')
INSERT [dbo].[Kullanici] ([id], [ad], [soyad], [sifre], [mail], [telefon], [rol_id], [kullaniciresim_id], [olusturulmatarih], [onaylimi], [engellimi]) VALUES (N'45a65569-8947-4b7e-b821-35bf9bddceb9', N'Admin', N'Admin', N'123', N'info@alabiverabi.com', N'+905551234567', N'c9246471-a962-43be-9847-30ab034845f0', NULL , CAST(N'2018-11-04 14:42:50.000' AS DateTime), 0, 0)
ALTER TABLE [dbo].[Favoriilan]  WITH CHECK ADD  CONSTRAINT [FK_Favoriilan_ilan] FOREIGN KEY([ilan_id])
REFERENCES [dbo].[ilan] ([id])
GO
ALTER TABLE [dbo].[Favoriilan] CHECK CONSTRAINT [FK_Favoriilan_ilan]
GO
ALTER TABLE [dbo].[Favoriilan]  WITH CHECK ADD  CONSTRAINT [FK_Favoriilan_Kullanici] FOREIGN KEY([kullanici_id])
REFERENCES [dbo].[Kullanici] ([id])
GO
ALTER TABLE [dbo].[Favoriilan] CHECK CONSTRAINT [FK_Favoriilan_Kullanici]
GO
ALTER TABLE [dbo].[ilan]  WITH CHECK ADD  CONSTRAINT [FK_ilan_AcikArtirma] FOREIGN KEY([acikartirma_id])
REFERENCES [dbo].[AcikArtirma] ([id])
GO
ALTER TABLE [dbo].[ilan] CHECK CONSTRAINT [FK_ilan_AcikArtirma]
GO
ALTER TABLE [dbo].[ilan]  WITH CHECK ADD  CONSTRAINT [FK_ilan_Kategori] FOREIGN KEY([kategori_id])
REFERENCES [dbo].[Kategori] ([id])
GO
ALTER TABLE [dbo].[ilan] CHECK CONSTRAINT [FK_ilan_Kategori]
GO
ALTER TABLE [dbo].[ilan]  WITH CHECK ADD  CONSTRAINT [FK_ilan_Kullanici] FOREIGN KEY([kullanici_id])
REFERENCES [dbo].[Kullanici] ([id])
GO
ALTER TABLE [dbo].[ilan] CHECK CONSTRAINT [FK_ilan_Kullanici]
GO
ALTER TABLE [dbo].[ilanMesaj]  WITH CHECK ADD  CONSTRAINT [FK_ilanMesaj_ilan] FOREIGN KEY([ilan_id])
REFERENCES [dbo].[ilan] ([id])
GO
ALTER TABLE [dbo].[ilanMesaj] CHECK CONSTRAINT [FK_ilanMesaj_ilan]
GO
ALTER TABLE [dbo].[ilanMesaj]  WITH CHECK ADD  CONSTRAINT [FK_ilanMesaj_Kullanici] FOREIGN KEY([satici_kullanici_id])
REFERENCES [dbo].[Kullanici] ([id])
GO
ALTER TABLE [dbo].[ilanMesaj] CHECK CONSTRAINT [FK_ilanMesaj_Kullanici]
GO
ALTER TABLE [dbo].[ilanMesaj]  WITH CHECK ADD  CONSTRAINT [FK_ilanMesaj_Kullanici1] FOREIGN KEY([alici_kullanici_id])
REFERENCES [dbo].[Kullanici] ([id])
GO
ALTER TABLE [dbo].[ilanMesaj] CHECK CONSTRAINT [FK_ilanMesaj_Kullanici1]
GO
ALTER TABLE [dbo].[ilanResim]  WITH CHECK ADD  CONSTRAINT [FK_ilanResim_ilan] FOREIGN KEY([ilan_id])
REFERENCES [dbo].[ilan] ([id])
GO
ALTER TABLE [dbo].[ilanResim] CHECK CONSTRAINT [FK_ilanResim_ilan]
GO
ALTER TABLE [dbo].[Kullanici]  WITH CHECK ADD  CONSTRAINT [FK_Kullanici_KullaniciResim] FOREIGN KEY([kullaniciresim_id])
REFERENCES [dbo].[KullaniciResim] ([id])
GO
ALTER TABLE [dbo].[Kullanici] CHECK CONSTRAINT [FK_Kullanici_KullaniciResim]
GO
ALTER TABLE [dbo].[Kullanici]  WITH CHECK ADD  CONSTRAINT [FK_Kullanici_Rol] FOREIGN KEY([rol_id])
REFERENCES [dbo].[Rol] ([id])
GO
ALTER TABLE [dbo].[Kullanici] CHECK CONSTRAINT [FK_Kullanici_Rol]
GO
ALTER TABLE [dbo].[Mesaj]  WITH CHECK ADD  CONSTRAINT [FK_Mesaj_ilanMesaj] FOREIGN KEY([ilanmesaj_id])
REFERENCES [dbo].[ilanMesaj] ([id])
GO
ALTER TABLE [dbo].[Mesaj] CHECK CONSTRAINT [FK_Mesaj_ilanMesaj]
GO
ALTER TABLE [dbo].[OneCikar]  WITH CHECK ADD  CONSTRAINT [FK_OneCikar_ilan] FOREIGN KEY([ilan_id])
REFERENCES [dbo].[ilan] ([id])
GO
ALTER TABLE [dbo].[OneCikar] CHECK CONSTRAINT [FK_OneCikar_ilan]
GO
ALTER TABLE [dbo].[Teklif]  WITH CHECK ADD  CONSTRAINT [FK_Teklif_AcikArtirma] FOREIGN KEY([acikartirma_id])
REFERENCES [dbo].[AcikArtirma] ([id])
GO
ALTER TABLE [dbo].[Teklif] CHECK CONSTRAINT [FK_Teklif_AcikArtirma]
GO
ALTER TABLE [dbo].[Teklif]  WITH CHECK ADD  CONSTRAINT [FK_Teklif_Kullanici] FOREIGN KEY([alici_kullanici_id])
REFERENCES [dbo].[Kullanici] ([id])
GO
ALTER TABLE [dbo].[Teklif] CHECK CONSTRAINT [FK_Teklif_Kullanici]
GO
