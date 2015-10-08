--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

SET default_with_oids = false;

--
-- Name: user; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE "user" (
    userid integer NOT NULL,
    name text NOT NULL
);


--
-- Name: user_userid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE user_userid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: user_userid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE user_userid_seq OWNED BY "user".userid;


--
-- Name: userproduct; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE userproduct (
    userproductid integer NOT NULL,
    userid integer NOT NULL,
    productid integer,
    productname text
);


--
-- Name: userproduct_userproductid_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE userproduct_userproductid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: userproduct_userproductid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE userproduct_userproductid_seq OWNED BY userproduct.userproductid;


--
-- Name: userid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY "user" ALTER COLUMN userid SET DEFAULT nextval('user_userid_seq'::regclass);


--
-- Name: userproductid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY userproduct ALTER COLUMN userproductid SET DEFAULT nextval('userproduct_userproductid_seq'::regclass);


--
-- Name: user_pk_userid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_pk_userid PRIMARY KEY (userid);


--
-- Name: user_unique_name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY "user"
    ADD CONSTRAINT user_unique_name UNIQUE (name);


--
-- Name: userproduct_pk_userproductid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY userproduct
    ADD CONSTRAINT userproduct_pk_userproductid PRIMARY KEY (userproductid);


--
-- Name: userproduct_unique_userid_productid_productname; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY userproduct
    ADD CONSTRAINT userproduct_unique_userid_productid_productname UNIQUE (userid, productid, productname);


--
-- Name: fki_userproduct_fk_userid; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_userproduct_fk_userid ON userproduct USING btree (userid);


--
-- Name: userproduct_fk_userid; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY userproduct
    ADD CONSTRAINT userproduct_fk_userid FOREIGN KEY (userid) REFERENCES "user"(userid);


--
-- PostgreSQL database dump complete
--

