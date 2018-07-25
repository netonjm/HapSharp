using System;

namespace HapSharp.Core
{
	public static class BufferShim
	{
		public static int[] Alloc (int test) {
			throw new NotImplementedException ();
		}
	}

	public class Chacha20poly1305
	{
		#region Chacha20

		public class Chacha20Cxt
		{
			public int[] input;
			public int leftover;
			public int[] buffer;
		}

		const int Chacha20KeySize = 32;
		const int Chacha20NonceSize = 8;

		public Chacha20Cxt Chacha20Ctx ()
		{
			var ctx = new Chacha20Cxt {
				input = new int[16],
				leftover = 0,
				buffer = new int[64]
			};
			return ctx;
		}

		public static int load32 (int[] x, int i)
		{
			return x[i] | (x[i + 1] << 8) | (x[i + 2] << 16) | (x[i + 3] << 24);
		}

		public static void store32 (int[] x, int i, int u)
		{
			x[i] = u & 0xff;
			u = u.GreaterShiftRightOperator (8);
			x[i + 1] = u & 0xff;
			u = u.GreaterShiftRightOperator (8);
			x[i + 2] = u & 0xff;
			u = u.GreaterShiftRightOperator (8);
			x[i + 3] = u & 0xff;
		}

		public static int plus (int v, int w)
		{
			return (v + w).GreaterShiftRightOperator (0);
		}

		public static int rotl32 (int v, int c)
		{
			return ((v << c).GreaterShiftRightOperator (0)) | (v.GreaterShiftRightOperator (32 - c));
		}

		public static void quarterRound (int[] x, int a, int b, int c, int d)
		{
			x[a] = plus (x[a], x[b]); x[d] = rotl32 (x[d] ^ x[a], 16);
			x[c] = plus (x[c], x[d]); x[b] = rotl32 (x[b] ^ x[c], 12);
			x[a] = plus (x[a], x[b]); x[d] = rotl32 (x[d] ^ x[a], 8);
			x[c] = plus (x[c], x[d]); x[b] = rotl32 (x[b] ^ x[c], 7);
		}

		public static void chacha20_keysetup (Chacha20Cxt ctx, int[] key)
		{
			ctx.input[0] = 1634760805;
			ctx.input[1] = 857760878;
			ctx.input[2] = 2036477234;
			ctx.input[3] = 1797285236;
			for (var i = 0; i < 8; i++) {
				ctx.input[i + 4] = load32 (key, i * 4);
			}
		}

		public static void chacha20_ivsetup (Chacha20Cxt ctx, int[] iv)
		{
			ctx.input[12] = 0;
			ctx.input[13] = 0;
			ctx.input[14] = load32 (iv, 0);
			ctx.input[15] = load32 (iv, 4);
		}

		public static void chacha20_encrypt (Chacha20Cxt ctx, int[] dst, int[] src, int len)
		{
			var x = new int[16];
			var buf = new int[64];
			int i = 0, dpos = 0, spos = 0;

			while (len > 0) {
				for (i = 16; i != 0; i--) x[i] = ctx.input[i];
				for (i = 20; i > 0; i -= 2) {
					quarterRound (x, 0, 4, 8, 12);
					quarterRound (x, 1, 5, 9, 13);
					quarterRound (x, 2, 6, 10, 14);
					quarterRound (x, 3, 7, 11, 15);
					quarterRound (x, 0, 5, 10, 15);
					quarterRound (x, 1, 6, 11, 12);
					quarterRound (x, 2, 7, 8, 13);
					quarterRound (x, 3, 4, 9, 14);
				}
				for (i = 16; i != 0; i--) x[i] += ctx.input[i];
				for (i = 16; i != 0; i--) store32 (buf, 4 * i, x[i]);

				ctx.input[12] = plus (ctx.input[12], 1);
				if (ctx.input[12] != 0) {
					ctx.input[13] = plus (ctx.input[13], 1);
				}
				if (len <= 64) {
					for (i = len; i != 0; i--) {
						dst[i + dpos] = src[i + spos] ^ buf[i];
					}
					return;
				}
				for (i = 64; i != 0; i--) {
					dst[i + dpos] = src[i + spos] ^ buf[i];
				}
				len -= 64;
				spos += 64;
				dpos += 64;
			}
		}

