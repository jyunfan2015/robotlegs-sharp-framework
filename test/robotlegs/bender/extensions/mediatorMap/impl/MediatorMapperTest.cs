using Moq;
using robotlegs.bender.framework.api;
using robotlegs.bender.extensions.matching;
using NUnit.Framework;
using robotlegs.bender.extensions.viewManager.support;
using robotlegs.bender.extensions.mediatorMap.support;
using robotlegs.bender.extensions.mediatorMap.api;
using robotlegs.bender.framework.impl;
using robotlegs.bender.extensions.mediatorMap.impl.support;
using robotlegs.bender.extensions.mediatorMap.dsl;


namespace robotlegs.bender.extensions.mediatorMap.impl
{
	public class MediatorMapperTest
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public Mock<IMediatorViewHandler> handler;

		public Mock<ILogger> logger;

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private MediatorMapper mapper;

		private ITypeFilter filter;

		/*============================================================================*/
		/* Test Setup and Teardown                                                    */
		/*============================================================================*/

		[SetUp]
		public void SetUp()
		{
			logger = new Mock<ILogger>();
			handler = new Mock<IMediatorViewHandler>();

			TypeMatcher matcher = new TypeMatcher().AllOf(typeof(SupportView));
			filter = matcher.CreateTypeFilter();
			mapper = new MediatorMapper(matcher.CreateTypeFilter(), handler.Object, logger.Object);
		}

		/*============================================================================*/
		/* Tests                                                                      */
		/*============================================================================*/

		[Test]
		public void ToMediator_Registers_MappingConfig_With_Handler()
		{
			object config = mapper.ToMediator(typeof(NullMediator));

			handler.Verify (_handler => _handler.AddMapping(It.Is<IMediatorMapping> (_arg1 => _arg1 == config)), Times.Once);
		}

		[Test]
		public void FromMediator_Unregisters_MappingConfig_From_Handler()
		{
			object oldConfig = mapper.ToMediator(typeof(NullMediator));
			mapper.FromMediator(typeof(NullMediator));

			handler.Verify (_handler => _handler.RemoveMapping(It.Is<IMediatorMapping> (_arg1 => _arg1 == oldConfig)), Times.Once);
		}

		[Test]
		public void FromMediator_Removes_Only_Specified_MappingConfig_From_Handler()
		{
			object config1 = mapper.ToMediator(typeof(NullMediator));
			object config2 = mapper.ToMediator(typeof(NullMediator2));
			mapper.FromMediator(typeof(NullMediator));

			handler.Verify (_handler => _handler.RemoveMapping(It.Is<IMediatorMapping> (_arg1 => _arg1 == config1)), Times.Once);
			handler.Verify (_handler => _handler.RemoveMapping(It.Is<IMediatorMapping> (_arg1 => _arg1 == config2)), Times.Never);
		}

		[Test]
		public void FromAll_Removes_All_MappingConfigs_From_Handler()
		{
			object config1 = mapper.ToMediator(typeof(NullMediator));
			object config2 = mapper.ToMediator(typeof(NullMediator2));

			mapper.FromAll();

			handler.Verify (_handler => _handler.RemoveMapping (It.Is<IMediatorMapping> (_arg1 => _arg1 == config1)), Times.Once);
			handler.Verify (_handler => _handler.RemoveMapping (It.Is<IMediatorMapping> (_arg1 => _arg1 == config2)), Times.Once);
		}

		[Test]
		public void ToMediator_Unregisters_Old_MappingConfig_And_Registers_New_One_When_Overwritten()
		{
			object oldConfig = mapper.ToMediator(typeof(NullMediator));
			object newConfig = mapper.ToMediator(typeof(NullMediator));

			handler.Verify (_handler => _handler.RemoveMapping(It.Is<IMediatorMapping> (_arg1 => _arg1 == oldConfig)), Times.Once);
			handler.Verify (_handler => _handler.AddMapping(It.Is<IMediatorMapping> (_arg1 => _arg1 == newConfig)), Times.Once);
		}

		[Test]
		public void ToMediator_Warns_When_Overwritten()
		{
			object oldConfig = mapper.ToMediator(typeof(NullMediator));
			mapper.ToMediator(typeof(NullMediator));

			logger.Verify (_logger => _logger.Warn(It.Is<object>(_arg1 => _arg1 is string), It.Is<object>(_arg2 => _arg2 == filter), It.Is<object>(_arg3 => _arg3 == oldConfig)), Times.Once);
		}
	}
}