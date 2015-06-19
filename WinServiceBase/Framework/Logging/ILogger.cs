using System;

namespace WinServiceBase.Framework.Logging
{
    public interface ILogger
    {
        void Trace( string format, params object[] args );
        void Debug( string format, params object[] args );
        void Info( string format, params object[] args );
        void Warn( string format, params object[] args );
        void Error( string format, params object[] args );
        void ErrorException( Exception err, string format, params object[] args );
        void Fatal( string format, params object[] args );
        void FatalException( Exception err, string format, params object[] args );

        // Allow the user to pass an environment and client string
        // <para> This is utilized in database logging</para>
        void Trace( string environment, string client, string format, params object[] args );
        void Debug( string environment, string client, string format, params object[] args );
        void Info( string environment, string client, string format, params object[] args );
        void Warn( string environment, string client, string format, params object[] args );
        void Error( string environment, string client, string format, params object[] args );
        void ErrorException( string environment, string client, Exception err, string format, params object[] args );
        void Fatal( string environment, string client, string format, params object[] args );
        void FatalException( string environment, string client, Exception err, string format, params object[] args );
    }
}