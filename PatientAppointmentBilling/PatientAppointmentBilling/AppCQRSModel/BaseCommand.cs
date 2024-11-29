using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCQRS
{

    # region marker interface pattern
    public interface ICommand : IRequest
    {
        Guid CommandId { get; }
    }

    public interface IQuery
    {

    }

    public interface IResult
    {

    }

    public interface IEvent
    {

    }

    //public interface IHandler<TCommand> where TCommand : ICommand
    //{
    //    bool Handle(TCommand t);
    //}
    #endregion

    #region BaseCommand Abstract class
    public abstract class BaseCommand : ICommand
    {
        public Guid CommandId { get; }
        public BaseCommand()
        {
            this.CommandId = Guid.NewGuid();
        }
    }
    #endregion
}