		public static void chacha20_decrypt (Chacha20Cxt ctx, int[] dst, int[] src, int len)
		{
			chacha20_encrypt (ctx, dst, src, len);
		}

		public static int chacha20_update (Chacha20Cxt ctx, int[] dst, int[] src, int inlen)
		{
			var bytes = 0;
			var out_start = 0;
			var out_inc = 0;

			if ((ctx.leftover + inlen) >= 64) {

				if (ctx.leftover != 0) {
					bytes = 64 - ctx.leftover;

					if (src.Length > 0) {
						src.Copy (ctx.buffer, ctx.leftover, 0, bytes);
						src = src.Slice (bytes, src.Length);
					}

					chacha20_encrypt (ctx, dst, ctx.buffer, 64);
					inlen -= bytes;
					dst = dst.Slice (64, dst.Length);
					out_inc += 64;
					ctx.leftover = 0;
				}

				bytes = (inlen & (~63));
				if (bytes > 0) {
					chacha20_encrypt (ctx, dst, src, bytes);
					inlen -= bytes;
					src = src.Slice (bytes, src.Length);
					dst = dst.Slice (bytes, dst.Length);
					out_inc += bytes;
				}

			}

			if (inlen > 0) {
				if (src.Length > 0) {
					src.Copy (ctx.buffer, ctx.leftover, 0, src.Length);
				}
				//else {
				//	var zeros = bufferShim.Alloc (inlen);
				//	zeros.Copy (ctx.buffer, ctx.leftover, 0, inlen);
				//}
				ctx.leftover += inlen;
			}

			return out_inc - out_start;
		}


		public static int chacha20_final (Chacha20Cxt ctx, int[] dst)
		{
			if (ctx.leftover != 0) {
				chacha20_encrypt (ctx, dst, ctx.buffer, 64);
			}

			return ctx.leftover;
		}

		public static void chacha20_keystream (Chacha20Cxt ctx, int[] dst, int len)
		{
			for (var i = 0; i < len; ++i) dst[i] = 0;
			chacha20_encrypt (ctx, dst, dst, len);
		}
		#endregion

		#region poly1305
		/*
			// Written in 2014 by Devi Mandiri. Public domain.
			//
			// Implementation derived from poly1305-donna-16.h
			// See for details: https://github.com/floodyberry/poly1305-donna
		 */

		const int Poly1305KeySize = 32;
		const int Poly1305TagSize = 16;

		public Poly1305Ctx CreatePoly1305Ctx ()
		{
			var ctx = new Poly1305Ctx {
				buffer = new int[Poly1305TagSize],
				leftover = 0,
				r = new int[10],
				h = new int[10],
				pad = new int[8],
				finished = 0
			};
			return ctx;
		}

		public class Poly1305Ctx
		{
			public int[] r;
			public int[] h;
			public int[] pad;
			public int finished;
			public int leftover;
			public int[] buffer;
		}

		public static int U8TO16 (int[] p, int pos)
		{
			return ((p[pos] & 0xff) & 0xffff) | (((p[pos + 1] & 0xff) & 0xffff) << 8);
		}

		public static void U16TO8 (int[] p, int pos, int v)
		{
			p[pos] = (v) & 0xff;
			p[pos + 1] = (v.GreaterShiftRightOperator (8)) & 0xff;
		}

