--
-- PostgreSQL database dump
--

-- Dumped from database version 15.4
-- Dumped by pg_dump version 15.4

-- Started on 2023-08-30 22:01:04

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE IF EXISTS "AutoSiteDb";
--
-- TOC entry 3380 (class 1262 OID 18038)
-- Name: AutoSiteDb; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "AutoSiteDb" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United States.1252';


ALTER DATABASE "AutoSiteDb" OWNER TO postgres;

\connect "AutoSiteDb"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 220 (class 1259 OID 18160)
-- Name: AnnouncementImages; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AnnouncementImages" (
    "AnnouncementId" uuid NOT NULL,
    "ImageId" uuid NOT NULL
);


ALTER TABLE public."AnnouncementImages" OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 18082)
-- Name: Announcements; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Announcements" (
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


ALTER TABLE public."Announcements" OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 18044)
-- Name: Features; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Features" (
    "Id" uuid NOT NULL,
    "CarBody" character varying(20) NOT NULL,
    "Fuel" text NOT NULL,
    "EngineSize" bigint NOT NULL,
    "Gearbox" text NOT NULL,
    "HorsePower" bigint NOT NULL
);


ALTER TABLE public."Features" OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 18051)
-- Name: Images; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Images" (
    "Id" uuid NOT NULL,
    "ImageUrl" text NOT NULL
);


ALTER TABLE public."Images" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 18058)
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
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


ALTER TABLE public."Users" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 18065)
-- Name: Vehicles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Vehicles" (
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


ALTER TABLE public."Vehicles" OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 18039)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 3374 (class 0 OID 18160)
-- Dependencies: 220
-- Data for Name: AnnouncementImages; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3373 (class 0 OID 18082)
-- Dependencies: 219
-- Data for Name: Announcements; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3369 (class 0 OID 18044)
-- Dependencies: 215
-- Data for Name: Features; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3370 (class 0 OID 18051)
-- Dependencies: 216
-- Data for Name: Images; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3371 (class 0 OID 18058)
-- Dependencies: 217
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."Users" ("Id", "FirstName", "LastName", "Email", "HashedPassword", "PhoneNumber", "Address", "Role", "DateCreated", "LastModified") VALUES ('af1d24e3-f8da-4148-b0da-c1765d85d059', '', '', 'admin', 'AHDuDwkyUD+Kd55n/AwoVV33cvxDEsu0fwrJlt5IAsf/g7nwPoh8BmSnfpiIEz8HcA==', '', '', 'Admin', '2023-08-30 19:22:40.140445+03', '2023-08-30 19:22:40.140445+03') ON CONFLICT DO NOTHING;


--
-- TOC entry 3372 (class 0 OID 18065)
-- Dependencies: 218
-- Data for Name: Vehicles; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3368 (class 0 OID 18039)
-- Dependencies: 214
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20230728202648_UpdateFromLongToGuid', '7.0.9') ON CONFLICT DO NOTHING;
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20230728215033_AddLastModified', '7.0.9') ON CONFLICT DO NOTHING;
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20230730210302_SomeMigration', '7.0.9') ON CONFLICT DO NOTHING;
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20230730211243_upd-db', '7.0.9') ON CONFLICT DO NOTHING;
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20230808150616_upd', '7.0.9') ON CONFLICT DO NOTHING;
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20230808192958_AddDateModifiedToAnnouncement', '7.0.9') ON CONFLICT DO NOTHING;


--
-- TOC entry 3219 (class 2606 OID 18164)
-- Name: AnnouncementImages PK_AnnouncementImages; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AnnouncementImages"
    ADD CONSTRAINT "PK_AnnouncementImages" PRIMARY KEY ("AnnouncementId", "ImageId");


--
-- TOC entry 3216 (class 2606 OID 18088)
-- Name: Announcements PK_Announcements; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Announcements"
    ADD CONSTRAINT "PK_Announcements" PRIMARY KEY ("Id");


--
-- TOC entry 3201 (class 2606 OID 18050)
-- Name: Features PK_Features; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Features"
    ADD CONSTRAINT "PK_Features" PRIMARY KEY ("Id");


