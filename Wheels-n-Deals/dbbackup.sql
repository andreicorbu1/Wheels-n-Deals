PGDMP                         {        
   AutoSiteDb    15.4    15.4 '    1           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            2           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            3           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            4           1262    18038 
   AutoSiteDb    DATABASE     �   CREATE DATABASE "AutoSiteDb" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United States.1252';
    DROP DATABASE "AutoSiteDb";
                postgres    false            �            1259    18160    AnnouncementImages    TABLE     n   CREATE TABLE public."AnnouncementImages" (
    "AnnouncementId" uuid NOT NULL,
    "ImageId" uuid NOT NULL
);
 (   DROP TABLE public."AnnouncementImages";
       public         heap    postgres    false            �            1259    18082    Announcements    TABLE     �  CREATE TABLE public."Announcements" (
    "Id" uuid NOT NULL,
    "Title" character varying(50) NOT NULL,
    "Description" character varying(1000) NOT NULL,
    "County" character varying(50) NOT NULL,
    "City" character varying(50) NOT NULL,
    "DateCreated" timestamp with time zone NOT NULL,
    "VehicleId" uuid NOT NULL,
    "OwnerId" uuid,
    "DateModified" timestamp with time zone DEFAULT '-infinity'::timestamp with time zone NOT NULL
);
 #   DROP TABLE public."Announcements";
       public         heap    postgres    false            �            1259    18044    Features    TABLE     �   CREATE TABLE public."Features" (
    "Id" uuid NOT NULL,
    "CarBody" character varying(20) NOT NULL,
    "Fuel" text NOT NULL,
    "EngineSize" bigint NOT NULL,
    "Gearbox" text NOT NULL,
    "HorsePower" bigint NOT NULL
);
    DROP TABLE public."Features";
       public         heap    postgres    false            �            1259    18051    Images    TABLE     W   CREATE TABLE public."Images" (
    "Id" uuid NOT NULL,
    "ImageUrl" text NOT NULL
);
    DROP TABLE public."Images";
       public         heap    postgres    false            �            1259    18058    Users    TABLE       CREATE TABLE public."Users" (
    "Id" uuid NOT NULL,
    "FirstName" character varying(50) NOT NULL,
    "LastName" character varying(50) NOT NULL,
    "Email" character varying(50) NOT NULL,
    "HashedPassword" character varying(150) NOT NULL,
    "PhoneNumber" character varying(10) NOT NULL,
    "Address" character varying(50) NOT NULL,
    "Role" text NOT NULL,
    "DateCreated" timestamp with time zone NOT NULL,
    "LastModified" timestamp with time zone DEFAULT '-infinity'::timestamp with time zone NOT NULL
);
    DROP TABLE public."Users";
       public         heap    postgres    false            �            1259    18065    Vehicles    TABLE     u  CREATE TABLE public."Vehicles" (
    "Id" uuid NOT NULL,
    "VinNumber" character varying(17) NOT NULL,
    "Make" character varying(50) NOT NULL,
    "Model" character varying(50) NOT NULL,
    "Year" bigint NOT NULL,
    "Mileage" bigint NOT NULL,
    "PriceInEuro" real NOT NULL,
    "TechnicalState" text NOT NULL,
    "FeatureId" uuid NOT NULL,
    "OwnerId" uuid
);
    DROP TABLE public."Vehicles";
       public         heap    postgres    false            �            1259    18039    __EFMigrationsHistory    TABLE     �   CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);
 +   DROP TABLE public."__EFMigrationsHistory";
       public         heap    postgres    false            .          0    18160    AnnouncementImages 
   TABLE DATA           K   COPY public."AnnouncementImages" ("AnnouncementId", "ImageId") FROM stdin;
    public          postgres    false    220   �3       -          0    18082    Announcements 
   TABLE DATA           �   COPY public."Announcements" ("Id", "Title", "Description", "County", "City", "DateCreated", "VehicleId", "OwnerId", "DateModified") FROM stdin;
    public          postgres    false    219   �3       )          0    18044    Features 
   TABLE DATA           d   COPY public."Features" ("Id", "CarBody", "Fuel", "EngineSize", "Gearbox", "HorsePower") FROM stdin;
    public          postgres    false    215   �3       *          0    18051    Images 
   TABLE DATA           4   COPY public."Images" ("Id", "ImageUrl") FROM stdin;
    public          postgres    false    216   �3       +          0    18058    Users 
   TABLE DATA           �   COPY public."Users" ("Id", "FirstName", "LastName", "Email", "HashedPassword", "PhoneNumber", "Address", "Role", "DateCreated", "LastModified") FROM stdin;
    public          postgres    false    217   �3       ,          0    18065    Vehicles 
   TABLE DATA           �   COPY public."Vehicles" ("Id", "VinNumber", "Make", "Model", "Year", "Mileage", "PriceInEuro", "TechnicalState", "FeatureId", "OwnerId") FROM stdin;
    public          postgres    false    218   �4       (          0    18039    __EFMigrationsHistory 
   TABLE DATA           R   COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
    public          postgres    false    214   �4       �           2606    18164 (   AnnouncementImages PK_AnnouncementImages 
   CONSTRAINT     �   ALTER TABLE ONLY public."AnnouncementImages"
    ADD CONSTRAINT "PK_AnnouncementImages" PRIMARY KEY ("AnnouncementId", "ImageId");
 V   ALTER TABLE ONLY public."AnnouncementImages" DROP CONSTRAINT "PK_AnnouncementImages";
       public            postgres    false    220    220            �           2606    18088    Announcements PK_Announcements 
   CONSTRAINT     b   ALTER TABLE ONLY public."Announcements"
    ADD CONSTRAINT "PK_Announcements" PRIMARY KEY ("Id");
 L   ALTER TABLE ONLY public."Announcements" DROP CONSTRAINT "PK_Announcements";
       public            postgres    false    219            �           2606    18050    Features PK_Features 
   CONSTRAINT     X   ALTER TABLE ONLY public."Features"
    ADD CONSTRAINT "PK_Features" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Features" DROP CONSTRAINT "PK_Features";
       public            postgres    false    215            �           2606    18057    Images PK_Images 
   CONSTRAINT     T   ALTER TABLE ONLY public."Images"
    ADD CONSTRAINT "PK_Images" PRIMARY KEY ("Id");
 >   ALTER TABLE ONLY public."Images" DROP CONSTRAINT "PK_Images";
       public            postgres    false    216            �           2606    18064    Users PK_Users 
   CONSTRAINT     R   ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");
 <   ALTER TABLE ONLY public."Users" DROP CONSTRAINT "PK_Users";
       public            postgres    false    217            �           2606    18071    Vehicles PK_Vehicles 
   CONSTRAINT     X   ALTER TABLE ONLY public."Vehicles"
    ADD CONSTRAINT "PK_Vehicles" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Vehicles" DROP CONSTRAINT "PK_Vehicles";
       public            postgres    false    218                       2606    18043 .   __EFMigrationsHistory PK___EFMigrationsHistory 
   CONSTRAINT     {   ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");
 \   ALTER TABLE ONLY public."__EFMigrationsHistory" DROP CONSTRAINT "PK___EFMigrationsHistory";
       public            postgres    false    214            �           1259    18175    IX_AnnouncementImages_ImageId    INDEX     e   CREATE INDEX "IX_AnnouncementImages_ImageId" ON public."AnnouncementImages" USING btree ("ImageId");
 3   DROP INDEX public."IX_AnnouncementImages_ImageId";
       public            postgres    false    220            �           1259    18115    IX_Announcements_OwnerId    INDEX     [   CREATE INDEX "IX_Announcements_OwnerId" ON public."Announcements" USING btree ("OwnerId");
 .   DROP INDEX public."IX_Announcements_OwnerId";
       public            postgres    false    219            �           1259    18116    IX_Announcements_VehicleId    INDEX     f   CREATE UNIQUE INDEX "IX_Announcements_VehicleId" ON public."Announcements" USING btree ("VehicleId");
 0   DROP INDEX public."IX_Announcements_VehicleId";
       public            postgres    false    219            �           1259    18137    IX_Images_ImageUrl    INDEX     V   CREATE UNIQUE INDEX "IX_Images_ImageUrl" ON public."Images" USING btree ("ImageUrl");
 (   DROP INDEX public."IX_Images_ImageUrl";
       public            postgres    false    216            �           1259    18117    IX_Users_Email    INDEX     N   CREATE UNIQUE INDEX "IX_Users_Email" ON public."Users" USING btree ("Email");
 $   DROP INDEX public."IX_Users_Email";
       public            postgres    false    217            �           1259    18118    IX_Vehicles_FeatureId    INDEX     U   CREATE INDEX "IX_Vehicles_FeatureId" ON public."Vehicles" USING btree ("FeatureId");
 +   DROP INDEX public."IX_Vehicles_FeatureId";
       public            postgres    false    218            �           1259    18119    IX_Vehicles_OwnerId    INDEX     Q   CREATE INDEX "IX_Vehicles_OwnerId" ON public."Vehicles" USING btree ("OwnerId");
 )   DROP INDEX public."IX_Vehicles_OwnerId";
       public            postgres    false    218            �           1259    18120    IX_Vehicles_VinNumber    INDEX     \   CREATE UNIQUE INDEX "IX_Vehicles_VinNumber" ON public."Vehicles" USING btree ("VinNumber");
 +   DROP INDEX public."IX_Vehicles_VinNumber";
       public            postgres    false    218            �           2606    18165 E   AnnouncementImages FK_AnnouncementImages_Announcements_AnnouncementId    FK CONSTRAINT     �   ALTER TABLE ONLY public."AnnouncementImages"
    ADD CONSTRAINT "FK_AnnouncementImages_Announcements_AnnouncementId" FOREIGN KEY ("AnnouncementId") REFERENCES public."Announcements"("Id") ON DELETE CASCADE;
 s   ALTER TABLE ONLY public."AnnouncementImages" DROP CONSTRAINT "FK_AnnouncementImages_Announcements_AnnouncementId";
       public          postgres    false    3216    220    219            �           2606    18170 7   AnnouncementImages FK_AnnouncementImages_Images_ImageId    FK CONSTRAINT     �   ALTER TABLE ONLY public."AnnouncementImages"
    ADD CONSTRAINT "FK_AnnouncementImages_Images_ImageId" FOREIGN KEY ("ImageId") REFERENCES public."Images"("Id") ON DELETE CASCADE;
 e   ALTER TABLE ONLY public."AnnouncementImages" DROP CONSTRAINT "FK_AnnouncementImages_Images_ImageId";
       public          postgres    false    3204    220    216            �           2606    18089 ,   Announcements FK_Announcements_Users_OwnerId    FK CONSTRAINT     �   ALTER TABLE ONLY public."Announcements"
    ADD CONSTRAINT "FK_Announcements_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES public."Users"("Id") ON DELETE CASCADE;
 Z   ALTER TABLE ONLY public."Announcements" DROP CONSTRAINT "FK_Announcements_Users_OwnerId";
       public          postgres    false    217    3207    219            �           2606    18094 1   Announcements FK_Announcements_Vehicles_VehicleId    FK CONSTRAINT     �   ALTER TABLE ONLY public."Announcements"
    ADD CONSTRAINT "FK_Announcements_Vehicles_VehicleId" FOREIGN KEY ("VehicleId") REFERENCES public."Vehicles"("Id") ON DELETE CASCADE;
 _   ALTER TABLE ONLY public."Announcements" DROP CONSTRAINT "FK_Announcements_Vehicles_VehicleId";
       public          postgres    false    218    3212    219            �           2606    18139 '   Vehicles FK_Vehicles_Features_FeatureId    FK CONSTRAINT     �   ALTER TABLE ONLY public."Vehicles"
    ADD CONSTRAINT "FK_Vehicles_Features_FeatureId" FOREIGN KEY ("FeatureId") REFERENCES public."Features"("Id") ON DELETE CASCADE;
 U   ALTER TABLE ONLY public."Vehicles" DROP CONSTRAINT "FK_Vehicles_Features_FeatureId";
       public          postgres    false    3201    215    218            �           2606    18077 "   Vehicles FK_Vehicles_Users_OwnerId    FK CONSTRAINT     �   ALTER TABLE ONLY public."Vehicles"
    ADD CONSTRAINT "FK_Vehicles_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES public."Users"("Id") ON DELETE CASCADE;
 P   ALTER TABLE ONLY public."Vehicles" DROP CONSTRAINT "FK_Vehicles_Users_OwnerId";
       public          postgres    false    3207    218    217            .      x������ � �      -      x������ � �      )      x������ � �      *      x������ � �      +   �   x�}�M�0����e��[N��bB�%�<u1�J�����I���jK�G��45�K|���̅�H� �M׺@-��������b���b�y�t1�`�}u}�R6>%�o��\t[gomY|�Qy>a�Q���f$�(�8D��"���}����0�      ,      x������ � �      (   �   x�e�?�0Gg�]l�W(e$1��$n&z�t�G�|!��?�-���5h�d�]F�?N<���Os�]!A�7S堵�������[Ҁ
���<�&�S��?Ea��<Ҟn�̂]F�����ܮ����o����x����=�J!�';�     