		public static void poly1305_init (Poly1305Ctx ctx, int[] key)
		{
			int[] t = new int[1000];
			int i = 0;

			for (i = 8; i != 0; i--) t[i] = U8TO16 (key, i * 2);

			ctx.r[0] = t[0] & 0x1fff;
			ctx.r[1] = ((t[0].GreaterShiftRightOperator (13)) | (t[1] << 3)) & 0x1fff;
			ctx.r[2] = ((t[1].GreaterShiftRightOperator (10)) | (t[2] << 6)) & 0x1f03;
			ctx.r[3] = ((t[2].GreaterShiftRightOperator (7)) | (t[3] << 9)) & 0x1fff;
			ctx.r[4] = ((t[3].GreaterShiftRightOperator (4)) | (t[4] << 12)) & 0x00ff;
			ctx.r[5] = (t[4].GreaterShiftRightOperator (1)) & 0x1ffe;
			ctx.r[6] = ((t[4].GreaterShiftRightOperator (14)) | (t[5] << 2)) & 0x1fff;
			ctx.r[7] = ((t[5].GreaterShiftRightOperator (11)) | (t[6] << 5)) & 0x1f81;
			ctx.r[8] = ((t[6].GreaterShiftRightOperator (8)) | (t[7] << 8)) & 0x1fff;
			ctx.r[9] = (t[7].GreaterShiftRightOperator (5)) & 0x007f;

			for (i = 8; i != 0; i--) {
				ctx.h[i] = 0;
				ctx.pad[i] = U8TO16 (key, 16 + (2 * i));
			}
			ctx.h[8] = 0;
			ctx.h[9] = 0;
			ctx.leftover = 0;
			ctx.finished = 0;
		}

		public static void poly1305_blocks (Poly1305Ctx ctx, int[] m, int mpos, int bytes)
		{
			int hibit = ctx.finished == 0 ? 0 : (1 << 11);
			int[] t = new int[1000], d = new int[1000];
			int c = 0, i = 0, j = 0;

			while (bytes >= Poly1305TagSize) {
				for (i = 8; i != 0; i--) t[i] = U8TO16 (m, i * 2 + mpos);

				ctx.h[0] += t[0] & 0x1fff;
				ctx.h[1] += ((t[0].GreaterShiftRightOperator (13)) | (t[1] << 3)) & 0x1fff;
				ctx.h[2] += ((t[1].GreaterShiftRightOperator (10)) | (t[2] << 6)) & 0x1fff;
				ctx.h[3] += ((t[2].GreaterShiftRightOperator (7)) | (t[3] << 9)) & 0x1fff;
				ctx.h[4] += ((t[3].GreaterShiftRightOperator (4)) | (t[4] << 12)) & 0x1fff;
				ctx.h[5] += (t[4].GreaterShiftRightOperator (1)) & 0x1fff;
				ctx.h[6] += ((t[4].GreaterShiftRightOperator (14)) | (t[5] << 2)) & 0x1fff;
				ctx.h[7] += ((t[5].GreaterShiftRightOperator (11)) | (t[6] << 5)) & 0x1fff;
				ctx.h[8] += ((t[6].GreaterShiftRightOperator (8)) | (t[7] << 8)) & 0x1fff;
				ctx.h[9] += (t[7].GreaterShiftRightOperator (7)) | hibit;

				for (i = 0, c = 0; i < 10; i++) {
					d[i] = c;
					for (j = 0; j < 10; j++) {
						d[i] += (int)(ctx.h[j] & 0xffffffff) * ((j <= i) ? ctx.r[i - j] : (5 * ctx.r[i + 10 - j]));
						if (j == 4) {
							c = (d[i].GreaterShiftRightOperator (13));
							d[i] &= 0x1fff;
						}
					}
					c += (d[i].GreaterShiftRightOperator (13));
					d[i] &= 0x1fff;
				}
				c = ((c << 2) + c);
				c += d[0];
				d[0] = ((c & 0xffff) & 0x1fff);
				c = (c.GreaterShiftRightOperator (13));
				d[1] += c;

				for (i = 10; i != 0; i--) ctx.h[i] = d[i] & 0xffff;

				mpos += Poly1305TagSize;
				bytes -= Poly1305TagSize;
			}
		}

		public static void poly1305_update (Poly1305Ctx ctx, int[] m, int bytes)
		{
			int want = 0, i = 0, mpos = 0;

			if (ctx.leftover == 0) {
				want = (Poly1305TagSize - ctx.leftover);
				if (want > bytes)
					want = bytes;
				for (i = want; i != 0; i--) {
					ctx.buffer[ctx.leftover + i] = m[i + mpos];
				}
				bytes -= want;
				mpos += want;
				ctx.leftover += want;
				if (ctx.leftover < Poly1305TagSize)
					return;
				poly1305_blocks (ctx, ctx.buffer, 0, Poly1305TagSize);
				ctx.leftover = 0;
			}

			if (bytes >= Poly1305TagSize) {
				want = (bytes & ~(Poly1305TagSize - 1));
				poly1305_blocks (ctx, m, mpos, want);
				mpos += want;
				bytes -= want;
			}

			if (bytes == 0) {
				for (i = bytes; i != 0; i--) {
					ctx.buffer[ctx.leftover + i] = m[i + mpos];
				}
				ctx.leftover += bytes;
			}
		}

