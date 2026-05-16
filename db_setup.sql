-- Run this in the Supabase SQL editor.
-- Creates the users table used by the .NET backend (POST /api/users).

CREATE TABLE IF NOT EXISTS public.users (
    id              BIGSERIAL PRIMARY KEY,
    first_name      VARCHAR(100) NOT NULL,
    last_name       VARCHAR(100) NOT NULL,
    email           VARCHAR(200) NOT NULL,
    phone           VARCHAR(20)  NOT NULL,
    profile_base64  TEXT         NOT NULL,
    birth_day       DATE         NOT NULL,
    occupation      VARCHAR(100) NOT NULL,
    gender          VARCHAR(20)  NOT NULL,
    created_at      TIMESTAMPTZ  NOT NULL DEFAULT now()
);

CREATE INDEX IF NOT EXISTS idx_users_email ON public.users (email);

-- If Row Level Security is enabled on the project, this table is accessed
-- by the .NET backend with the postgres role (full access), so no RLS policy
-- is required. Leave RLS disabled on this table, or add a policy if you
-- later expose it via PostgREST.
ALTER TABLE public.users DISABLE ROW LEVEL SECURITY;