--
-- TOC entry 3204 (class 2606 OID 18057)
-- Name: Images PK_Images; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Images"
    ADD CONSTRAINT "PK_Images" PRIMARY KEY ("Id");


--
-- TOC entry 3207 (class 2606 OID 18064)
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- TOC entry 3212 (class 2606 OID 18071)
-- Name: Vehicles PK_Vehicles; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Vehicles"
    ADD CONSTRAINT "PK_Vehicles" PRIMARY KEY ("Id");


--
-- TOC entry 3199 (class 2606 OID 18043)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 3217 (class 1259 OID 18175)
-- Name: IX_AnnouncementImages_ImageId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_AnnouncementImages_ImageId" ON public."AnnouncementImages" USING btree ("ImageId");


--
-- TOC entry 3213 (class 1259 OID 18115)
-- Name: IX_Announcements_OwnerId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Announcements_OwnerId" ON public."Announcements" USING btree ("OwnerId");


--
-- TOC entry 3214 (class 1259 OID 18116)
-- Name: IX_Announcements_VehicleId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Announcements_VehicleId" ON public."Announcements" USING btree ("VehicleId");


--
-- TOC entry 3202 (class 1259 OID 18137)
-- Name: IX_Images_ImageUrl; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Images_ImageUrl" ON public."Images" USING btree ("ImageUrl");


--
-- TOC entry 3205 (class 1259 OID 18117)
-- Name: IX_Users_Email; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Users_Email" ON public."Users" USING btree ("Email");


--
-- TOC entry 3208 (class 1259 OID 18118)
-- Name: IX_Vehicles_FeatureId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Vehicles_FeatureId" ON public."Vehicles" USING btree ("FeatureId");


--
-- TOC entry 3209 (class 1259 OID 18119)
-- Name: IX_Vehicles_OwnerId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Vehicles_OwnerId" ON public."Vehicles" USING btree ("OwnerId");


--
-- TOC entry 3210 (class 1259 OID 18120)
-- Name: IX_Vehicles_VinNumber; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX "IX_Vehicles_VinNumber" ON public."Vehicles" USING btree ("VinNumber");


--
-- TOC entry 3224 (class 2606 OID 18165)
-- Name: AnnouncementImages FK_AnnouncementImages_Announcements_AnnouncementId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AnnouncementImages"
    ADD CONSTRAINT "FK_AnnouncementImages_Announcements_AnnouncementId" FOREIGN KEY ("AnnouncementId") REFERENCES public."Announcements"("Id") ON DELETE CASCADE;


--
-- TOC entry 3225 (class 2606 OID 18170)
-- Name: AnnouncementImages FK_AnnouncementImages_Images_ImageId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AnnouncementImages"
    ADD CONSTRAINT "FK_AnnouncementImages_Images_ImageId" FOREIGN KEY ("ImageId") REFERENCES public."Images"("Id") ON DELETE CASCADE;


--
-- TOC entry 3222 (class 2606 OID 18089)
-- Name: Announcements FK_Announcements_Users_OwnerId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Announcements"
    ADD CONSTRAINT "FK_Announcements_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- TOC entry 3223 (class 2606 OID 18094)
-- Name: Announcements FK_Announcements_Vehicles_VehicleId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Announcements"
    ADD CONSTRAINT "FK_Announcements_Vehicles_VehicleId" FOREIGN KEY ("VehicleId") REFERENCES public."Vehicles"("Id") ON DELETE CASCADE;


--
-- TOC entry 3220 (class 2606 OID 18139)
-- Name: Vehicles FK_Vehicles_Features_FeatureId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Vehicles"
    ADD CONSTRAINT "FK_Vehicles_Features_FeatureId" FOREIGN KEY ("FeatureId") REFERENCES public."Features"("Id") ON DELETE CASCADE;


--
-- TOC entry 3221 (class 2606 OID 18077)
-- Name: Vehicles FK_Vehicles_Users_OwnerId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Vehicles"
    ADD CONSTRAINT "FK_Vehicles_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


-- Completed on 2023-08-30 22:01:04

--
-- PostgreSQL database dump complete
--