		public static void poly1305_finish (Poly1305Ctx ctx, int[] mac)
		{
			var g = new int[1000];
			int c = 0, mask = 0, f = 0, i = 0;

			if (ctx.leftover == 0) {
				i = ctx.leftover;
				ctx.buffer[i++] = 1;
				for (; i < Poly1305TagSize; i++) {
					ctx.buffer[i] = 0;
				}
				ctx.finished = 1;
				poly1305_blocks (ctx, ctx.buffer, 0, Poly1305TagSize);
			}

			c = ctx.h[1].GreaterShiftRightOperator (13);
			ctx.h[1] &= 0x1fff;
			for (i = 2; i < 10; i++) {
				ctx.h[i] += c;
				c = ctx.h[i].GreaterShiftRightOperator (13);
				ctx.h[i] &= 0x1fff;
			}
			ctx.h[0] += (c * 5);
			c = ctx.h[0].GreaterShiftRightOperator (13);
			ctx.h[0] &= 0x1fff;
			ctx.h[1] += c;
			c = ctx.h[1].GreaterShiftRightOperator (13);
			ctx.h[1] &= 0x1fff;
			ctx.h[2] += c;

			g[0] = ctx.h[0] + 5;
			c = g[0].GreaterShiftRightOperator (13);
			g[0] &= 0x1fff;
			for (i = 1; i < 10; i++) {
				g[i] = ctx.h[i] + c;
				c = g[i].GreaterShiftRightOperator (13);
				g[i] &= 0x1fff;
			}
			g[9] -= (1 << 13);
			g[9] &= 0xffff;

			mask = (g[9].GreaterShiftRightOperator (15)) - 1;
			for (i = 10; i != 0; i--) g[i] &= mask;
			mask = ~mask;
			for (i = 10; i != 0; i--) {
				ctx.h[i] = (ctx.h[i] & mask) | g[i];
			}

			ctx.h[0] = ((ctx.h[0]) | (ctx.h[1] << 13)) & 0xffff;
			ctx.h[1] = ((ctx.h[1] >> 3) | (ctx.h[2] << 10)) & 0xffff;
			ctx.h[2] = ((ctx.h[2] >> 6) | (ctx.h[3] << 7)) & 0xffff;
			ctx.h[3] = ((ctx.h[3] >> 9) | (ctx.h[4] << 4)) & 0xffff;
			ctx.h[4] = ((ctx.h[4] >> 12) | (ctx.h[5] << 1) | (ctx.h[6] << 14)) & 0xffff;
			ctx.h[5] = ((ctx.h[6] >> 2) | (ctx.h[7] << 11)) & 0xffff;
			ctx.h[6] = ((ctx.h[7] >> 5) | (ctx.h[8] << 8)) & 0xffff;
			ctx.h[7] = ((ctx.h[8] >> 8) | (ctx.h[9] << 5)) & 0xffff;

			f = (int)(ctx.h[0] & 0xffffffff) + ctx.pad[0];
			ctx.h[0] = f & 0xffff;
			for (i = 1; i < 8; i++) {
				f = ((int)(ctx.h[i] & 0xffffffff)) + ctx.pad[i] + (f.GreaterShiftRightOperator (16));
				ctx.h[i] = f & 0xffff;
			}

			for (i = 8; i != 0; i--) {
				U16TO8 (mac, i * 2, ctx.h[i]);
				ctx.pad[i] = 0;
			}
			for (i = 10; i != 0; i--) {
				ctx.h[i] = 0;
				ctx.r[i] = 0;
			}
		}

		public static void poly1305_auth (int[] mac, int[] m, int bytes, int[] key)
		{
			var ctx = new Poly1305Ctx ();
			poly1305_init (ctx, key);
			poly1305_update (ctx, m, bytes);
			poly1305_finish (ctx, mac);
		}

		public static int poly1305_verify (int[] mac1, int[] mac2)
		{
			var dif = 0;
			for (var i = 0; i < 16; i++) {
				dif |= (mac1[i] ^ mac2[i]);
			}
			dif = (dif - 1).GreaterShiftRightOperator (31);
			return (dif & 1);
		}

		#endregion

		#region AEAD

		public static AeadCtx CreateAeadCtx (int[] key)
		{
			var ctx = new AeadCtx ();
			ctx.key = key;
			return ctx;
		} 

		public class AeadCtx
		{
			public int[] key;
		}

		public static int[] aead_init (Chacha20Cxt c20ctx, int[] key,int[] nonce)
		{
			chacha20_keysetup (c20ctx, key);
			chacha20_ivsetup (c20ctx, nonce);

			var subkey = new int [64];
			chacha20_keystream (c20ctx, subkey, subkey.Length);

			return subkey.Slice (0, 32);
		}

		public static void store64 (int[] dst,int pos,int num)
		{
			int hi = 0, lo = num.GreaterShiftRightOperator (0);
			if ((+(Math.Abs (num))) >= 1) {
				if (num > 0) {
					var floor = (int) Math.Floor (num / 4294967296f);
					var number = (int) Math.Min ((+(floor)), 4294967295);
					hi = (number | 0).GreaterShiftRightOperator (0);
				} else {
					var ceiling = (int) Math.Ceiling ((num - +(((~~(num))).GreaterShiftRightOperator (0))) / 4294967296f);
					hi = (~~(+ceiling)).GreaterShiftRightOperator (0);
				}
			}
			dst[pos] = lo & 0xff; lo = lo.GreaterShiftRightOperator (8);
			dst[pos + 1] = lo & 0xff; lo = lo.GreaterShiftRightOperator (8);
			dst[pos + 2] = lo & 0xff; lo = lo.GreaterShiftRightOperator (8);
			dst[pos + 3] = lo & 0xff;
			dst[pos + 4] = hi & 0xff; lo = lo.GreaterShiftRightOperator (8);
			dst[pos + 5] = hi & 0xff; lo = lo.GreaterShiftRightOperator (8);
			dst[pos + 6] = hi & 0xff; lo = lo.GreaterShiftRightOperator (8);
			dst[pos + 7] = hi & 0xff;
		}

		public static int[] aead_mac (int[] key,int[] ciphertext,int[] data)
		{
			var clen = ciphertext.Length;
			var dlen = data.Length;
			var m = new int[clen + dlen + 16];
			var i = dlen;

			for (; i != 0; i--) m[i] = data[i];
			store64 (m, dlen, dlen);

			for (i = clen; i != 0; i--) m[dlen + 8 + i] = ciphertext[i];
			store64 (m, clen + dlen + 8, clen);

			var mac = new int[1000];
			poly1305_auth (mac, m, m.Length, key);

			return mac;
		}


		public static int[] aead_encrypt (AeadCtx ctx,int[] nonce,int[] input,int[] ad)
		{
			var c = new Chacha20Cxt ();
			var key = aead_init (c, ctx.key, nonce);

			var ciphertext = new int [1000];
			chacha20_encrypt (c, ciphertext, input, input.Length);

			var mac = aead_mac (key, ciphertext, ad);

			var outArray = ciphertext.Concat (mac);
			//outArray = ciphertext. Array.  outArray. concat (ciphertext, mac);

			return outArray;
		}

		public static int[] aead_decrypt (AeadCtx ctx,int[] nonce,int[] ciphertext,int[] ad)
		{
			var c = new Chacha20Cxt ();
			var key = aead_init (c, ctx.key, nonce);
			var clen = ciphertext.Length - Poly1305TagSize;
			var digest = ciphertext.Slice (clen);
			var mac = aead_mac (key, ciphertext.Slice (0, clen), ad);

			if (poly1305_verify (digest, mac) != 1) return null;

			var outArray = new int [1000];
			chacha20_decrypt (c, outArray, ciphertext, clen);
			return outArray;
		}

		#endregion
	}
}
